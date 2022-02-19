# dotnetapi
DDD API in .NET Core

A simple .NET Core API following the Domain Driven Design Pattern

Dapper is used as ORM.

The infrastructure includes the find, create, update, and delete abstract methods for each model in database. These methods can also be overridden when specific functionality is needed.

This project consists of one model, the upload model, which represents a file.

The infrastructure assumes a connection to Postgres but can be easily modified.

This project can be easily scaled to meet any needs by following the same logic for each model in our Domain.

Swagger is used for documentation. The documentation is produced automatically by following the logic in Uploads controller. Controllers can also be exluded from swagger.

Custom error codes are included and can be extended in Config/CustomErrorCodes.cs
