# Prueba Técnica Nubox

## Contexto
La empresa cuenta con un sistema que calcula sueldos de trabajadores, el cual recibe la nómina y la información básica necesaria para generar las liquidaciones.  
Actualmente buscan un proveedor para el control de asistencia que entregue dispositivos físicos de marcaje y permita integrar la información con el sistema de sueldos.  
La solución esperada debe facilitar el intercambio de datos entre ambos sistemas de manera **segura, escalable, observable, trazable y resiliente**.

---

## Propuesta de Arquitectura
Se planteó una arquitectura basada en **microservicios**, separando responsabilidades de forma clara:

- **Microservicio de Integración de Asistencias**  
  Encargado de registrar las asistencias (individual o masiva mediante JSON/Excel).  
  Valida empleados, evita duplicados y persiste en base de datos.  
  Consolida totales de horas normales y extras por período y publica un **evento RabbitMQ** con esta información.

- **Microservicio de Integración de Liquidaciones**  
  Encargado de **escuchar los eventos publicados por Integración de Asistencias**, almacenar los datos recibidos en su propia base de datos y dejarlos listos para generar liquidaciones.  
  Actualmente implementado con el **esqueleto del consumidor RabbitMQ**, lo que permite demostrar la integración y trazabilidad.

- **Microservicio de Cálculo de Liquidaciones (visión futura)**  
  Este será un tercer microservicio que aplicará las reglas de negocio de liquidación sobre los datos recibidos.  
  Permitirá desacoplar aún más la lógica de cálculo, siendo extensible y escalable en fases posteriores.

---

## Estado Actual

### Microservicio de Integración de Asistencias
Implementado con:
- Registro individual de asistencias.  
- Consultas por empleado en un rango de fechas.  
- Cálculo de totales de horas trabajadas y extras.  
- Carga masiva desde JSON.  
- Carga masiva desde archivos Excel (pendiente de finalizar).  
- **Productor RabbitMQ** que publica eventos de asistencias consolidadas.  

### Microservicio de Integración de Liquidaciones
- Definido el **contrato de evento** `EventoTotalesAsistenciaConsolidados`.  
- Configuración de **Consumer RabbitMQ** (en desarrollo).  
- Preparado para almacenar en su BD los totales recibidos y dejarlos listos para ser usados por el motor de cálculo.  

### Microservicio de Cálculo de Liquidaciones
- **Pendiente de implementación**. Será el motor especializado que procesará las reglas de negocio sobre los datos ya integrados.  

---

## Justificación de Arquitectura

- **Microservicios**: permiten aislar responsabilidades, escalar de forma independiente y desplegar por separado.  
- **RabbitMQ**: asegura comunicación asincrónica y resiliente. Si un servicio cae, los eventos quedan encolados hasta que el consumidor vuelva a estar disponible.  
- **Bases de datos separadas**: cada microservicio es dueño de su información, garantizando autonomía y reducción de acoplamiento.  
- **Eventos consolidados**: se transporta solo lo necesario para cálculo (horas normales, extras, ausencias), reduciendo complejidad y latencia.  
- **Escalabilidad**: cada microservicio puede aumentar réplicas de forma independiente; RabbitMQ maneja backpressure y distribución de mensajes.  
- **Observabilidad y trazabilidad**: se incluyen `CorrelationId`, `ArchivoFuente` y `TimestampUtc` en los eventos para auditar de extremo a extremo.  

---

## Siguientes Pasos

Con lo desarrollado, el MVP se encuentra implementado con dos microservicios que se comunican a través de RabbitMQ.  
Quedan pendientes solo las capas de seguridad y validación automática.
 
1. Completar el **Consumer en Integración de Liquidaciones**, con persistencia idempotente en BD.  
2. Incorporar métricas y logs estructurados para reforzar trazabilidad de extremo a extremo.  
3. **Agregar seguridad con autenticación y JWT** (pendiente del MVP).  
4. **Implementar pruebas unitarias** de los principales flujos (pendiente del MVP).  
5. Actualizar `docker-compose` que levante SQL Server y los microservicios. 
6. A mediano plazo: desarrollar el **Microservicio de Cálculo de Liquidaciones**, especializado en reglas de negocio, independiente de IntegraciónLiquidaciones.

