using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfTransactions
{
    public class ThingWriter : ThingAccess, IDisposable
    {
        object _value;

        //public ThingWriter() { /* Nothing */ }

        //public ThingWriter(ThingEntities store) : base(store) { /* Nothing */ }

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
            foreach (var thing in Store.Things)
            {
                Store.Entry(thing).State = EntityState.Deleted;
            }

            Store.SaveChanges();
        }

        public ThingWriter InTransaction()
        {
            if (!HasOpenTransaction) BeginTransaction();

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

        public new ThingWriter Rollback()
        {
            base.Rollback();

            return this;
        }

        public new ThingWriter Commit()
        {
            base.Commit();

            return this;
        }
    }
}
