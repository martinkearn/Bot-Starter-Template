using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using StarterBot.Dialogs.DialogB.Resources;
using System.Threading;
using System.Threading.Tasks;

namespace StarterBot.Dialogs.DialogB
{
    public class DialogBDialog : ComponentDialog
    {
        public DialogBDialog(UserState userState) : base(nameof(DialogBDialog))
        {
            InitialDialogId = nameof(DialogBDialog);

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
            await stepContext.Context.SendActivityAsync($"{DialogBStrings.Welcome}", cancellationToken: cancellationToken);
            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync();
        }
    }
}
