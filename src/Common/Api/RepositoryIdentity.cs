using Common.Api.Exceptions;

namespace Common.Api;

public record RepositoryIdentity
{
    public RepositoryIdentity(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new RepositoryIdentityValueCantBeEmptyException();
        }

        Value = value;
    }

    public Guid Value { get; }
}