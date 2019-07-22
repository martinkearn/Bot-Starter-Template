// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.DependencyInjection;
using $safeprojectname$.Adapters;
using $safeprojectname$.Bots;
using $safeprojectname$.Dialogs.Root;
using $safeprojectname$.Helpers;
using $safeprojectname$.Interfaces;
using $safeprojectname$.Middleware;
using $safeprojectname$.Services;

namespace $safeprojectname$
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            services.AddSingleton<IBotServices, BotServices>();

            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            services.AddSingleton<ICredentialProvider, ConfigurationCredentialProvider>();

            services.AddSingleton<IStorage, MemoryStorage>();

            services.AddSingleton<UserState>();

            services.AddSingleton<ConversationState>();

            services.AddSingleton<ShowTypingMiddleware>();

            services.AddSingleton<ResetStateMiddleware>();

            services.AddSingleton<RootDialog>();

            services.AddTransient<IBot, MainBot<RootDialog>>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDefaultFiles()
                .UseStaticFiles()
                .UseMvc();
        }
    }
}
