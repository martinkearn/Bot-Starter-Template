using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using StarterBot.Dialogs.DialogA.Resources;
using StarterBot.Models;
using System.Threading;
using System.Threading.Tasks;

namespace StarterBot.Dialogs.DialogA
{
    public class DialogADialog : ComponentDialog
    {
        private readonly IStatePropertyAccessor<UserProfile> _userProfileAccessor;

        public DialogADialog(UserState userState) : base(nameof(DialogADialog))
        {
            _userProfileAccessor = userState.CreateProperty<UserProfile>("UserProfile");

            InitialDialogId = nameof(DialogADialog);

            // Add waterfall dialog steps
            var waterfallSteps = new WaterfallStep[]
            {
                SayHiAskNameAsync,
                AskAgeAsync,
                EndAsync,
            };
            AddDialog(new WaterfallDialog(InitialDialogId, waterfallSteps));
            
            // Add Prompts
            AddDialog(new TextPrompt("namePrompt"));
            AddDialog(new NumberPrompt<int>("agePrompt"));
        }

        private async Task<DialogTurnResult> SayHiAskNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync($"{DialogAStrings.Welcome}", cancellationToken: cancellationToken);

            // Get the current profile object from user state.
            var userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);

            //Check if user already provided country in DialogB and modify messages with provided info
            var msg = (string.IsNullOrEmpty(userProfile.Country)) ? "What's your name?" : $"You already provided your country: {userProfile.Country}, what's your name?";

            return await stepContext.PromptAsync("namePrompt", new PromptOptions { Prompt = MessageFactory.Text(msg) }, cancellationToken);
        }

        private async Task<DialogTurnResult> AskAgeAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["name"] = (string)stepContext.Result;

            return await stepContext.PromptAsync("agePrompt", new PromptOptions { Prompt = MessageFactory.Text(string.Format($"What's your age {stepContext.Values["name"]}?")) }, cancellationToken);
        }

        private async Task<DialogTurnResult> EndAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            stepContext.Values["age"] = (int)stepContext.Result;

            // Get the current profile object from user state.
            var userProfile = await _userProfileAccessor.GetAsync(stepContext.Context, () => new UserProfile(), cancellationToken);
            userProfile.Name = (string)stepContext.Values["name"];
            userProfile.Age = (int)stepContext.Values["age"];


            //Check if user already provided country in DialogB and modify messages with provided info
            var msg = (string.IsNullOrEmpty(userProfile.Country)) ? $"Thank you {userProfile.Name} for providing your age {userProfile.Age}." : $"Thank you {userProfile.Name}, {userProfile.Age} from {userProfile.Country} for providing your information.";
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(msg), cancellationToken);

            return await stepContext.EndDialogAsync();
        }
    }
}
