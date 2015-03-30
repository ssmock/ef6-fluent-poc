using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EfTransactions;

namespace Tests
{
    [TestClass]
    public class ThingWriterTests : ThingTestBase
    {
        [TestMethod]
        public void Adds_Thing()
        {
            string name = GetName();
            string description = GetDescription();

            var id = WriterService.AddThing(name, description).Value<int>();
            
            Assert.IsTrue(id > 0, "Thing not created.");
        }

        [TestMethod]
        public void Adds_Thing_In_Transaction()
        {
            InRolledBackTransaction(writer =>
            {
                string name = GetTransientName();
                string description = GetTransientDescription();

                var id = writer
                    .AddThing(name, description)
                    .Value<int>();

                Assert.IsTrue(id > 0, "Thing not created.");
            });
        }

        [TestMethod]
        public void Adds_Thing_In_Transaction_And_Commits()
        {
            string name = GetName();
            string description = GetDescription();

            var id = WriterService
                .InTransaction()
                .AddThing(name, description)
                .Commit()
                .Value<int>();

            Assert.IsTrue(id > 0, "Thing not created.");
            AssertNoTransients();
        }

        [TestMethod]
        public void Adds_Thing_In_Transaction_With_Tap()
        {
            InRolledBackTransaction(writer =>
            {
                string name = GetTransientName();
                string description = GetTransientDescription();

                int id;

                writer
                    .AddThing(name, description)
                    .Tap<int>(out id);

                Assert.IsTrue(id > 0, "Thing not created.");
            });
        }

        [TestMethod]
        public void Adds_Things_In_Transaction_With_Taps()
        {
            InRolledBackTransaction(writer =>
            {
                string name1 = GetTransientName();
                string description1 = GetTransientDescription();

                string name2 = GetTransientName();
                string description2 = GetTransientDescription();

                int id1;
                int id2;

                writer
                    .AddThing(name1, description1)
                    .Tap<int>(out id1)
                    .AddThing(name2, description2)
                    .Tap<int>(out id2);

                Assert.IsTrue(id1 > 0, "Thing 1 not created.");
                Assert.IsTrue(id2 > id1, "Thing 2 not created after thing 1.");
            });
        }

        [TestMethod]
        public void Clears_All_Things()
        {
            string name1 = GetTransientName();
            string description1 = GetTransientDescription();

            WriterService.AddThing(name1, description1);

            WriterService.ClearAllThings();

            var count = ReaderService.GetAllThings().Count();

            Assert.AreEqual(0, count, 
                string.Format("At least one thing remains."));
        }

        [TestMethod]
        public void Adds_One_Thing_Into_Blank_Slate()
        {
            InTabulaRasaTransaction(writer =>
            {
                var name = GetTimeStampedString("LONER");
                var description = GetTimeStampedString("LONER DESC");

                writer.AddThing(name, description).Value<int>();

                var things = ReaderServiceFrom(writer).GetAllThings();

                Assert.AreEqual(1, things.Count());
            });
        }
    }
}
