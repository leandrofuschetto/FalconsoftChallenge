WebAPI Orders challenge

Table of contents
Como ejecutarlo
Tecnologías
Endpoints
DB

Como ejecutarlo
La API fue desarrollada en Visual Studio 2022, solamente hay que correr el proyecto en el IDE.
Para la DB, en la primer corrida de la aplicacion, la BD es creada y valores iniciales son insertados en las tablas. Correr sobre (localdb)\MSSQLLocalDB

Technlogies
C#
NET 8
EF Core
SQL Express
Linq

Endpoints
Al correr la WebAPI se abre swagger en donde se pueden visualizar los siguientes endpoints:

POST /api/v1/authentication: Endpoint para generar el JWT y autenticarse en la app. (User: leandrof; Pass: lean1234)

GET /api/v1/orders: Endpoint que retorna la lista de todas las Orders, paginada por cursor.. devuelve de a 10 registros.
GET /api/v1/orders/{id}: Obtener una Orden por Id.
PATCH /api/v1/orders/{id}: Endpoint para actualizar el Status de una Order.
PATCH /api/v1/orders/{id}/orderitems/{itemId}: Endpoint para actualizar la "Quantity" de un item de la orden.


DB Entities
Users
Clients
Orders
OrderItems
Products  
