using Jcg.CategorizedRepository.CategorizedRepo.Support;
using Testing.Common.Support.Assertions;
using Testing.Common.Support.Extensions;
using Testing.CommonV2.Mocks;
using Testing.CommonV2.Types;

namespace Jcg.CategorizedRepository.UnitTests.CategorizedRepo.Support
{
    public class LookupMapperAdapterTests
    {
        public LookupMapperAdapterTests()
        {
            Adaptee = new();

            Sut = new(Adaptee.Object);
        }
        private LookupMapperMock Adaptee { get; }

        private LookupMapperAdapter<LookupDatabaseModel, Lookup> Sut { get; }


        [Fact]
        public void Adapts()
        {
            // ************ ARRANGE ************

            var input = CreateCategoryIndex(out var input1, out var input2);

            Adaptee.Setup(input1, out var output1);
            Adaptee.Setup(input2, out var output2);

            // ************ ACT ****************

            var result = Sut.Map(input);

            // ************ ASSERT *************

            result.ShouldBeEquivalent(output1.ToCollection(output2),(x,y)=>
                x.Equals(y));

            
        }
    }
}
