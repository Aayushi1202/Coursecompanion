// <copyright file="TabConfigurationController.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.LearnNow.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Teams.Apps.LearnNow.Common;
    using Microsoft.Teams.Apps.LearnNow.Infrastructure;
    using Microsoft.Teams.Apps.LearnNow.Infrastructure.Models;
    using Microsoft.Teams.Apps.LearnNow.Models;

    /// <summary>
    /// Controller to handle tab configuration API operations.
    /// </summary>
    [Route("api/tab-configuration")]
    [ApiController]
    [Authorize]
    public class TabConfigurationController : BaseController
    {
        /// <summary>
        /// Logs errors and information.
        /// </summary>
        private readonly ILogger<TabConfigurationController> logger;

        /// <summary>
        /// Instance for handling commom operations with entity collection.
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabConfigurationController"/> class.
        /// </summary>
        /// <param name="logger">Logs errors and information.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <param name="unitOfWork">TabConfigurationRepository repository for working with tab configuration data.</param>
        public TabConfigurationController(
            ILogger<TabConfigurationController> logger,
            TelemetryClient telemetryClient,
            IUnitOfWork unitOfWork)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Post call to store team prefrence details in storage.
        /// </summary>
        /// <param name="tabConfigurationDetail">Holds tab configuration detail entity data.</param>
        /// <returns>Returns true for successful operation.</returns>
        [HttpPost]
        public async Task<IActionResult> PostAsync(TabConfigurationViewModel tabConfigurationDetail)
        {
            this.RecordEvent("TabConfiguration - HTTP Post call.", RequestType.Initiated);
            this.logger.LogInformation("Call to add tab configuration details.");

            if (tabConfigurationDetail == null)
            {
                this.RecordEvent("TabConfiguration - HTTP Post call.", RequestType.Failed);
                return this.BadRequest("Error while saving tab configuration details to storage.");
            }

            try
            {
                var tabConfigurationEntityModel = new TabConfiguration()
                {
                    TeamId = tabConfigurationDetail.TeamId,
                    ChannelId = tabConfigurationDetail.ChannelId,
                    LearningModuleId = tabConfigurationDetail.LearningModuleId,
                    CreatedBy = this.UserObjectId,
                    UpdatedBy = this.UserObjectId,
                    CreatedOn = DateTime.UtcNow,
                    UpdatedOn = DateTime.UtcNow,
                };
                this.unitOfWork.TabConfigurationRepository.Add(tabConfigurationEntityModel);
                await this.unitOfWork.SaveChangesAsync();
                this.RecordEvent("TabConfiguration - HTTP Post call.", RequestType.Succeeded);
                return this.Ok(tabConfigurationEntityModel);
            }
            catch (Exception ex)
            {
                this.RecordEvent("TabConfiguration - HTTP Post call.", RequestType.Failed);
                this.logger.LogError(ex, $"Error while saving tab configuration details.");
                throw;
            }
        }

        /// <summary>
        /// Get details of a tab configuration by entity Id.
        /// </summary>
        /// <param name="id">Unique identifier of teams tab.</param>
        /// <returns>Returns tab configuration details received from storage.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            try
            {
                this.logger.LogInformation("Initiated call for fetching tab configuration details from storage");
                this.RecordEvent("TabConfiguration - HTTP Get call.", RequestType.Initiated);
                var tabConfigurations = await this.unitOfWork.TabConfigurationRepository.GetAsync(id);

                if (tabConfigurations == null)
                {
                    this.logger.LogError(StatusCodes.Status404NotFound, $"The tab configuration detail that user is trying to get does not exists for tab Id: {id}.");
                    this.RecordEvent("Resource - HTTP Get call failed.", RequestType.Failed);
                    return this.NotFound($"No tab configuration detail found for Id: {id}.");
                }

                this.logger.LogInformation($"GET call for fetching tab configuration details from storage is successful.");
                this.RecordEvent("TabConfiguration - HTTP Get call", RequestType.Succeeded);

                return this.Ok(tabConfigurations);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"Error while getting tab configuration details from storage for tab ID : {id}");
                this.RecordEvent($"TabConfiguration - HTTP Get call", RequestType.Failed);
                throw;
            }
        }

        /// <summary>
        /// Patch call to update tab configuration details in storage.
        /// </summary>
        /// <param name="id">Tab identifier.</param>
        /// <param name="tabConfigurationDetail">Holds tab configuration detail entity data.</param>
        /// <returns>Returns true for successful operation.</returns>
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchAsync(Guid id, TabConfigurationViewModel tabConfigurationDetail)
        {
            this.RecordEvent("TabConfiguration - HTTP Patch call.", RequestType.Initiated);
            this.logger.LogInformation("TabConfiguration - HTTP Patch call initiated.");

            if (tabConfigurationDetail == null)
            {
                this.RecordEvent("TabConfiguration - HTTP Patch call.", RequestType.Failed);
                return this.BadRequest("Error while updating tab configuration details to storage.");
            }

            try
            {
                if (id == null || id == Guid.Empty)
                {
                    this.logger.LogError($"Tab Id is either null or empty.");
                    this.RecordEvent("TabConfiguration - HTTP Patch call failed.", RequestType.Failed);
                    return this.BadRequest("Tab Id cannot be null or empty guid.");
                }

                var existingTabConfiguration = await this.unitOfWork.TabConfigurationRepository.GetAsync(id);

                if (existingTabConfiguration == null)
                {
                    this.logger.LogError(StatusCodes.Status404NotFound, $"The tab configuration detail that user is trying to update does not exists for Id: {id}.");
                    this.RecordEvent("TabConfiguration - HTTP Patch call failed.", RequestType.Failed);
                    return this.NotFound($"No tab configuration detail exists for tab Id: {id}.");
                }

                existingTabConfiguration.UpdatedOn = DateTime.UtcNow;
                existingTabConfiguration.UpdatedBy = this.UserObjectId;
                existingTabConfiguration.LearningModuleId = tabConfigurationDetail.LearningModuleId;

                this.unitOfWork.TabConfigurationRepository.Update(existingTabConfiguration);
                await this.unitOfWork.SaveChangesAsync();
                this.RecordEvent("TabConfiguration - HTTP Patch call.", RequestType.Succeeded);
                return this.Ok(existingTabConfiguration);
            }
            catch (Exception ex)
            {
                this.RecordEvent("TabConfiguration - HTTP Patch call.", RequestType.Failed);
                this.logger.LogError(ex, $"TabConfiguration - HTTP Patch call failed, for tab Id: {id}.");
                throw;
            }
        }
    }
}