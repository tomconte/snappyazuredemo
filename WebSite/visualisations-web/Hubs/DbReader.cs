using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using visualisations_web.DataAccess;

namespace visualisations_web.Hubs
{
    public class DbReader<THubContext> where THubContext : Hub
    {        
        private IHubContext _hubContext;
        public DbReader(string deviceId)
        {
            _hubContext = GlobalHost.ConnectionManager.GetHubContext<THubContext>();
            Task.Run(() => Run(deviceId));
        }

        private async Task Run(string deviceId)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["LoadAvgDb"].ConnectionString;

            while (true)
            {
                using (LoadAvgContext context = new LoadAvgContext(connectionString))
                {
					try
					{
						var reading = context.Readings.Where(t => t.Device==deviceId).OrderByDescending(t => t.Time).Take(512).ToList();
						_hubContext.Clients.Group(deviceId).pump(reading);
					}
					catch (Exception ex)
					{
						System.Diagnostics.Trace.TraceError(ex.Message);
						if (ex.InnerException != null)
							System.Diagnostics.Trace.TraceError(ex.InnerException.Message);
					}
                }

                await Task.Delay(1000);
            }
        }
    }
}
