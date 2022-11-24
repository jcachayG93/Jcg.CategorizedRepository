# Jcg.Repositories

A repository of entities that belong to a category.

It sits between a database client and the app to automate these features:
- Soft delete and restore.
- Mapping between segregated models for the database and the client so they can evolve sepparately.
- Integrated index table model for deleted and for non-deleted entities for efficient queries.
- Unit of work pattern.
- Optimistic concurrency.

## Advantages:
- Database agnostic.
- Simple api.
- Per request unit of work.

## Motivation
I needed to abstract the features above into a library, so I can use it for a microservices application (implementing all those features for each microservice would be tedious)

## Dependencies
Net 7.0

## Getting started
1. Read the integration tests

