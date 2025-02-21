# NSerio.SalesDatePrediction.WebApi

La carpeta contiene:

- API
[
 - Front [Angular]
 - D3Task [HTML, CSS, JS]
]

## Tecnologías Backend
- .NET 8
- SQL Server
- Entity Framework Core
- Dapper
- Swagger

## Tecnologías Frontend
- Angular 17
- Angular Material
- TypeScript
- CSS
- HTML

## Estructura del Proyecto

### Backend
```
NSerio.SalesDatePrediction.WebApi/
├── Controllers/
│   ├── SalesDatePredictionController.cs
│   ├── OrdersController.cs
│   ├── EmployeesController.cs
│   ├── ShippersController.cs
│   └── ProductsController.cs
├── Core/
│   ├── Domain/
│   │   └── Entities/
│   ├── DTOs/
│   └── Interfaces/
└── Infrastructure/
├── Data/
├── Repositories/
└── Helpers/
```
### Frontend
```
sales-date-prediction/
├── src/
│   ├── app/
│   │   ├── components/
│   │   │   ├── sales-prediction/
│   │   │   ├── view-orders-modal/
│   │   │   └── new-order-modal/
│   │   ├── services/
│   │   └── models/
│   └── assets/
```
## Instalación

1. Clonar el repositorio
```bash
Backend
cd NSerio.SalesDatePrediction.WebApi
dotnet restore
dotnet run
```
```bash
Frontend
cd sales-date-prediction
npm install
ng serve
```
URLs

API: http://localhost:5163
Swagger: http://localhost:5163/swagger
Frontend: http://localhost:4200

Funcionalidades
API Endpoints

GET /api/SalesDatePrediction
GET /api/Orders/customer/{customerId}
POST /api/Orders
GET /api/Employees
GET /api/Products
GET /api/Shippers

Frontend

Vista principal de predicciones
Vista modal de órdenes por cliente
Formulario modal para nueva orden
Búsqueda y paginación

