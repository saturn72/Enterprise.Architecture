using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Object.Extensions.Tests
{
    public class EnumerableExtensionsTests
    {
        [Theory]
        [MemberData(nameof(EnumerableExtensions_IsNullOrEmpty_Data))]
        public void EnumerableExtensions_IsNullOrEmpty(IEnumerable<string> source, bool expResult)
        {
            source.IsNullOrEmpty().ShouldBe(expResult);
        }

        public static IEnumerable<object[]> EnumerableExtensions_IsNullOrEmpty_Data =>
           new[]{
                new object[] { null as IList<string>, true},
                new object[] { new string[]{}, true},
                new object[] { new []{"a","b","c"}, false},
           };
    }
}