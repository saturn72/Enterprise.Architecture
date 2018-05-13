using System.Collections.Generic;

namespace Programmer.Db.LiteDb
{
    public static class LiteDbAdapterExtensions
    {
        public static IEnumerable<TObject> GetAll<TObject>(this LiteDbAdapter dbAdapter)
        {
            return dbAdapter.Query(d => d.GetCollection<TObject>().FindAll());
        }

        public static TObject Create<TObject>(this LiteDbAdapter dbAdapter, TObject toAdd)
        {
            var inserted = default(TObject);

            dbAdapter.Command(db=>
            {
                var col = db.GetCollection<TObject>();
                    col.Insert(toAdd);
                inserted = toAdd;
            });
            return inserted;
        }
    }
}
