// <copyright file="IMemberValidationService.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.LearnNow.Services.MicrosoftGraph.GroupMembers
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for member validation Service.
    /// </summary>
    public interface IMemberValidationService
    {
        /// <summary>
        /// Method to validate whether current user is a member of teacher's security group.
        /// </summary>
        /// <param name="userAadObjectId">Azure Active Directory id of current user.</param>
        /// <param name="authorizationHeaders">HttpRequest authorization headers.</param>
        /// <returns>Returns true if current user is a member of teacher's security group.</returns>
        Task<bool> ValidateMemberAsync(string userAadObjectId, string authorizationHeaders);

        /// <summary>
        /// Method to validate whether current user is a member of administrators security group.
        /// </summary>
        /// <param name="userAadObjectId">Azure Active Directory id of current user.</param>
        /// <param name="authorizationHeaders">HttpRequest authorization headers.</param>
        /// <returns>Returns true if current user is a member of administrators security group.</returns>
        Task<bool> ValidateAdminAsync(string userAadObjectId, string authorizationHeaders);
    }
}