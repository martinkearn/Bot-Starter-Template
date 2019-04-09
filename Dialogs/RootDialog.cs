using StarterBot.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterBot.Dialogs
{
    public class RootDialog : ComponentDialog
    {

        private IStrings _strings;

        public RootDialog(UserState userState, IConfiguration configuration, IStrings strings)
            : base(nameof(RootDialog))
        {

            InitialDialogId = nameof(RootDialog);
            _strings = strings;

            // Define the steps of the waterfall dialog and add it to the set.
            var waterfallSteps = new WaterfallStep[]
            {
                SayHiAsync,
                EndAsync,
            };

            AddDialog(new WaterfallDialog(InitialDialogId, waterfallSteps));

            // Child dialogs
            // have not got any child dialogs yet
        }

        private async Task<DialogTurnResult> SayHiAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //var utterance = (string)stepContext.Options;

            await stepContext.Context.SendActivityAsync($"Hi from RootDialog.SayHiAsync", cancellationToken: cancellationToken);

            var someString = await _strings.GetString("foo", stepContext.Context.Activity.Locale);
            await stepContext.Context.SendActivityAsync($"{someString}", cancellationToken: cancellationToken);


            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

            return await stepContext.EndDialogAsync();
        }
    }
}
