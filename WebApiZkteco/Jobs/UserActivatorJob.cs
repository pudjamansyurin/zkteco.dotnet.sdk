using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebApiZkteco.Services;

namespace WebApiZkteco.Jobs
{
    public class UserActivatorJob : CronJobService
    {
        private readonly ILogger<UserActivatorJob> _logger;
        private readonly IUserService _user;
        private readonly ISdkService _sdk;

        public UserActivatorJob(IScheduleConfig<UserActivatorJob> config, ILogger<UserActivatorJob> logger, IUserService user, ISdkService sdk)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _user = user;
            _sdk = sdk;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("UserActivatorJob starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} UserActivatorJob checking pending users");

            if (_user.HasPending())
            {
                _user.GetPending().ForEach(u =>
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

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("UserActivatorJob is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}
