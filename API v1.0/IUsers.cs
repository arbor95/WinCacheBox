﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace GeocachingAPI
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Rest;
    using Models;

    /// <summary>
    /// Users operations.
    /// </summary>
    public partial interface IUsers
    {
        /// <summary>
        /// Get a user
        /// </summary>
        /// This method will return a user.
        /// <param name='referenceCode'>
        /// The reference code of the user (example: PR18).
        /// </param>
        /// <param name='apiVersion'>
        /// The requested API version
        /// </param>
        /// <param name='fields'>
        /// Property fields you want to return, defaults to referenceCode
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<User>> GetUserWithHttpMessagesAsync(string referenceCode, string apiVersion, string fields = "referenceCode", Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Get the images attached to a user profile
        /// </summary>
        /// This method will return a list of images.
        /// <param name='referenceCode'>
        /// The reference code of the user (example: PR18).
        /// </param>
        /// <param name='apiVersion'>
        /// The requested API version
        /// </param>
        /// <param name='skip'>
        /// Amount of images to skip over used for pagination. Defaults to 0.
        /// </param>
        /// <param name='take'>
        /// Amount of images to include in results. Defaults to 10.
        /// </param>
        /// <param name='fields'>
        /// Properties you want to return. Defaults to url.
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<Image>>> GetImagesWithHttpMessagesAsync(string referenceCode, string apiVersion, int? skip = 0, int? take = 10, string fields = "url", Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Get an account's souvenirs
        /// </summary>
        /// This method will return a list of souvenirs.
        /// <param name='referenceCode'>
        /// The reference code of the user (example: PR18).
        /// </param>
        /// <param name='apiVersion'>
        /// The requested API version
        /// </param>
        /// <param name='skip'>
        /// </param>
        /// <param name='take'>
        /// </param>
        /// <param name='fields'>
        /// Property fields you want to return, defaults to title
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<Souvenir>>> GetSouvenirsWithHttpMessagesAsync(string referenceCode, string apiVersion, int? skip = 0, int? take = 20, string fields = "title", Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Get a list of users
        /// </summary>
        /// This method will return a list of users.
        /// <param name='apiVersion'>
        /// The requested API version
        /// </param>
        /// <param name='referenceCodes'>
        /// comma delimited list of user reference codes to retrieve (example:
        /// PR1,PR2,PR3)
        /// </param>
        /// <param name='usernames'>
        /// comma delimited list of usernames to retrieve
        /// </param>
        /// <param name='fields'>
        /// fields you want to return, defaults to "referenceCode"
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<User>>> GetUsersWithHttpMessagesAsync(string apiVersion, string referenceCodes = default(string), string usernames = default(string), string fields = "referenceCode", Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Get a list of user's geocache lists
        /// </summary>
        /// This method will return a list of geocache lists.
        /// <param name='referenceCode'>
        /// user identifier, use "me" to get lists for calling user
        /// </param>
        /// <param name='apiVersion'>
        /// The requested API version
        /// </param>
        /// <param name='types'>
        /// comma delimited list of list types to return (fl, wl, il, bm, pq).
        /// Defaults to "bm"
        /// </param>
        /// <param name='skip'>
        /// how many lists to skip over
        /// </param>
        /// <param name='take'>
        /// how many lists to retrieve
        /// </param>
        /// <param name='fields'>
        /// fields you want to return, defaults to referenceCode
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<GeocacheList>>> GetListsWithHttpMessagesAsync(string referenceCode, string apiVersion, string types = "bm", int? skip = 0, int? take = 10, string fields = "referenceCode", Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Get a list of a user's geocache logs
        /// </summary>
        /// This method will return a list of geocache lists.
        /// <param name='referenceCode'>
        /// user identifier, use "me" to get lists for calling user
        /// </param>
        /// <param name='apiVersion'>
        /// The requested API version
        /// </param>
        /// <param name='skip'>
        /// how many lists to skip over
        /// </param>
        /// <param name='take'>
        /// how many lists to retrieve
        /// </param>
        /// <param name='afterDate'>
        /// filters results to logs with logdates after this date (inclusive)
        /// </param>
        /// <param name='beforeDate'>
        /// filters results to logs with logdates before this date (inclusive)
        /// </param>
        /// <param name='fields'>
        /// fields you want to return, defaults to referenceCode
        /// </param>
        /// <param name='includeArchived'>
        /// flag to include archived logs
        /// </param>
        /// <param name='logTypes'>
        /// log types to include in response, defaults to all
        /// </param>
        /// <param name='expand'>
        /// fields to include with base geocache log object
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<GeocacheLog>>> GetGeocacheLogsWithHttpMessagesAsync(string referenceCode, string apiVersion, int? skip = 0, int? take = 10, DateTime? afterDate = default(DateTime?), DateTime? beforeDate = default(DateTime?), string fields = "referenceCode", bool? includeArchived = false, string logTypes = default(string), string expand = "", Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}