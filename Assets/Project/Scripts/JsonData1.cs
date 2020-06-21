

[System.Serializable]
public class JsonData1
{
    public Image[] outline_images;
    public Meta meta;

    [System.Serializable]
     public class Meta
    {
        public object prev;
        public object next;
    }

    [System.Serializable] public class Image
    {
        public string id;
        public string title;
        public bool processed;
        public bool is_favorite;
        public int favorites_count;
        public string created_at;
        public SVG svg;
        public Stat stat;
        public Painter[] last_painters;
    }

    [System.Serializable] public class Painter
    {
        public int id;
        public string full_name;
        public string gender;
        public string userpic;
        public bool is_followed;
        public bool is_blocked;
    }

    [System.Serializable] public class Stat
    {
        public int colorings_count;
        public int comments_count;
        public int likes_count;
        public int sharings_count;
    }

    [System.Serializable] public class SVG
    {
        public IMG feed;
        public IMG original;
        public IMG inverted;
        public IMG th150;
        public IMG th300;
    }

    [System.Serializable] public class IMG
    {
        public int size;
        public int width = 0;
        public int height = 0;
        public bool KnownSize => width > 0 && height > 0;
        public string url;
        public string mime_type;
        public bool IsSVG => mime_type.ToLower( ).Contains( "svg" );
        public bool IsPNG => mime_type.ToLower( ).Contains( "png" );
    }
}


