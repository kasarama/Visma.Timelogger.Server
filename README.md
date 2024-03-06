# Server solution for Visma e-conomic hiring task

User Stories:
1. As a freelancer I want to be able to register how I spend time on my _projects_, so that I can provide my _customers_ with an overview of my work.
2. As a freelancer I want to be able to get an _overview of my time registrations per project_, so that I can create correct invoices for my customers.
3. As a freelancer I want to be able to _sort my projects by their deadline_, so that I can prioritise my work.

Individual time registrations should be 30 minutes or longer, and once a project is complete it can no longer receive new registrations. You do not need to create an actual invoice.

## Considerations

The test code coverage report can be viewed by opening index.html in the 'testcoverageresult' folder.

This solution is implemented by following patterns such as:
 - Clean Architecture - consists of a domain layer, an application layer, an infrastructure layer, and an API layer. The domain and application layers are at the centre of the project (the system core). The core is independent of the infrastructure. It uses the interfaces and abstractions in the core system, but implements them outside the core system
 - MVC - Model represents application logic, View – describes how to display some part of the model within the user interface, Controller – accepts input from the user and responds to his actions. MVC allows developers to work on different aspects of the application independently, making it easier to manage and extend the software over time.
 - Repository - provides a centralized way to access and manipulate data from a data store, encapsulates the logic required to access the data source
 - Service - provides a way to group related operations and behaviour, promotes code reusability

User input is validated with FluentValidation, and sanitized with System.Web.HttpUtility.HtmlEncode method.

Based on the presented User Stories, at least two subdomains can be distinguished: Project Management and Invoice Management. That leads to the choice of microservice architecture for the project. It is scalable and independently deployable as each service  has its source code repository. Each element can be extended or replaced without affecting other services. Also, failures in one microservice do not necessarily affect the entire system. Microservices can use an event-driven architecture, enabling asynchronous communication and decoupling between services. However, data consistency becomes challenging.


## Development

To run this project you will need both .NET Core v7.0 installed on your environment.

Server - `dotnet restore` - to restore Nuget packages, `dotnet build` - to build the solution, `cd Visma.Timelogger.Api && dotnet run` - starts a server on http://localhost:5105. You can download Visual Studio Code. The project was tested on Windows 10.

The server solution contains an API with both: the basic Entity Framework in-memory context that acts as a database and the SqlServer. You can switch between them in [PersistenceServiceRegistration](https://github.com/kasarama/Visma.Timelogger.Server/blob/b8209677d9e75aca6e7db0700b1ff3e447458d99/Visma.Timelogger.Infrastructure/PersistenceServiceRegistration.cs#L13).

To run SqlServer run `docker-compose up -d`. It will start SqlServer at port 14433 and RabbitMQ on its default ports (15672).

On the branch 'feature_eventbus' is added the implementation of Publishing to the broker. You may or may not start the RabbitMQ service - the service's unavailability is handled.

The collection of valid HTTP requests is in this [json file](https://github.com/kasarama/Visma.Timelogger.Server/blob/dev/Visma.TimeLogger.postman_collection.json). Although the test data is generated randomly at application start, so check available projects for a chosen freelancer with the `Project/AllProjects` request
To mimic the authorization process a [middleware](Visma.Timelogger.Api/Middleware/AuthorizationMiddleware.cs) checks the `User`header. The valid User header values are :
 - `freelancer1`
 - `freelancer2`
 - `customer2`
 - `customer1`
   
