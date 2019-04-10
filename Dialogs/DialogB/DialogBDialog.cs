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
                SayHiAndAskCountryAsync,
                EndAsync,
            };
            AddDialog(new WaterfallDialog(InitialDialogId, waterfallSteps));

            // Add Prompts
            AddDialog(new TextPrompt("countryPrompt"));
        }

        private async Task<DialogTurnResult> SayHiAndAskCountryAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            // Get the current profile object from user state.
            var state = await _globalUserStateAccessor.GetAsync(stepContext.Context, () => new GlobalUserState());

            //Check if user already provided Name and Age in DialogA and modify messages with provided info
            var msg = (string.IsNullOrEmpty(state.Name) && state.Age == 0) ? $"{DialogBStrings.Welcome}" : $"{DialogBStrings.Welcome}, {state.Name}";
            await stepContext.Context.SendActivityAsync(msg, cancellationToken: cancellationToken);

            var promptmsg = (string.IsNullOrEmpty(state.Name) && state.Age == 0) ? $"Where are you from?" : $"Where are you from {state.Name}?";
            return await stepContext.PromptAsync("countryPrompt", new PromptOptions { Prompt = MessageFactory.Text(string.Format(promptmsg))}, cancellationToken);
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["country"] = (string)stepContext.Result;

            // Get the current profile object from user state.
            var state = await _globalUserStateAccessor.GetAsync(stepContext.Context, () => new GlobalUserState());
            state.Country = (string)stepContext.Values["country"];

            //Check if user already provided Name and Age in DialogA and modify messages with provided info
            var msg = (string.IsNullOrEmpty(state.Name) && state.Age == 0) ? $"Thank you for your providing your country as {state.Country}" : $"Thank you {state.Name}, {state.Age} from {state.Country} for providing your information.";

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);

            return await stepContext.EndDialogAsync();
        }
    }
}
