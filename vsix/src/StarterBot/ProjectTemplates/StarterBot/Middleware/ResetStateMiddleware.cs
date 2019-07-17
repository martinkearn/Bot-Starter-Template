using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using $safeprojectname$.Interfaces;
using $safeprojectname$.Middleware.Resources;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace $safeprojectname$.Middleware
{
    public class ResetStateMiddleware : IMiddleware
    {
        private readonly UserState _userState;
        private readonly ConversationState _conversationState;
        private readonly List<string> _resetPhrases = new List<string>() { "clear state", "reset" };

        public ResetStateMiddleware(IBotServices botServices, UserState userState, ConversationState conversationState)
        {
            _userState = userState ?? throw new NullReferenceException(nameof(userState));
            _conversationState = conversationState ?? throw new NullReferenceException(nameof(conversationState));
        }

        public async Task OnTurnAsync(ITurnContext turnContext, NextDelegate next, CancellationToken cancellationToken = default)
        {
            if (turnContext == null)
            {
                throw new ArgumentNullException(nameof(turnContext));
            }

            if (turnContext.Activity.Type == ActivityTypes.Message)
            {
                if (_resetPhrases.Contains(turnContext.Activity.Text?.ToLowerInvariant()))
                {
                    await _conversationState.ClearStateAsync(turnContext);
                    await _userState.ClearStateAsync(turnContext);

                    await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
                    await _userState.SaveChangesAsync(turnContext, false, cancellationToken);

                    await turnContext.SendActivityAsync(MessageFactory.Text(MiddlewareStrings.Reset, MiddlewareStrings.ResetSpeak), cancellationToken);
                    return;
                }
            }
            await next(cancellationToken).ConfigureAwait(false);
        }
    }
}
