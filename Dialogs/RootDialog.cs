using StarterBot.Interfaces;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Builder.Dialogs.Choices;

namespace StarterBot.Dialogs
{
    public class RootDialog : ComponentDialog
    {
        private const string ChoicePromptName = "choiceprompt";
        private const string ChoicePromptDialogA = "Dialog A";
        private const string ChoicePromptDialogB = "Dialog B";

        private IStrings _strings;

        public RootDialog(UserState userState, IConfiguration configuration, IStrings strings) : base(nameof(RootDialog))
        {
            InitialDialogId = nameof(RootDialog);
            _strings = strings;

            // Define the steps of the waterfall dialog and add it to the set.
            var waterfallSteps = new WaterfallStep[]
            {
                SayHiAsync,
                PromptForFlowAsync,
                HandleFlowResultAsync,
                EndAsync,
            };

            // Child dialogs
            AddDialog(new WaterfallDialog(InitialDialogId, waterfallSteps));
            AddDialog(new ChoicePrompt(ChoicePromptName));
            AddDialog(new DialogA(userState, strings));
            AddDialog(new DialogB(userState, strings));
        }

        private async Task<DialogTurnResult> SayHiAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync($"{await _strings.GetString("rootdialogwelcome")}", cancellationToken: cancellationToken);

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> PromptForFlowAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(ChoicePromptName,
                new PromptOptions
                {
                    Choices = ChoiceFactory.ToChoices(new List<string> { ChoicePromptDialogA, ChoicePromptDialogB }),
                    Prompt = MessageFactory.Text(await _strings.GetString("whichflow")),
                    RetryPrompt = MessageFactory.Text(await _strings.GetString("invalidresponse"))

                },
                cancellationToken).ConfigureAwait(false);
        }

        private async Task<DialogTurnResult> HandleFlowResultAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var result = ((FoundChoice)stepContext.Result).Value;
            switch (result)
            {
                case ChoicePromptDialogA:
                    return await stepContext.BeginDialogAsync(nameof(DialogA));
                case ChoicePromptDialogB:
                    return await stepContext.BeginDialogAsync(nameof(DialogB));
                default:
                    return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            //return await stepContext.EndDialogAsync();
            // Restart the root dialog
            return await stepContext.ReplaceDialogAsync(InitialDialogId).ConfigureAwait(false);
        }
    }
}
