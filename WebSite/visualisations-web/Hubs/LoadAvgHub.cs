using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace visualisations_web.Hubs
{
    public class LoadAvgHub : Hub
    {
        static Dictionary<string, DbReader<LoadAvgHub>> runningTasks = new Dictionary<string, DbReader<LoadAvgHub>>();

        public void StartReadingPump(string deviceId)
        {
            Groups.Add(Context.ConnectionId, deviceId);

            if (!runningTasks.ContainsKey(deviceId))
                runningTasks.Add(deviceId, new DbReader<LoadAvgHub>(deviceId));
        }
    }
}