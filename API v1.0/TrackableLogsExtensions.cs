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
    /// Extension methods for TrackableLogs.
    /// </summary>
    public static partial class TrackableLogsExtensions
    {
            /// <summary>
            /// Get a single trackable log
            /// </summary>
            /// This method will return a single trackable log.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the trackable log (example: TL100).
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// Property fields you want to return, defaults to referencecode
            /// </param>
            /// <param name='expand'>
            /// </param>
            public static TrackableLog GetTrackableLog(this ITrackableLogs operations, string referenceCode, string apiVersion, string fields = "referencecode", string expand = "")
            {
                return Task.Factory.StartNew(s => ((ITrackableLogs)s).GetTrackableLogAsync(referenceCode, apiVersion, fields, expand), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Get a single trackable log
            /// </summary>
            /// This method will return a single trackable log.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the trackable log (example: TL100).
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// Property fields you want to return, defaults to referencecode
            /// </param>
            /// <param name='expand'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<TrackableLog> GetTrackableLogAsync(this ITrackableLogs operations, string referenceCode, string apiVersion, string fields = "referencecode", string expand = "", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetTrackableLogWithHttpMessagesAsync(referenceCode, apiVersion, fields, expand, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Update a trackable log
            /// </summary>
            /// This method will return a trackable log.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The log reference code (example: TL100).
            /// </param>
            /// <param name='log'>
            /// An instance of the log that is being modified. Text is the only modified
            /// parameter
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// Property fields you want to return, defaults to referencecode
            /// </param>
            public static TrackableLog UpdateTrackableLog(this ITrackableLogs operations, string referenceCode, TrackableLog log, string apiVersion, string fields = "referencecode")
            {
                return Task.Factory.StartNew(s => ((ITrackableLogs)s).UpdateTrackableLogAsync(referenceCode, log, apiVersion, fields), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Update a trackable log
            /// </summary>
            /// This method will return a trackable log.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The log reference code (example: TL100).
            /// </param>
            /// <param name='log'>
            /// An instance of the log that is being modified. Text is the only modified
            /// parameter
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
            public static async Task<TrackableLog> UpdateTrackableLogAsync(this ITrackableLogs operations, string referenceCode, TrackableLog log, string apiVersion, string fields = "referencecode", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.UpdateTrackableLogWithHttpMessagesAsync(referenceCode, log, apiVersion, fields, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Deletes a trackable log
            /// </summary>
            /// This method will not return anything in the body.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the trackable log (example: TL100).
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            public static object DeleteTrackableLog(this ITrackableLogs operations, string referenceCode, string apiVersion)
            {
                return Task.Factory.StartNew(s => ((ITrackableLogs)s).DeleteTrackableLogAsync(referenceCode, apiVersion), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Deletes a trackable log
            /// </summary>
            /// This method will not return anything in the body.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the trackable log (example: TL100).
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> DeleteTrackableLogAsync(this ITrackableLogs operations, string referenceCode, string apiVersion, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteTrackableLogWithHttpMessagesAsync(referenceCode, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Get a the images attached to a trackable log
            /// </summary>
            /// This method will return a list of images.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the trackable log (example: TL100).
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
            /// Properties you want to return. Defaults to referencecode.
            /// </param>
            public static IList<Image> GetImages(this ITrackableLogs operations, string referenceCode, string apiVersion, int? skip = 0, int? take = 10, string fields = "referencecode")
            {
                return Task.Factory.StartNew(s => ((ITrackableLogs)s).GetImagesAsync(referenceCode, apiVersion, skip, take, fields), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Get a the images attached to a trackable log
            /// </summary>
            /// This method will return a list of images.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the trackable log (example: TL100).
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
            /// Properties you want to return. Defaults to referencecode.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<Image>> GetImagesAsync(this ITrackableLogs operations, string referenceCode, string apiVersion, int? skip = 0, int? take = 10, string fields = "referencecode", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetImagesWithHttpMessagesAsync(referenceCode, apiVersion, skip, take, fields, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Add an image to a trackable log
            /// </summary>
            /// This method will return a single image.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the trackable log (example: TL100).
            /// </param>
            /// <param name='image'>
            /// image to add
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// Property fields you want to return, defaults to url
            /// </param>
            public static Image AddImage(this ITrackableLogs operations, string referenceCode, PostImage image, string apiVersion, string fields = "url")
            {
                return Task.Factory.StartNew(s => ((ITrackableLogs)s).AddImageAsync(referenceCode, image, apiVersion, fields), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Add an image to a trackable log
            /// </summary>
            /// This method will return a single image.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the trackable log (example: TL100).
            /// </param>
            /// <param name='image'>
            /// image to add
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            /// <param name='fields'>
            /// Property fields you want to return, defaults to url
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<Image> AddImageAsync(this ITrackableLogs operations, string referenceCode, PostImage image, string apiVersion, string fields = "url", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.AddImageWithHttpMessagesAsync(referenceCode, image, apiVersion, fields, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Add a log to a trackable
            /// </summary>
            /// This method will return the created trackable log.
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
            /// Property fields you want to return, defaults to referencecode
            /// </param>
            public static TrackableLog CreateTrackableLog(this ITrackableLogs operations, PostTrackableLog log, string apiVersion, string fields = "referenceCode")
            {
                return Task.Factory.StartNew(s => ((ITrackableLogs)s).CreateTrackableLogAsync(log, apiVersion, fields), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Add a log to a trackable
            /// </summary>
            /// This method will return the created trackable log.
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
            /// Property fields you want to return, defaults to referencecode
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<TrackableLog> CreateTrackableLogAsync(this ITrackableLogs operations, PostTrackableLog log, string apiVersion, string fields = "referenceCode", CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.CreateTrackableLogWithHttpMessagesAsync(log, apiVersion, fields, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Deletes a trackable log image
            /// </summary>
            /// This method will not return anything in the body.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the trackable log (example: TL100).
            /// </param>
            /// <param name='imageGuid'>
            /// the guid of the image
            /// </param>
            /// <param name='apiVersion'>
            /// The requested API version
            /// </param>
            public static object DeleteTrackableLogImages(this ITrackableLogs operations, string referenceCode, Guid imageGuid, string apiVersion)
            {
                return Task.Factory.StartNew(s => ((ITrackableLogs)s).DeleteTrackableLogImagesAsync(referenceCode, imageGuid, apiVersion), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Deletes a trackable log image
            /// </summary>
            /// This method will not return anything in the body.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='referenceCode'>
            /// The reference code of the trackable log (example: TL100).
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
            public static async Task<object> DeleteTrackableLogImagesAsync(this ITrackableLogs operations, string referenceCode, Guid imageGuid, string apiVersion, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.DeleteTrackableLogImagesWithHttpMessagesAsync(referenceCode, imageGuid, apiVersion, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

    }
}
