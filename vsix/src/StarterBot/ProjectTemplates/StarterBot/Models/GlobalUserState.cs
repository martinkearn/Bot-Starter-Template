// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace $safeprojectname$.Models
{
    public class GlobalUserState
    {
        public bool DidBotWelcomeUser { get; set; } = false;

        public string Name { get; set; }

        public int Age { get; set; }

        public string Country { get; set; }
    }
}
