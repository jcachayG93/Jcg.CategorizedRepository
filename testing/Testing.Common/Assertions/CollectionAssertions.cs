namespace Testing.Common.Assertions
{
    public static class CollectionAssertions
    {
        public static void ShouldBeEquivalent<T1, T2>(
            this IEnumerable<T1> col1, IEnumerable<T2> col2,
            Func<T1, T2, bool> criteria)
        {
            if (!col1.Any())
            {
                Assert.Fail("Collection 1 is empty");
            }

            if (col1.Count() != col2.Count())
            {
                Assert.Fail(
                    "Both collections must have the same number of elements");
            }

            var caseA = col1.All(x =>
                col2.Any(y => criteria(x, y)));

            if (!caseA)
            {
                Assert.Fail(
                    "Every item in col 1 must have one equivalent in collection 2");
            }

            var caseB = col2.All(y =>
                col1.Any(x => criteria(x, y)));

            if (!caseB)
            {
                Assert.Fail(
                    "Every item in col2 must have one equivalent in collection 1");
            }
        }
    }
}