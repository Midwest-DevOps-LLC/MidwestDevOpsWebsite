using System;
using System.Collections.Generic;
using System.Text;

namespace MDO.RESTDataEntities.Standard.ThirdParty.Spotify
{
    public class CurrentlyPlayingResponse
    { 
        public long timestamp { get; set; }
        public RecentlyPlayedResponse.ItemsClass.ContextClass context { get; set; } = new RecentlyPlayedResponse.ItemsClass.ContextClass();
        public long progress_ms { get; set; }
        public RecentlyPlayedResponse.ItemsClass.TrackClass item { get; set; } = new RecentlyPlayedResponse.ItemsClass.TrackClass();
        public string currently_playing_type { get; set; }
        public ActionsClass actions { get; set; } = new ActionsClass();
        public bool is_playing { get; set; }

        public class ActionsClass
        {
            public DisallowsClass disallows { get; set; } = new DisallowsClass();

            public class DisallowsClass
            {
                public bool resuming { get; set; }
            }
        }
    }
}
