using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfTransactions
{
    public class ThingAccess : IDisposable
    {
        ThingEntities _store;
        DbContextTransaction _transaction;

        internal ThingEntities Store
        {
            get
            {
                if (_store == null)
                {
                    _store = new ThingEntities();
                }

                return _store;
            }
        }

        protected void SetStore(ThingEntities store)
        {
            _store = store;
        }

        public bool HasOpenTransaction
        {
            get
            {
                return _transaction != null;
            }
        }

        protected void BeginTransaction()
        {
            _transaction = Store.Database.BeginTransaction();
        }

        public void Dispose()
        {
            if (HasOpenTransaction) _transaction.Dispose();
        }

        protected void Rollback()
        {
            if (HasOpenTransaction)
            {
                _transaction.Rollback();
                _transaction = null;
            }
        }

        protected void Commit()
        {
            if (HasOpenTransaction)
            {
                _transaction.Commit();
                _transaction = null;
            }
        }
    }
}
