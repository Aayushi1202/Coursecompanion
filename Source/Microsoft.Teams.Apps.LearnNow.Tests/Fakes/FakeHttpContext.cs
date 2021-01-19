// <copyright file="FakeHttpContext.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.LearnNow.Tests.Fakes
{
    using System.Security.Claims;
    using System.Security.Principal;
    using Microsoft.AspNetCore.Http;
    using Moq;

    /// <summary>
    /// Class to fake HTTP Context
    /// </summary>
    public class FakeHttpContext
    {
        /// <summary>
        /// Make fake HTTP context for unit testing.
        /// </summary>
        /// <returns>Fake HTTP context</returns>
        public static HttpContext GetMockHttpContextWithUserClaims()
        {
            var userAadObjectId = "1a1cce71-2833-4345-86e2-e9047f73e6af";
            var context = new Mock<HttpContext>();
            var request = new Mock<HttpRequest>();
            var response = new Mock<HttpContext>();
            var user = new Mock<ClaimsPrincipal>();
            var identity = new Mock<IIdentity>();
            var claim = new Claim[]
            {
                new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", userAadObjectId.ToString()),
            };

            context.Setup(ctx => ctx.User).Returns(user.Object);

            user.Setup(ctx => ctx.Identity).Returns(identity.Object);
            user.Setup(ctx => ctx.Claims).Returns(claim);

            identity.Setup(id => id.IsAuthenticated).Returns(true);
            identity.Setup(id => id.Name).Returns("test");

            return context.Object;
        }

        /// <summary>
        /// Get default http context with user identity
        /// </summary>
        /// <returns>http context</returns>
        public static HttpContext GetDefaultContextWithUserIdentity()
        {
            var userAadObjectId = "1a1cce71-2833-4345-86e2-e9047f73e6af";

            var context = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[]
                            {
                            new Claim(
                                "http://schemas.microsoft.com/identity/claims/objectidentifier",
                                userAadObjectId.ToString()),
                            })),
            };

            context.Request.Headers["Authorization"] = "fake_token";

            return context;
        }
    }
}
