using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using StarterBot.Dialogs.DialogA.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace StarterBot.Dialogs.DialogA
{
    public class DialogADialog : ComponentDialog
    {
        public DialogADialog(UserState userState) : base(nameof(DialogADialog))
        {
            InitialDialogId = nameof(DialogADialog);

            // Add child dialogs
            var waterfallSteps = new WaterfallStep[]
            {
                SayHiAsync,
                EndAsync,
            };

            AddDialog(new WaterfallDialog(InitialDialogId, waterfallSteps));
        }

        private async Task<DialogTurnResult> SayHiAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync($"{DialogAStrings.Welcome}", cancellationToken: cancellationToken);
            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync();
        }
    }
}
