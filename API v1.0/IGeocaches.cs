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
    /// Geocaches operations.
    /// </summary>
    public partial interface IGeocaches
    {
        /// <summary>
        /// Get a single Geocache
        /// </summary>
        /// This method will return a single Geocache.
        /// <param name='referenceCode'>
        /// The reference code of the geocache (example: GC25).
        /// </param>
        /// <param name='apiVersion'>
        /// The requested API version
        /// </param>
        /// <param name='lite'>
        /// Select to return a geocache object without the description and
        /// hints
        /// </param>
        /// <param name='expand'>
        /// fields to include with base geocache object
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
        Task<HttpOperationResponse<Geocache>> GetGeocacheWithHttpMessagesAsync(string referenceCode, string apiVersion, bool? lite = false, string expand = "", string fields = "referenceCode", Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Get a list of images for a geocache
        /// </summary>
        /// This method will return a list of images.
        /// <param name='referenceCode'>
        /// The reference code of the geocache (example: GC25).
        /// </param>
        /// <param name='apiVersion'>
        /// The requested API version
        /// </param>
        /// <param name='skip'>
        /// how many images to skip
        /// </param>
        /// <param name='take'>
        /// how many images to retrieve
        /// </param>
        /// <param name='fields'>
        /// fields you want to return, defaults to "url"
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<Image>>> GetImagesWithHttpMessagesAsync(string referenceCode, string apiVersion, int? skip = 0, int? take = 10, string fields = "url", Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Get a list of Users that have favorited a geocache
        /// </summary>
        /// This method will return a list of users.
        /// <param name='referenceCode'>
        /// The reference code of the geocache (example: GC25)
        /// </param>
        /// <param name='apiVersion'>
        /// The requested API version
        /// </param>
        /// <param name='skip'>
        /// how many users to skip
        /// </param>
        /// <param name='take'>
        /// how many users to retrieve
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
        Task<HttpOperationResponse<IList<User>>> GetFavoritedByWithHttpMessagesAsync(string referenceCode, string apiVersion, int? skip = 0, int? take = 10, string fields = "referenceCode", Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Get a list of geocaches
        /// </summary>
        /// This method will return a list of geocaches.
        /// <param name='referenceCodes'>
        /// comma delimited list of geocache reference codes to retrieve
        /// (example: GC25,GC26,GC27).
        /// </param>
        /// <param name='apiVersion'>
        /// The requested API version
        /// </param>
        /// <param name='lite'>
        /// Select to return a geocache object without the description and
        /// hints
        /// </param>
        /// <param name='expand'>
        /// fields to include with base geocache object
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
        Task<HttpOperationResponse<IList<Geocache>>> GetGeocachesWithHttpMessagesAsync(string referenceCodes, string apiVersion, bool? lite = false, string expand = "", string fields = "referenceCode", Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Get a list of trackables in a geocache
        /// </summary>
        /// This method will return a list of trackables.
        /// <param name='referenceCode'>
        /// The reference code of the geocache (example: GC25).
        /// </param>
        /// <param name='apiVersion'>
        /// The requested API version
        /// </param>
        /// <param name='skip'>
        /// how many trackables to skip
        /// </param>
        /// <param name='take'>
        /// how many trackables to retrieve
        /// </param>
        /// <param name='fields'>
        /// fields you want to return, defaults to referenceCode
        /// </param>
        /// <param name='expand'>
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<Trackable>>> GetTrackablesWithHttpMessagesAsync(string referenceCode, string apiVersion, int? skip = 0, int? take = 10, string fields = "referenceCode", string expand = "", Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Get a list of geocache logs for the specified geocache
        /// </summary>
        /// This method will return a list of geocache logs.
        /// <param name='referenceCode'>
        /// The reference code of the geocache (example: GC25).
        /// </param>
        /// <param name='apiVersion'>
        /// The requested API version
        /// </param>
        /// <param name='skip'>
        /// how many logs to skip over
        /// </param>
        /// <param name='take'>
        /// how many logs to retrieve
        /// </param>
        /// <param name='expand'>
        /// fields to include with base geocache object
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
        Task<HttpOperationResponse<IList<GeocacheLog>>> GetLogsWithHttpMessagesAsync(string referenceCode, string apiVersion, int? skip = 0, int? take = 10, string expand = "", string fields = "referenceCode", Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Search for Geocaches
        /// </summary>
        /// This method will return search results.
        /// <param name='q'>
        /// The query used on the geocaches
        /// </param>
        /// <param name='apiVersion'>
        /// The requested API version
        /// </param>
        /// <param name='sort'>
        /// Defaults to distance if coords are provided otherwise
        /// favoritepoints (distance, favorites, cachename, size, difficulty,
        /// terrain, founddate, placeddate, id)
        /// </param>
        /// <param name='lite'>
        /// Return a lite version of geocache (no description, hint, or
        /// </param>
        /// <param name='skip'>
        /// Defaults to 0, how many geocaches to skip
        /// </param>
        /// <param name='take'>
        /// Defaults to 20, how many geocaches to return
        /// </param>
        /// <param name='expand'>
        /// fields to include with base geocache object
        /// </param>
        /// <param name='fields'>
        /// Properties you want to return, defaults to "referencecode"
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<IList<Geocache>>> SearchWithHttpMessagesAsync(string q, string apiVersion, string sort = "", bool? lite = true, int? skip = 0, int? take = 50, string expand = "", string fields = "referenceCode", Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}
