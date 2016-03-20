using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

namespace App
{
    public static class HttpResponseExtensions
    {
        public enum ContentExtensions
        {
            css,
            js,
            png,
            jpg,
            gif
        }
        public static class MediaTypeNames
        {
            public const string Css = "text/css";
            public const string Js = "text/javascript";
            public const string Png = "image/png";
            public const string Jpeg = System.Net.Mime.MediaTypeNames.Image.Jpeg;
            public const string Gif = System.Net.Mime.MediaTypeNames.Image.Gif;
        }

        public static void SetContentTypeBasedOnExtension(this HttpResponseBase @this, string extension)
        {
            ContentExtensions ext;
            if (!Enum.TryParse(extension, out ext))
            {
                throw Exceptions.UnsupportedExtenion(extension);
            }

            switch (ext)
            {
                case ContentExtensions.css:
                    @this.ContentType = MediaTypeNames.Css;
                    break;
                case ContentExtensions.js:
                    @this.ContentType = MediaTypeNames.Js;
                    break;
                case ContentExtensions.png:
                    @this.ContentType = MediaTypeNames.Png;
                    break;
                case ContentExtensions.jpg:
                    @this.ContentType = MediaTypeNames.Jpeg;
                    break;
                case ContentExtensions.gif:
                    @this.ContentType = MediaTypeNames.Gif;
                    break;
            }
        }
    }
}
