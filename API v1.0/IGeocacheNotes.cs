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
    /// GeocacheNotes operations.
    /// </summary>
    public partial interface IGeocacheNotes
    {
        /// <summary>
        /// Upsert a geocache note for the calling user
        /// </summary>
        /// This method will return the upserted text.
        /// <param name='referenceCode'>
        /// The identifier of the geocache (ex: GC25)
        /// </param>
        /// <param name='geocacheNote'>
        /// The geocache note text.
        /// </param>
        /// <param name='apiVersion'>
        /// The requested API version
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<string>> UpsertNoteWithHttpMessagesAsync(string referenceCode, GeocacheNote geocacheNote, string apiVersion, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// Delete a geocache note for the calling user
        /// </summary>
        /// This method will return no content.
        /// <param name='referenceCode'>
        /// The identifier of the geocache (ex: GC25)
        /// </param>
        /// <param name='apiVersion'>
        /// The requested API version
        /// </param>
        /// <param name='customHeaders'>
        /// The headers that will be added to request.
        /// </param>
        /// <param name='cancellationToken'>
        /// The cancellation token.
        /// </param>
        Task<HttpOperationResponse<object>> DeleteNoteWithHttpMessagesAsync(string referenceCode, string apiVersion, Dictionary<string, List<string>> customHeaders = null, CancellationToken cancellationToken = default(CancellationToken));
    }
}