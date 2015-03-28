using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ThingReaderTests : ThingTestBase
    {
        [TestMethod]
        public void Gets_All_Things()
        {
            WriterService.AddThing(GetName(), GetDescription());
            WriterService.AddThing(GetName(), GetDescription());

            var things = ReaderService.GetAllThings();

            Assert.IsTrue(things.Count() >= 2, 
                "Fewer than two things were retrieved.");
        }
    }
}
