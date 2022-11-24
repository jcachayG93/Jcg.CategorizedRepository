
# Nodes for Developers

## Project Dependencies
The main motivation for these projects structure is to hide types from the client while exposing the API. This would be easier using Dependency Injection but 
I wanted to avoid this library depending on a specific DI Framework.

### Common.Api and Common 
These two could have been merged into a single project, but, I sepparated because I wanted to set the Api namespace to match **Jcg.Repositories.Api** and
keep it fixed. So, I set the project root namespace to avoid changing it by accident.

### Support projects
There are 3, they implement the library features:
- Support.UnitOfWork
- Support.DataModelRepository
- Support.CategorizedRepository

This projects depend on the Common projects only. They know about each other only thru abstractions that reside in Common. 

### CategorizedRepository.Factories
This project has a root namespace to match the Common.Api project namespace: **Jcg.Repositories.Api**

But, the factory in this project could not be placed in common because it needs to access the support projects. 

### Jcg.Repositories
There is nothing here, but:
1. Contains the package settings.
2. Depends on everything else.

This is the project that will be packed as the entry point into a nuget package

***Thought about placing the CategorizedRepository Factory*** here, but, that would make hiding internal types from other projects impossible.
