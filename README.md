# Jcg.Repositories

***Data access repositories that plug between the database and the client app, seamlessly providing advanced features.***

It sits between a database client and the app to automate these features:
- Soft delete and restore.
- Mapping between segregated models for the database and the client so they can evolve independently.
- Integrated index table model for deleted and non-deleted entities for efficient queries.
- Unit of work pattern.
- Optimistic concurrency.

The client interacts with a simple repository API.

The library interacts with the database via a database client, so it does not know anything about the underlying database technology.

## Database requirements
The database must support **transactions.**
## Motivation
I needed to abstract the features above into a library, so I could use it for microservice applications (implementing all those features for each microservice would be tedious and prone to error)

## Dependencies
Net 7.0

## Getting started
1. Read the [integration tests](https://github.com/jcachayG93/Jcg.DataAccessRepositories/tree/main/testing/Integration)
2. Read the [Wiki](https://github.com/jcachayG93/Jcg.DataAccessRepositories/wiki)

