using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard.ThirdParty.Spotify
{
    public class SearchResponse
    {
        public TracksClass tracks { get; set; } = new TracksClass();

        public ArtistsClass artists { get; set; } = new ArtistsClass();

        public class TracksClass
        {
            public string href { get; set; }
            public List<RecentlyPlayedResponse.ItemsClass.TrackClass> items { get; set; } = new List<RecentlyPlayedResponse.ItemsClass.TrackClass>();
        }

        public class ArtistsClass
        {
            public string href { get; set; }
            public List<ItemsClass> items { get; set; } = new List<ItemsClass>();
            public int limit { get; set; }
            public int? next { get; set; }
            public int offset { get; set; }
            public int? previous { get; set; }
            public int total { get; set; }

            public class ItemsClass
            {
                public GetMeResponse.ExternalUrls external_urls { get; set; } = new GetMeResponse.ExternalUrls();
                public FollowersClass followers { get; set; } = new FollowersClass();
                public List<string> genres { get; set; } = new List<string>();
                public string href { get; set; }
                public string id { get; set; }
                public List<GetMeResponse.ImagesClass> images { get; set; } = new List<GetMeResponse.ImagesClass>();
                public string name { get; set; }
                public int popularity { get; set; }
                public string type { get; set; }
                public string uri { get; set; }

                public class FollowersClass
                {
                    public string href { get; set; }
                    public long total { get; set; }
                }
            }
        }
    }
}
