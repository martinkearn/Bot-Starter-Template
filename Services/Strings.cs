using StarterBot.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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
                { "welcome", "Welcome to the bot, {0}" },
                { "foo", "foo" }
            };
        }

        public async Task<string> GetString(string key, string language, params string[] tokens)
        {
            return await Task.FromResult(string.Format(_strings[key.ToLower()], tokens));            
        }
    }
}
