* Project goals *
The goal of this project is to show how to write integration tests that can be:
- easy to write
- hard typed
- no magic strings
- easily randomized
- easily to understand: the test describe the test logic, without technical details
- maintainable in the long term (10 years)
- don't rely on 3rd part tools
- avoid copy-paste
- managed by testers

* How to write integration tests *
We have found a pattern that can solve all the problems we have encountered and that be easily followed. 
The  pattern will help to write tests to check happy scenarios, bad scenarios, and uncommon scenarios like wrongly handmade json bodies.
The system could look awkards from a "developer" point-of-view, but it's perfectly correct from a "tester/maintenability" point-of-view.
We suggest to STRONGLY follow the pattern and naming cobventions. If you cannot follow these, maybe you have problems in your codebase.
A common problem is namespaces/classnames conflict, in this case the integration-tests project should bypass the problem.

* Project structure *
The project structure follow the same convention explain in the Core project.

Here there's a new layer, "01-DataContracts", that will contain the DTOs/Models used by the test.
I strongly suggest to use a manual copy of all needed DLLs, so you will not break backwards compatibility without knowledge. Testers will be happy.

Project "Stellar.RestApi.Example" is a REST api example project, using in-memory repository. It has been keept simple by purpose, just to made clearly visible the pattern.
Project "Stellar.RestApi.Example.Models" is a project with all the DTOs/Models you need to test. If you don't have a dedicated library, just import your monolith dll/exe.

* The pattern *
- Follow the pattern as shown in the example. It has been found after hundred of tests.
- One endpoint can be tested in many different ways, this can be done easily following the pattern.
- Don't provide parameters for functions/methods in the Scenario.
- Store values globally (requests & responses): this is why you don't need to return values for functions
- Name global variables by same typename 
- One endpoint for test file
- Shared/common filtering is in the constructor of the scenario: 
  - use optional constructur parameters to specify only the logical-parameter that you want to force.
  - you can have hundred optional parameters, but it's not a problem, you need to specify one some of them in your specific test. C# has a good syntax to handle with this.
- Specific filtering must be done in the test using the "prepare" call and then override/manipulate the global request (see Clients_get_client_success)
- If you need to solve a problem that is not shown in a example, ask us
- Do not check assertions inside scenario, only in test. 
- The logger will provide you the callstack of everythink happpend
- Don't write private functions because code is not needed to be reusable, if you are thinking so, then there is something strange (ask to us)
- The DLLs in the "01-DataContracts" folder should be committed, and updated manually: avoid automatic updates or dynamic linking (it's for regression testing)
- If a DTO/Model shared by your DLL doesn't contain all the properties you need (see EditClient -> EditClientRequest) create a local model that extends the original one (EditClientRequestExtended)
- A Scenario should work with all flows with an empty constructor: 
  - the constructor should set default values for everything (or keep as null, it depends on your domain) 
  - all the Prepare methods should take care, using the constructor properties, of generating all the missing values
  - the Prepare methods can contain switch cases to implement all the specific behaviours. You don't need private methods, because the code should not be re-usable (otherwise you are doing something wrong).

* Naming convention *
Clients_get_client_success => Context_HttpMethod_type-of-test
Scenarios has RequestName (AddClient), and "Prepare" + RequestName (PrepareAddClient)
Global variables is named as the type

* Writing a test *
A test should contain:
- create a new Scenario, use constructor's parameters to specify specific behaviours you need in this specific test
- A scenario entry-point, basically it's a step behind your test. It will prepare the global request you need in your REST api call.
- (optional) if you need: override properties in the global request (needed only for bad scenarios or specific tests)
- call you backend and validate your response

Reference: 
https://www.stellarsolutions.it/come-implementare-integration-tests-end-to-end/

* How setup a integration-tests project *
- Add the DLLs to your DTOs/models, using the DLLs in 01-DataContracts
- Include the DLLs from the Core project (a nuget package is not available yet)
- Write your own Scenarios and Tests
