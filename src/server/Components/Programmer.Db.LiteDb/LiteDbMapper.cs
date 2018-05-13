using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using LiteDB;
using Programmer.Common.Domain.Treatment;

namespace Programmer.Db.LiteDb
{
    public class LiteDbMapper
    {
        public static void Map(string dbName)
        {
            using (var db = new LiteDatabase(dbName))
            {
                GetCollectionAndEnsureIndex(db, new Expression<Func<TreatmentModel, object>>[]
                {
                    x => x.SessionId,
                    x => x.Id
                });
            }
        }

        private static void GetCollectionAndEnsureIndex<TDomainModel>(LiteDatabase db, IEnumerable<Expression<Func<TDomainModel, object>>> indexes)
        {
            var col = db.GetCollection<TDomainModel>();
            foreach (var exp in indexes)
                col.EnsureIndex(exp);
        }
    }
}