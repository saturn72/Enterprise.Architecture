using System;
using Xunit;
using Shouldly;

namespace Object.Extensions.Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("   ")]
        [InlineData(null)]
        [InlineData("")]
        public void StringExtensions_HasValue_ReturnsFalse(string source)
        {
            source.HasValue().ShouldBeFalse();
        }
    }
}
