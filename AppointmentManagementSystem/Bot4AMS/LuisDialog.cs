using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis.Models;
using System.Threading.Tasks;
using Microsoft.Bot.Connector;
using Bot4AMS.Services;
using System.Globalization;

namespace Bot4AMS
{
    [LuisModel("8dd24c70-7b91-4f36-b5a3-c7669cf93638", "3265f3c51c984d29b1fa0bf440a64f47")]
    [Serializable]
    public class LuisDialog : LuisDialog<object>
    {
        //String variables for operations, gathered from user by the BOT
        string primary_amsid="", secondary_amsid = "", dateofappointment = "", timeslot = "",CurrentIntent="",newTimeSlot="";
        string weekStartDate, weekEndDate, monthStartDate, monthEndDate;

        //-----------------------------------------------------------Section for Create Appointment --------------------------------------------------

        [LuisIntent("CreateAppointment")]
        public async Task CreateOperation(IDialogContext context, LuisResult luisResponse)
        {
            context.ConversationData.TryGetValue("primaryamsid",out primary_amsid);
            if (primary_amsid == "" || primary_amsid == null)
            {
                await context.PostAsync("Please login to your AMS account: [Format: login username Ex: login nishanthpavurapu]");
                context.Done(true);
            }
            else { 
            CurrentIntent = "CreateAppointment";
            for (int i = 0; i < luisResponse.Entities.Count(); i++)
            {
                switch (luisResponse.Entities[i].Type)
                {
                    case "amsuser::secondaryuser":
                        secondary_amsid = luisResponse.Entities[i].Entity;
                       
                        break;
                    case "builtin.datetime.date":
                        dateofappointment = luisResponse.Entities[i].Resolution["date"].ToString();
                        string date_Pattern = "yyyy-MM-dd";
                        DateTime dateParsed;
                        DateTime.TryParseExact(dateofappointment, date_Pattern, null, System.Globalization.DateTimeStyles.None, out dateParsed);
                        dateofappointment = dateParsed.ToString("MM/dd/yyyy");
                        //adding value to object
                      
                        break;
                    case "builtin.datetime.time":
                        timeslot = luisResponse.Entities[i].Resolution["time"].ToString();
                        timeslot = timeslot.Replace("T", "");
                        break;
                }
            }

            if (primary_amsid != "" && secondary_amsid != "" && dateofappointment != "" && timeslot != "")
            {
                await context.PostAsync(await CreateAppointment(primary_amsid, secondary_amsid, dateofappointment, timeslot));
            }
            else
            {
                if (secondary_amsid == "")
                {
                    await context.PostAsync("Please tell me with whom do you want an appointment [Format: AMS UserID Ex: nishanthpavurapu]");
                    context.Wait(ProcessMissingSecondaryUserInfo);
                }
                else if (dateofappointment=="")
                {
                    await context.PostAsync("Please enter missing Date of Appointment information [Format: mm/dd/yyyy]");
                    context.Wait(ProcessMissingDateInfo);
                }
                else if (timeslot == "")
                {
                    await context.PostAsync("Please enter missing timeslot of Appointment information [Format: 17:00]");
                    context.Wait(ProcessMissingTimeInfo);
                }
            }
            }
        }

        //-----------------------------------------------------------Section for Cancel Appointment --------------------------------------------------

        [LuisIntent("CancelAppointment")]
        public async Task CancelOperation(IDialogContext context, LuisResult luisResponse)
        {
            context.ConversationData.TryGetValue("primaryamsid", out primary_amsid);
            if (primary_amsid == "" || primary_amsid == null)
            {
                await context.PostAsync("Please login to your AMS account: [Format: login username Ex: login nishanthpavurapu]");
                context.Done(true);
            }
            else
            { 
            CurrentIntent = "CancelAppointment";
            for (int i = 0; i < luisResponse.Entities.Count(); i++)
            {
                switch (luisResponse.Entities[i].Type)
                {
                    case "builtin.datetime.date":
                        dateofappointment = luisResponse.Entities[i].Resolution["date"].ToString();
                        string date_Pattern = "yyyy-MM-dd";
                        DateTime dateParsed;
                        DateTime.TryParseExact(dateofappointment, date_Pattern, null, System.Globalization.DateTimeStyles.None, out dateParsed);
                        dateofappointment = dateParsed.ToString("MM/dd/yyyy");
                        break;

                    case "builtin.datetime.time":
                        timeslot = luisResponse.Entities[i].Resolution["time"].ToString();
                        timeslot = timeslot.Replace("T", "");
                        break;
                }

            }
            if (primary_amsid != "" && dateofappointment != "" && timeslot != "")
            {
                await context.PostAsync(await CancelAppointment(primary_amsid, dateofappointment, timeslot));
            }
            else
            {
                if (dateofappointment == "")
                {
                    await context.PostAsync("Please enter missing Date of Appointment information [Format: mm/dd/yyyy]");
                    context.Wait(ProcessMissingDateInfo);
                }
                else if (timeslot == "")
                {
                    await context.PostAsync("Please enter missing timeslot of Appointment information [Format: 17:00]");
                    context.Wait(ProcessMissingTimeInfo);
                }
            }
        }
        }
        //-----------------------------------------------------------Section for Edit Appointment --------------------------------------------------

