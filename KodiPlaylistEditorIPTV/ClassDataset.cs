namespace PlaylistEditor
{
    public class ClassDataset
    {
        public enum Modified { Yes, No, Reset }

        public string[] testArray = new[]{  "tvg-name", "tvg-id", "tvg-title", "tvg-logo", "tvg-chno", "tvg-shift",
                "group-title", "radio", "catchup", "catchup-source", "catchup-days", "catchup-correction",
                "provider", "provider-type", "provider-logo", "provider-countries", "provider-languages",
                "media", "media-dir", "media-size" };


        public class colArray
        {
            private static string[] _coltypes;
            public string[] ColTypes
            {
                get
                {
                    return _coltypes = new[] {
                    "tvg-name", "tvg-id", "tvg-title", "tvg-logo", "tvg-chno", "tvg-shift",
                    "group-title", "radio", "catchup", "catchup-source", "catchup-days", "catchup-correction",
                    "provider", "provider-type", "provider-logo", "provider-countries", "provider-languages",
                    "media", "media-dir", "media-size"};

                }
            }

        }


    }
}
