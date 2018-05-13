using System;
using System.Collections.Generic;
using LiteDB;
using Moq;
using Shouldly;
using Xunit;

namespace Programmer.Db.LiteDb.Tests
{
    public class LiteDbAdapterExtensionsTests
    {
        [Fact]
        public void LiteDbAdapterExtensions_GetAll()
        {
            var da = new Mock<LiteDbAdapter>("dbName.db");
            LiteDbAdapterExtensions.GetAll<string>(da.Object);

            da.Verify(d => d.Query(It.IsAny<Func<LiteDatabase, IEnumerable<string>>>()), Times.Once);
        }

        [Fact]
        public void LiteDbAdapterExtensions_Create()
        {
            var da = new Mock<LiteDbAdapter>("dbName.db");
            LiteDbAdapterExtensions.Create<string>(da.Object, "my string");

            da.Verify(d => d.Command(It.IsAny<Action<LiteDatabase>>()), Times.Once);
        }
    }
}
