using Common.Api.Exceptions;

namespace Common.Api;

/// <summary>
///     Represents a Key
/// </summary>
public record RepositoryIdentity
{
    /// <summary>
    ///     reates the key
    /// </summary>
    /// <param name="value">They underlying value</param>
    /// <exception cref="RepositoryIdentityValueCantBeEmptyException">Thown if the value is empty/exception>
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