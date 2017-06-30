using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;
using System.Net;
using System.Xml.Linq;
using System.Linq;

namespace TechBot.Dialogs
{
    [LuisModel("e7b5f21e-222a-41e3-aee9-07909a845e6a", "09775384b50d489e8d33906f8aa00fa8")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {
        //[LuisIntent("")]
        //[LuisIntent("None")]
        //public async Task None(IDialogContext context, LuisResult result)
        //{
        //    string message = $"Sorry, I did not understand '{result.Query}'. Type 'help' if you need assistance.";

        //    await context.PostAsync("Sorry I didn't get that..");

        //    context.Wait(this.MessageReceived);
        //}

        [LuisIntent("Mail.IncreaseQuota")]
        public async Task IncreaseQuota(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
            await context.PostAsync("You want to increase quota");

            //***************************

            //context.Call(new MailboxQuotaDialog(), ResumeAfterOptionDialog);

            //***************************
            context.Wait(this.MessageReceived);
        }

        [LuisIntent("Access.OnlineMeetingService")]
        public async Task AccessOnlineMeetingService(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
            await context.PostAsync("You want to access an online meeting");
           // await context.PostAsync($"Please wait while I check your mailbox details...");

            //Get Quota
            var client = new WebClient();
            var url = "https://bot.tadpolelab.com:8443/SmartObjectServices/rest/Test/TestingSMO/Load?ID=1";

            //await context.PostAsync($"Please wait while I check your mailbox details...");

            client.Headers[HttpRequestHeader.ContentType] = "application/xml";
            client.Headers[HttpRequestHeader.Authorization] = "Basic azItYm90XGxvY2FsYWRtaW46SzI0ZG0xbk4xejRyIQ=="; //k2-bot\localadmin; 

            var responseString = client.DownloadString(url);

            var xDoc = XDocument.Parse(responseString);

            var name = (from e in xDoc.Descendants("Name") select e.Value).SingleOrDefault();

            // return our reply to the user
            await context.PostAsync($"The name is {name}");
            context.Wait(this.MessageReceived);
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                context.Wait(MessageReceived);
            }
        }
    }
}