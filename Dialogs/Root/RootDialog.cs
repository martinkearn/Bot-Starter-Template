using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Extensions.Configuration;
using StarterBot.Dialogs.DialogA;
using StarterBot.Dialogs.DialogB;
using StarterBot.Dialogs.Root.Resources;
using StarterBot.Interfaces;
using StarterBot.Resources;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace StarterBot.Dialogs.Root
{
    public class RootDialog : ComponentDialog
    {
        private const string ChoicePromptName = "choiceprompt";
        private const string ChoicePromptDialogA = "Dialog A";
        private const string ChoicePromptDialogB = "Dialog B";

        public RootDialog(UserState userState, IConfiguration configuration) : base(nameof(RootDialog))
        {
            InitialDialogId = nameof(RootDialog);

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
            AddDialog(new DialogADialog(userState));
            AddDialog(new DialogBDialog(userState));
        }

        private async Task<DialogTurnResult> SayHiAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync($"{RootStrings.Welcome}", cancellationToken: cancellationToken);

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> PromptForFlowAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(ChoicePromptName,
                new PromptOptions
                {
                    Choices = ChoiceFactory.ToChoices(new List<string> { ChoicePromptDialogA, ChoicePromptDialogB }),
                    Prompt = MessageFactory.Text(RootStrings.WhichFlowPrompt),
                    RetryPrompt = MessageFactory.Text(SharedStrings.InvalidResponseToChoicePrompt)
                },
                cancellationToken).ConfigureAwait(false);
        }

        private async Task<DialogTurnResult> HandleFlowResultAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var result = ((FoundChoice)stepContext.Result).Value;
            switch (result)
            {
                case ChoicePromptDialogA:
                    return await stepContext.BeginDialogAsync(nameof(DialogADialog));
                case ChoicePromptDialogB:
                    return await stepContext.BeginDialogAsync(nameof(DialogBDialog));
                default:
                    return await stepContext.NextAsync(cancellationToken: cancellationToken);
            }
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Restart the root dialog
            return await stepContext.ReplaceDialogAsync(InitialDialogId).ConfigureAwait(false);
        }
    }
}