---

## Observaciones
La carpeta `SampleData` fue creada inicialmente para almacenar archivos JSON de ejemplo que simulan el envío de datos del partner de asistencia al microservicio de Integración de Asistencia.  
En la versión final, se espera que estos datos sean publicados como eventos en RabbitMQ y consumidos de manera asincrónica.

---

## Scripts SQL

Adjunto scripts para crear la base de datos, tablas paramétricas y datos de prueba:

```sql
CREATE TABLE TipoNomina (
    IdTipoNomina INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(20) NOT NULL
);

CREATE TABLE TipoJornada (
    IdTipoJornada INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(30) NOT NULL
);

CREATE TABLE EstadoAsistencia (
    IdEstadoAsistencia INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(30) NOT NULL
);

CREATE TABLE TipoOrigenDato (
    IdTipoOrigenDato INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(20) NOT NULL
);

CREATE TABLE Empresa (
    IdEmpresa INT IDENTITY(1,1) PRIMARY KEY,
    RutEmpresa VARCHAR(20) NOT NULL,
    RazonSocial VARCHAR(200) NOT NULL,
    IdTipoNomina INT NOT NULL,
    CONSTRAINT FK_Empresa_TipoNomina FOREIGN KEY (IdTipoNomina)
        REFERENCES TipoNomina(IdTipoNomina)
);

CREATE TABLE Empleado (
    IdEmpleado INT IDENTITY(1,1) PRIMARY KEY,
    IdEmpresa INT NOT NULL,
    Rut VARCHAR(20) NOT NULL,
    Nombres VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    Email VARCHAR(100),
    Cargo VARCHAR(100),
    Departamento VARCHAR(100),
    FechaIngreso DATE NOT NULL,
    SalarioBase DECIMAL(18,2) NOT NULL,
    HoraEntrada TIME NOT NULL,
    HoraSalida TIME NOT NULL,
    HorasSemanales INT NOT NULL,
    Activo BIT NOT NULL,
    CONSTRAINT FK_Empleado_Empresa FOREIGN KEY (IdEmpresa) 
        REFERENCES Empresa(IdEmpresa)
);

CREATE TABLE Asistencia (
    IdAsistencia INT IDENTITY(1,1) PRIMARY KEY,
    IdEmpleado INT NOT NULL,
    Fecha DATE NOT NULL,
    HoraEntrada TIME NULL,
    HoraSalida TIME NULL,
    HorasTrabajadas DECIMAL(5,2) NOT NULL,
    HorasExtras DECIMAL(5,2) NOT NULL,
    IdTipoJornada INT NOT NULL,
    IdEstadoAsistencia INT NOT NULL,
    IdTipoOrigenDato INT NOT NULL,
    Observaciones VARCHAR(255) NULL,
    Ubicacion VARCHAR(100) NULL,
    DispositivoMarcaje VARCHAR(100) NULL,
    CONSTRAINT FK_Asistencia_Empleado FOREIGN KEY (IdEmpleado) 
        REFERENCES Empleado(IdEmpleado),
    CONSTRAINT FK_Asistencia_TipoJornada FOREIGN KEY (IdTipoJornada)
        REFERENCES TipoJornada(IdTipoJornada),
    CONSTRAINT FK_Asistencia_EstadoAsistencia FOREIGN KEY (IdEstadoAsistencia)
        REFERENCES EstadoAsistencia(IdEstadoAsistencia),
    CONSTRAINT FK_Asistencia_TipoOrigenDato FOREIGN KEY (IdTipoOrigenDato)
        REFERENCES TipoOrigenDato(IdTipoOrigenDato)
);

-- TABLAS PARAMÉTRICAS
INSERT INTO EstadoAsistencia (Nombre) VALUES
('Pendiente'),
('Procesado'),
('Error'),
('Rechazado');

INSERT INTO TipoJornada (Nombre) VALUES
('Normal'),
('Con Horas Extras'),
('Licencia Médica'),
('Ausencia Injustificada'),
('Recuperación');

INSERT INTO TipoNomina (Nombre) VALUES
('Semanal'),
('Quincenal'),
('Mensual');

INSERT INTO TipoOrigenDato (Nombre) VALUES
('API'),
('Excel');

-- DATOS DE PRUEBA
INSERT INTO Empresa (RutEmpresa, RazonSocial, IdTipoNomina)
VALUES ('76895432-1', 'Empresa test', 3); -- 3 = Mensual

DECLARE @IdEmpresa INT = SCOPE_IDENTITY();
INSERT INTO Empleado (
    IdEmpresa,
    Rut,
    Nombres,
    Apellidos,
    Email,
    Cargo,
    Departamento,
    FechaIngreso,
    SalarioBase,
    HoraEntrada,
    HoraSalida,
    HorasSemanales,
    Activo
)
VALUES (
    @IdEmpresa,
    '12345678-9',
    'Juan',
    'Pérez',
    'juan.perez@empresa.com',
    'Analista',
    'TI',
    '2022-01-10',
    5000000,
    '08:30:00',
    '17:30:00',
    45,
    1
);

-- Consultas de validación
/*
SELECT * FROM EstadoAsistencia;
SELECT * FROM TipoJornada;
SELECT * FROM TipoNomina;
SELECT * FROM TipoOrigenDato;
SELECT * FROM Empresa;
SELECT * FROM Empleado;
SELECT * FROM Asistencia;
*/
```

