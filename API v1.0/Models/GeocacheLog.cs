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

    public partial class GeocacheLog
    {
        /// <summary>
        /// Initializes a new instance of the GeocacheLog class.
        /// </summary>
        public GeocacheLog() { }

        /// <summary>
        /// Initializes a new instance of the GeocacheLog class.
        /// </summary>
        public GeocacheLog(DateTime loggedDate, string text, string geocacheCode, string referenceCode = default(string), string ownerCode = default(string), User owner = default(User), int? imageCount = default(int?), bool? isEncoded = default(bool?), bool? isArchived = default(bool?), IList<Image> images = default(IList<Image>), string url = default(string), string geocacheName = default(string), string ianaTimezoneId = default(string), string type = default(string), GeocacheLogType geocacheLogType = default(GeocacheLogType), Coordinates updatedCoordinates = default(Coordinates), bool? usedFavoritePoint = default(bool?))
        {
            ReferenceCode = referenceCode;
            OwnerCode = ownerCode;
            Owner = owner;
            ImageCount = imageCount;
            IsEncoded = isEncoded;
            IsArchived = isArchived;
            Images = images;
            Url = url;
            GeocacheName = geocacheName;
            IanaTimezoneId = ianaTimezoneId;
            LoggedDate = loggedDate;
            Text = text;
            Type = type;
            GeocacheLogType = geocacheLogType;
            UpdatedCoordinates = updatedCoordinates;
            GeocacheCode = geocacheCode;
            UsedFavoritePoint = usedFavoritePoint;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "referenceCode")]
        public string ReferenceCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ownerCode")]
        public string OwnerCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "owner")]
        public User Owner { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "imageCount")]
        public int? ImageCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "isEncoded")]
        public bool? IsEncoded { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "isArchived")]
        public bool? IsArchived { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "images")]
        public IList<Image> Images { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "geocacheName")]
        public string GeocacheName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ianaTimezoneId")]
        public string IanaTimezoneId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "loggedDate")]
        public DateTime LoggedDate { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "geocacheLogType")]
        public GeocacheLogType GeocacheLogType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "updatedCoordinates")]
        public Coordinates UpdatedCoordinates { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "geocacheCode")]
        public string GeocacheCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "usedFavoritePoint")]
        public bool? UsedFavoritePoint { get; set; }

        /// <summary>
        /// Validate the object. Throws ValidationException if validation fails.
        /// </summary>
        public virtual void Validate()
        {
            if (Text == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Text");
            }
            if (GeocacheCode == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "GeocacheCode");
            }
            if (this.Owner != null)
            {
                this.Owner.Validate();
            }
            if (this.UpdatedCoordinates != null)
            {
                this.UpdatedCoordinates.Validate();
            }
        }
    }
}
