// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using $safeprojectname$.Dialogs.CancelAndHelp;
using $safeprojectname$.Dialogs.Country.Resources;
using $safeprojectname$.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace $safeprojectname$.Dialogs.Country
{
    public class CountryDialog : CancelAndHelpDialog
    {
        private const string CountryPromptName = "countryprompt";
        private const string CountryStepKey = "country";
        private IStatePropertyAccessor<GlobalUserState> _globalUserStateAccessor;

        public CountryDialog(UserState userState) : base(nameof(CountryDialog))
        {
            _globalUserStateAccessor = userState.CreateProperty<GlobalUserState>(nameof(GlobalUserState));

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

            // Child dialogs
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new TextPrompt(CountryPromptName));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
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

            // Construct prompt
            var msg = (string.IsNullOrEmpty(state.Name) && state.Age == 0) ?
                CountryStrings.WhereFrom :
                String.Format(CountryStrings.WhereFrom_Name, state.Name);
            var promptOptions = new PromptOptions { Prompt = MessageFactory.Text(string.Format(msg)) };

            return await stepContext.PromptAsync(CountryPromptName, promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> HandleCountryAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Store the result in stepcontext
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

            // Simulate long running task (which will display the progress indicator through middleware)
            await Task.Delay(3000);

            //Check if user already provided Name and Age in DialogA and modify messages with provided info
            var msg = (string.IsNullOrEmpty(state.Name) && state.Age == 0) ?
                String.Format(CountryStrings.ThankYou_Country, state.Country) :
                String.Format(CountryStrings.ThankYou_NameAgeCountry, state.Name, state.Age, state.Country);

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }
    }
}
