using LibAmiibo.Data.Figurine;
using System.Collections.Generic;
using System.Linq;

namespace LibAmiibo.Images
{
    public class ImageManager
    {
        private const string ImagePrefix = "LibAmiibo.icon_";
        private const string ImagePostfix = ".png";

        private const string Empty = "LibAmiibo.Images.empty.png";

        public static readonly ImageManager Instance = new ImageManager();

        private readonly HashSet<Amiibo> knownAmiibos;
        public IEnumerable<Amiibo> KnownAmiibos => knownAmiibos;

        private ImageManager()
        {
            string[] names = GetType().Assembly.GetManifestResourceNames();
            knownAmiibos = new HashSet<Amiibo>(names.Where(x => x.StartsWith(ImagePrefix)).Select(x => Amiibo.FromStatueId(NameToStatue(x))));
        }

        private string NameToStatue(string name) => name.Substring(ImagePrefix.Length, 16);

        private string StatueToName(string statue) => string.Join("", ImagePrefix, statue.ToLower(), ImagePostfix);

        internal string GetImageResourceName(Amiibo statue)
        {
            if (statue.AmiiboNo == 0xFFFF || statue.StatueNameInternal == null)
                return Empty;

            if (knownAmiibos.Contains(statue))
                return StatueToName(statue.StatueId);

            foreach (var item in knownAmiibos)
            {
                if (item.AmiiboNo == statue.AmiiboNo)
                    return StatueToName(item.StatueId);
            }

            foreach (var item in knownAmiibos)
            {
                if (item.CharacterId == statue.CharacterId)
                    return StatueToName(item.StatueId);
            }

            return Empty;
        }
    }
}