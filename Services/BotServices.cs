using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Configuration;
using Microsoft.Extensions.Configuration;
using StarterBot.Interfaces;
using System;

namespace StarterBot.Services
{
    public class BotServices : IBotServices
    {
        public BotServices(IConfiguration configuration)
        {
            // Read the setting for cognitive services (LUIS, QnA) from the appsettings.json or secrets.json
            Dispatch = ReadLuisRecognizer(configuration, "Dispatch");
            MainLuis = ReadLuisRecognizer(configuration, "MainLuis");
            MainQnA = ReadQnAMaker(configuration, "MainQnA");
        }

        public LuisRecognizer Dispatch { get; private set; }
        public LuisRecognizer MainLuis { get; private set; }
        public QnAMaker MainQnA { get; private set; }

        private LuisRecognizer ReadLuisRecognizer(IConfiguration configuration, string name)
        {
            try
            {
                //var services = configuration.GetSection("BotServices");
                var luisService = new LuisService
                {
                    AppId = configuration[$"Luis-{name}-AppId"],
                    AuthoringKey = configuration[$"Luis-{name}-Authoringkey"],
                    Region = configuration[$"Luis-{name}-Region"]
                };
                return new LuisRecognizer(new LuisApplication(
                    luisService.AppId,
                    luisService.AuthoringKey,
                    luisService.GetEndpoint()),
                    new LuisPredictionOptions { IncludeAllIntents = true, IncludeInstanceData = true },
                    true);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private QnAMaker ReadQnAMaker(IConfiguration configuration, string name)
        {
            try
            {
                return new QnAMaker(new QnAMakerEndpoint
                {
                    KnowledgeBaseId = configuration[$"QnA-{name}-KBId"],
                    EndpointKey = configuration[$"QnA-{name}-EndpointKey"],
                    Host = configuration[$"QnA-{name}-HostName"]
                });
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
