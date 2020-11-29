using System;
using System.Collections.Generic;
using System.Text;

namespace WebApiCore.Utils.Browser
{
    public class BaseBrowser
    {
        public string Name { get; set; }
        public string Maker { get; set; }
        public BrowserType Type { get; set; } = BrowserType.Generic;
        public Version Version { get; set; }

        public BaseBrowser() { }
        public BaseBrowser(BrowserType browserType) => Type = browserType;
        public BaseBrowser(BrowserType browserType, Version version) : this(browserType) => Version = version;

        public BaseBrowser(string name)
        {

        }
        public Version ToVersion(string version)
        {
            version = RemoveWhitespace(version);
            return Version.TryParse(version, out var parsedVersion) ? parsedVersion : new Version(0, 0);
        }
        public string RemoveWhitespace(string version) => version?.Replace(" ", "", StringComparison.OrdinalIgnoreCase);
    }

    public enum BrowserType
    {
        IE,
        Chrome,
        Safari,
        Firefox,
        Edge,
        Opera,
        Generic
    }

    public class BrowserHelper
    {
        public static string GetBrwoserInfo(string userAgent)
        {
            var ie = new InternetExplorer(userAgent);
            if (ie.Type == BrowserType.IE)
                return string.Format("{0} {1}", ie.Type.ToString(), ie.Version);
            var firefox = new Firefox(userAgent);
            if (firefox.Type == BrowserType.Firefox)
                return string.Format("{0} {1}", firefox.Type.ToString(), firefox.Version);
            var edge = new Edge(userAgent);
            if (edge.Type == BrowserType.Edge)
                return string.Format("{0} {1}", edge.Type.ToString(), edge.Version);
            var opera = new Opera(userAgent);
            if (opera.Type == BrowserType.Opera)
                return string.Format("{0} {1}", opera.Type.ToString(), opera.Version);
            var chrome = new Chrome(userAgent);
            if (chrome.Type == BrowserType.Chrome)
                return string.Format("{0} {1}", chrome.Type.ToString(), chrome.Version);
            var safari = new Safari(userAgent);
            if (safari.Type == BrowserType.Safari)
                return string.Format("{0} {1}", safari.Type.ToString(), safari.Version);
            return string.Empty;
        }
    }
}
