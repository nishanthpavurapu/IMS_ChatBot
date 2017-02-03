using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using Bot4AMS.Services;
using Bot4AMS.Model;
using Bot4AMS.Controllers;
using Microsoft.Bot.Builder.Dialogs;

namespace Bot4AMS
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        internal static IDialog<object> MakeRoot()
        {
            return Chain.From(() => new LuisDialog());
        }

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            
            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, MakeRoot);
            }
            else
            {
                HandleSystemMessage(activity);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);

        }

        private async Task<string> CreateAppointment(string primary_amsid, string secondary_amsid, string dateofappointment, string timeslot)
        {
            var result = await AmsBotService.CreateAppointment(primary_amsid, secondary_amsid, dateofappointment, timeslot);
            return result;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}