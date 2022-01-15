using Quartz;
using QuartzWorker;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

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
