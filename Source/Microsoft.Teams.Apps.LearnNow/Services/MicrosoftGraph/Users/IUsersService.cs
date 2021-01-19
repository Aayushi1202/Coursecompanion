﻿// <copyright file="IUsersService.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.LearnNow.Services.MicrosoftGraph.Users
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Teams.Apps.LearnNow.Models;

    /// <summary>
    /// Get the User data.
    /// </summary>
    public interface IUsersService
    {
        /// <summary>
        /// Get users information from graph API.
        /// </summary>
        /// <param name="loggedInUserObjectId">Azure AD user id of the signed in user</param>
        /// <param name="authorizationHeader">Authorization header value. Usually this value will be provided in HTTP context of signed in user.</param>
        /// <param name="userObjectIds">Collection of AAD Object ids of users.</param>
        /// <returns>Returns user details for specified ids.</returns>
        Task<IEnumerable<UserDetail>> GetUserDisplayNamesAsync(string loggedInUserObjectId, string authorizationHeader, IEnumerable<string> userObjectIds);
    }
}
