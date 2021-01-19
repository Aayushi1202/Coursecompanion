// <copyright file="MustBeMemberOfSecurityGroupPolicyHandler.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.LearnNow.Authentication.AuthenticationPolicy.AuthenticationPolicy
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Teams.Apps.LearnNow.Common;
    using Microsoft.Teams.Apps.LearnNow.Services.MicrosoftGraph.GroupMembers;

    /// <summary>
    /// This authorization handler is created to validate whether user is member/owner of security group.
    /// The class implements AuthorizationHandler for handling MustBeMemberOfSecurityGroupPolicyRequirement authorization.
    /// </summary>
    public class MustBeMemberOfSecurityGroupPolicyHandler : AuthorizationHandler<MustBeMemberOfSecurityGroupPolicyRequirement>
    {
        /// <summary>
        /// Instance of MemberValidationService to validate member.
        /// </summary>
        private readonly MemberValidationService memberValidationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MustBeMemberOfSecurityGroupPolicyHandler"/> class.
        /// </summary>
        /// <param name="memberValidationService">Instance of member validation service to validate whether is valid user,</param>
        public MustBeMemberOfSecurityGroupPolicyHandler(MemberValidationService memberValidationService)
        {
            this.memberValidationService = memberValidationService;
        }

        /// <summary>
        /// This method handles the authorization requirement.
        /// </summary>
        /// <param name="context">AuthorizationHandlerContext instance.</param>
        /// <param name="requirement">IAuthorizationRequirement instance.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeMemberOfSecurityGroupPolicyRequirement requirement)
        {
            context = context ?? throw new ArgumentNullException(nameof(context));

            string teamId = string.Empty;
            var oidClaimType = Constants.OidClaimType;

            var oidClaim = context.User.Claims.FirstOrDefault(p => oidClaimType == p.Type);

            if (context.Resource is AuthorizationFilterContext authorizationFilterContext)
            {
                // Wrap the request stream so that we can rewind it back to the start for regular request processing.
                authorizationFilterContext.HttpContext.Request.EnableBuffering();

                var isUserPartOfTeachersGroup = await this.memberValidationService.ValidateMemberAsync(oidClaim.Value, authorizationFilterContext.HttpContext.Request.Headers["Authorization"].ToString());
                if (isUserPartOfTeachersGroup)
                {
                    context.Succeed(requirement);
                }

                // Check whether user is part of Administrator group or not. Administrator has access to edit and delete other teachers content.
                var isUserPartOfAdminGroup = await this.memberValidationService.ValidateAdminAsync(oidClaim.Value, authorizationFilterContext.HttpContext.Request.Headers["Authorization"].ToString());
                if (isUserPartOfAdminGroup)
                {
                    context.Succeed(requirement);
                }
            }
        }
    }
}