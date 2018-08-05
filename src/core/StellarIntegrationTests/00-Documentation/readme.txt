* Project structure *
The project's folder structure follow the onion/clean architecture pattern. So the complete folder structure should be:
00-Documentation
00-Tools: externat tools needed to build/run the project
01-Core: Basic types, extension methods, functionality that can be shared with all layers, like a logger.
02-Domain: Data types related to the "domain", like exceptions, events, domain interfaces, and model interfaces.
03-Service interfaces: Interfaces that will be implemented by the infrastructure or services.
04-Application Services: Services that use one or more infrastructure services to provide a domain logic, implement only interfaces.
05-Infrastructure Services: Services that implement a specific feature. Here there will be communications with the database or REST Apis, with microservices or third-party services, usually towards REST Api.
06-Clients/Frontend: Contains SPA applications or clients that communicate on the bus (like RabbitMQ) that will then use the lower layers for the business / domain logic.
07-Tools: Internal tools, therefore not publicly available, as is the layer 06

It means that code in level 03 (Service interfaces) connot use code in a upper level, like 04 (Application services).

This project is the core to create your own integration tests. See examples for detailed informations.

* Needed improvements  *
- create a nuget package, so in a test project the source code (and the dll's hard link) are not needed anymore
- implement message-bus core functionality
- improve RestClient to support xml, etc

Reference: 
https://www.stellarsolutions.it/come-implementare-integration-tests-end-to-end/
https://www.stellarsolutions.it/come-realizzare-una-applicazione-enterprise-moderna-backend-e-frontend/
