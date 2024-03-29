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
    /// Extension methods for UserWaypoints.
    /// </summary>
    public static partial class UserWaypointsExtensions
    {
            /// <summary>
            /// Get a list of user waypoints for the calling user
            /// </summary>
            /// This method will return an array of user waypoints.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='skip'>
            /// How many waypoints to skip (default = 0)
            /// </param>
            /// <param name='take'>
            /// How many drafts to return (default = 10)
            /// </param>
            /// <param name='includeCorrectedCoordinates'>
            /// Include corrected coordinates in the results. default = false
            /// </param>
            /// <param name='fields'>
            /// Properties you want to return (default = referenceCode)
            /// </param>
            public static IList<UserWaypoint> GetUserWaypoints(this IUserWaypoints operations, string apiVersion, int? skip = 0, int? take = 10, bool? includeCorrectedCoordinates = false, string fields = "referencecode")
            {
                return Task.Factory.StartNew(s => ((IUserWaypoints)s).GetUserWaypointsAsync(apiVersion, skip, take, includeCorrectedCoordinates, fields), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Get a list of user waypoints for the calling user
            /// </summary>
            /// This method will return an array of user waypoints.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='skip'>
            /// How many waypoints to skip (default = 0)
            /// </param>
            /// <param name='take'>
            /// How many drafts to return (default = 10)
            /// </param>
            /// <param name='includeCorrectedCoordinates'>
            /// Include corrected coordinates in the results. default = false
            /// </param>
            /// <param name='fields'>
            /// Properties you want to return (default = referenceCode)
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<UserWaypoint>> GetUserWaypointsAsync(this IUserWaypoints operations, string apiVersion, int? skip = 0, int? take = 10, bool? includeCorrectedCoordinates = false, string fields = "referencecode", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetUserWaypointsWithHttpMessagesAsync(apiVersion, skip, take, includeCorrectedCoordinates, fields, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Create a user waypoint
            /// </summary>
            /// This method will return the user waypoint created.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='userWaypoint'>
            /// The user waypoint to create.
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// Properties you want to return (default = referenceCode)
            /// </param>
            public static UserWaypoint CreateUserWaypoint(this IUserWaypoints operations, PostUserWaypoint userWaypoint, string apiVersion, string fields = "referencecode")
            {
                return Task.Factory.StartNew(s => ((IUserWaypoints)s).CreateUserWaypointAsync(userWaypoint, apiVersion, fields), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Create a user waypoint
            /// </summary>
            /// This method will return the user waypoint created.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='userWaypoint'>
            /// The user waypoint to create.
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// Properties you want to return (default = referenceCode)
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<UserWaypoint> CreateUserWaypointAsync(this IUserWaypoints operations, PostUserWaypoint userWaypoint, string apiVersion, string fields = "referencecode", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.CreateUserWaypointWithHttpMessagesAsync(userWaypoint, apiVersion, fields, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Gets the user waypoints for a geocache
            /// </summary>
            /// This method will return a paged list of userwaypoints
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the geocache (example: GC25).
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='skip'>
            /// How many user waypoints to skip. default = 0
            /// </param>
            /// <param name='take'>
            /// How many user waypoints to include in result set. default = 10
            /// </param>
            /// <param name='includeCorrectedCoordinates'>
            /// Include corrected coordinates in the results. default = false
            /// </param>
            /// <param name='fields'>
            /// Properties you want to return. default = referencecode
            /// </param>
            public static IList<UserWaypoint> GetGeocacheUserWaypoints(this IUserWaypoints operations, string referenceCode, string apiVersion, int? skip = 0, int? take = 10, bool? includeCorrectedCoordinates = false, string fields = "referencecode")
            {
                return Task.Factory.StartNew(s => ((IUserWaypoints)s).GetGeocacheUserWaypointsAsync(referenceCode, apiVersion, skip, take, includeCorrectedCoordinates, fields), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Gets the user waypoints for a geocache
            /// </summary>
            /// This method will return a paged list of userwaypoints
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the geocache (example: GC25).
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='skip'>
            /// How many user waypoints to skip. default = 0
            /// </param>
            /// <param name='take'>
            /// How many user waypoints to include in result set. default = 10
            /// </param>
            /// <param name='includeCorrectedCoordinates'>
            /// Include corrected coordinates in the results. default = false
            /// </param>
            /// <param name='fields'>
            /// Properties you want to return. default = referencecode
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<UserWaypoint>> GetGeocacheUserWaypointsAsync(this IUserWaypoints operations, string referenceCode, string apiVersion, int? skip = 0, int? take = 10, bool? includeCorrectedCoordinates = false, string fields = "referencecode", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetGeocacheUserWaypointsWithHttpMessagesAsync(referenceCode, apiVersion, skip, take, includeCorrectedCoordinates, fields, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Update a user waypoint
            /// </summary>
            /// This method will return the updated user waypoint.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The identifier of the user waypoint
            /// </param>
            /// <param name='userWaypoint'>
            /// The user waypoint to update.
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// Properties you want to return (default = referenceCode)
            /// </param>
            public static UserWaypoint UpdateUserWaypoint(this IUserWaypoints operations, string referenceCode, UserWaypoint userWaypoint, string apiVersion, string fields = "referencecode")
            {
                return Task.Factory.StartNew(s => ((IUserWaypoints)s).UpdateUserWaypointAsync(referenceCode, userWaypoint, apiVersion, fields), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Update a user waypoint
            /// </summary>
            /// This method will return the updated user waypoint.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The identifier of the user waypoint
            /// </param>
            /// <param name='userWaypoint'>
            /// The user waypoint to update.
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// Properties you want to return (default = referenceCode)
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<UserWaypoint> UpdateUserWaypointAsync(this IUserWaypoints operations, string referenceCode, UserWaypoint userWaypoint, string apiVersion, string fields = "referencecode", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.UpdateUserWaypointWithHttpMessagesAsync(referenceCode, userWaypoint, apiVersion, fields, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Delete a user waypoint
            /// </summary>
            /// This method will return no content.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The identifier of the user waypoint
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            public static object DeleteUserWaypoint(this IUserWaypoints operations, string referenceCode, string apiVersion)
            {
                return Task.Factory.StartNew(s => ((IUserWaypoints)s).DeleteUserWaypointAsync(referenceCode, apiVersion), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Delete a user waypoint
            /// </summary>
            /// This method will return no content.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The identifier of the user waypoint
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> DeleteUserWaypointAsync(this IUserWaypoints operations, string referenceCode, string apiVersion, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteUserWaypointWithHttpMessagesAsync(referenceCode, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Upsert a corrected coordinate for the calling user
            /// </summary>
            /// This method will return the created or updated corrected coordinate.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// the geocache identifier
            /// </param>
            /// <param name='coordinates'>
            /// The corrected coordinates to upsert
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// Properties you want to return (default = referenceCode)
            /// </param>
            public static UserWaypoint UpsertCorrectedCoordinates(this IUserWaypoints operations, string referenceCode, Coordinates coordinates, string apiVersion, string fields = "referencecode")
            {
                return Task.Factory.StartNew(s => ((IUserWaypoints)s).UpsertCorrectedCoordinatesAsync(referenceCode, coordinates, apiVersion, fields), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Upsert a corrected coordinate for the calling user
            /// </summary>
            /// This method will return the created or updated corrected coordinate.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// the geocache identifier
            /// </param>
            /// <param name='coordinates'>
            /// The corrected coordinates to upsert
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// Properties you want to return (default = referenceCode)
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<UserWaypoint> UpsertCorrectedCoordinatesAsync(this IUserWaypoints operations, string referenceCode, Coordinates coordinates, string apiVersion, string fields = "referencecode", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.UpsertCorrectedCoordinatesWithHttpMessagesAsync(referenceCode, coordinates, apiVersion, fields, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Delete a corrected coordinate for the calling user
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// geocache identifier
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            public static object DeleteCorrectedCoordinates(this IUserWaypoints operations, string referenceCode, string apiVersion)
            {
                return Task.Factory.StartNew(s => ((IUserWaypoints)s).DeleteCorrectedCoordinatesAsync(referenceCode, apiVersion), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Delete a corrected coordinate for the calling user
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// geocache identifier
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> DeleteCorrectedCoordinatesAsync(this IUserWaypoints operations, string referenceCode, string apiVersion, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteCorrectedCoordinatesWithHttpMessagesAsync(referenceCode, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
