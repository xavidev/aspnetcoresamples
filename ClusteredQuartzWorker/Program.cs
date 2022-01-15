using Quartz;
using QuartzWorker;

//NOTA: Se puede habilitar la persistencia sin el clustering.

/*
 *Quartz.NET stores a list of jobs and triggers in the database and uses database locking 
 *to ensure only a single instance of your app handles a trigger and runs the associated job.
 *
 *
 *Quartz.NET stores data in your database, but it doesn’t attempt to create the tables it uses 
 *itself. Instead, you must manually add the required tables. Quartz.NET provides SQL scripts 
 *on GitHub for all of the supported database server types, including MS SQL Server, SQLite, PostgreSQL, MySQL, and many more: http://mng.bz/JDeZ.
 *
 *Considerations for clustering: https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/advanced-enterprise-features.html
 */

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctxt, services) =>
    {
        var connectionString = ctxt.Configuration.GetConnectionString("DefaultConnection");

        services.AddQuartz(q =>
        {
            q.SchedulerId = "AUTO"; //Para que cada instancia de la app tenga un ID unico del 'scheduler'

            q.UseMicrosoftDependencyInjectionJobFactory();

            q.UsePersistentStore(o =>
            {
                o.UseSqlServer(connectionString);
                o.UseClustering();
                o.UseProperties = true;
                o.UseJsonSerializer();
            });

            var jobKey = new JobKey("DummyJob Example");
            q.AddJob<DummyJob>(jobKey);

            //Simple Trigger
            q.AddTrigger(config =>
            {
                config
                .ForJob(jobKey)
                .WithIdentity($"{jobKey}trigger") //Provide a unique name for the trigger for use in logging and in clustered scenarios.
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithInterval(TimeSpan.FromMinutes(5))
                    .RepeatForever()
                    );
            });

            //Scheduled Trigger
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity("Update exchange rates trigger")
                .WithSchedule(CronScheduleBuilder
                    .WeeklyOnDayAndHourAndMinute(DayOfWeek.Friday, 17, 30)));
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        services.AddScoped<IDummyDbContext, DummyContext>();
        services.AddScoped<IDummyService, DummyService>();
    })
    .Build();

await host.RunAsync();