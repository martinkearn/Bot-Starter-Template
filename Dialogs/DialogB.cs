using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using StarterBot.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace StarterBot.Dialogs
{
    public class DialogB : ComponentDialog
    {
        private IStrings _strings;

        public DialogB(UserState userState, IStrings strings) : base(nameof(DialogB))
        {
            InitialDialogId = nameof(DialogB);
            _strings = strings;

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
            await stepContext.Context.SendActivityAsync($"{await _strings.GetString("dialogbwelcome")}", cancellationToken: cancellationToken);
            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync();
        }
    }
}
