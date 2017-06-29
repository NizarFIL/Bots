using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Collections.Generic;

namespace TechBot.Dialogs
{
    //[Serializable]
    //public class RootDialog : IDialog<object>
    //{
    //    public async Task StartAsync(IDialogContext context)
    //    {
    //        await context.PostAsync("Hello !");
    //        context.Wait(MessageReceivedAsync);

    //        return;
    //    }

    //    private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
    //    {

    //        await context.PostAsync("Hello !!!");

    //        context.Wait(MessageReceivedAsync);
    //    }
    //}

    [LuisModel("e7b5f21e-222a-41e3-aee9-07909a845e6a", "54b3a614d45047a5a53a46fe644be1b3")]
    [Serializable]
    public class RootDialog : LuisDialog<object>
    {

        [LuisIntent("None")]
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I'm sorry. I didn't understand you.");
            context.Wait(MessageReceived);
        }

        [LuisIntent("IncreaseQuota")]
        public async Task IncreaseQuota(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;

            await context.PostAsync("You want to increase quota");

        }


        [LuisIntent("Access.OnlineMeetingService")]
        public async Task AccessOnlineMeetingService(IDialogContext context, IAwaitable<IMessageActivity> activity, LuisResult result)
        {
            var message = await activity;
            await context.PostAsync("You want to access an online meeting");

        }

    }
}