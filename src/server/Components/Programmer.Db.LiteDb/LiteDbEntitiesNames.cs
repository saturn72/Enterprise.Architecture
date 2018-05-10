using System;
using System.Collections.Generic;
using Programmer.Common.Domain.Treatment;

namespace Programmer.Db.LiteDb
{
    internal class LiteDbEntitiesNames
    {
        internal static readonly IDictionary<Type, string> EntityToDocumentName
            = BuildTableKeys();

        private static IDictionary<Type, string> BuildTableKeys()
        {
            var allPairs = new[]
            {
                BuildTableName(typeof(TreatmentModel)),
            };


            var res = new Dictionary<Type, string>();
            foreach (var pair in allPairs)
                res.Add(pair.Key, pair.Value);
            return res;
        }

        private static KeyValuePair<Type, string> BuildTableName(Type type)
        {
            return new KeyValuePair<Type, string>(type, type.Name.ToLower() + "s");
        }
    }
}