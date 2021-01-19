// <copyright file="TagMapper.cs" company="Microsoft">
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
    /// A Tag mapper class that contains methods related to tag model mappings.
    /// </summary>
    public class TagMapper : ITagMapper
    {
        /// <summary>
        /// Gets tag entity model from view model.
        /// </summary>
        /// <param name="tagViewModel">Tag view model object.</param>
        /// <param name="userAadObjectId">Azure Active Directory id of current logged-in user.</param>
        /// <returns>Returns a tag entity model object.</returns>
        public Tag MapToDTO(
            TagViewModel tagViewModel,
            Guid userAadObjectId)
        {
            tagViewModel = tagViewModel ?? throw new ArgumentNullException(nameof(tagViewModel));

            return new Tag
            {
                TagName = tagViewModel.TagName,
                CreatedBy = userAadObjectId,
                UpdatedBy = userAadObjectId,
                CreatedOn = DateTimeOffset.Now,
                UpdatedOn = DateTimeOffset.Now,
            };
        }

        /// <summary>
        /// Gets tag view model from entity model.
        /// </summary>
        /// <param name="tags">Collection of tag entity model objects.</param>
        /// <param name="userDetails">Collection of user detail objects.</param>
        /// <returns>Returns collection of tag view model objects.</returns>
        public IEnumerable<TagViewModel> MapToViewModel(
            IEnumerable<Tag> tags,
            IEnumerable<UserDetail> userDetails)
        {
            tags = tags ?? throw new ArgumentNullException(nameof(tags));

            return tags.Select(tag => new TagViewModel
            {
                Id = tag.Id,
                TagName = tag.TagName,
                UpdatedOn = tag.UpdatedOn,
                UserDisplayName = userDetails.ToList().Find(user => user.UserId == tag.CreatedBy).DisplayName,
            });
        }
    }
}
