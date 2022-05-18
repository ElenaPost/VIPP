using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace VIPP.Hubs
{
	public class FeedbackHub : Hub
	{
		public void AddFeedback(string text, string userId)
		{
			Clients.Client(userId).addFeedback(text);
		}
		public void AddFinalFeedback(string text, string userId)
		{
			Clients.Client(userId).addFinalFeedback(text);
		}
	}
}