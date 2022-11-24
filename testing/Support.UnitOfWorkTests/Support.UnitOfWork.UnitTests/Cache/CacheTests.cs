using Common.Api;
using FluentAssertions;
using Moq;
using Support.UnitOfWork.Cache.Imp;
using Support.UnitOfWork.InternalExceptions;
using Testing.Common.Support.Assertions;

namespace Support.UnitOfWork.UnitTests.Cache
{
    public class CacheTests
    {
        public CacheTests()
        {
            Key = RandomString();

            Data = RandomData();

            Sut = new();
        }

        private string Key { get; }

        private IETagDto<Payload> Data { get; }

        private Cache<Payload> Sut { get; }

        private IETagDto<Payload> RandomData()
        {
            return RandomData(RandomString());
        }

        private IETagDto<Payload> RandomData(string eTag)
        {
            return Mock.Of<IETagDto<Payload>>(t =>
                t.Etag == eTag &&
                t.Payload == RandomPayload());
        }

        private IEnumerable<TestUpsertData> RandomDataToAdd(int count)
        {
            return Enumerable.Range(0, count)
                .Select(i => new TestUpsertData(RandomString(), RandomData()))
                .ToList();
        }

        private Payload RandomPayload()
        {
            return new();
        }


        [Fact]
        public void HasKey_TrueIfAddHasBeenCalledForKey()
        {
            // ************ ARRANGE ************

            Sut.Add(Key, Data);

            // ************ ACT ****************

            var result = Sut.HasKey(Key);

            // ************ ASSERT *************

            result.Should().BeTrue();
        }


        [Fact]
        public void Get_AddWasntCalledBeforeForKey_Throws()
        {
            // ************ ARRANGE ************

            // ************ ACT ****************

            var act = () => { Sut.Get(RandomString()); };

            // ************ ASSERT *************

            act.Should().Throw<CacheException>();
        }


        [Fact]
        public void Get_AddWasCalledBefore_ReturnsPayload()
        {
            // ************ ARRANGE ************

            Sut.Add(Key, Data);

            // ************ ACT ****************

            var result = Sut.Get(Key);

            // ************ ASSERT *************

            result.Should().Be(Data.Payload);
        }


        [Fact]
        public void Add_KeyAlreadyExists_Throws()
        {
            // ************ ARRANGE ************

            Sut.Add(Key, RandomData());

            // ************ ACT ****************

            var act = () => { Sut.Add(Key, RandomData()); };

            // ************ ASSERT *************

            act.Should().Throw<CacheException>();
        }


        [Fact]
        public void Upsert_ReplacesPayload()
        {
            // ************ ARRANGE ************

            Sut.Add(Key, RandomData());

            var updatedPayload = RandomPayload();

            // ************ ACT ****************

            Sut.Upsert(Key, updatedPayload);

            // ************ ASSERT *************

            Sut.Get(Key).Should().Be(updatedPayload);
        }


        [Fact]
        public void
            GetUpsertedItems_ReturnItemsThatWereUpserted_WithOriginalKeyAndETagButUpdatedPayload()
        {
            // ************ ARRANGE ************

            var dataToAddThenUpsert = RandomDataToAdd(1).ToList();

            foreach (var d in dataToAddThenUpsert)
            {
                Sut.Add(d.Key, RandomData(d.Data.Etag));
            }

            // ************ ACT ****************

            foreach (var d in dataToAddThenUpsert)
            {
                Sut.Upsert(d.Key, d.Data.Payload);
            }

            // ************ ASSERT *************

            var result = Sut.UpsertedItems.ToList();

            var a = dataToAddThenUpsert.First();
            var b = result.First();

            var x = a.Key == b.Key;
            var y = a.Data.Etag == b.ETag;


            Sut.UpsertedItems.ShouldBeEquivalent(dataToAddThenUpsert, (x, y) =>
                x.Key == y.Key &&
                x.ETag == y.Data.Etag &&
                x.PayLoad.Equals(y.Data.Payload));
        }


        [Fact]
        public void Upsert_NoMatchingKeyInCache_ETagIsNull()
        {
            // ************ ARRANGE ************

            var payload = RandomPayload();

            // ************ ACT ****************

            Sut.Upsert(Key, payload);

            var result = Sut.UpsertedItems.First();

            // ************ ASSERT *************

            result.Key.Should().Be(Key);
        }


        public class Payload
        {
        }

        private record TestUpsertData(string Key, IETagDto<Payload> Data);
    }
}