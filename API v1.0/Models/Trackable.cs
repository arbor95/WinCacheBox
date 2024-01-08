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

    public partial class Trackable
    {
        /// <summary>
        /// Initializes a new instance of the Trackable class.
        /// </summary>
        public Trackable() { }

        /// <summary>
        /// Initializes a new instance of the Trackable class.
        /// </summary>
        public Trackable(string referenceCode = default(string), string iconUrl = default(string), string name = default(string), string goal = default(string), string description = default(string), string releasedDate = default(string), string originCountry = default(string), bool? allowedToBeCollected = default(bool?), string ownerCode = default(string), User owner = default(User), string holderCode = default(string), User holder = default(User), bool? inHolderCollection = default(bool?), string currentGeocacheCode = default(string), bool? isMissing = default(bool?), string type = default(string), TrackableType trackableType = default(TrackableType), int? imageCount = default(int?), string trackingNumber = default(string), string url = default(string), string currentGeocacheName = default(string), double? kilometersTraveled = default(double?), double? milesTraveled = default(double?), IList<TrackableLog> trackableLogs = default(IList<TrackableLog>), IList<Image> images = default(IList<Image>))
        {
            ReferenceCode = referenceCode;
            IconUrl = iconUrl;
            Name = name;
            Goal = goal;
            Description = description;
            ReleasedDate = releasedDate;
            OriginCountry = originCountry;
            AllowedToBeCollected = allowedToBeCollected;
            OwnerCode = ownerCode;
            Owner = owner;
            HolderCode = holderCode;
            Holder = holder;
            InHolderCollection = inHolderCollection;
            CurrentGeocacheCode = currentGeocacheCode;
            IsMissing = isMissing;
            Type = type;
            TrackableType = trackableType;
            ImageCount = imageCount;
            TrackingNumber = trackingNumber;
            Url = url;
            CurrentGeocacheName = currentGeocacheName;
            KilometersTraveled = kilometersTraveled;
            MilesTraveled = milesTraveled;
            TrackableLogs = trackableLogs;
            Images = images;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "referenceCode")]
        public string ReferenceCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "iconUrl")]
        public string IconUrl { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "goal")]
        public string Goal { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "releasedDate")]
        public string ReleasedDate { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "originCountry")]
        public string OriginCountry { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "allowedToBeCollected")]
        public bool? AllowedToBeCollected { get; set; }

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
        [JsonProperty(PropertyName = "holderCode")]
        public string HolderCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "holder")]
        public User Holder { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "inHolderCollection")]
        public bool? InHolderCollection { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "currentGeocacheCode")]
        public string CurrentGeocacheCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "isMissing")]
        public bool? IsMissing { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "trackableType")]
        public TrackableType TrackableType { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "imageCount")]
        public int? ImageCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "trackingNumber")]
        public string TrackingNumber { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "currentGeocacheName")]
        public string CurrentGeocacheName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "kilometersTraveled")]
        public double? KilometersTraveled { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "milesTraveled")]
        public double? MilesTraveled { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "trackableLogs")]
        public IList<TrackableLog> TrackableLogs { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "images")]
        public IList<Image> Images { get; set; }

        /// <summary>
        /// Validate the object. Throws ValidationException if validation fails.
        /// </summary>
        public virtual void Validate()
        {
            if (this.Owner != null)
            {
                this.Owner.Validate();
            }
            if (this.Holder != null)
            {
                this.Holder.Validate();
            }
            if (this.TrackableLogs != null)
            {
                foreach (var element in this.TrackableLogs)
                {
                    if (element != null)
                    {
                        element.Validate();
                    }
                }
            }
        }
    }
}