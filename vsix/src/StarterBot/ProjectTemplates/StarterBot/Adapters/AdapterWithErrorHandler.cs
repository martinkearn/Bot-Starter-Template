using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Logging;
using $safeprojectname$.Middleware;
using System;

namespace $safeprojectname$.Adapters
{
    public class AdapterWithErrorHandler : BotFrameworkHttpAdapter
    {
        public AdapterWithErrorHandler(ICredentialProvider credentialProvider, 
            ILogger<BotFrameworkHttpAdapter> logger, 
            ResetStateMiddleware resetStateMiddleware,
            ShowTypingMiddleware showTypingMiddleware,
            ConversationState conversationState = null)
          : base(credentialProvider)
        {
            if (credentialProvider == null)
            {
                throw new NullReferenceException(nameof(credentialProvider));
            }

            if (logger == null)
            {
                throw new NullReferenceException(nameof(logger));
            }

            if (showTypingMiddleware == null)
            {
                throw new NullReferenceException(nameof(showTypingMiddleware));
            }

            if (resetStateMiddleware == null)
            {
                throw new NullReferenceException(nameof(resetStateMiddleware));
            }

            // Add middleware to the adapter's middleware pipeline
            Use(showTypingMiddleware);
            Use(resetStateMiddleware);

            OnTurnError = async (turnContext, exception) =>
            {
                // Log any leaked exception from the application.
                logger.LogError($"Exception caught : {exception.Message}");

                // Send a catch-all apology to the user.
                await turnContext.SendActivityAsync("Sorry, it looks like something went wrong.");

                if (conversationState != null)
                {
                    try
                    {
                        // Delete the conversationState for the current conversation to prevent the
                        // bot from getting stuck in a error-loop caused by being in a bad state.
                        // ConversationState should be thought of as similar to "cookie-state" in a Web pages.
                        await conversationState.DeleteAsync(turnContext);
                    }
                    catch (Exception e)
                    {
                        logger.LogError($"Exception caught on attempting to Delete ConversationState : {e.Message}");
                    }
                }
            };
        }
    }
}
