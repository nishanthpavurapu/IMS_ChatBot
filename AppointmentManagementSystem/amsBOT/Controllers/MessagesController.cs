using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Newtonsoft.Json;
using amsBOT.Services;
using Microsoft.Bot.Builder.FormFlow;

namespace amsBOT
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {

        //String variables for operations, gathered from user by the BOT
        string primary_amsid, secondary_amsid = "", dateofappointment = "", timeslot = "";
        Activity reply;

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(activity.ServiceUrl));
            var operation = string.Empty;
            if (activity.Type == ActivityTypes.Message)
            {
                var phrase = activity.Text;
                var luisResponse = await LuisService.ParseUserInput(phrase);
                if (luisResponse.intents.Count() > 0)
                {
                    switch (luisResponse.topScoringIntent.intent)
                    {
                        case "CreateAppointment":
                            operation = luisResponse.topScoringIntent.intent;
                            break;
                    }

                    if (operation == "CreateAppointment")
                    {
                        secondary_amsid = "";
                        dateofappointment="";
                        timeslot="";
                        primary_amsid = "nishanthpavurapu";
                        for (int i = 0; i < luisResponse.entities.Count(); i++)
                        {
                            switch (luisResponse.entities[i].type)
                            {
                                case "amsuser::secondaryuser":
                                    secondary_amsid = luisResponse.entities[i].entity;
                                    break;
                                case "builtin.datetime.date":
                                    dateofappointment = luisResponse.entities[i].resolution.date.ToString();
                                    string date_Pattern = "yyyy-MM-dd";
                                    DateTime dateParsed;
                                    DateTime.TryParseExact(dateofappointment, date_Pattern, null, System.Globalization.DateTimeStyles.None, out dateParsed);
                                    dateofappointment = dateParsed.ToString("MM/dd/yyyy");
                                    break;
                                case "builtin.datetime.time":
                                    timeslot = luisResponse.entities[i].resolution.time.ToString();
                                    timeslot = timeslot.Replace("T", "");
                                    break;
                                case "createoperation":
                                    break;
                            }
                        }
                        if (primary_amsid != "" && secondary_amsid != "" && dateofappointment != "" && timeslot != "")
                        {
                            await connector.Conversations.ReplyToActivityAsync(activity.CreateReply(await CreateAppointment(primary_amsid, secondary_amsid, dateofappointment, timeslot)));
                        }
                        else
                        {
                           
                        }
                    }
                    else
                    {
                        await connector.Conversations.ReplyToActivityAsync(activity.CreateReply($"Sorry! I didn't get you. \n I provide below services, Please try one of them again. \n\n 1. Will create a appointment for you \n 2. Will re-Schedule any of your appointment \n 3. Will cancel your appointments \n\n\n Thank you!"));
                    }
                }
                else
                {
                    await connector.Conversations.ReplyToActivityAsync(activity.CreateReply("Sorry! I didn't get what you said."));
                }


                //if (message.Text == "Nishanth")
                //{
                //    return message.CreateReplyMessage($"Hello Boss! Welcome back.");
                //}

                //if (message.Text == "create appointment")
                //{
                //    return message.CreateReplyMessage(await CreateAppointment());
                //}

                //// calculate something for us to return
                //int length = (message.Text ?? string.Empty).Length;

                //// return our reply to the user
                //return message.CreateReplyMessage($"You sent {length} characters");
            }
            else
            {
                return HandleSystemMessage(activity);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task<string> CreateAppointment(string primary_amsid,string secondary_amsid,string dateofappointment,string timeslot)
        {
            var result = await Services.AmsBotService.CreateAppointment(primary_amsid,secondary_amsid, dateofappointment, timeslot);
            return result;
        }

        private async Task GetMissingStrings(IDialogContext context, IAwaitable<string> result)
        {
            if (secondary_amsid == "")
            {
                secondary_amsid = await result;
                PromptDialog.Text(context, GetMissingStrings, "With whom do you want me to schedule your appointment[AMS Username]: ");
            }
            if (dateofappointment == "")
            {
                dateofappointment = await result;
                PromptDialog.Text(context, GetMissingStrings, "For which date? ");
            }
            if (timeslot == "")
            {
                timeslot = await result;
                PromptDialog.Text(context, GetMissingStrings, "Tell me the time slot[Example 5.30 or 5.00]: ");
            }
        }

        private HttpResponseMessage HandleSystemMessage(Activity activity)
        {
            if (activity.Type == ActivityTypes.Ping)
            {
            }
            else if (activity.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (activity.Type == ActivityTypes.ConversationUpdate)
            {
            }
            else if (activity.Type == ActivityTypes.Typing)
            {
            }
            //else if (message.Type == "UserAddedToConversation")
            //{
            //}
            //else if (message.Type == "UserRemovedFromConversation")
            //{
            //}
            //else if (message.Type == "EndOfConversation")
            //{
            //}

            return null;
        }
    }
}