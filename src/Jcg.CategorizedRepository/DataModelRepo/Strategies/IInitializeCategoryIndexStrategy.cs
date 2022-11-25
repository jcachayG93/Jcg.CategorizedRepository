namespace Jcg.CategorizedRepository.DataModelRepo.Strategies;

internal interface IInitializeCategoryIndexStrategy
{
    Task InitializeCategoryIndexes(CancellationToken cancellationToken);
}