using System;
using Microsoft.EntityFrameworkCore;

namespace BackgroundTasksServices
{
    public class UsingScopedDependencies : BackgroundService
    {
        private readonly IServiceProvider serviceProvider;

        public UsingScopedDependencies(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested)
            {
                /*
                 * El tiempo de vida de los servicios deben ser igual o superior al tiempo de vida del HostedService(Singleton).
                 * Por lo tanto, creamos un scope nuevo para poder usar los servicios con un tiempo de vida inferior.
                 * Importante disposeralos despues de usarlos para evitar MemoryLeaks.
                 */
                using (var scope = this.serviceProvider.CreateScope())
                {
                    var scopedProvider = scope.ServiceProvider;
                    var dbContext = scopedProvider.GetRequiredService<AppDBContext>();
                    var dummyService = scopedProvider.GetRequiredService<DummyService>();

                    //Simulamos que vamos a buscar un registro en BDD que nos servira para ejecutar un proceso.
                    var process = await dbContext.Set<DummyProcess>().FirstOrDefaultAsync();
                    if (process != null)
                    {
                        await dummyService.LongRunningMethod(process);
                        dbContext.Remove(process);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}
