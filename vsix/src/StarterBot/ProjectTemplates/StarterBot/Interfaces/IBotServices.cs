// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.AI.QnA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace $safeprojectname$.Interfaces
{
    public interface IBotServices
    {
        LuisRecognizer Dispatch { get; }

        LuisRecognizer MainLuis { get; }

        QnAMaker MainQnA { get; }
    }
}
