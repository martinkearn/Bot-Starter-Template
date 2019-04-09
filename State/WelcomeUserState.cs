using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterBot.State
{
    public class WelcomeUserState
    {
        // Gets or sets whether the user has been welcomed in the conversation.
        public bool DidBotWelcomeUser { get; set; } = false;
    }
}
