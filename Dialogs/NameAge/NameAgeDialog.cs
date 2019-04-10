// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using StarterBot.Dialogs.CancelAndHelp;
using StarterBot.Dialogs.NameAge.Resources;
using StarterBot.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StarterBot.Dialogs.NameAge
{
    public class NameAgeDialog : CancelAndHelpDialog
    {
        private const string NamePromptName = "nameprompt";
        private const string AgePromptName = "ageprompt";
        private const string NameStepKey = "name";
        private const string AgeStepKey = "age";
        private IStatePropertyAccessor<GlobalUserState> _globalUserStateAccessor;

        public NameAgeDialog(UserState userState) : base(nameof(NameAgeDialog))
        {
            _globalUserStateAccessor = userState.CreateProperty<GlobalUserState>(nameof(GlobalUserState));

            // Add waterfall dialog steps
            var waterfallSteps = new WaterfallStep[]
            {
                SayHiAsync,
                AskNameAsync,
                HandleNameAsync,
                AskAgeAsync,
                HandleAgeAsync,
                SaveStateAsync,
                SummaryAsync,
                EndAsync,
            };

            // Child dialogs            
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            AddDialog(new TextPrompt(NamePromptName));
            AddDialog(new NumberPrompt<int>(AgePromptName));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> SayHiAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync($"{NameAgeStrings.Welcome}", cancellationToken: cancellationToken);

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> AskNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var state = await _globalUserStateAccessor.GetAsync(stepContext.Context, () => new GlobalUserState());

            //Check if user already provided country in DialogB and modify messages with provided info
            var msg = (string.IsNullOrEmpty(state.Country)) ?
                NameAgeStrings.WhatsName :
                String.Format(NameAgeStrings.WhatsName_Country, state.Country);

            return await stepContext.PromptAsync(NamePromptName, new PromptOptions { Prompt = MessageFactory.Text(msg) }, cancellationToken);
        }

        private async Task<DialogTurnResult> HandleNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values[NameStepKey] = (string)stepContext.Result;

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> AskAgeAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(AgePromptName, new PromptOptions { Prompt = MessageFactory.Text(string.Format($"What's your age {stepContext.Values["name"]}?")) }, cancellationToken);
        }

        private async Task<DialogTurnResult> HandleAgeAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values[AgeStepKey] = (int)stepContext.Result;

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> SaveStateAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Update state from stepContent
            var state = await _globalUserStateAccessor.GetAsync(stepContext.Context, () => new GlobalUserState());
            state.Name = (string)stepContext.Values[NameStepKey];
            state.Age = (int)stepContext.Values[AgeStepKey];

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> SummaryAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var state = await _globalUserStateAccessor.GetAsync(stepContext.Context, () => new GlobalUserState());

            //Check if user already provided country in DialogB and modify messages with provided info
            var msg = (string.IsNullOrEmpty(state.Country)) ?
                String.Format(NameAgeStrings.ThankYou_NameAge, state.Name, state.Age) :
                String.Format(NameAgeStrings.ThankYou_NameAgeCountry, state.Name, state.Age, state.Country);

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync();
        }
    }
}
