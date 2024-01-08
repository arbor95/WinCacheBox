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

    public partial class GeoTour
    {
        /// <summary>
        /// Initializes a new instance of the GeoTour class.
        /// </summary>
        public GeoTour() { }

        /// <summary>
        /// Initializes a new instance of the GeoTour class.
        /// </summary>
        public GeoTour(string referenceCode = default(string), string name = default(string), string description = default(string), Coordinates postedCoordinates = default(Coordinates), int? geocacheCount = default(int?), string url = default(string), string coverImageUrl = default(string), string logoImageUrl = default(string), int? favoritePoints = default(int?), Sponsor sponsor = default(Sponsor))
        {
            ReferenceCode = referenceCode;
            Name = name;
            Description = description;
            PostedCoordinates = postedCoordinates;
            GeocacheCount = geocacheCount;
            Url = url;
            CoverImageUrl = coverImageUrl;
            LogoImageUrl = logoImageUrl;
            FavoritePoints = favoritePoints;
            Sponsor = sponsor;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "referenceCode")]
        public string ReferenceCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "postedCoordinates")]
        public Coordinates PostedCoordinates { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "geocacheCount")]
        public int? GeocacheCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "coverImageUrl")]
        public string CoverImageUrl { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "logoImageUrl")]
        public string LogoImageUrl { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "favoritePoints")]
        public int? FavoritePoints { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "sponsor")]
        public Sponsor Sponsor { get; set; }

        /// <summary>
        /// Validate the object. Throws ValidationException if validation fails.
        /// </summary>
        public virtual void Validate()
        {
            if (this.PostedCoordinates != null)
            {
                this.PostedCoordinates.Validate();
            }
        }
    }
}