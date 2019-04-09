using StarterBot.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarterBot.Services
{
    public class Strings : IStrings
    {
        private readonly Dictionary<string, string> _strings;
        public Strings()
        {
            _strings = new Dictionary<string, string>
            {
                { "welcome", "Welcome to the bot" },
                { "whichflow", "Which conversation flow would you like to see?" },
                { "rootdialogwelcome", "You are in root dialog" },
                { "invalidresponse", "I'm sorry, that wasn't a valid response. Please select one of the options" }
            };
        }

        public async Task<string> GetString(string key, string language, params string[] tokens)
        {
            return await Task.FromResult(string.Format(_strings[key.ToLower()], tokens));            
        }
    }
}
