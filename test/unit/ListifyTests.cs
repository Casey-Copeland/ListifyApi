using ListifyApi.Entities;
using System;
using System.Linq;
using Xunit;

namespace ListifyApi.UnitTests
{
    public class ListifyTests
    {
        [Fact]
        public void ConstructorThrowsArgumentOutOfRange()
        {
            //Given the following values
            var minValue = 5;
            var maxValue = 1;

            //When the constructor is called with those values
            var lazyListify = new Lazy<Listify>(() => new Listify(minValue, maxValue));

            //Then the constructor should throw an ArgumentOutOfRangeException
            Assert.Throws<ArgumentOutOfRangeException>(() => lazyListify.Value);
        }

        [Fact]
        public void IndexValueMatchesExpectedValue()
        {
            //Given the following values
            var minValue = 10;
            var maxValue = 100;
            var index = 10;
            var list = new Listify(minValue, maxValue);

            //When I access the index position
            var value = list[index];

            //Then I should get back the value of the index position
            Assert.Equal(minValue + index, value);
        }

        [Fact]
        public void IndexValueThrowsIndexOutOfRangeException()
        {
            //Given the following values
            var minValue = 10;
            var maxValue = 100;
            var index = 2000;
            var list = new Listify(minValue, maxValue);

            //When I access the index position
            var lazyListifyIndex = new Lazy<Listify>(list);

            //Then the index call should throw an ArgumentOutOfRangeException
            Assert.Throws<IndexOutOfRangeException>(() => lazyListifyIndex.Value[index]);
        }

        [Fact]
        public void CountShouldMatchCountValue()
        {
            //Given the following values
            var minValue = 10;
            var maxValue = 100;
            var list = new Listify(minValue, maxValue);

            //When I get the Count
            var count = list.Count;

            //Then the count should match the expected value
            Assert.Equal(maxValue - minValue, count);
        }

        [Theory]
        [InlineData(12, true)]
        [InlineData(2500, false)]
        [InlineData(10, true)]
        [InlineData(100, true)]
        [InlineData(9, false)]
        [InlineData(-100, false)]
        public void ContainValue(int value, bool shouldContain)
        {
            //Given the following values
            var minValue = 10;
            var maxValue = 100;
            var list = new Listify(minValue, maxValue);

            //When I get whether it contains the value
            var containsValue = list.Contains(value);

            //Then it should match shouldContain
            Assert.Equal(shouldContain, containsValue);
        }

        [Fact]
        public void VerifyContainsAllValues()
        {
            //Given the following values
            var minValue = 0;
            var maxValue = 10;

            //When I create the listify instance along with a compareable Enumerable.Range()
            var list = new Listify(minValue, maxValue);
            var expectedValues = Enumerable.Range(minValue, maxValue);

            //Then they should be equal
            Assert.True(list.SequenceEqual(expectedValues));
        }
    }
}
