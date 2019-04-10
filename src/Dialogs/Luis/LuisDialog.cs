// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

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
            // Add waterfall dialog steps
            var waterfallSteps = new WaterfallStep[]
            {
                GetModelAsync,
                EndAsync,
            };

            // Child dialogs
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
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
