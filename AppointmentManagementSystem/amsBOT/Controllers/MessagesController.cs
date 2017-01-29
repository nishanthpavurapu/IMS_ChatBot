using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Utilities;
using Newtonsoft.Json;
using amsBOT.Services;

namespace amsBOT
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
       
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<Message> Post([FromBody]Message message)
        {
            var operation = string.Empty;
            if (message.Type == "Message")
            {
                var phrase = message.Text;
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
                        string primary_amsid, secondary_amsid="", dateofappointment="", timeslot="";
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
                        return message.CreateReplyMessage(await CreateAppointment(primary_amsid,secondary_amsid,dateofappointment,timeslot));
                    }
                    else
                    {
                        return message.CreateReplyMessage("Unknown Entity");
                    }
                }
                else
                {
                    return message.CreateReplyMessage("Sorry! I didn't get what you said.");
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
                return HandleSystemMessage(message);
            }
        }

        private async Task<string> CreateAppointment(string primary_amsid,string secondary_amsid,string dateofappointment,string timeslot)
        {
            var result = await Services.AmsBotService.CreateAppointment(primary_amsid,secondary_amsid, dateofappointment, timeslot);
            return result;
        }

        private Message HandleSystemMessage(Message message)
        {
            if (message.Type == "Ping")
            {
                Message reply = message.CreateReplyMessage();
                reply.Type = "Ping";
                return reply;
            }
            else if (message.Type == "DeleteUserData")
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == "BotAddedToConversation")
            {
            }
            else if (message.Type == "BotRemovedFromConversation")
            {
            }
            else if (message.Type == "UserAddedToConversation")
            {
            }
            else if (message.Type == "UserRemovedFromConversation")
            {
            }
            else if (message.Type == "EndOfConversation")
            {
            }

            return null;
        }
    }
}