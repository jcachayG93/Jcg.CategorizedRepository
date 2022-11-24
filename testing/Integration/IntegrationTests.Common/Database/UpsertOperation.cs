namespace IntegrationTests.Common.Database;

public record UpsertOperation(string Key, string ETag, object Payload);