using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfTransactions
{
    public class ThingReader
    {
        ThingEntities _store;

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

        public IEnumerable<Thing> GetAllThings()
        {
            return Store.Things;
        }
    }
}
