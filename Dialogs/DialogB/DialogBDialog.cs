using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using StarterBot.Dialogs.CancelAndHelp;
using StarterBot.Dialogs.DialogB.Resources;
using StarterBot.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterBot.Dialogs.DialogB
{
    public class DialogBDialog : CancelAndHelpDialog
    {
        private IStatePropertyAccessor<GlobalUserState> _globalUserStateAccessor;

        public DialogBDialog(UserState userState) : base(nameof(DialogBDialog))
        {
            _globalUserStateAccessor = userState.CreateProperty<GlobalUserState>(nameof(GlobalUserState));

            InitialDialogId = nameof(DialogBDialog);

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
            AddDialog(new TextPrompt("countryPrompt"));
        }

        private async Task<DialogTurnResult> SayHiAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var state = await _globalUserStateAccessor.GetAsync(stepContext.Context, () => new GlobalUserState());

            //Check if user already provided Name and Age in DialogA and modify messages with provided info
            var msg = (string.IsNullOrEmpty(state.Name) && state.Age == 0) ? 
                $"{DialogBStrings.Welcome}" : 
                $"{DialogBStrings.Welcome}, {state.Name}";
            await stepContext.Context.SendActivityAsync(msg, cancellationToken: cancellationToken);

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> AskCountryAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var state = await _globalUserStateAccessor.GetAsync(stepContext.Context, () => new GlobalUserState());

            var promptmsg = (string.IsNullOrEmpty(state.Name) && state.Age == 0) ? 
                $"Where are you from?" : 
                $"Where are you from {state.Name}?";

            return await stepContext.PromptAsync("countryPrompt", new PromptOptions { Prompt = MessageFactory.Text(string.Format(promptmsg))}, cancellationToken);
        }

        private async Task<DialogTurnResult> HandleCountryAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["country"] = (string)stepContext.Result;

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> SaveStateAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var state = await _globalUserStateAccessor.GetAsync(stepContext.Context, () => new GlobalUserState());
            state.Country = (string)stepContext.Values["country"];

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> SummaryAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var state = await _globalUserStateAccessor.GetAsync(stepContext.Context, () => new GlobalUserState());

            //Check if user already provided Name and Age in DialogA and modify messages with provided info
            var msg = (string.IsNullOrEmpty(state.Name) && state.Age == 0) ?
                $"Thank you for your providing your country as {state.Country}" :
                $"Thank you {state.Name}, {state.Age} from {state.Country} for providing your information.";

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);

            return await stepContext.NextAsync(cancellationToken: cancellationToken);
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.EndDialogAsync();
        }
    }
}
