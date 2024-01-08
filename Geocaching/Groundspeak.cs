using System;

using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using GeocachingAPI;
using GeocachingAPI.Models;
using Microsoft.Rest;

namespace WinCachebox.Geocaching
{

    public struct ImageLink
    {
        public Uri uri;
        public bool isContentBodyImage;
    }

    public struct CreatePocketQueryByCoordinateInfo
    {
        public string PQName;
        public int Radius;
        public string UnitType;
        public double LatDegs;
        public double LongDegs;
    }

    class Groundspeak

    {
        public String apiVersion;
        private APIv10 client;
        private static Groundspeak groundspeak;

        private Groundspeak()
        {
            client = new APIv10(new Microsoft.Rest.TokenCredentials(Config.GetAccessToken(), "bearer"));
            client.BaseUri = new Uri("https://api.groundspeak.com/");
            apiVersion = "1.0";
        }

        public static Groundspeak getInstance()
        {
            if (groundspeak == null)
                groundspeak = new Groundspeak();
            return groundspeak;

        }

        public APIv10 SetGeocachingAPIClient(String token)
        {
            client = new APIv10(new Microsoft.Rest.TokenCredentials(token, "bearer"));
            return client;
        }

        public APIv10 GetGeocachingAPIClient()
        {
            return client;
        }

        /*
         * usage:           
            Groundspeak gs = new Groundspeak();
            gs.getFriends();

         */
        public void getFriends()
        {
            Friends f = new Friends(client);
            IList<User> friends = f.GetFriends(apiVersion, 0, 10, "referenceCode,Username");            
        }

        public void DownloadSinglePocketQuery(GeocacheList pocketQuery)
        {
            // todo API1.0
            Lists l = new Lists(client);
            byte[] result = ListsExtensions.GetZippedPocketQuery(l, pocketQuery.ReferenceCode, apiVersion);
            // String s = "";
            // byte[] result = System.Convert.FromBase64String(s);

            string local = Config.GetString("PocketQueryFolder") + "\\" + pocketQuery.Name + "_" + pocketQuery.LastUpdatedDateUtc.Value.ToString("yyyyMMddHHmmss") + ".zip";

            FileStream fs;
            if (File.Exists(local))
                fs = new FileStream(local, FileMode.Truncate);
            else
                fs = new FileStream(local, FileMode.CreateNew | FileMode.CreateNew);

            fs.Write(result, 0, result.Length); ;
            fs.Close();
        }

        public static string DecodeFrom64(string encodedData)
        {

            byte[] encodedDataAsBytes

                = System.Convert.FromBase64String(encodedData);

            string returnValue =

               System.Text.Encoding.Default.GetString(encodedDataAsBytes);

            return returnValue;

        }

        public Dictionary<string, ImageLink> GetAllImageLinks(string GcCode, bool importLogImages)
        {
            Dictionary<string, ImageLink> retDict = new Dictionary<string, ImageLink>();
            Geocaches g = new Geocaches(client);
            long startTs = Environment.TickCount;
            do
            {
                try
                {
                    IList<Image> l = GeocachesExtensions.GetImages(g, GcCode, apiVersion, 0, 50, "url,description,Guid,referenceCode");
                    string SpoilersDescriptionTags = Config.GetString("SpoilersDescriptionTags");
                    string[] spoilersArr = SpoilersDescriptionTags.Split(';');
                    foreach (Image item in l)
                    {
                        bool isCacheImage = item.ReferenceCode.StartsWith("GC");

                        if (importLogImages || isCacheImage) // !item.Url.Contains("/cache/log/")
                        {
                            ImageLink imageLink = new ImageLink
                            {
                                uri = new Uri(item.Url),
                                isContentBodyImage = true
                            };

                            if (!isCacheImage) // item.Url.Contains("/cache/log/")
                            {
                                imageLink.isContentBodyImage = false;
                            }

                            int i = 1;
                            string storedImageName = "";
                            /*
                            if (storedImageName.Length == 0)
                                storedImageName = item.Description;
                                */
                            if (storedImageName.Length == 0)
                                storedImageName = item.Guid.ToString();

                            // handle same name images.
                            while (retDict.ContainsKey(storedImageName))
                            {
                                storedImageName = item.Guid + i.ToString();
                                i++;
                            }

                            if (SpoilersDescriptionTags != "")
                            {
                                for (int strNumber = 0; strNumber < spoilersArr.Length; strNumber++)
                                {
                                    if (HttpUtility.HtmlDecode(storedImageName.ToLower()).IndexOf(spoilersArr[strNumber].ToLower()) >= 0)
                                    {
                                        retDict.Add(storedImageName, imageLink);
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                retDict.Add(storedImageName, imageLink);
                            }
                        }
                    }
                    return retDict;
                }
                catch (Exception ex)
                {
                    String ignored = ex.ToString();
                    // todo API1.0
                    // handle limit retry (Limit of API Call for Images is ?? calls every minute)
                    /*
                    if (imagesResp.Status.StatusCode == 140)
                    {
                        //                       Global.AddLog("API Limit GetImagesForGeocache - Waiting 15 seconds");
                        Thread.Sleep(15000);
                        if (Environment.TickCount > startTs + 60000)
                        {
                            //                            Global.AddLog("Timeout Receiving CacheImages");
                            break;
                        }
                        // number of calls exceed Limit
                        // wait 5 seconds and repeat

                    }
                    else
                    {
                        break;
                    }
                    */
                }

            } while (true);
            // return retDict;
        }

        public int GetCachesFound()
        {
            int cachesFound = -1;

            cachesFound = UserInfo("findCount").FindCount.Value;
            return cachesFound;
        }

        public User UserInfo(String fields)
        {
            return new Users(client).GetUser("me", apiVersion, fields);
        }
    }
}
