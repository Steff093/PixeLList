using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixeLList.Helpers
{
    internal static class ResourceExtensions
    {
        private static readonly ResourceLoader _resourceLoader = new();

        public static string GetLocalized(this string resourceKey)
        {
            return _resourceLoader.GetString(resourceKey);
        }
    }
}
