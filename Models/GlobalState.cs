using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterBot.Models
{
    public class GlobalState
    {
        public bool DidBotWelcomeUser { get; set; } = false;

        public string Name { get; set; }

        public int Age { get; set; }

        public string Country { get; set; }
    }
}
