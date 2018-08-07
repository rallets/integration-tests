# integration-tests
Easy and long-term maintainable way to run integration tests through Rest Apis and Json content.

Implement your tests without "json strings", without "copying & pasting code", without weak "javascript code", but instead **do reuse your DLL DTOs/models** and write **only strongly typed C#**.

The code is written in C# and ASP.NET and doesn't require a database.

## Test example
```
public void Clients_get_client_success()
{
    var scenario = new DefaultScenario(_logger, country: ECountry.Norway);
    scenario.PrepareGetClientRequest();

    var api = new StellarTestApi<GetClientRequest, GetClientResponse>(_logger);

    var url = $"{scenario.BaseUrl}v1/clients/{scenario.GetClientRequest.Id}";

    api
        .SetRequest(scenario.GetClientRequest)
        .Execute(api.RestClient.GetAsync<GetClientResponse>, url, scenario.DefaultHeaders)
        .ValidateResponse(IsValidResponse);
}
```
## EXAMPLE project

### Project goals
The goal of this project is to show how to write integration tests that can be:
* easy to write
* hard typed
* no magic strings
* easily randomized
* easily to understand: the test describe the test logic, without technical details
* maintainable in the long term (10 years)
* don't rely on 3rd part tools
* avoid copy-paste
* manageable by testers without strong coding experience

### How-to write integration tests
After implementing hundred of tests, we have found a pattern that can solve all the problems we have encountered and that be easily followed.

**The pattern will help you to write tests to check happy scenarios, bad scenarios, and uncommon scenarios like wrongly handmade json bodies.**

*The system could look awkards from a "developer" point-of-view, but it's perfectly correct from a "tester/maintenability" point-of-view.*

*We suggest to STRONGLY follow the pattern and naming conventions. If you cannot follow these, maybe you have problems in your codebase.*

*A common problem is namespaces/classnames conflict, in this case the integration-tests project should bypass the problem.*

### Project structure
##### The project structure follow the same convention explained in the Core project.

Here there's a new layer, "01-DataContracts", that will contain the DTOs/Models used by the test.

I strongly suggest to use a manual copy of all needed DLLs, so you will not break backwards compatibility without knowledge. Testers will be happy.

Project "Stellar.RestApi.Example" is a REST api example project, using in-memory repository. It has been keept simple by purpose, just to made clearly visible the pattern.

Project "Stellar.RestApi.Example.Models" is a project with all the DTOs/Models you need to test. If you don't have a dedicated library, just import your monolith dll/exe.

### The pattern
* Follow the pattern as shown in the example. It has been found after hundred of tests.
* One endpoint can be tested in many different ways, this can be done easily following the pattern.
* Don't provide parameters for functions/methods in the Scenario.
* Store values globally (requests & responses): this is why you don't need to return values for functions
* Name global variables by same typename 
* One endpoint for test file
* Shared/common filtering is in the constructor of the scenario: 
  - use optional constructur parameters to specify only the logical-parameter that you want to force.
  - you can have hundred optional parameters, but it's not a problem, you need to specify one some of them in your specific test. C# has a good syntax to handle with this.
* Specific filtering must be done in the test using the "prepare" call and then override/manipulate the global request (see Clients_get_client_success)
* If you need to solve a problem that is not shown in a example, ask us
* Do not check assertions inside scenario, only in test. 
* The logger will provide you the callstack of everythink happpend
* Don't write private functions because code is not needed to be reusable, if you are thinking so, then there is something strange (ask to us)
* The DLLs in the "01-DataContracts" folder should be committed, and updated manually: avoid automatic updates or dynamic linking (it's for regression testing)
* If a DTO/Model shared by your DLL doesn't contain all the properties you need (see EditClient -> EditClientRequest) create a local model that extends the original one (EditClientRequestExtended)
* A Scenario should work with all flows with an empty constructor: 
  - the constructor should set default values for everything (or keep as null, it depends on your domain) 
  - all the Prepare methods should take care, using the constructor properties, of generating all the missing values
  - the Prepare methods can contain switch cases to implement all the specific behaviours. You don't need private methods, because the code should not be re-usable (otherwise you are doing something wrong).

### Naming convention
**See examples for a better understanding**

Clients_get_client_success => Context_HttpMethod_type-of-test

Scenarios has RequestName (AddClient), and "Prepare" + RequestName (PrepareAddClient)

Global variables are named as the type.

### How-to write a test
A test should contain:
* create a new Scenario, use constructor's parameters to specify specific behaviours you need in this specific test
* A scenario entry-point, basically it's a step behind your test. It will prepare the global request you need in your REST api call.
* (optional) if you need: override properties in the global request (needed only for bad scenarios or specific tests)
* call you backend and validate your response

### How-to setup a integration-tests project
* Add the DLLs to your DTOs/models, using the DLLs in 01-DataContracts
* Include the DLLs from the Core project (a nuget package is not available yet)
* Write your own Scenarios and Tests

## CORE project

### Project structure
The project's folder structure follow the onion/clean architecture pattern. So the complete folder structure should be:
* 00-Documentation
* 00-Tools: externat tools needed to build/run the project
* 01-Core: Basic types, extension methods, functionality that can be shared with all layers, like a logger.
* 02-Domain: Data types related to the "domain", like exceptions, events, domain interfaces, and model interfaces.
* 03-Service interfaces: Interfaces that will be implemented by the infrastructure or services.
* 04-Application Services: Services that use one or more infrastructure services to provide a domain logic, implement only interfaces.
* 05-Infrastructure Services: Services that implement a specific feature. Here there will be communications with the database or REST Apis, with microservices or third-party services, usually towards REST Api.
* 06-Clients/Frontend: Contains SPA applications or clients that communicate on the bus (like RabbitMQ) that will then use the lower layers for the business / domain logic.
* 07-Tools: Internal tools, therefore not publicly available, as is the layer 06

It means that code in level 03 (Service interfaces) connot use code in a upper level, like 04 (Application services).

This project is the core to create your own integration tests. See examples for detailed informations.

### Needed improvements
* create a nuget package, so in a test project the source code (and the dll's hard link) are not needed anymore
* implement message-bus core functionality
* improve RestClient to support xml, etc

## References
[How to implement integration tests/e2e](https://www.stellarsolutions.it/come-implementare-integration-tests-end-to-end/)

[How to write a modern enterprise application - Frontend and backend](https://www.stellarsolutions.it/come-realizzare-una-applicazione-enterprise-moderna-backend-e-frontend/)
