using System;
using System.Collections.Generic;
using System.Linq;
using OVRSimpleJSON;
using SpatialConnect.Wwise.Core;

namespace SpatialConnect.Wwise
{
    public class AttenuationOption : IAttenuationOption
    {
        private readonly IWaapi waapi_;
        private readonly ulong subscriptionIdDistance_;
        
        public string Name { get; }
        public string Id { get; }
        private readonly IAttenuationCurveChangedMessageChannel attenuationCurveChangedMessageChannel_;

        public AttenuationType Type
        {
            get
            {
                bool IsCustomDefined()
                {
                    var idArrayParent = WaapiUtility.ToJSONArray(Id);
                    var selectArray = WaapiUtility.ToJSONArray("parent");
                    var transformArray = new JSONArray();
                    transformArray.Add(new JSONObject {["select"] = selectArray});
                    var argsWithTransform = new JSONObject
                        {["from"] = new JSONObject {["id"] = idArrayParent}, ["transform"] = transformArray};
                    var resultParent = waapi_.Call("ak.wwise.core.object.get", argsWithTransform.ToString(), "{}");

                    return !JSON.Parse(resultParent)["return"].Linq.Any();
                }

                if (IsNone(Id))
                    return AttenuationType.None;
                return IsCustomDefined() ? AttenuationType.Custom : AttenuationType.ShareSet;
            }
        }

        public List<Point> Points
        {
            get
            {
                var args = new JSONObject {["object"] = Id, ["curveType"] = "VolumeDryUsage"};
                var curveResult = waapi_.Call("ak.wwise.core.object.getAttenuationCurve", args.ToString(), "{}");

                if (curveResult == "") 
                    return new List<Point>();

                var pointJsonArray = (JSONArray)JSONNode.Parse(curveResult)["points"];
                var pointList = pointJsonArray.Linq.Select(point => {
                    var p = point.Value;
                    return new Point(p["x"], p["y"]);
                }).ToList();

                return pointList;
            }
            set
            {
                var pointsArray = new JSONArray();
                foreach (var point in value)
                    pointsArray.Add(new JSONObject {["x"] = point.x, ["y"] = point.y, ["shape"] = "Linear"});

                var args = new JSONObject
                {
                    ["object"] = Id,
                    ["curveType"] = "VolumeDryUsage",
                    ["use"] = "Custom",
                    ["points"] = pointsArray
                };

                waapi_.Call("ak.wwise.core.object.setAttenuationCurve", args.ToString(), "{}");
            }
        }

        public uint MaxDistance
        {
            get
            {
                var idArray = WaapiUtility.ToJSONArray(Id);
                var args = new JSONObject {["from"] = new JSONObject {["id"] = idArray}};
                var returnArray = WaapiUtility.ToJSONArray("@RadiusMax");
                var options = new JSONObject {["return"] = returnArray};
                var result = waapi_.Call("ak.wwise.core.object.get", args.ToString(), options.ToString());

                return (uint) JSON.Parse(result)["return"][0]["@RadiusMax"];
            }
            set
            {
                var args = new JSONObject
                {
                    ["property"] = "RadiusMax",
                    ["object"] = Id,
                    ["value"] = value
                };

                waapi_.Call("ak.wwise.core.object.setProperty", args.ToString(), "{}");
            }
        }

        public AttenuationOption(string id, string name, IAttenuationCurveChangedMessageChannel attenuationCurveChangedMessageChannel, IFactory factory = null)
        {
            factory = factory ?? new Factory();
            waapi_ = factory.CreateWaapi();
            Id = id;
            Name = name;
            attenuationCurveChangedMessageChannel_ = attenuationCurveChangedMessageChannel;

            if (Type != AttenuationType.ShareSet) 
                return;

            attenuationCurveChangedMessageChannel_.AttenuationCurveChanged += OnAttenuationCurveChanged;
            var argsDistance = new JSONObject { ["object"] = Id, ["property"] = "RadiusMax" };
            subscriptionIdDistance_ = waapi_.Subscribe("ak.wwise.core.object.propertyChanged", argsDistance.ToString(),
                OnMaxDistanceChanged);
        }
        
        private static bool IsNone(string id)
        {
            return id == "{00000000-0000-0000-0000-000000000000}";
        }
        
        private void OnAttenuationCurveChanged(string attenuationId)
        {
            if (attenuationId == Id)
                CurveChanged?.Invoke(Points);
        }
        
        private void OnMaxDistanceChanged(ulong subscriptionId, string contents)
        {
            var value = (uint)JSON.Parse(contents)["new"];
            MaxDistanceChanged?.Invoke(value);
        }

        public void Dispose()
        {
            if (subscriptionIdDistance_ == 0)
                return;
            
            attenuationCurveChangedMessageChannel_.AttenuationCurveChanged -= OnAttenuationCurveChanged;
            waapi_.Unsubscribe(subscriptionIdDistance_);
        }

        public event Action<List<Point>> CurveChanged;
        public event Action<uint> MaxDistanceChanged;
    }
}
