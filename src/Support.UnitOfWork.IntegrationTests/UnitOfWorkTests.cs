using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Support.UnitOfWork.Api.Exceptions;
using Testing.Common.Assertions;

namespace Support.UnitOfWork.IntegrationTests
{
    public class UnitOfWorkTests : TestBase
    {
        [Fact]
        public async Task GetNonDeletedItemsCategoryIndex_NoCategoryIndex_Throws()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var fun = new Func<Task>(async () =>
            {
                await Sut.GetNonDeletedItemsCategoryIndex(CancellationToken.None);
            });

            // ************ ASSERT *************

            await fun.Should().ThrowAsync<CategoryIndexIsUninitializedException>();

        }

        [Fact]
        public async Task GetDeletedItemsCategoryIndex_NoCategoryIndex_Throws()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var fun = new Func<Task>(async () =>
            {
                await Sut.GetDeletedItemsCategoryIndex(CancellationToken.None);
            });

            // ************ ASSERT *************

            await fun.Should().ThrowAsync<CategoryIndexIsUninitializedException>();

        }

        [Fact]
        public async Task DeletedCategoryIndex_Upsert_Then_Get()
        {
            // ************ ARRANGE ************

            var index = CreateCategoryIndex(3);

            // ************ ACT ****************

            await Sut.UpsertDeletedItemsCategoryIndex(index, CancellationToken.None);


            var result = await Sut.GetDeletedItemsCategoryIndex(CancellationToken.None);

            // ************ ASSERT *************

            result.Lookups.ShouldBeEquivalent(index.Lookups,(x,y)=>x.SomeValue == y.SomeValue);
        }
    }
}
