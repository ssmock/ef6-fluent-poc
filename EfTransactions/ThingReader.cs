using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EfTransactions
{
    public class ThingReader: ThingAccess
    {
        public static ThingReader WithSharedStore(ThingAccess other)
        {
            ThingReader result = new ThingReader();
            result.SetStore(other.Store);

            return result;
        }

        public IEnumerable<Thing> GetAllThings()
        {
            return Store.Things;
        }
    }
}
