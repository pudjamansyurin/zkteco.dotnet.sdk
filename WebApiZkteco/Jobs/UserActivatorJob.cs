using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiZkteco.Models;
using WebApiZkteco.Services;

namespace WebApiZkteco.Jobs
{
    public class UserActivatorJob : CronJobService
    {
        private readonly ILogger<UserActivatorJob> _logger;
        private readonly IServiceScopeFactory _sp;

        public UserActivatorJob(IScheduleConfig<UserActivatorJob> config, ILogger<UserActivatorJob> logger, IServiceScopeFactory sp)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _sp = sp;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("UserActivatorJob starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            using (var scope = _sp.CreateScope())
            {
                IUserService _user = scope.ServiceProvider.GetRequiredService<IUserService>();
                ISdkService _sdk = scope.ServiceProvider.GetRequiredService<ISdkService>();

                var users = _user.GetPending();

                if (users.Count == 0)
                {
                    _logger.LogInformation($"{DateTime.Now:hh:mm:ss} UserActivatorJob no pending users");
                }
                else
                {
                    _logger.LogInformation($"{DateTime.Now:hh:mm:ss} UserActivatorJob got " + users.Count + " pending users");

                    users.ForEach(u =>
                    {
                        _logger.LogInformation("Enabling user " + u.sUserID);
                        try
                        {
                            _sdk.SetUser(u);
                            _user.Enable(u);
                        }
                        catch (Exception e)
                        {
                            _logger.LogInformation(e.Message);
                        }
                    });
                }
            }
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("UserActivatorJob is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
