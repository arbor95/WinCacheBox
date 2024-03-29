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

    public partial class HQPromotionMetadata
    {
        /// <summary>
        /// Initializes a new instance of the HQPromotionMetadata class.
        /// </summary>
        public HQPromotionMetadata() { }

        /// <summary>
        /// Initializes a new instance of the HQPromotionMetadata class.
        /// </summary>
        public HQPromotionMetadata(string pageTitle = default(string), string linkText = default(string), string linkSubText = default(string), string iconData = default(string), string relativeUrl = default(string), int? campaignId = default(int?), DateTime? linkVisibleStartDateUtc = default(DateTime?), DateTime? linkVisibleEndDateUtc = default(DateTime?))
        {
            PageTitle = pageTitle;
            LinkText = linkText;
            LinkSubText = linkSubText;
            IconData = iconData;
            RelativeUrl = relativeUrl;
            CampaignId = campaignId;
            LinkVisibleStartDateUtc = linkVisibleStartDateUtc;
            LinkVisibleEndDateUtc = linkVisibleEndDateUtc;
        }

        /// <summary>
        /// Page title for the campaign.
        /// </summary>
        [JsonProperty(PropertyName = "pageTitle")]
        public string PageTitle { get; set; }

        /// <summary>
        /// Text that should be displayed on the link/button leading to the
        /// campaign page.
        /// </summary>
        [JsonProperty(PropertyName = "linkText")]
        public string LinkText { get; set; }

        /// <summary>
        /// SubText that should be displayed on the link/button leading to the
        /// campaign page.
        /// </summary>
        [JsonProperty(PropertyName = "linkSubText")]
        public string LinkSubText { get; set; }

        /// <summary>
        /// Byte array containing the icon for the campaign in png
        /// </summary>
        [JsonProperty(PropertyName = "iconData")]
        public string IconData { get; set; }

        /// <summary>
        /// Link off geocaching root for the campaign ("/play/leaderboard",
        /// "/play/hqpromo/campaignname" for example)
        /// </summary>
        [JsonProperty(PropertyName = "relativeUrl")]
        public string RelativeUrl { get; set; }

        /// <summary>
        /// Unique Campaign Identifier
        /// </summary>
        [JsonProperty(PropertyName = "campaignId")]
        public int? CampaignId { get; set; }

        /// <summary>
        /// UTC date and time at which the link should start showing up to get
        /// to the page (on the profile page, in the app, ...)
        /// </summary>
        [JsonProperty(PropertyName = "linkVisibleStartDateUtc")]
        public DateTime? LinkVisibleStartDateUtc { get; set; }

        /// <summary>
        /// UTC date and time at which the link should stop showing up to get
        /// to the page (on the profile page, in the app, ...)
        /// </summary>
        [JsonProperty(PropertyName = "linkVisibleEndDateUtc")]
        public DateTime? LinkVisibleEndDateUtc { get; set; }

    }
}
