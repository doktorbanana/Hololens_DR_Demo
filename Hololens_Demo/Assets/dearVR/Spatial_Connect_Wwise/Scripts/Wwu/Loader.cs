using System;
using System.IO;
using SpatialConnect.Wwise.Core.Wwu;

namespace SpatialConnect.Wwise.Wwu
{
    public class Loader : ILoader
    {
        public string Load(string path)
        {
            try
            {
                var xmlString = File.ReadAllText(path);
                return xmlString;
            }
            catch (Exception e)
            {
                throw new FileLoadException(e.Message, e);
            }
        }
    }
}
