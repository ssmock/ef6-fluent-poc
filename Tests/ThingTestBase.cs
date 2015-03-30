using EfTransactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class ThingTestBase
    {
        const string TRANSIENT_THING_NAME = "TRANS THING";

        ThingWriter _writer;
        ThingReader _reader;

        protected ThingWriter WriterService
        {
            get
            {
                if (_writer == null)
                {
                    _writer = new ThingWriter();
                }

                return _writer;
            }
        }

        protected ThingReader ReaderService
        {
            get
            {
                if (_reader == null)
                {
                    _reader = new ThingReader();
                }

                return _reader;
            }
        }

        protected ThingReader ReaderServiceFrom(ThingWriter writer)
        {
            return ThingReader.WithSharedStore(WriterService);
        }

        protected string GetTransientName()
        {
            return GetTimeStampedString(TRANSIENT_THING_NAME);
        }

        protected string GetTransientDescription()
        {
            return GetTimeStampedString("TRANS DESC");
        }

        protected string GetName()
        {
            return GetTimeStampedString("THING");
        }

        protected string GetDescription()
        {
            return GetTimeStampedString("THING DESC");
        }

        protected string GetTimeStampedString(string baseString)
        {
            return string.Format("{0}@{1}", baseString, DateTime.Now.Ticks);
        }

        protected void AssertNoTransients()
        {
            if(WriterService.HasOpenTransaction)
            {
                WriterService.Rollback();
            }

            var transientCount = ReaderService
                .GetAllThings()
                .Count(thing => 
                    thing.Name.IndexOf(TRANSIENT_THING_NAME) == 0);

            Assert.AreEqual(0, transientCount, "Transient things linger.");
        }

        protected void InRolledBackTransaction(Action<ThingWriter> action)
        {
            action(WriterService.InTransaction());
            
            WriterService.Rollback();
            
            AssertNoTransients();
        }

        protected void InTabulaRasaTransaction(Action<ThingWriter> action)
        {
            InRolledBackTransaction(writer =>
            {
                writer.ClearAllThings();

                action(writer);
            });
        }
    }
}
