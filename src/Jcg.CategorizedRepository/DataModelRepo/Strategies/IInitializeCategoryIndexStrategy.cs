namespace Support.DataModelRepository.Strategies;

internal interface IInitializeCategoryIndexStrategy
{
    Task InitializeCategoryIndexes(CancellationToken cancellationToken);
}