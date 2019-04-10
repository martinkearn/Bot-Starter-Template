using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using StarterBot.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterBot.Dialogs.Luis
{
    public class LuisDialog : ComponentDialog
    {
        private LuisModel _luisModel;

        public LuisDialog(UserState userState) : base(nameof(LuisDialog))
        {
            InitialDialogId = nameof(LuisDialog);

            // Add child dialogs
            var waterfallSteps = new WaterfallStep[]
            {
                GetModelAsync,
                EndAsync,
            };

            AddDialog(new WaterfallDialog(InitialDialogId, waterfallSteps));
        }

        private async Task<DialogTurnResult> GetModelAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Read the Luis results from the calling dialog
            _luisModel = (LuisModel)stepContext.Options;

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync();
        }
    }
}
