﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using $safeprojectname$.Bots.Resources;
using $safeprojectname$.Extensions;
using $safeprojectname$.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace $safeprojectname$.Bots
{
    public class MainBot<T> : ActivityHandler where T : Dialog
    {
        private const string WebChatChannelId = "webchat";
        private readonly BotState _conversationState;
        private readonly BotState _userState;
        private readonly ILogger _logger;
        private readonly Dialog _dialog;
        private IStatePropertyAccessor<GlobalUserState> _globalStateAccessor;

        public MainBot(ConversationState conversationState, UserState userState, T dialog, ILogger<MainBot<T>> logger)
        {
            if (conversationState == null)
            {
                throw new System.ArgumentNullException(nameof(conversationState));
            }

            if (logger == null)
            {
                throw new System.ArgumentNullException(nameof(logger));
            }

            _conversationState = conversationState;
            _userState = userState;
            _logger = logger;
            _dialog = dialog;
            _globalStateAccessor = _userState.CreateProperty<GlobalUserState>(nameof(GlobalUserState));

            _logger.LogTrace("Turn start.");
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have occured during the turn. This avoids the need to explicitly save changes to state each time
            await _conversationState.SaveChangesAsync(turnContext, false, cancellationToken);
            await _userState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        // Process incoming message
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var state = await _globalStateAccessor.GetAsync(turnContext, () => new GlobalUserState());

            // If the user has not yet been welcomed, welcome them. Make sure the welcome inlcudes a prompt to throw the message away and ask them to enter a new message
            if (state.DidBotWelcomeUser == false)
            {
                // Set state
                state.DidBotWelcomeUser = true;

                // Send welcome message
                var name = turnContext.Activity.From.Name ?? string.Empty;
                await turnContext.SendActivityAsync($"{String.Format(MainBotStrings.Welcome_name, name)}", cancellationToken: cancellationToken);

                // Run the initial dialog
                await _dialog.Run(turnContext, _conversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken : cancellationToken);
            }
            else
            {
                // Run the initial dialog
                await _dialog.Run(turnContext, _conversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken : cancellationToken);
            }
        }

        // Greet when users are added to the conversation.
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            // Welcome each member that was added
            foreach (var member in membersAdded)
            {
                // The bot itself is a conversation member too ... this check makes sure this is not the bot joining
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    // Look for web chat channel because it sends this event when a user messages so we want to only do this if not webchat. Webchat welcome is handled on receipt of first message
                    if (turnContext.Activity.ChannelId.ToLower() != WebChatChannelId)
                    {
                        // Set state
                        var state = await _globalStateAccessor.GetAsync(turnContext, () => new GlobalUserState());
                        state.DidBotWelcomeUser = true;

                        // Send welcome message
                        var name = member.Name ?? string.Empty;
                        await turnContext.SendActivityAsync($"{String.Format(MainBotStrings.WelcomeToTheConversation_name, name)}", cancellationToken: cancellationToken);

                        // Run the initial dialog
                        await _dialog.Run(turnContext, _conversationState.CreateProperty<DialogState>(nameof(DialogState)), cancellationToken : cancellationToken);
                    }
                }
            }
        }


    }
}
