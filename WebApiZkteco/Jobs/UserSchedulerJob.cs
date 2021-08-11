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
    public class UserSchedulerJob : CronJobService
    {
        private readonly ILogger<UserSchedulerJob> _logger;
        private readonly IServiceScopeFactory _sp;

        public UserSchedulerJob(IScheduleConfig<UserSchedulerJob> config, ILogger<UserSchedulerJob> logger, IServiceScopeFactory sp)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _sp = sp;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("UserScheduler starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            using (var scope = _sp.CreateScope())
            {
                IUserService _user = scope.ServiceProvider.GetRequiredService<IUserService>();
                ISdkService _sdk = scope.ServiceProvider.GetRequiredService<ISdkService>();

                var shouldActive = checkSouldActiveUsers(_sdk, _user);
                var shouldDeactive = checkSouldDeactiveUsers(_sdk, _user);
                var active = _user.Active().Count;

                _logger.LogInformation($"{DateTime.Now:hh:mm:ss} UserScheduler: +" + shouldActive + ", -" + shouldDeactive + ", " + active + " active.");
            }
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("UserScheduler is stopping.");
            return base.StopAsync(cancellationToken);
        }

        private int checkSouldActiveUsers(ISdkService _sdk, IUserService _user)
        {
            var users = _user.ShouldActive();

            if (users.Count > 0)
            {

                users.ForEach(u =>
                {
                    _logger.LogInformation("Enabling user " + u.sUserID);
                    try
                    {
                        if (u.idwFingerIndex > 0)
                        {
                            _sdk.SetUser(u);
                        }
                        _user.SetActive(u, true);
                    }
                    catch (Exception e)
                    {
                        _logger.LogInformation(e.Message);
                    }
                });
            }
            return users.Count;
        }

        private int checkSouldDeactiveUsers(ISdkService _sdk, IUserService _user)
        {
            var users = _user.ShouldDeactive();

            if (users.Count > 0)
            {
                users.ForEach(u =>
                {
                    _logger.LogInformation("Disabling user " + u.sUserID);
                    try
                    {
                        if (u.idwFingerIndex > 0)
                        {
                            _sdk.DeleteUser(u);
                        }
                        _user.SetActive(u, false);
                    }
                    catch (Exception e)
                    {
                        _logger.LogInformation(e.Message);
                    }
                });
            }
            return users.Count;
        }
    }
}
