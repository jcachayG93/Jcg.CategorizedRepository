using IntegrationTests.Common.Types;

namespace IntegrationTests.Common.Database;

public record UpsertOperation(string Key, string ETag, IClone Payload);