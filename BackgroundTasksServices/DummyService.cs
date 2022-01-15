namespace BackgroundTasksServices
{
    public class DummyService
    {
        public Task LongRunningMethod(DummyProcess? process)
        {
            return Task.CompletedTask;
        }
    }
}