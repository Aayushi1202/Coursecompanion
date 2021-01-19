// <copyright file="LearningModuleMapper.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.LearnNow.ModelMappers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Teams.Apps.LearnNow.Infrastructure.Models;
    using Microsoft.Teams.Apps.LearnNow.Models;

    /// <summary>
    /// A model class that contains methods related to  learning module model mappings.
    /// </summary>
    public class LearningModuleMapper : ILearningModuleMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LearningModuleMapper"/> class.
        /// </summary>
        public LearningModuleMapper()
        {
        }

        /// <summary>
        /// Gets  learning module entity model from view model.
        /// </summary>
        /// <param name="learningModuleViewModel"> Learning module view model object.</param>
        /// <param name="userAadObjectId">Azure Active Directory id of current logged-in user.</param>
        /// <returns>Returns a  learning module entity model</returns>
        public LearningModule MapToDTO(
           LearningModuleViewModel learningModuleViewModel,
           Guid userAadObjectId)
        {
            learningModuleViewModel = learningModuleViewModel ?? throw new ArgumentNullException(nameof(learningModuleViewModel));

            return new LearningModule
            {
                Id = learningModuleViewModel.Id,
                Title = learningModuleViewModel.Title,
                Description = learningModuleViewModel.Description,
                SubjectId = learningModuleViewModel.SubjectId,
                GradeId = learningModuleViewModel.GradeId,
                ImageUrl = learningModuleViewModel.ImageUrl,
                CreatedOn = DateTimeOffset.Now,
                UpdatedOn = DateTimeOffset.Now,
                CreatedBy = userAadObjectId,
                UpdatedBy = userAadObjectId,
                LearningModuleTag = learningModuleViewModel.LearningModuleTag.ToList(),
            };
        }

        /// <summary>
        /// Gets  learning module view model from entity model.
        /// </summary>
        /// <param name="learningModule"> Learning module entity model object.</param>
        /// <param name="userDetails">List of user detail object.</param>
        /// <returns>Returns a  learning module view model object.</returns>
        public LearningModuleViewModel MapToViewModel(
            LearningModule learningModule,
            IEnumerable<UserDetail> userDetails)
        {
            learningModule = learningModule ?? throw new ArgumentNullException(nameof(learningModule));
            userDetails = userDetails ?? throw new ArgumentNullException(nameof(userDetails));

            return new LearningModuleViewModel
            {
                Id = learningModule.Id,
                Title = learningModule.Title,
                Description = learningModule.Description,
                GradeId = learningModule.GradeId,
                SubjectId = learningModule.SubjectId,
                Subject = learningModule.Subject,
                Grade = learningModule.Grade,
                ImageUrl = learningModule.ImageUrl,
                CreatedBy = learningModule.CreatedBy,
                UpdatedBy = learningModule.UpdatedBy,
                CreatedOn = learningModule.CreatedOn,
                UpdatedOn = learningModule.UpdatedOn,
                LearningModuleTag = learningModule.LearningModuleTag,
                IsLikedByUser = false,
                VoteCount = 0,
                ResourceCount = 0,
                UserDisplayName = userDetails.ToList().Find(user => user.UserId == learningModule.UpdatedBy).DisplayName,
            };
        }

        /// <summary>
        /// Gets learning module entity model from view model.
        /// </summary>
        /// <param name="learningModuleViewModel"> Learning module view model object.</param>
        /// <param name="userAadObjectId">Azure Active Directory id of current logged-in user.</param>
        /// <returns>Returns a  learning module entity model</returns>
        public LearningModule PatchAndMapToDTO(
            LearningModuleViewModel learningModuleViewModel,
            Guid userAadObjectId)
        {
            learningModuleViewModel = learningModuleViewModel ?? throw new ArgumentNullException(nameof(learningModuleViewModel));

            return new LearningModule
            {
                Id = learningModuleViewModel.Id,
                Title = learningModuleViewModel.Title,
                Description = learningModuleViewModel.Description,
                SubjectId = learningModuleViewModel.SubjectId,
                GradeId = learningModuleViewModel.GradeId,
                ImageUrl = learningModuleViewModel.ImageUrl,
                UpdatedOn = DateTimeOffset.Now,
                UpdatedBy = userAadObjectId,
                CreatedOn = learningModuleViewModel.CreatedOn,
                CreatedBy = learningModuleViewModel.CreatedBy,
            };
        }

        /// <summary>
        /// Gets  learning module view model from entity model.
        /// </summary>
        /// <param name="learningModule"> Learning module entity model object.</param>
        /// <param name="userAadObjectId">Azure Active Directory id of current logged-in user.</param>
        /// <param name="learningModuleVotes">List of learning module votes.</param>
        /// <param name="resourceCount">Count of learning module resources.</param>
        /// <param name="userDetails">List of user details object.</param>
        /// <returns>Returns a  learning module view model object.</returns>
        public LearningModuleViewModel PatchAndMapToViewModel(
            LearningModule learningModule,
            Guid userAadObjectId,
            IEnumerable<LearningModuleVote> learningModuleVotes,
            int resourceCount,
            IEnumerable<UserDetail> userDetails)
        {
            learningModule = learningModule ?? throw new ArgumentNullException(nameof(learningModule));
            learningModuleVotes = learningModuleVotes ?? throw new ArgumentNullException(nameof(learningModuleVotes));
            userDetails = userDetails ?? throw new ArgumentNullException(nameof(userDetails));

            return new LearningModuleViewModel
            {
                Id = learningModule.Id,
                Title = learningModule.Title,
                Description = learningModule.Description,
                GradeId = learningModule.GradeId,
                SubjectId = learningModule.SubjectId,
                Subject = learningModule.Subject,
                Grade = learningModule.Grade,
                ImageUrl = learningModule.ImageUrl,
                CreatedBy = learningModule.CreatedBy,
                UpdatedBy = learningModule.UpdatedBy,
                CreatedOn = learningModule.CreatedOn,
                UpdatedOn = learningModule.UpdatedOn,
                LearningModuleTag = learningModule.LearningModuleTag,
                IsLikedByUser = learningModuleVotes.Any(vote => vote.UserId == userAadObjectId),
                VoteCount = learningModuleVotes.Count(),
                ResourceCount = resourceCount,
                UserDisplayName = userDetails.ToList().Find(user => user.UserId == learningModule.CreatedBy).DisplayName,
            };
        }

        /// <summary>
        /// Gets learning module view models from entity models.
        /// </summary>
        /// <param name="moduleWithVotesAndResources">Learning module entity object collection.</param>
        /// <param name="userAadObjectId">Azure Active Directory id of current logged-in user.</param>
        /// <param name="userDetails">List of user details object.</param>
        /// <returns>Returns a collection of learning module view models.</returns>
        public IEnumerable<LearningModuleViewModel> MapToViewModels(
            Dictionary<Guid, List<LearningModuleDetailModel>> moduleWithVotesAndResources,
            Guid userAadObjectId,
            IEnumerable<UserDetail> userDetails)
        {
            moduleWithVotesAndResources = moduleWithVotesAndResources ?? throw new ArgumentNullException(nameof(moduleWithVotesAndResources));
            userDetails = userDetails ?? throw new ArgumentNullException(nameof(userDetails));

            var learningModuleDetails = new List<LearningModuleViewModel>();
            foreach (var learningModule in moduleWithVotesAndResources)
            {
                learningModuleDetails.Add(new LearningModuleViewModel()
                {
                    Id = learningModule.Key,
                    Title = learningModule.Value.FirstOrDefault()?.Title,
                    VoteCount = (int)learningModule.Value.FirstOrDefault().Votes.Count(),
                    IsLikedByUser = (bool)learningModule.Value.FirstOrDefault()?.Votes?.Any(v => v.UserId == userAadObjectId),
                    Description = learningModule.Value.FirstOrDefault()?.Description,
                    GradeId = (Guid)learningModule.Value.FirstOrDefault().GradeId,
                    SubjectId = (Guid)learningModule.Value.FirstOrDefault().SubjectId,
                    Subject = learningModule.Value.FirstOrDefault()?.Subject,
                    Grade = learningModule.Value.FirstOrDefault()?.Grade,
                    ImageUrl = learningModule.Value.FirstOrDefault()?.ImageUrl,
                    CreatedBy = (Guid)learningModule.Value.FirstOrDefault()?.CreatedBy,
                    UpdatedBy = (Guid)learningModule.Value.FirstOrDefault()?.UpdatedBy,
                    CreatedOn = learningModule.Value.FirstOrDefault()?.CreatedOn,
                    UpdatedOn = learningModule.Value.FirstOrDefault()?.UpdatedOn,
                    LearningModuleTag = learningModule.Value.FirstOrDefault()?.LearningModuleTag,
                    UserDisplayName = userDetails.ToList().Find(user => user.UserId == learningModule.Value.FirstOrDefault()?.CreatedBy)?.DisplayName,
                    ResourceCount = (int)learningModule.Value.FirstOrDefault()?.ResourceModuleMappings.Count(),
                });
            }

            return learningModuleDetails;
        }

        /// <summary>
        /// Gets learning module view model from entity model.
        /// </summary>
        /// <param name="learningModule">Learning module entity model object.</param>
        /// <param name="userAadObjectId">Azure Active Directory id of current logged-in user.</param>
        /// <param name="learningModuleVotes">List of learning module votes.</param>
        /// <returns>Returns a learning module view model object.</returns>
        public LearningModuleViewModel MapToViewModel(
            LearningModule learningModule,
            Guid userAadObjectId,
            IEnumerable<LearningModuleVote> learningModuleVotes)
        {
            learningModule = learningModule ?? throw new ArgumentNullException(nameof(learningModule));

            return new LearningModuleViewModel
            {
                Id = learningModule.Id,
                Title = learningModule.Title,
                Description = learningModule.Description,
                Subject = learningModule.Subject,
                SubjectId = learningModule.SubjectId,
                Grade = learningModule.Grade,
                GradeId = learningModule.GradeId,
                ImageUrl = learningModule.ImageUrl,
                LearningModuleTag = learningModule.LearningModuleTag,
                IsLikedByUser = learningModuleVotes.Any(vote => vote.UserId == userAadObjectId),
            };
        }
    }
}