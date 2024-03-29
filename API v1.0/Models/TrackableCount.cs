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

    public partial class TrackableCount
    {
        /// <summary>
        /// Initializes a new instance of the TrackableCount class.
        /// </summary>
        public TrackableCount() { }

        /// <summary>
        /// Initializes a new instance of the TrackableCount class.
        /// </summary>
        public TrackableCount(TrackableType trackableType = default(TrackableType), int? count = default(int?))
        {
            TrackableType = trackableType;
            Count = count;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "trackableType")]
        public TrackableType TrackableType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "count")]
        public int? Count { get; set; }

    }
}
