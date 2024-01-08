﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace GeocachingAPI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Rest;
    using Models;

    /// <summary>
    /// Extension methods for Friends.
    /// </summary>
    public static partial class FriendsExtensions
    {
            /// <summary>
            /// Get a list of friends for the calling user
            /// </summary>
            /// This method will return a list of Users.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='skip'>
            /// How many friends to skip (default = 0)
            /// </param>
            /// <param name='take'>
            /// How many friends to return (default = 10, max = 50)
            /// </param>
            /// <param name='fields'>
            /// Properties you want to return, defaults to referenceCode
            /// </param>
            public static IList<User> GetFriends(this IFriends operations, string apiVersion, int? skip = 0, int? take = 10, string fields = "referenceCode")
            {
                return Task.Factory.StartNew(s => ((IFriends)s).GetFriendsAsync(apiVersion, skip, take, fields), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Get a list of friends for the calling user
            /// </summary>
            /// This method will return a list of Users.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='skip'>
            /// How many friends to skip (default = 0)
            /// </param>
            /// <param name='take'>
            /// How many friends to return (default = 10, max = 50)
            /// </param>
            /// <param name='fields'>
            /// Properties you want to return, defaults to referenceCode
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<User>> GetFriendsAsync(this IFriends operations, string apiVersion, int? skip = 0, int? take = 10, string fields = "referenceCode", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetFriendsWithHttpMessagesAsync(apiVersion, skip, take, fields, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get a list of friend requests for the calling user
            /// </summary>
            /// This method will return a list of requests including both inbound and
            /// outbound requests.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='skip'>
            /// How many requests to skip (default = 0)
            /// </param>
            /// <param name='take'>
            /// How many requests to return (default = 10, max = 50)
            /// </param>
            /// <param name='fields'>
            /// Properties you want to return, defaults to id
            /// </param>
            public static IList<FriendRequest> GetFriendRequests(this IFriends operations, string apiVersion, int? skip = 0, int? take = 10, string fields = "id")
            {
                return Task.Factory.StartNew(s => ((IFriends)s).GetFriendRequestsAsync(apiVersion, skip, take, fields), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Get a list of friend requests for the calling user
            /// </summary>
            /// This method will return a list of requests including both inbound and
            /// outbound requests.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='skip'>
            /// How many requests to skip (default = 0)
            /// </param>
            /// <param name='take'>
            /// How many requests to return (default = 10, max = 50)
            /// </param>
            /// <param name='fields'>
            /// Properties you want to return, defaults to id
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<FriendRequest>> GetFriendRequestsAsync(this IFriends operations, string apiVersion, int? skip = 0, int? take = 10, string fields = "id", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetFriendRequestsWithHttpMessagesAsync(apiVersion, skip, take, fields, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Create a friend request
            /// </summary>
            /// This method will return the friend request created.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='friendRequest'>
            /// The friend request to create.
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// Properties you want to return, defaults to id
            /// </param>
            public static FriendRequest CreateFriendRequest(this IFriends operations, FriendRequest friendRequest, string apiVersion, string fields = "id")
            {
                return Task.Factory.StartNew(s => ((IFriends)s).CreateFriendRequestAsync(friendRequest, apiVersion, fields), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Create a friend request
            /// </summary>
            /// This method will return the friend request created.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='friendRequest'>
            /// The friend request to create.
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// Properties you want to return, defaults to id
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<FriendRequest> CreateFriendRequestAsync(this IFriends operations, FriendRequest friendRequest, string apiVersion, string fields = "id", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.CreateFriendRequestWithHttpMessagesAsync(friendRequest, apiVersion, fields, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Accept a friend request
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='requestId'>
            /// friend request identifier
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            public static void AcceptFriendRequest(this IFriends operations, int requestId, string apiVersion)
            {
                Task.Factory.StartNew(s => ((IFriends)s).AcceptFriendRequestAsync(requestId, apiVersion), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Accept a friend request
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='requestId'>
            /// friend request identifier
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task AcceptFriendRequestAsync(this IFriends operations, int requestId, string apiVersion, CancellationToken cancellationToken = default(CancellationToken))
            {
                await operations.AcceptFriendRequestWithHttpMessagesAsync(requestId, apiVersion, null, cancellationToken).ConfigureAwait(false);
            }

            /// <summary>
            /// Removes a friend
            /// </summary>
            /// This method will return no content.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='userCode'>
            /// The identifier of the friend (their user reference code)
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            public static object RemoveFriend(this IFriends operations, string userCode, string apiVersion)
            {
                return Task.Factory.StartNew(s => ((IFriends)s).RemoveFriendAsync(userCode, apiVersion), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Removes a friend
            /// </summary>
            /// This method will return no content.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='userCode'>
            /// The identifier of the friend (their user reference code)
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> RemoveFriendAsync(this IFriends operations, string userCode, string apiVersion, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.RemoveFriendWithHttpMessagesAsync(userCode, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Delete a friend request
            /// </summary>
            /// This method will return no content.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='requestId'>
            /// The identifier of the friend request
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            public static object DeleteFriendRequest(this IFriends operations, int requestId, string apiVersion)
            {
                return Task.Factory.StartNew(s => ((IFriends)s).DeleteFriendRequestAsync(requestId, apiVersion), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Delete a friend request
            /// </summary>
            /// This method will return no content.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='requestId'>
            /// The identifier of the friend request
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> DeleteFriendRequestAsync(this IFriends operations, int requestId, string apiVersion, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteFriendRequestWithHttpMessagesAsync(requestId, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}