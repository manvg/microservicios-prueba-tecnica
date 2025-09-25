# Prueba Técnica Nubox

## Contexto
La empresa cuenta con un sistema que calcula sueldos de trabajadores, el cual recibe la nómina y la información básica necesaria para generar las liquidaciones.  
Actualmente buscan un proveedor para el control de asistencia que entregue dispositivos físicos de marcaje y permita integrar la información con el sistema de sueldos.  
La solución esperada debe facilitar el intercambio de datos entre ambos sistemas de manera “segura, escalable, observable, trazable y resiliente”.

## Propuesta
Se planteó una arquitectura basada en microservicios, separando responsabilidades de forma clara:

- Microservicio de Integración de Asistencias: encargado de registrar las asistencias, tanto de manera individual como masiva (JSON o Excel). Valida la existencia de empleados, evita duplicados y persiste los datos en base de datos. Además, expone consultas para obtener asistencias por empleado y calcular el total de horas normales y extras.
- Microservicio de Cálculo de Liquidaciones: encargado de recibir las horas consolidadas y aplicar reglas de negocio sobre valor hora, horas extras y descuentos, devolviendo como resultado la liquidación del trabajador.

## Estado actual
El microservicio de integración de asistencias se encuentra implementado con:

- Registro individual de asistencias.
- Consultas por empleado en un rango de fechas.
- Cálculo de totales de horas trabajadas y extras.
- Carga masiva desde JSON.
- Carga masiva desde archivos Excel, procesando los registros por día y descartando duplicados. (pendiente de implementar).

El microservicio de cálculo de liquidaciones queda pendiente de implementación.

## Siguientes pasos
Si me otorgan más tiempo, quiero avanzar en:

- Implementar el microservicio de cálculo de liquidaciones.
- Agregar RabbitMQ para simular un esquema de producción con dos microservicios comunicándose por eventos.
- Agregar seguridad con autenticación y JWT.
- Test unitarios.
- Preparar un docker-compose que levante: base de datos SQL Server, RabbitMQ y los microservicios.
- Documentar con diagramas y ejemplos de flujo de eventos con el fin de apoyar la presentación de la solución.

Considero que con esta arquitectura se cumple con un MVP, ya que se aplican principios de arquitectura limpia y SOLID, separando bien las responsabilidades y permitiendo escalar a futuro con el uso eventos con RabbitMQ.

# Prueba Técnica Nubox

## Contexto
La empresa cuenta con un sistema que calcula sueldos de trabajadores, el cual recibe la nómina y la información básica necesaria para generar las liquidaciones.  
Actualmente buscan un proveedor para el control de asistencia que entregue dispositivos físicos de marcaje y permita integrar la información con el sistema de sueldos.  
La solución esperada debe facilitar el intercambio de datos entre ambos sistemas de manera “segura, escalable, observable, trazable y resiliente”.

## Propuesta
Se planteó una arquitectura basada en microservicios, separando responsabilidades de forma clara:

- Microservicio de Integración de Asistencias: encargado de registrar las asistencias, tanto de manera individual como masiva (JSON o Excel). Valida la existencia de empleados, evita duplicados y persiste los datos en base de datos. Además, expone consultas para obtener asistencias por empleado y calcular el total de horas normales y extras.
- Microservicio de Cálculo de Liquidaciones: encargado de recibir las horas consolidadas y aplicar reglas de negocio sobre valor hora, horas extras y descuentos, devolviendo como resultado la liquidación del trabajador.

## Estado actual
El microservicio de integración de asistencias se encuentra implementado con:

- Registro individual de asistencias.
- Consultas por empleado en un rango de fechas.
- Cálculo de totales de horas trabajadas y extras.
- Carga masiva desde JSON.
- Carga masiva desde archivos Excel, procesando los registros por día y descartando duplicados. (pendiente de implementar).

El microservicio de cálculo de liquidaciones queda pendiente de implementación.

## Siguientes pasos
Si me otorgan más tiempo, quiero avanzar en:

- Implementar el microservicio de cálculo de liquidaciones.
- Agregar RabbitMQ para simular un esquema de producción con dos microservicios comunicándose por eventos.
- Agregar seguridad con autenticación y JWT.
- Test unitarios.
- Preparar un docker-compose que levante: base de datos SQL Server, RabbitMQ y los microservicios.
- Documentar con diagramas y ejemplos de flujo de eventos con el fin de apoyar la presentación de la solución.

Considero que con esta arquitectura se cumple con un MVP, ya que se aplican principios de arquitectura limpia y SOLID, separando bien las responsabilidades y permitiendo escalar a futuro con el uso eventos con RabbitMQ.

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
