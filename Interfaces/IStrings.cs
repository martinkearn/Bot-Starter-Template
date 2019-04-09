using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StarterBot.Interfaces
{
    public interface IStrings
    {
        Task<string> GetString(string key, string language);
    }
}
