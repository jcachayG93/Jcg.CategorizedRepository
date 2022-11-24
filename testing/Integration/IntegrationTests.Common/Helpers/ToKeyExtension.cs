using Common.Api;

namespace IntegrationTests.Common.Helpers
{
    public static class ToKeyExtension
    {
        public static RepositoryIdentity ToKey(this Guid id)
        {
            return new(id);
        }
    }
}