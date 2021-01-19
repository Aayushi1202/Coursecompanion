// <copyright file="ResourceMapper.cs" company="Microsoft">
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
    /// A model class that contains methods related to resource model mappings.
    /// </summary>
    public class ResourceMapper : IResourceMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceMapper"/> class.
        /// </summary>
        public ResourceMapper()
        {
        }

        /// <summary>
        /// Gets resource entity model from view model.
        /// </summary>
        /// <param name="resourceViewModel">Resource view model object.</param>
        /// <param name="userAadObjectId">Azure Active Directory id of current logged-in user.</param>
        /// <returns>Returns a resource entity model</returns>
        public Resource MapToDTO(
            ResourceViewModel resourceViewModel,
            Guid userAadObjectId)
        {
            resourceViewModel = resourceViewModel ?? throw new ArgumentNullException(nameof(resourceViewModel));

            return new Resource
            {
                Id = resourceViewModel.Id,
                Title = resourceViewModel.Title,
                Description = resourceViewModel.Description,
                SubjectId = resourceViewModel.SubjectId,
                GradeId = resourceViewModel.GradeId,
                ImageUrl = resourceViewModel.ImageUrl,
                LinkUrl = resourceViewModel.LinkUrl,
                AttachmentUrl = resourceViewModel.AttachmentUrl,
                CreatedOn = DateTimeOffset.Now,
                UpdatedOn = DateTimeOffset.Now,
                CreatedBy = userAadObjectId,
                UpdatedBy = userAadObjectId,
                ResourceType = resourceViewModel.ResourceType,
                Grade = resourceViewModel.Grade,
                Subject = resourceViewModel.Subject,
                ResourceTag = resourceViewModel.ResourceTag?.ToList(),
            };
        }

        /// <summary>
        /// Gets resource view model from entity model.
        /// </summary>
        /// <param name="resource">Resource entity model object.</param>
        /// <param name="userDetails">List of user detail object.</param>
        /// <returns>Returns a resource view model object.</returns>
        public ResourceViewModel MapToViewModel(
            Resource resource,
            IEnumerable<UserDetail> userDetails)
        {
            resource = resource ?? throw new ArgumentNullException(nameof(resource));
            userDetails = userDetails ?? throw new ArgumentNullException(nameof(userDetails));

            return new ResourceViewModel
            {
                Id = resource.Id,
                Title = resource.Title,
                Description = resource.Description,
                GradeId = resource.GradeId,
                SubjectId = resource.SubjectId,
                Subject = resource.Subject,
                Grade = resource.Grade,
                ImageUrl = resource.ImageUrl,
                LinkUrl = resource.LinkUrl,
                AttachmentUrl = resource.AttachmentUrl,
                ResourceType = resource.ResourceType,
                ResourceTag = resource.ResourceTag,
                CreatedBy = resource.CreatedBy,
                UpdatedBy = resource.UpdatedBy,
                CreatedOn = DateTimeOffset.Now,
                UpdatedOn = DateTimeOffset.Now,
                IsLikedByUser = false,
                VoteCount = 0,
                UserDisplayName = userDetails.ToList().Find(user => user.UserId == resource.UpdatedBy).DisplayName,
            };
        }

        /// <summary>
        /// Gets resource view model from entity model.
        /// </summary>
        /// <param name="resource">Resource entity model object.</param>
        /// <param name="userAadObjectId">Azure Active Directory id of current logged-in user.</param>
        /// <param name="resourceVotes">List of resource votes.</param>
        /// <returns>Returns a resource view model object.</returns>
        public ResourceViewModel MapToViewModel(
            Resource resource,
            Guid userAadObjectId,
            IEnumerable<ResourceVote> resourceVotes)
        {
            resource = resource ?? throw new ArgumentNullException(nameof(resource));

            return new ResourceViewModel
            {
                Id = resource.Id,
                Title = resource.Title,
                Description = resource.Description,
                SubjectId = resource.SubjectId,
                Subject = resource.Subject,
                GradeId = resource.GradeId,
                Grade = resource.Grade,
                ImageUrl = resource.ImageUrl,
                LinkUrl = resource.LinkUrl,
                AttachmentUrl = resource.AttachmentUrl,
                ResourceType = resource.ResourceType,
                ResourceTag = resource.ResourceTag,
                CreatedOn = resource.CreatedOn,
                CreatedBy = resource.CreatedBy,
                IsLikedByUser = resourceVotes.Any(vote => vote.UserId == userAadObjectId),
            };
        }

        /// <summary>
        /// Gets resource entity model from view model.
        /// </summary>
        /// <param name="resourceViewModel">Resource view model object.</param>
        /// <param name="userAadObjectId">Azure Active Directory id of current logged-in user.</param>
        /// <returns>Returns a resource entity model</returns>
        public Resource PatchAndMapToDTO(
            ResourceViewModel resourceViewModel,
            Guid userAadObjectId)
        {
            resourceViewModel = resourceViewModel ?? throw new ArgumentNullException(nameof(resourceViewModel));

            return new Resource
            {
                Id = resourceViewModel.Id,
                Title = resourceViewModel.Title,
                Description = resourceViewModel.Description,
                SubjectId = resourceViewModel.SubjectId,
                GradeId = resourceViewModel.GradeId,
                ImageUrl = resourceViewModel.ImageUrl,
                LinkUrl = resourceViewModel.LinkUrl,
                AttachmentUrl = resourceViewModel.AttachmentUrl,
                UpdatedOn = DateTimeOffset.Now,
                UpdatedBy = userAadObjectId,
                CreatedOn = resourceViewModel.CreatedOn,
                CreatedBy = resourceViewModel.CreatedBy,
                ResourceType = resourceViewModel.ResourceType,
            };
        }

        /// <summary>
        /// Gets resource view model from entity model.
        /// </summary>
        /// <param name="resource">Resource entity model object.</param>
        /// <param name="userAadObjectId">Azure Active Directory id of current logged-in user.</param>
        /// <param name="resourceVotes">List of resource votes.</param>
        /// <param name="userDetails">List of user details object.</param>
        /// <returns>Returns a resource view model object.</returns>
        public ResourceViewModel PatchAndMapToViewModel(
            Resource resource,
            Guid userAadObjectId,
            IEnumerable<ResourceVote> resourceVotes,
            IEnumerable<UserDetail> userDetails)
        {
            resource = resource ?? throw new ArgumentNullException(nameof(resource));
            resourceVotes = resourceVotes ?? throw new ArgumentNullException(nameof(resourceVotes));
            userDetails = userDetails ?? throw new ArgumentNullException(nameof(userDetails));

            return new ResourceViewModel
            {
                Id = resource.Id,
                Title = resource.Title,
                Description = resource.Description,
                GradeId = resource.GradeId,
                SubjectId = resource.SubjectId,
                Subject = resource.Subject,
                Grade = resource.Grade,
                ImageUrl = resource.ImageUrl,
                LinkUrl = resource.LinkUrl,
                AttachmentUrl = resource.AttachmentUrl,
                ResourceType = resource.ResourceType,
                ResourceTag = resource.ResourceTag,
                CreatedBy = resource.CreatedBy,
                UpdatedBy = resource.UpdatedBy,
                CreatedOn = resource.CreatedOn,
                UpdatedOn = resource.UpdatedOn,
                IsLikedByUser = resourceVotes.Any(vote => vote.UserId == userAadObjectId),
                VoteCount = resourceVotes.Count(),
                UserDisplayName = userDetails.ToList().Find(user => user.UserId == resource.CreatedBy)?.DisplayName,
            };
        }

        /// <summary>
        /// Gets resource view models from entity models.
        /// </summary>
        /// <param name="filteredResources">Resource entity object collection.</param>
        /// <param name="userAadObjectId">Azure Active Directory id of current logged-in user.</param>
        /// <param name="userDetails">List of user details object.</param>
        /// <returns>Returns a collection of resource view models.</returns>
        public IEnumerable<ResourceViewModel> MapToViewModels(
            Dictionary<Guid, List<ResourceDetailModel>> filteredResources,
            Guid userAadObjectId,
            IEnumerable<UserDetail> userDetails)
        {
            filteredResources = filteredResources ?? throw new ArgumentNullException(nameof(filteredResources));
            userDetails = userDetails ?? throw new ArgumentNullException(nameof(userDetails));

            var resourceDetails = new List<ResourceViewModel>();

            foreach (var resource in filteredResources)
            {
                resourceDetails.Add(new ResourceViewModel()
                {
                    Id = resource.Key,
                    Title = resource.Value.FirstOrDefault()?.Title,
                    VoteCount = (int)resource.Value.FirstOrDefault()?.Votes?.Count(),
                    IsLikedByUser = (bool)resource.Value.FirstOrDefault()?.Votes?.Any(v => v.UserId == userAadObjectId),
                    Description = resource.Value.FirstOrDefault()?.Description,
                    GradeId = (Guid)resource.Value.FirstOrDefault().GradeId,
                    SubjectId = (Guid)resource.Value.FirstOrDefault().SubjectId,
                    Subject = resource.Value.FirstOrDefault()?.Subject,
                    Grade = resource.Value.FirstOrDefault()?.Grade,
                    ImageUrl = resource.Value.FirstOrDefault()?.ImageUrl,
                    LinkUrl = resource.Value.FirstOrDefault()?.LinkUrl,
                    AttachmentUrl = resource.Value.FirstOrDefault()?.AttachmentUrl,
                    ResourceType = (int)resource.Value.FirstOrDefault()?.ResourceType,
                    ResourceTag = resource.Value.FirstOrDefault()?.ResourceTag,
                    CreatedBy = (Guid)resource.Value.FirstOrDefault()?.CreatedBy,
                    UpdatedBy = (Guid)resource.Value.FirstOrDefault()?.UpdatedBy,
                    CreatedOn = resource.Value.FirstOrDefault()?.CreatedOn,
                    UpdatedOn = resource.Value.FirstOrDefault()?.UpdatedOn,
                    UserDisplayName = userDetails.ToList().Find(user => user.UserId == resource.Value.FirstOrDefault()?.CreatedBy)?.DisplayName,
                });
            }

            return resourceDetails;
        }

        /// <summary>
        /// Gets resource view models from entity models.
        /// </summary>
        /// <param name="filteredResources">Resource entity object collection.</param>
        /// <param name="userAadObjectId">Azure Active Directory id of current logged-in user.</param>
        /// <returns>Returns a collection of resource view models.</returns>
        public IEnumerable<ResourceViewModel> MapToViewModels(
            Dictionary<Guid, List<ResourceDetailModel>> filteredResources,
            Guid userAadObjectId)
        {
            filteredResources = filteredResources ?? throw new ArgumentNullException(nameof(filteredResources));

            var resourceDetails = new List<ResourceViewModel>();

            foreach (var resource in filteredResources)
            {
                resourceDetails.Add(new ResourceViewModel()
                {
                    Id = resource.Key,
                    Title = resource.Value.FirstOrDefault()?.Title,
                    VoteCount = (int)resource.Value.FirstOrDefault().Votes?.Count(),
                    IsLikedByUser = (bool)resource.Value.FirstOrDefault().Votes?.Any(v => v.UserId == userAadObjectId),
                    Description = resource.Value.FirstOrDefault()?.Description,
                    GradeId = (Guid)resource.Value.FirstOrDefault().GradeId,
                    SubjectId = (Guid)resource.Value.FirstOrDefault().SubjectId,
                    Subject = resource.Value.FirstOrDefault()?.Subject,
                    Grade = resource.Value.FirstOrDefault()?.Grade,
                    ImageUrl = resource.Value.FirstOrDefault()?.ImageUrl,
                    LinkUrl = resource.Value.FirstOrDefault()?.LinkUrl,
                    AttachmentUrl = resource.Value.FirstOrDefault()?.AttachmentUrl,
                    ResourceType = (int)resource.Value.FirstOrDefault()?.ResourceType,
                    ResourceTag = resource.Value.FirstOrDefault()?.ResourceTag,
                    CreatedBy = (Guid)resource.Value.FirstOrDefault()?.CreatedBy,
                    UpdatedBy = (Guid)resource.Value.FirstOrDefault()?.UpdatedBy,
                    CreatedOn = resource.Value.FirstOrDefault()?.CreatedOn,
                    UpdatedOn = resource.Value.FirstOrDefault()?.UpdatedOn,
                });
            }

            return resourceDetails;
        }
    }
}
