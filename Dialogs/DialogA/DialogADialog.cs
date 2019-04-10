﻿using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using StarterBot.Dialogs.CancelAndHelp;
using StarterBot.Dialogs.DialogA.Resources;
using StarterBot.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterBot.Dialogs.DialogA
{
    public class DialogADialog : CancelAndHelpDialog
    {
        private IStatePropertyAccessor<GlobalUserState> _globalUserStateAccessor;

        public DialogADialog(UserState userState) : base(nameof(DialogADialog))
        {
            _globalUserStateAccessor = userState.CreateProperty<GlobalUserState>(nameof(GlobalUserState));

            InitialDialogId = nameof(DialogADialog);

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
            AddDialog(new WaterfallDialog(InitialDialogId, waterfallSteps));
            
            // Add Prompts
            AddDialog(new TextPrompt("namePrompt"));
            AddDialog(new NumberPrompt<int>("agePrompt"));
        }

        private async Task<DialogTurnResult> SayHiAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync($"{DialogAStrings.Welcome}", cancellationToken: cancellationToken);

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> AskNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var state = await _globalUserStateAccessor.GetAsync(stepContext.Context, () => new GlobalUserState());

            //Check if user already provided country in DialogB and modify messages with provided info
            var msg = (string.IsNullOrEmpty(state.Country)) ? 
                "What's your name?" : 
                $"You already provided your country: {state.Country}, what's your name?";

            return await stepContext.PromptAsync("namePrompt", new PromptOptions { Prompt = MessageFactory.Text(msg) }, cancellationToken);
        }

        private async Task<DialogTurnResult> HandleNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["name"] = (string)stepContext.Result;

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> AskAgeAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync("agePrompt", new PromptOptions { Prompt = MessageFactory.Text(string.Format($"What's your age {stepContext.Values["name"]}?")) }, cancellationToken);
        }

        private async Task<DialogTurnResult> HandleAgeAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["age"] = (int)stepContext.Result;

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> SaveStateAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Update state from stepContent
            var state = await _globalUserStateAccessor.GetAsync(stepContext.Context, () => new GlobalUserState());
            state.Name = (string)stepContext.Values["name"];
            state.Age = (int)stepContext.Values["age"];

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> SummaryAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var state = await _globalUserStateAccessor.GetAsync(stepContext.Context, () => new GlobalUserState());

            //Check if user already provided country in DialogB and modify messages with provided info
            var msg = (string.IsNullOrEmpty(state.Country)) ? $"Thank you {state.Name} for providing your age {state.Age}." : $"Thank you {state.Name}, {state.Age} from {state.Country} for providing your information.";
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync();
        }
    }
}
