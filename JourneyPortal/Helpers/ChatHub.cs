using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JourneyPortal.Helpers
{
    public class ChatHubHub : Hub
    {
        public void Send(string name, string message)
        {
            Clients.All.appendNewMessage(name, message);
        }
    }
}