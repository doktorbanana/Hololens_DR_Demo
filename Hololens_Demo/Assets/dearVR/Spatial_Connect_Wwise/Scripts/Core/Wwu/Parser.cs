using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace SpatialConnect.Wwise.Core.Wwu
{
    public enum WwuFileLoadState
    {
        Success,
        FileLoadError,
        XmlParseError,
        NoMixingSessionError
    }
    
    public class ParseResult
    {
        public WwuFileLoadState WwuFileLoadState { get; }
        public MixingSessionPropertySet[] MixingSessionPropertySet { get; }

        public ParseResult(WwuFileLoadState wwuFileLoadState, MixingSessionPropertySet[] mixingSessionPropertySet)
        {
            WwuFileLoadState = wwuFileLoadState;
            MixingSessionPropertySet = mixingSessionPropertySet;
        }
    }
    
    public interface IParser 
    {
        ParseResult Parse();
    }

    public class Parser : IParser
    {
        
        private readonly ILoader loader_;
        private readonly string path_;
    
        public Parser(string path, ILoader loader)
        {
            loader_ = loader;
            path_ =  path;
        }

        public ParseResult Parse()
        {
            try
            {
                var xmlString = loader_.Load(path_);
                var xml = XDocument.Parse(xmlString);
                var mixingSessionPropertySets =  xml.Descendants("MixingSession")
                    .Select(mixingSessionElement =>
                    {
                        var mixingSessionId = mixingSessionElement.Attribute("ID")?.Value;
                        var mixingSessionName = mixingSessionElement.Attribute("Name")?.Value;
                        var objectRefs = mixingSessionElement.Descendants("ObjectRef")
                            .Select(objectRef => objectRef.Attribute("ID")?.Value).ToArray();
                        return new MixingSessionPropertySet(mixingSessionId, mixingSessionName, objectRefs);
                    }).ToArray();

                return mixingSessionPropertySets.Length == 0 ? 
                    new ParseResult(WwuFileLoadState.NoMixingSessionError, null) :
                    new ParseResult(WwuFileLoadState.Success, mixingSessionPropertySets);
            }
            catch (Exception exception)
            {
                switch (exception)
                {
                    case FileLoadException _:
                        return new ParseResult(WwuFileLoadState.FileLoadError, null);
                    case XmlException _:
                        return new ParseResult(WwuFileLoadState.XmlParseError, null);
                }            
            }

            throw new InvalidDataException("unexpected XML data");
        }
    }
}
