using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Object.Extensions.Tests
{
    public class ObjectExtensionsTests
    {
        [Theory]
        [InlineData(1, "Int32")]
        [InlineData("string", "String")]
        public void ObjectExtensions_ToCSharpName_ByInstance(object o, string expName)
        {
            o.ToCSharpName().ShouldBe(expName);
        }

        [Theory]
        [InlineData(typeof(int), "Int32")]
        [InlineData(typeof(string), "String")]
        [InlineData(typeof(IEnumerable<IList<string>>), "IEnumerable<IList<String>>")]
        public void ObjectExtensions_ToCSharpName_ByType(Type type, string expName)
        {
            type.ToCSharpName().ShouldBe(expName);
        }
    }
}
