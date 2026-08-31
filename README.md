# Aplicaciones Distribuidas - Microservicios con RabbitMQ y API Gateway
**Autor:** Steveen Carabalí

Tecnologías:
* ASP.NET Core Web API
* RabbitMQ
* Docker
* Docker Compose
* API Gateway con YARP
* SQL Server
* Swagger

## Estructura del proyecto

```text
/
├── Api_Gateway_Tarea/
├── Categoria_Tarea_A/
├── Vehiculo_Tarea_A/
├── BaseDatos/
├── docker-compose.yml
├── .gitignore
└── README.md
 ```
## Base de datos

Los scripts necesarios para crear las bases de datos se encuentran en:

*BaseDatos/

	Ejecutar primero los archivos .sql en SQL Server.

	Los scripts incluyen:

	Creación de la base de datos.

	Creación del usuario.

	Permisos de lectura y escritura.

	Creación de tablas.

	Datos necesarios para las pruebas.
	
## Ejecutar el proyecto

Abrir Docker Desktop.

*Luego abrir una terminal en la carpeta donde se encuentra 'docker-compose.yml' y ejecutar:

	docker compose up --build

*Verificar contenedores

	docker ps
	
## Probar los servicios

Una vez que los contenedores estén ejecutándose, se pueden probar los siguientes servicios y puertos:

	API Gateway - Puerto 5000: http://localhost:5000  

	API CATEGORIA - Puerto 5000: http://localhost:5000/api/Categoria		

	API VEHICULO - Puerto 5000: http://localhost:5000/api/Vehiculo

(Las peticiones principales hacia los microservicios pueden realizarse mediante el API Gateway.)


## Microservicio Categoría - Puerto 5001

	Microservicios Api Categoría - Puerto 5001: http://localhost:5001/api/Categoria

	Microservicios Categoría Swagger - Puerto 5001: http://localhost:5001/swagger
	
## Microservicio Vehículo - Puerto 5002
 
	Microservicios Api Vehículo - Puerto 5001: http://localhost:5002/api/Vehiculo

	Microservicios Vehículo Swagger - Puerto 5001: http://localhost:5002/swagger
	
## RabbitMQ

La interfaz de administración de RabbitMQ se puede consultar en: http://localhost:15672

	Las credenciales de RabbitMQ se encuentran configuradas en docker-compose.yml.

	Desde la interfaz de RabbitMQ se pueden verificar:

	Colas creadas.

	Cantidad de mensajes.

	Mensajes pendientes.

	Consumidores conectados.

	Conexiones y canales.
	
## Detener el proyecto

docker compose down

## Funcionamiento general

Cliente
   ↓
API Gateway :5000
   ↓
Microservicio Categoría :5001
   ↓
RabbitMQ
   ↓
Microservicio Vehículo :5002
   ↓
SQL Server