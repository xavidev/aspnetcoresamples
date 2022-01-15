using System;
using Quartz;

namespace QuartzWorker
{
    [DisallowConcurrentExecution] //Le dice a Quartz que no cree otro job al dispararse el trigger si aun se esta ejecutando.
    public class DummyJob : IJob
    {
        private readonly IDummyService service;
        private readonly IDummyDbContext dbContext;

        //El constructor puede recivir cualquier servicio registrado en el contenedor.
        //No nos tenemos que preocupar de crear el Scope (como en los otros casos) manualmente para usar dependencias 'Scoped'
        //Quartz se encarga de toda esta implementacion permitiendo usar inyeccion de dependencias en el constructor automaticamente.
        public DummyJob(IDummyService service, IDummyDbContext dbContext)
        {
            this.service = service;
            this.dbContext = dbContext;
        }

        public Task Execute(IJobExecutionContext context)
        {
            this.service.DoThing();
            this.dbContext.Save();

            return Task.CompletedTask;
        }
    }
}

