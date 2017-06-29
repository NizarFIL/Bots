namespace TechBot.Dialogs
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using System.Net;
    using System.Xml.Linq;
    using System.Linq;

    [Serializable]
    public class MailboxQuotaDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            // await context.PostAsync("Ok, let's chack you current quota first.");
            await context.PostAsync("What is ythe ID?");
            context.Wait(this.MessageReceivedAsync);

            return;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // calculate something for us to return
            int length = (activity.Text ?? string.Empty).Length;

            await context.PostAsync($"Please wait while I check your mailbox details...");

            //Get Quota
            var client = new WebClient();
            var url = "https://bot.tadpolelab.com:8443/SmartObjectServices/rest/Test/TestingSMO/Load?ID=" + activity.Text;

            client.Headers[HttpRequestHeader.ContentType] = "application/xml";
            client.Headers[HttpRequestHeader.Authorization] = "Basic azItYm90XGxvY2FsYWRtaW46SzI0ZG0xbk4xejRyIQ=="; //k2-bot\localadmin; 

            var responseString = client.DownloadString(url);

            var xDoc = XDocument.Parse(responseString);

            var name = (from e in xDoc.Descendants("Name") select e.Value).SingleOrDefault();
            
            // return our reply to the user
            await context.PostAsync($"The name is {name}");

        }
    }
}



