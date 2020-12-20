using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using StbSharp;

namespace LibAmiibo.Images
{
    class ExternalResourceManager
    {
        private Assembly Assembly { get; set; } = typeof(ExternalResourceManager).Assembly;
        private const string IMAGE_BASE = "LibAmiibo.";

        public static readonly ExternalResourceManager Instance = new ExternalResourceManager();

        public Image GetImage(string name)
        {
            var resFilestream = this.Assembly?.GetManifestResourceStream(IMAGE_BASE + name);
            if (resFilestream == null)
                return null;

            byte[] bytes = new byte[resFilestream.Length];
            resFilestream.Read(bytes, 0, bytes.Length);
            return StbImage.LoadFromMemory(bytes, StbImage.STBI_rgb_alpha);
        }

        public IEnumerable<string> GetNames()
        {
            return this.Assembly?.GetManifestResourceNames().Where(n => n.StartsWith(IMAGE_BASE)).Select(n => n.Substring(IMAGE_BASE.Length));
        }
    }
}
