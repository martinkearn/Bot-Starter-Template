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
        private readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;

        public DialogBDialog(UserState userState) : base(nameof(DialogBDialog))
        {
            // Define user userProfile State
            _userProfileAccessor = userState.CreateProperty<UserProfile>("UserProfile");

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
            var userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);

            //Check if user already provided Name and Age in DialogA and modify messages with provided info
            var msg = (string.IsNullOrEmpty(userProfile.Name) && userProfile.Age == 0) ? $"{DialogBStrings.Welcome}" : $"{DialogBStrings.Welcome}, {userProfile.Name}";
            await stepContext.Context.SendActivityAsync(msg, cancellationToken: cancellationToken);

            var promptmsg = (string.IsNullOrEmpty(userProfile.Name) && userProfile.Age == 0) ? $"Where are you from?" : $"Where are you from {userProfile.Name}?";
            return await stepContext.PromptAsync("countryPrompt", new PromptOptions { Prompt = MessageFactory.Text(string.Format(promptmsg))}, cancellationToken);
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["country"] = (string)stepContext.Result;

            // Get the current profile object from user state.
            var userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);
            userProfile.Country = (string)stepContext.Values["country"];

            //Check if user already provided Name and Age in DialogA and modify messages with provided info
            var msg = (string.IsNullOrEmpty(userProfile.Name) && userProfile.Age == 0) ? $"Thank you for your providing your country as {userProfile.Country}" : $"Thank you {userProfile.Name}, {userProfile.Age} from {userProfile.Country} for providing your information.";

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);

            return await stepContext.EndDialogAsync();
        }
    }
}