        [LuisIntent("EditAppointment")]
        public async Task EditOperation(IDialogContext context, LuisResult luisResponse)
        {
            context.ConversationData.TryGetValue("primaryamsid", out primary_amsid);
            if (primary_amsid == "" || primary_amsid == null)
            {
                await context.PostAsync("Please login to your AMS account: [Format: login username Ex: login nishanthpavurapu]");
                context.Done(true);
            }
            else
            { 
            CurrentIntent = "EditAppointment";
            for (int i = 0; i < luisResponse.Entities.Count(); i++)
            {
                switch (luisResponse.Entities[i].Type)
                {
                    case "builtin.datetime.date":
                        dateofappointment = luisResponse.Entities[i].Resolution["date"].ToString();
                        string date_Pattern = "yyyy-MM-dd";
                        DateTime dateParsed;
                        DateTime.TryParseExact(dateofappointment, date_Pattern, null, System.Globalization.DateTimeStyles.None, out dateParsed);
                        dateofappointment = dateParsed.ToString("MM/dd/yyyy");
                        break;

                    case "builtin.datetime.time":
                        timeslot = luisResponse.Entities[i].Resolution["time"].ToString();
                        timeslot = timeslot.Replace("T", "");
                        break;
                }

            }
            if (dateofappointment == "")
            {
                await context.PostAsync("Please enter missing Date of Appointment information [Format: mm/dd/yyyy]");
                context.Wait(ProcessMissingDateInfo);
            }
            else if (timeslot == "")
            {
                await context.PostAsync("Please enter missing timeslot of Appointment information [Format: 17:00]");
                context.Wait(ProcessMissingTimeInfo);
            }
            else
            {
                await context.PostAsync("Please enter new timeslot of Appointment information [Format: 17:00]");
                context.Wait(ProcessNewTimeInfo);
            }
        }
        }

        //-----------------------------------------------------------Section for View Appointment or (s) --------------------------------------------------

        [LuisIntent("ViewAppointments")]
        public async Task ViewAppointments(IDialogContext context, LuisResult luisResponse)
        {
            context.ConversationData.TryGetValue("primaryamsid", out primary_amsid);
            if (primary_amsid == "" || primary_amsid == null)
            {
                await context.PostAsync("Please login to your AMS account: [Format: login username Ex: login nishanthpavurapu]");
                context.Done(true);
            }
            else
            {
                CurrentIntent = "ViewAppointments";
                for (int i = 0; i < luisResponse.Entities.Count(); i++)
                {
                    switch (luisResponse.Entities[i].Type)
                    {
                        case "builtin.datetime.date":
                            dateofappointment = luisResponse.Entities[i].Resolution["date"].ToString();
                            if (dateofappointment.Contains("W"))
                            {
                                string temp = dateofappointment;
                                temp = temp.Replace("W", "");
                                string[] date_strings = temp.Split('-');
                                DateTime dt = GetDateFromWeekNumberAndDayOfWeek(Convert.ToInt32(date_strings[0]), Convert.ToInt32(date_strings[1]), 1);
                                weekStartDate = dt.ToString("MM/dd/yyyy");
                                dt = dt.AddDays(4);
                                weekEndDate = dt.ToString("MM/dd/yyyy");
                            }
                            else if (dateofappointment.Length == 7)
                            {
                                if (dateofappointment.Contains("XXXX"))
                                {
                                    dateofappointment = dateofappointment.Replace("XXXX", "2017");
                                }
                                string[] date_split = dateofappointment.Split('-');
                                monthStartDate = date_split[1] + "/01/" + date_split[0];
                                DateTime endOfMonth = new DateTime(Convert.ToInt32(date_split[0]), Convert.ToInt32(date_split[1]),DateTime.DaysInMonth(Convert.ToInt32(date_split[0]), Convert.ToInt32(date_split[1])));
                                monthEndDate = endOfMonth.ToString("MM/dd/yyyy");
                                dateofappointment = "";
                            }
                            else
                            {
                                string date_Pattern = "yyyy-MM-dd";
                                DateTime dateParsed;
                                DateTime.TryParseExact(dateofappointment, date_Pattern, null, System.Globalization.DateTimeStyles.None, out dateParsed);
                                dateofappointment = dateParsed.ToString("MM/dd/yyyy");
                            }                            
                            break;

                        case "builtin.datetime.time":
                            timeslot = "";
                            timeslot = luisResponse.Entities[i].Resolution["time"].ToString();
                            timeslot = timeslot.Replace("T", "");
                            break;
                    }

                }
               // if (dateofappointment == "")
                //{
                 //   await context.PostAsync("Please enter missing Date of Appointment information [Format: mm/dd/yyyy]");
                  //  context.Wait(ProcessMissingDateInfo);
               // }
                //else
               // {
                  //  if (dateofappointment == "" || dateofappointment == null)
                    if(primary_amsid != null || primary_amsid != "")
                    {
                        if (dateofappointment == null || dateofappointment == "") {dateofappointment = "none";}
                        if (weekStartDate == null || weekStartDate == "") {weekStartDate = "none";}
                        if (weekEndDate == null || weekEndDate == "") {weekEndDate = "none";}
                        if (monthEndDate == null || monthEndDate == "") { monthEndDate = "none"; }
                        if (monthStartDate == null || monthStartDate == "") { monthStartDate = "none"; }

                    await context.PostAsync(await ViewAppointments(primary_amsid,dateofappointment,weekStartDate,weekEndDate,monthStartDate,monthEndDate));
                        context.Done(true);
                    }
                   // else
                   // {
                   //     await context.PostAsync(await ViewAppointment(primary_amsid, dateofappointment));
                    //    context.Done(true);
                   // }
                //}
            }
        }
        //-----------------------------------------------------------Section for Login LUIS Intent --------------------------------------------------

