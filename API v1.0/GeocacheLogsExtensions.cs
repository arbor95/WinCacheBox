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
    /// Extension methods for GeocacheLogs.
    /// </summary>
    public static partial class GeocacheLogsExtensions
    {
            /// <summary>
            /// Get a single geocache log
            /// </summary>
            /// This method will return a single geocache log.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the geocache log (example: GL100).
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='expand'>
            /// fields to include with base geocache log object
            /// </param>
            /// <param name='fields'>
            /// Property fields you want to return, defaults to referencecode
            /// </param>
            public static GeocacheLog GetGeocacheLog(this IGeocacheLogs operations, string referenceCode, string apiVersion, string expand = "", string fields = "referencecode")
            {
                return Task.Factory.StartNew(s => ((IGeocacheLogs)s).GetGeocacheLogAsync(referenceCode, apiVersion, expand, fields), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Get a single geocache log
            /// </summary>
            /// This method will return a single geocache log.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the geocache log (example: GL100).
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='expand'>
            /// fields to include with base geocache log object
            /// </param>
            /// <param name='fields'>
            /// Property fields you want to return, defaults to referencecode
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<GeocacheLog> GetGeocacheLogAsync(this IGeocacheLogs operations, string referenceCode, string apiVersion, string expand = "", string fields = "referencecode", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetGeocacheLogWithHttpMessagesAsync(referenceCode, apiVersion, expand, fields, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Update a geocache log
            /// </summary>
            /// This method will return a geocache log.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The log reference code (example: GL100).
            /// </param>
            /// <param name='log'>
            /// An instance of the log that is being modified
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// Property fields you want to return, defaults to referencecode
            /// </param>
            public static GeocacheLog UpdateGeocacheLog(this IGeocacheLogs operations, string referenceCode, GeocacheLog log, string apiVersion, string fields = "referencecode")
            {
                return Task.Factory.StartNew(s => ((IGeocacheLogs)s).UpdateGeocacheLogAsync(referenceCode, log, apiVersion, fields), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Update a geocache log
            /// </summary>
            /// This method will return a geocache log.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The log reference code (example: GL100).
            /// </param>
            /// <param name='log'>
            /// An instance of the log that is being modified
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// Property fields you want to return, defaults to referencecode
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<GeocacheLog> UpdateGeocacheLogAsync(this IGeocacheLogs operations, string referenceCode, GeocacheLog log, string apiVersion, string fields = "referencecode", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.UpdateGeocacheLogWithHttpMessagesAsync(referenceCode, log, apiVersion, fields, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Delete a geocache log
            /// </summary>
            /// This method will not have a response body.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the geocache log (example: GL100).
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            public static object DeleteGeocacheLog(this IGeocacheLogs operations, string referenceCode, string apiVersion)
            {
                return Task.Factory.StartNew(s => ((IGeocacheLogs)s).DeleteGeocacheLogAsync(referenceCode, apiVersion), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Delete a geocache log
            /// </summary>
            /// This method will not have a response body.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the geocache log (example: GL100).
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> DeleteGeocacheLogAsync(this IGeocacheLogs operations, string referenceCode, string apiVersion, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteGeocacheLogWithHttpMessagesAsync(referenceCode, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get a the images attached to a geocache log
            /// </summary>
            /// This method will return a list of images.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the geocache log (example: GL100).
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
            /// Properties you want to return. Defaults to "url".
            /// </param>
            public static IList<Image> GetImages(this IGeocacheLogs operations, string referenceCode, string apiVersion, int? skip = 0, int? take = 10, string fields = "url")
            {
                return Task.Factory.StartNew(s => ((IGeocacheLogs)s).GetImagesAsync(referenceCode, apiVersion, skip, take, fields), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Get a the images attached to a geocache log
            /// </summary>
            /// This method will return a list of images.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the geocache log (example: GL100).
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
            /// Properties you want to return. Defaults to "url".
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<Image>> GetImagesAsync(this IGeocacheLogs operations, string referenceCode, string apiVersion, int? skip = 0, int? take = 10, string fields = "url", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetImagesWithHttpMessagesAsync(referenceCode, apiVersion, skip, take, fields, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Add an image to a geocache log
            /// </summary>
            /// This method will return a single Geocache.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the geocache log (example: GL100).
            /// </param>
            /// <param name='image'>
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// fields to return on the response object, defaults to "url"
            /// </param>
            public static Image AddImage(this IGeocacheLogs operations, string referenceCode, PostImage image, string apiVersion, string fields = "url")
            {
                return Task.Factory.StartNew(s => ((IGeocacheLogs)s).AddImageAsync(referenceCode, image, apiVersion, fields), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Add an image to a geocache log
            /// </summary>
            /// This method will return a single Geocache.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the geocache log (example: GL100).
            /// </param>
            /// <param name='image'>
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// fields to return on the response object, defaults to "url"
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<Image> AddImageAsync(this IGeocacheLogs operations, string referenceCode, PostImage image, string apiVersion, string fields = "url", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AddImageWithHttpMessagesAsync(referenceCode, image, apiVersion, fields, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Add a log to a geocache
            /// </summary>
            /// This method will return the created geocache log.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='log'>
            /// The log to add
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// the fields to return in the response body, defaults to referencecode
            /// </param>
            public static GeocacheLog CreateGeocacheLog(this IGeocacheLogs operations, PostGeocacheLog log, string apiVersion, string fields = "referencecode")
            {
                return Task.Factory.StartNew(s => ((IGeocacheLogs)s).CreateGeocacheLogAsync(log, apiVersion, fields), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Add a log to a geocache
            /// </summary>
            /// This method will return the created geocache log.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='log'>
            /// The log to add
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// the fields to return in the response body, defaults to referencecode
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<GeocacheLog> CreateGeocacheLogAsync(this IGeocacheLogs operations, PostGeocacheLog log, string apiVersion, string fields = "referencecode", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.CreateGeocacheLogWithHttpMessagesAsync(log, apiVersion, fields, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Deletes a geocache log image
            /// </summary>
            /// This method will not return anything in the body.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the geocache log (example: GL100).
            /// </param>
            /// <param name='imageGuid'>
            /// the guid of the image
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            public static object DeleteGeocacheLogImages(this IGeocacheLogs operations, string referenceCode, Guid imageGuid, string apiVersion)
            {
                return Task.Factory.StartNew(s => ((IGeocacheLogs)s).DeleteGeocacheLogImagesAsync(referenceCode, imageGuid, apiVersion), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Deletes a geocache log image
            /// </summary>
            /// This method will not return anything in the body.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the geocache log (example: GL100).
            /// </param>
            /// <param name='imageGuid'>
            /// the guid of the image
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> DeleteGeocacheLogImagesAsync(this IGeocacheLogs operations, string referenceCode, Guid imageGuid, string apiVersion, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteGeocacheLogImagesWithHttpMessagesAsync(referenceCode, imageGuid, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
