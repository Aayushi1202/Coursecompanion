// <copyright file="TeamMembersService.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.LearnNow.Services.Teams.TeamMembers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Integration.AspNet.Core;
    using Microsoft.Bot.Builder.Teams;
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Schema.Teams;
    using Microsoft.Extensions.Logging;
    using Microsoft.Teams.Apps.LearnNow.Common;
    using Microsoft.Teams.Apps.LearnNow.Common.Interfaces;

    /// <summary>
    /// Class that handles the helper methods to fetch team channel information.
    /// </summary>
    public class TeamMembersService : ITeamMembersService
    {
        /// <summary>
        /// Bot adapter.
        /// </summary>
        private readonly IBotFrameworkHttpAdapter botAdapter;

        /// <summary>
        /// Microsoft application credentials.
        /// </summary>
        private readonly MicrosoftAppCredentials microsoftAppCredentials;

        /// <summary>
        /// Instance to send logs to the Application Insights service.
        /// </summary>
        private readonly ILogger<TeamMembersService> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamMembersService"/> class.
        /// </summary>
        /// <param name="botAdapter">Bot adapter.</param>
        /// <param name="microsoftAppCredentials">Microsoft application credentials.</param>
        /// <param name="logger">Instance to send logs to the Application Insights service.</param>
        public TeamMembersService(
            IBotFrameworkHttpAdapter botAdapter,
            MicrosoftAppCredentials microsoftAppCredentials,
            ILogger<TeamMembersService> logger)
        {
            this.botAdapter = botAdapter;
            this.microsoftAppCredentials = microsoftAppCredentials;
            this.logger = logger;
        }

        /// <summary>
        /// To fetch team member information for specified team.
        /// Return null if the member is not found in team id or either of the information is incorrect.
        /// Caller should handle null value to throw unauthorized if required
        /// </summary>
        /// <param name="teamId">Team id.</param>
        /// <param name="userId">User Object id.</param>
        /// <returns>Returns team member information.</returns>
        public async Task<TeamsChannelAccount> GetTeamMemberAsync(string teamId, string userId)
        {
            TeamsChannelAccount teamMember = new TeamsChannelAccount();

            try
            {
                // TODO : To implement team entity and get team details once storage implementation is done.
                // var teamDetails = await this.teamStorageProvider.GetTeamDetailAsync(teamId);
                string serviceUrl = "https://smba.trafficmanager.net/in/";

                var conversationReference = new ConversationReference
                {
                    ChannelId = Constants.TeamsBotFrameworkChannelId,
                    ServiceUrl = serviceUrl,
                };
                await ((BotFrameworkAdapter)this.botAdapter).ContinueConversationAsync(
                    this.microsoftAppCredentials.MicrosoftAppId,
                    conversationReference,
                    async (context, token) =>
                    {
                        teamMember = await TeamsInfo.GetTeamMemberAsync(context, userId, teamId, CancellationToken.None);
                    }, default);
            }
#pragma warning disable CA1031 // Catching general exceptions to log exception details in telemetry client.
            catch (Exception ex)
#pragma warning restore CA1031 // Catching general exceptions to log exception details in telemetry client.
            {
                this.logger.LogError(ex, $"Error occurred while fetching team member for team: {teamId} - user object id: {userId} ");

                // Return null if the member is not found in team id or either of the information is incorrect.
                // Caller should handle null value to throw unauthorized if required.
                return null;
            }

            return teamMember;
        }
    }
}
