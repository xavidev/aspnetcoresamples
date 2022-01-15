using System;
namespace QuartzWorker
{
    public class DummyService : IDummyService
    {
        public void DoThing()
        {
            Console.WriteLine("Doing dummy thing");
        }
    }

    public class DummyContext : IDummyDbContext
    {
        public void Save()
        {
            Console.WriteLine("Saving dummy thing");
        }
    }
}

