using System;
using LiteDB;

namespace Programmer.Db.LiteDb
{
    public class LiteDbAdapter
    {
        #region consts
        private readonly string _dbName;

        #endregion
        #region ctor
        public LiteDbAdapter(string dbName)
        {
            _dbName = dbName;
        }
        #endregion
        public virtual void Command(Action<LiteDatabase> command)
        {
            using (var db = new LiteDatabase(_dbName))
            {
                command(db);
            }

        }
        public virtual TQueryResult Query<TQueryResult>(Func<LiteDatabase, TQueryResult> query)
        {
            using (var db = new LiteDatabase(_dbName))
            {
                return query(db);
            }
        }
    }
}