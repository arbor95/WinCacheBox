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

    public partial class PostListGeocache
    {
        /// <summary>
        /// Initializes a new instance of the PostListGeocache class.
        /// </summary>
        public PostListGeocache() { }

        /// <summary>
        /// Initializes a new instance of the PostListGeocache class.
        /// </summary>
        public PostListGeocache(string referenceCode = default(string), string name = default(string))
        {
            ReferenceCode = referenceCode;
            Name = name;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "referenceCode")]
        public string ReferenceCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

    }
}