## Ejemplos JSON de prueba

### Registrar asistencia individual
**POST /api/asistencia**
```json
{
  "idEmpleado": 1,
  "fecha": "2025-09-24T08:30:00",
  "horaEntrada": "2025-09-24T08:30:00",
  "horaSalida": "2025-09-24T17:30:00",
  "horasTrabajadas": 8,
  "horasExtras": 1,
  "idTipoJornada": 1,
  "idEstadoAsistencia": 1,
  "idTipoOrigenDato": 1,
  "observaciones": "Turno normal",
  "ubicacion": "Oficina Central",
  "dispositivoMarcaje": "Reloj 01"
}
```
### 2. Consultar asistencias por empleado
**GET /api/asistencia/empleado/1?desde=2025-09-20&hasta=2025-09-30**

_No requiere body. Devuelve la lista de asistencias del empleado 1 en el rango de fechas._

---

### 3. Calcular total de horas
**GET /api/asistencia/empleado/1/total-horas?desde=2025-09-20&hasta=2025-09-30**

_No requiere body. Devuelve el total de horas normales y extras para el empleado 1._

---

### 4. Registrar asistencias de forma masiva
**POST /api/asistencia/carga-masiva**
```json
[
  {
    "idEmpleado": 1,
    "fecha": "2025-09-22T08:30:00",
    "horaEntrada": "2025-09-22T08:30:00",
    "horaSalida": "2025-09-22T17:30:00",
    "horasTrabajadas": 8,
    "horasExtras": 0,
    "idTipoJornada": 1,
    "idEstadoAsistencia": 1,
    "idTipoOrigenDato": 2,
    "observaciones": "Carga masiva",
    "ubicacion": "Sucursal Norte",
    "dispositivoMarcaje": "Excel Upload"
  },
  {
    "idEmpleado": 1,
    "fecha": "2025-09-23T08:30:00",
    "horaEntrada": "2025-09-23T08:30:00",
    "horaSalida": "2025-09-23T17:30:00",
    "horasTrabajadas": 7.5,
    "horasExtras": 2,
    "idTipoJornada": 2,
    "idEstadoAsistencia": 1,
    "idTipoOrigenDato": 2,
    "observaciones": "Horas extras",
    "ubicacion": "Sucursal Norte",
    "dispositivoMarcaje": "Excel Upload"
  }
]
'''


