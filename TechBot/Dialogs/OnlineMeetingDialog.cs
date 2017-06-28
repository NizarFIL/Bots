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
    public class OnlineMeetingDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("You want to access an onlime meeting service");
        }

    }
}
