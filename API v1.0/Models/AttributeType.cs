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

    public partial class AttributeType
    {
        /// <summary>
        /// Initializes a new instance of the AttributeType class.
        /// </summary>
        public AttributeType() { }

        /// <summary>
        /// Initializes a new instance of the AttributeType class.
        /// </summary>
        public AttributeType(int? id = default(int?), string name = default(string), bool? hasYesOption = default(bool?), bool? hasNoOption = default(bool?), string yesIconUrl = default(string), string noIconUrl = default(string), string notChosenIconUrl = default(string))
        {
            Id = id;
            Name = name;
            HasYesOption = hasYesOption;
            HasNoOption = hasNoOption;
            YesIconUrl = yesIconUrl;
            NoIconUrl = noIconUrl;
            NotChosenIconUrl = notChosenIconUrl;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public int? Id { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "hasYesOption")]
        public bool? HasYesOption { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "hasNoOption")]
        public bool? HasNoOption { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "yesIconUrl")]
        public string YesIconUrl { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "noIconUrl")]
        public string NoIconUrl { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "notChosenIconUrl")]
        public string NotChosenIconUrl { get; set; }

    }
}