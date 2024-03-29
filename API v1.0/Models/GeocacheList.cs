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

    public partial class GeocacheList
    {
        /// <summary>
        /// Initializes a new instance of the GeocacheList class.
        /// </summary>
        public GeocacheList() { }

        /// <summary>
        /// Initializes a new instance of the GeocacheList class.
        /// </summary>
        public GeocacheList(string name, string referenceCode = default(string), DateTime? lastUpdatedDateUtc = default(DateTime?), DateTime? createdDateUtc = default(DateTime?), int? count = default(int?), int? findCount = default(int?), string ownerCode = default(string), string url = default(string), string description = default(string), int? typeId = default(int?), bool? isPublic = default(bool?), bool? isShared = default(bool?))
        {
            ReferenceCode = referenceCode;
            LastUpdatedDateUtc = lastUpdatedDateUtc;
            CreatedDateUtc = createdDateUtc;
            Count = count;
            FindCount = findCount;
            OwnerCode = ownerCode;
            Url = url;
            Name = name;
            Description = description;
            TypeId = typeId;
            IsPublic = isPublic;
            IsShared = isShared;
        }

        /// <summary>
        /// This unqiuely identifies the list.  Use this code to get more
        /// details about this list. Example (PQ25)
        /// </summary>
        [JsonProperty(PropertyName = "referenceCode")]
        public string ReferenceCode { get; set; }

        /// <summary>
        /// When was the list last updated.  If the list is a pocket query
        /// then this property references the last time it was generated.
        /// (default order: desc)
        /// </summary>
        [JsonProperty(PropertyName = "lastUpdatedDateUtc")]
        public DateTime? LastUpdatedDateUtc { get; set; }

        /// <summary>
        /// When the list was created
        /// </summary>
        [JsonProperty(PropertyName = "createdDateUtc")]
        public DateTime? CreatedDateUtc { get; set; }

        /// <summary>
        /// Number of items on the list
        /// </summary>
        [JsonProperty(PropertyName = "count")]
        public int? Count { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "findCount")]
        public int? FindCount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ownerCode")]
        public string OwnerCode { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// Name of the list
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Description of the list
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// List Type
        /// </summary>
        [JsonProperty(PropertyName = "typeId")]
        public int? TypeId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "isPublic")]
        public bool? IsPublic { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "isShared")]
        public bool? IsShared { get; set; }

        /// <summary>
        /// Validate the object. Throws ValidationException if validation fails.
        /// </summary>
        public virtual void Validate()
        {
            if (Name == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Name");
            }
        }
    }
}