        [LuisIntent("LoginIntent")]
        public async Task LoginUser(IDialogContext context, LuisResult luisResponse)
        {
            CurrentIntent = "LoginIntent";
            for (int i = 0; i < luisResponse.Entities.Count(); i++)
            {
                switch (luisResponse.Entities[i].Type)
                {
                    case "amsuser::primaryuser":
                        primary_amsid = luisResponse.Entities[i].Entity;
                        context.ConversationData.SetValue("primaryamsid", primary_amsid);
                        break;
                }

            }
            if (primary_amsid == "")
            {
                await context.PostAsync($"Please provide me your AMS Username[Ex: nishanthpavurapu]");
                context.Wait(GetLoginUserName);
            }
            else
            {
                await context.PostAsync($"Please provide me password for AMS Username:{primary_amsid}");
                context.Wait(LoginUser);
            }
        }

        //-----------------------------------------------------------Section for Greetings LUIS Intent --------------------------------------------------

        [LuisIntent("Greetings")]
        public async Task GreetUser(IDialogContext context, LuisResult luisResponse)
        {
            context.ConversationData.TryGetValue("primaryamsid", out primary_amsid);
            if (primary_amsid == "" || primary_amsid == null)
            {
                await context.PostAsync("Hello, Welcome! \n\nPlease login to your AMS account: [Format: login username Ex: login nishanthpavurapu]");
                context.Done(true);
            }
            else
            {
                await context.PostAsync($"Hello Welcome. \n I provide below services, You can try one of them. \n\n 1. Will create a appointment for you \n 2. Will re-Schedule any of your appointment \n 3. Will cancel your appointments \n\n\n Thank you!");
                context.Done(true);
            }
        }

        //-----------------------------------------------------------Section for None LUIS Intent --------------------------------------------------

        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult luisResponse)
        {
            await context.PostAsync($"Sorry! I didn't get you. \n I provide below services, Please try one of them again. \n\n 1. Will create a appointment for you \n 2. Will re-Schedule any of your appointment \n 3. Will cancel your appointments \n\n\n Thank you!");
            context.Done(true);
        }

        //----------------------------------------------------------- Child Dialogs for user --------------------------------------------------------

        private async Task ProcessMissingSecondaryUserInfo(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var res = await result;
            secondary_amsid = res.Text;
            if (dateofappointment == "")
            {
                await context.PostAsync("Please enter missing Date of Appointment information [Format: mm/dd/yyyy]");
                context.Wait(ProcessMissingDateInfo);
            }
            else if (timeslot == "")
            {
                await context.PostAsync("Please enter missing timeslot of Appointment information [Format: 17:00]");
                context.Wait(ProcessMissingTimeInfo);
            }
            else
            {
                await context.PostAsync(await CreateAppointment(primary_amsid, secondary_amsid, dateofappointment, timeslot));
            }
        }

