﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace GeocachingAPI.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    public partial class LogDraft
    {
        /// <summary>
        /// Initializes a new instance of the LogDraft class.
        /// </summary>
        public LogDraft() { }

        /// <summary>
        /// Initializes a new instance of the LogDraft class.
        /// </summary>
        public LogDraft(string referenceCode = default(string), int? imageCount = default(int?), Guid? guid = default(Guid?), string geocacheCode = default(string), string logType = default(string), GeocacheLogType geocacheLogType = default(GeocacheLogType), string note = default(string), DateTime? loggedDateUtc = default(DateTime?), DateTime? loggedDate = default(DateTime?), bool? useFavoritePoint = default(bool?), string geocacheName = default(string))
        {
            ReferenceCode = referenceCode;
            ImageCount = imageCount;
            Guid = guid;
            GeocacheCode = geocacheCode;
            LogType = logType;
            GeocacheLogType = geocacheLogType;
            Note = note;
            LoggedDateUtc = loggedDateUtc;
            LoggedDate = loggedDate;
            UseFavoritePoint = useFavoritePoint;
            GeocacheName = geocacheName;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "referenceCode")]
        public string ReferenceCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "imageCount")]
        public int? ImageCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "guid")]
        public Guid? Guid { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "geocacheCode")]
        public string GeocacheCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "logType")]
        public string LogType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "geocacheLogType")]
        public GeocacheLogType GeocacheLogType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "loggedDateUtc")]
        public DateTime? LoggedDateUtc { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "loggedDate")]
        public DateTime? LoggedDate { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "useFavoritePoint")]
        public bool? UseFavoritePoint { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "geocacheName")]
        public string GeocacheName { get; set; }

    }
}
