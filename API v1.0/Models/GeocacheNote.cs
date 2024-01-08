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

    public partial class GeocacheNote
    {
        /// <summary>
        /// Initializes a new instance of the GeocacheNote class.
        /// </summary>
        public GeocacheNote() { }

        /// <summary>
        /// Initializes a new instance of the GeocacheNote class.
        /// </summary>
        public GeocacheNote(string note)
        {
            Note = note;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "note")]
        public string Note { get; set; }

        /// <summary>
        /// Validate the object. Throws ValidationException if validation fails.
        /// </summary>
        public virtual void Validate()
        {
            if (Note == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "Note");
            }
        }
    }
}
