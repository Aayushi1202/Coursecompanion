// <copyright file="TeamDetail.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.LearnNow.Models
{
    using System;

    /// <summary>
    /// Class contains team details where application is installed.
    /// </summary>
    public class TeamDetail
    {
        /// <summary>
        /// Gets or sets the date time when the application is installed.
        /// </summary>
        public DateTime BotInstalledOn { get; set; }

        /// <summary>
        /// Gets or sets service URL.
        /// </summary>
        public string ServiceUrl { get; set; }
    }
}
