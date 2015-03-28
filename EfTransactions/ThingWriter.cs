using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfTransactions
{
    public class ThingWriter : IDisposable
    {
        ThingEntities _store;
        DbContextTransaction _transaction;
        object _value;

        private ThingEntities Store
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

        public bool HasOpenTransaction
        {
            get
            {
                return _transaction != null;
            }
        }

        public ThingWriter AddThing(string name, string description)
        {
            var newThing = new Thing
            {
                Name = name,
                Description = description
            };

            Store.Things.Add(newThing);

            Store.SaveChanges();

            _value = newThing.Id;

            return this;
        }

        public void ClearAllThings()
        {
            foreach(var thing in Store.Things)
            {
                Store.Entry(thing).State = EntityState.Deleted;
            }

            Store.SaveChanges();
        }

        public ThingWriter InTransaction()
        {
            if (_transaction == null)
            {
                _transaction = Store.Database.BeginTransaction();
            }

            return this;
        }

        public T Value<T>()
        {
            return (T)_value;
        }

        public ThingWriter Tap<T>(out T value)
        {
            value = (T)_value;

            return this;
        }

        public void Dispose()
        {
            if (HasOpenTransaction) _transaction.Dispose();
        }

        public ThingWriter Rollback()
        {
            if (HasOpenTransaction)
            {
                _transaction.Rollback();
                _transaction = null;
            }

            return this;
        }

        public ThingWriter Commit()
        {
            if (HasOpenTransaction)
            {
                _transaction.Commit();
                _transaction = null;
            }

            return this;
        }
    }
}