        private async Task ProcessMissingDateInfo(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var res = await result;
            dateofappointment = res.Text;
            if (timeslot == "" && CurrentIntent != "ViewAppointments")
            {
                await context.PostAsync("Please enter missing timeslot of Appointment information [Format: 17:00]");
                context.Wait(ProcessMissingTimeInfo);
            }
            else if (CurrentIntent == "CreateAppointment")
            {
                await context.PostAsync(await CreateAppointment(primary_amsid, secondary_amsid, dateofappointment, timeslot));
            }
            else if (CurrentIntent == "EditAppointment")
            {
                await context.PostAsync("Please enter new timeslot of Appointment information [Format: 17:00]");
                context.Wait(ProcessNewTimeInfo);
            }
            else if (CurrentIntent == "CancelAppointment")
            {
                await context.PostAsync(await CancelAppointment(primary_amsid, dateofappointment, timeslot));
            }
            //else if (CurrentIntent == "ViewAppointments")
            //{
            //    if (timeslot == "" || timeslot == null)
            //    {
            //        await context.PostAsync(await ViewAppointments(primary_amsid));
            //        context.Done(true);
            //    }
            //    else
            //    {
            //        await context.PostAsync(await ViewAppointment(primary_amsid, dateofappointment));
            //        context.Done(true);
            //    }
            //}
        }

        private async Task ProcessMissingTimeInfo(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var res = await result;
            timeslot = res.Text;
          //  context.Wait(MessageReceived);
            if (CurrentIntent == "CreateAppointment")
            {
                await context.PostAsync(await CreateAppointment(primary_amsid, secondary_amsid, dateofappointment, timeslot));
                context.Done(true);
            }
            else if (CurrentIntent == "EditAppointment")
            {
                await context.PostAsync("Please enter new timeslot of Appointment information [Format: 17:00]");
                context.Wait(ProcessNewTimeInfo);
            }
            else
            {
                await context.PostAsync(await CancelAppointment(primary_amsid, dateofappointment, timeslot));
                context.Done(true);
            }
        }

        private async Task ProcessNewTimeInfo(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var res = await result;
            newTimeSlot = res.Text;
            context.Wait(MessageReceived);
            await context.PostAsync(await UpdateAppointment(primary_amsid, dateofappointment,timeslot, newTimeSlot));
            context.Done(true);
        }

        private async Task LoginUser(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var res = await result;
            string password = res.Text;
            context.Wait(MessageReceived);
            await context.PostAsync(await LoginUser(primary_amsid, password));
            context.Done(true);
        }
        private async Task GetLoginUserName(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var res = await result;
            primary_amsid = res.Text;
            context.ConversationData.SetValue("primaryamsid", primary_amsid);
            context.Wait(MessageReceived);
            await context.PostAsync($"Please provide me password for AMS Username:{primary_amsid}");
            context.Wait(LoginUser);
        }

        //-----------------------------------------------------------Section for Calling AMS Services --------------------------------------------------

        private async Task<string> CreateAppointment(string primary_amsid, string secondary_amsid, string dateofappointment, string timeslot)
        {
            var result = await AmsBotService.CreateAppointment(primary_amsid, secondary_amsid, dateofappointment, timeslot);
            return result;
        }

        private async Task<string> CancelAppointment(string primary_amsid, string dateofappointment, string timeslot)
        {
            var result = await AmsBotService.CancelAppointment(primary_amsid, dateofappointment, timeslot);
            return result;
        }
        private async Task<string> UpdateAppointment(string primary_amsid, string dateofappointment, string timeslot,string newtimeslot)
        {
            var result = await AmsBotService.UpdateAppointment(primary_amsid, dateofappointment, timeslot,newtimeslot);
            return result;
        }
        private async Task<string> LoginUser(string primary_amsid, string password)
        {
            var result = await AmsBotService.LoginUser(primary_amsid, password);
            return result;
        }
        private async Task<string> ViewAppointment(string primary_amsid, string dateofappointment)
        {
            var result = await AmsBotService.ViewAppointment(primary_amsid, dateofappointment);
            return result;
        }
        private async Task<string> ViewAppointments(string primary_amsid, string dateofappointment, string weekstartdate, string weekenddate, string monthstartdate, string monthenddate)
        {
            var result = await AmsBotService.ViewAppointments(primary_amsid, dateofappointment, weekstartdate,weekEndDate,monthstartdate,monthenddate);
            return result;
        }


        //day of week - 0 (sunday), 1(monday)
        static DateTime GetDateFromWeekNumberAndDayOfWeek(int yearNumber, int weekNumber, int dayOfWeek)
        {
            int firstWeek;
            DateTime jan1 = new DateTime(yearNumber, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;

            if (dayOfWeek == 1)
            {
                firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            }
            else
            {
                firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Friday);
            }
            var weekNum = weekNumber;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

    }
}