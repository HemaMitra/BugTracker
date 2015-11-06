using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Security.Principal;

namespace BugTracker
{
    public class SignalRNotiHub : Hub
    {
        public void SendNotifications(string recipient,string message)
        {
            var hub = GlobalHost.ConnectionManager.GetHubContext<SignalRNotiHub>();
            //Clients.All.addNewMessageToPage(name, message);
            hub.Clients.All.receiveSRNoti(message);
            //hub.Clients.User(recipient).receiveSrNoti(message);


        }
    }
}