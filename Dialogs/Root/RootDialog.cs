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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StarterBot.Dialogs.Root
{
    public class RootDialog : ComponentDialog
    {
        private const string ChoicePromptName = "choiceprompt";
        private const string ChoicePromptDialogA = "Dialog A";
        private const string ChoicePromptDialogB = "Dialog B";
        private IBotServices _botServices;

        public RootDialog(IBotServices botServices, UserState userState) : base(nameof(RootDialog))
        {
            InitialDialogId = nameof(RootDialog);
            _botServices = botServices;

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

            // If required, get the utterance like this: 
            var utterance = (string)stepContext.Options;

            #region Dispatch exmaple
            //// This is a sample for calling Dispatch accross Luis and QNA via BotServices for getting top level intent and answering directly if QNA
            //var dispatchResults = await _botServices.Dispatch.RecognizeAsync(stepContext.Context, cancellationToken);
            //var dispatchTopScoringIntent = dispatchResults?.GetTopScoringIntent();
            //var dispatchTopIntent = dispatchTopScoringIntent.Value.intent;
            //switch (dispatchTopIntent)
            //{
            //    case "luismodelname":

            //        // Look at generating a strongly typed LuisModel using LuisGen. Use it as follows:
            //        //var luisResultModel = await _botServices.MainLuis.RecognizeAsync<LuisModel>(stepContext.Context, CancellationToken.None);

            //        // Typically you can start a new dialog here to process the luis result you can pass the luisResultModel in as follows
            //        // return await stepContext.BeginDialogAsync(nameof(LuisDialog), luisResultModel);

            //        // You can read the luisResultModel in the LuisDialog as follows:
            //        // _luisModel = (RecognizerResult)stepContext.Options;

            //        break;

            //    case "qnamodelname":
            //        var results = await _botServices.MainQnA.GetAnswersAsync(stepContext.Context);
            //        if (results.Any())
            //        {
            //            await stepContext.Context.SendActivityAsync(results.First().Answer, cancellationToken: cancellationToken);
            //        }
            //        else
            //        {
            //            await stepContext.Context.SendActivityAsync(RootStrings.NoQNAAnswer);
            //        }
            //        break;

            //    case "None":
            //        await stepContext.Context.SendActivityAsync(RootStrings.NoIntent);
            //        break;

            //    default:
            //        await stepContext.Context.SendActivityAsync(RootStrings.NoIntent);
            //        break;
            //}
            #endregion

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
