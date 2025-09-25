Prueba Técnica Nubox

-Contexto
La empresa cuenta con un sistema que calcula sueldos de trabajadores, el cual recibe la nómina y la información básica necesaria para generar las liquidaciones.
Actualmente buscan un proveedor para el control de asistencia que entregue dispositivos físicos de marcaje y permita integrar la información con el sistema de sueldos.
La solución esperada debe facilitar el intercambio de datos entre ambos sistemas de manera “segura, escalable, observable, trazable y resiliente”.

-Propuesta
Se planteó una arquitectura basada en microservicios, separando responsabilidades de forma clara:
  •	Microservicio de Integración de Asistencias: encargado de registrar las asistencias, tanto de manera individual como masiva (JSON o Excel). Valida la existencia de empleados, evita 
    duplicados y persiste los datos en base de datos. Además, expone consultas para obtener asistencias por empleado y calcular el total de horas normales y extras.
  •	Microservicio de Cálculo de Liquidaciones: encargado de recibir las horas consolidadas y aplicar reglas de negocio sobre valor hora, horas extras y descuentos, devolviendo como 
    resultado la liquidación del trabajador.

-Estado actual
El microservicio de integración de asistencias se encuentra implementado con:
  •	Registro individual de asistencias.
  •	Consultas por empleado en un rango de fechas.
  •	Cálculo de totales de horas trabajadas y extras.
  •	Carga masiva desde JSON.
  •	Carga masiva desde archivos Excel, procesando los registros por día y descartando duplicados. (pendiente de implementar).
  
El microservicio de cálculo de liquidaciones queda pendiente de implementación.

Siguientes pasos
Si me otorgan más tiempo, quiero avanzar en:
  •	Implementar el microservicio de cálculo de liquidaciones.
  •	Agregar RabbitMQ para simular un esquema de producción con dos microservicios comunicándose por eventos.
  •	Agregar seguridad con autenticación y JWT.
  •	Test unitarios.
  •	Preparar un docker-compose que levante: base de datos SQL Server, RabbitMQ y los microservicios.
  •	Documentar con diagramas y ejemplos de flujo de eventos con el fin de apoyar la presentación de la solución.

Considero que con esta arquitectura se cumple con un MVP, ya que se aplican principios de arquitectura limpia y SOLID, separando bien las responsabilidades y 
permitiendo escalar a futuro con el uso eventos con RabbitMQ.

