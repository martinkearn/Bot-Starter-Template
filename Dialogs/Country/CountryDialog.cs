using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using StarterBot.Dialogs.CancelAndHelp;
using StarterBot.Dialogs.Country.Resources;
using StarterBot.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StarterBot.Dialogs.Country
{
    public class CountryDialog : CancelAndHelpDialog
    {
        private const string CountryPromptName = "countryprompt";
        private const string CountryStepKey = "country";
        private IStatePropertyAccessor<GlobalUserState> _globalUserStateAccessor;

        public CountryDialog(UserState userState) : base(nameof(CountryDialog))
        {
            _globalUserStateAccessor = userState.CreateProperty<GlobalUserState>(nameof(GlobalUserState));

            InitialDialogId = nameof(CountryDialog);

            // Add waterfall dialog steps
            var waterfallSteps = new WaterfallStep[]
            {
                SayHiAsync,
                AskCountryAsync,
                HandleCountryAsync,
                SaveStateAsync,
                SummaryAsync,
                EndAsync,
            };
            AddDialog(new WaterfallDialog(InitialDialogId, waterfallSteps));

            // Add Prompts
            AddDialog(new TextPrompt(CountryPromptName));
        }

        private async Task<DialogTurnResult> SayHiAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var state = await _globalUserStateAccessor.GetAsync(stepContext.Context, () => new GlobalUserState());

            //Check if user already provided Name and Age in DialogA and modify messages with provided info
            var msg = (string.IsNullOrEmpty(state.Name) && state.Age == 0) ? 
                CountryStrings.Welcome : 
                String.Format(CountryStrings.Welcome_Name, state.Name);
            await stepContext.Context.SendActivityAsync(msg, cancellationToken: cancellationToken);

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> AskCountryAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var state = await _globalUserStateAccessor.GetAsync(stepContext.Context, () => new GlobalUserState());

            var promptmsg = (string.IsNullOrEmpty(state.Name) && state.Age == 0) ?
                CountryStrings.WhereFrom :
                String.Format(CountryStrings.Welcome_Name, state.Name);

            return await stepContext.PromptAsync(CountryPromptName, new PromptOptions { Prompt = MessageFactory.Text(string.Format(promptmsg))}, cancellationToken);
        }

        private async Task<DialogTurnResult> HandleCountryAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values[CountryStepKey] = (string)stepContext.Result;

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> SaveStateAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var state = await _globalUserStateAccessor.GetAsync(stepContext.Context, () => new GlobalUserState());
            state.Country = (string)stepContext.Values[CountryStepKey];

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> SummaryAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var state = await _globalUserStateAccessor.GetAsync(stepContext.Context, () => new GlobalUserState());

            //Check if user already provided Name and Age in DialogA and modify messages with provided info
            var msg = (string.IsNullOrEmpty(state.Name) && state.Age == 0) ?
                String.Format(CountryStrings.ThankYou_Country, state.Country) :
                String.Format(CountryStrings.ThankYou_Country, state.Name, state.Age, state.Country);

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync();
        }
    }
}
