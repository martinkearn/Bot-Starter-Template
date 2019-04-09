using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
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
        private readonly IBotServices _botServices;

        public RootDialog(IBotServices botServices, UserState userState) : base(nameof(RootDialog))
        {
            InitialDialogId = nameof(RootDialog);
            _botServices = botServices;

            // Define the steps of the waterfall dialog and add it to the set.
            var waterfallSteps = new WaterfallStep[]
            {
                SayHiAsync,
                GetIntentAsync,
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
            // If required, get the utterance like this: 
            var utterance = (string)stepContext.Options;

            await stepContext.Context.SendActivityAsync($"{RootStrings.Welcome}", cancellationToken: cancellationToken);

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> GetIntentAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            #region Dispatch example
            //// You can delete this whole 'Dispatch example' region if you are not using Dispatch, Luis or QNA. (You should also delete Models/LuisModel.cs and Dialogs/Luis/LuisDialog.cs if you are not using Luis)
            //// This is a sample for calling Dispatch accross Luis and QNA via BotServices for getting top level intent and answering directly if QNA.
            //var dispatchResults = await _botServices.Dispatch.RecognizeAsync(stepContext.Context, cancellationToken);
            //var dispatchTopScoringIntent = dispatchResults?.GetTopScoringIntent();
            //var dispatchTopIntent = dispatchTopScoringIntent.Value.intent;
            //switch (dispatchTopIntent)
            //{
            //    case "luismodelname":
            //        // Call the main luis model to get detailed intent and entity results. Dispatch will only provide top level model
            //        var luisResultModel = await _botServices.MainLuis.RecognizeAsync<LuisModel>(stepContext.Context, CancellationToken.None);
            //        // Start a new dialog here and can pass the luisResultModel in to process the luis result 
            //        return await stepContext.BeginDialogAsync(nameof(LuisDialog), luisResultModel);

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
                    Choices = ChoiceFactory.ToChoices(new List<string> { RootStrings.DialogAPrompt, RootStrings.DialogBPrompt }),
                    Prompt = MessageFactory.Text(RootStrings.WhichFlowPrompt),
                    RetryPrompt = MessageFactory.Text(SharedStrings.InvalidResponseToChoicePrompt)
                },
                cancellationToken).ConfigureAwait(false);
        }

        private async Task<DialogTurnResult> HandleFlowResultAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var result = ((FoundChoice)stepContext.Result).Value;

            // A switch statement would be better but that requires a constant so not a RESX value.
            if (result == RootStrings.DialogAPrompt)
            {
                return await stepContext.BeginDialogAsync(nameof(DialogADialog));
            }
            else if (result == RootStrings.DialogBPrompt)
            {
                return await stepContext.BeginDialogAsync(nameof(DialogBDialog));
            }
            else
            {
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
