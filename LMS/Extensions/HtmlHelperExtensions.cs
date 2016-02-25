using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace System.Web.Mvc
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Used to create plain hyperlinks.
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="url">A url to an external source</param>
        /// <param name="linkText">Text to be displayed for the hyperlink</param>
        /// <returns></returns>
        public static MvcHtmlString Hyperlink(this HtmlHelper helper, string url, string linkText)
        {
            return MvcHtmlString.Create(String.Format(@"<a href='{0}'>{1}</a>", url, linkText));
        }
    }
}