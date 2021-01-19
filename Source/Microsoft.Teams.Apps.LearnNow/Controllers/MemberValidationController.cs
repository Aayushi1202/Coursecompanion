// <copyright file="MemberValidationController.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.LearnNow.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Teams.Apps.LearnNow.Common;
    using Microsoft.Teams.Apps.LearnNow.Models;
    using Microsoft.Teams.Apps.LearnNow.Services.MicrosoftGraph.GroupMembers;

    /// <summary>
    /// Controller to handle API operation for security group members.
    /// </summary>
    [Route("api/groupmember")]
    [ApiController]
    [Authorize]
    public class MemberValidationController : BaseController
    {
        /// <summary>
        /// Sends logs to the Application Insights service.
        /// </summary>
        private readonly ILogger<MemberValidationController> logger;

        /// <summary>
        /// Instance of MemberValidationService to validate member.
        /// </summary>
        private readonly MemberValidationService memberValidationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberValidationController"/> class.
        /// </summary>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <param name="logger">Instance to send logs to the Application Insights service.</param>
        /// <param name="memberValidationService">Instance of MemberValidationService to validate member of a security group.</param>
        public MemberValidationController(
             TelemetryClient telemetryClient,
             ILogger<MemberValidationController> logger,
             MemberValidationService memberValidationService)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.memberValidationService = memberValidationService;
        }

        /// <summary>
        /// Validate if user is a member of teachers or administrators security group.
        /// </summary>
        /// <returns>Returns whether current logged-in user is a part of security group or not to check if user is a administrator, teacher or student.</returns>
        [HttpGet]
        public async Task<IActionResult> ValidateIfUserIsMemberOfSecurityGroupAsync()
        {
            try
            {
                var userRoleDetails = new UserRole();
                userRoleDetails.IsTeacher = await this.memberValidationService.ValidateMemberAsync(this.UserObjectId.ToString(), this.Request.Headers["Authorization"].ToString());
                userRoleDetails.IsAdmin = await this.memberValidationService.ValidateAdminAsync(this.UserObjectId.ToString(), this.Request.Headers["Authorization"].ToString());

                this.RecordEvent("ValidateIfUserIsMemberOfSecurityGroupAsync - HTTP Get call succeeded.", RequestType.Succeeded);
                return this.Ok(userRoleDetails);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while validating if user is member of security group.");
                this.RecordEvent("ValidateIfUserIsMemberOfSecurityGroupAsync - HTTP Get call failed.", RequestType.Failed);
                throw;
            }
        }
    }
}