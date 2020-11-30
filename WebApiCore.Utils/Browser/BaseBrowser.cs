using System;

namespace WebApiCore.Utils.Browser
{
    internal class BaseBrowser
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
}
