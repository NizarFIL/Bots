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
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Hi! I'm Botty. I do not do much now, but I'll reply with a random number when you send me a message.");
            //context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // get random number
            Random rnd = new Random();
            int number = rnd.Next(1000);

            // return our reply to the user
            await context.PostAsync($"{number}");

            context.Wait(MessageReceivedAsync);
        }
    }
}