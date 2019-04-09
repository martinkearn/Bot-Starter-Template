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
                { "welcome", "Welcome to the bot" },
                { "foo", "foo" }
            };
        }

        public async Task<string> GetString(string key, string language)
        {
            return await Task.FromResult(_strings[key.ToLower()]);            
        }
    }
}
