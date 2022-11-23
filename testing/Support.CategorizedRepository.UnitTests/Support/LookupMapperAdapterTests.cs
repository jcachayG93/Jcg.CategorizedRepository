using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Support.CategorizedRepository.Support;
using Testing.Common.Assertions;
using Testing.Common.Extensions;
using Testing.Common.Mocks;
using Testing.Common.Types;

namespace Support.CategorizedRepository.UnitTests.Support
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
        public async Task Adapts()
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
