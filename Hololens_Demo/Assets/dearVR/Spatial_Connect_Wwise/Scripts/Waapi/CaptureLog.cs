using System;
using SpatialConnect.Wwise.Core;
using OVRSimpleJSON;

namespace SpatialConnect.Wwise
{
    public class CaptureLog : ICaptureLog
    {
        private IWaapi waapi_;

        public CaptureLog(IFactory factory = null)
        {
            factory = factory ?? new Factory();
            waapi_ = factory.CreateWaapi();
        }

        public void Subscribe()
        {
            var typesArray = WaapiUtility.ToJSONArray("Event", "Notification");

            var options = new JSONObject{ ["types"] = typesArray };

            waapi_.Subscribe("ak.wwise.core.profiler.captureLog.itemAdded", options.ToString(), OnLogItemAdded);
        }

        private void OnLogItemAdded(ulong subscriptionId, string contents)
        {
            var json = JSON.Parse(contents);

            // The gameObjectId has to be obtained on a different way than JSON.Parse(),
            // since OVRSimpleJSON does not support the required type UInt64. - Dominik
            var gameObjectIdString = WaapiUtility.GetValueFromJson("gameObjectId", contents);
            if(gameObjectIdString.Length == 0)
                return;

            var gameObjectId = Convert.ToUInt64(gameObjectIdString, System.Globalization.CultureInfo.InvariantCulture);

            if(json["type"] == "Event" && json["description"] == "Event Triggered")
            {
                EventTriggered?.Invoke(new EventPropertySet(json["objectId"], json["objectName"], (int)gameObjectId, json["gameObjectName"], (uint)json["playingId"]));
            }

            if(json["type"] == "Notification" && json["description"] == "Event Finished")
            {
                EventFinished?.Invoke((uint)json["playingId"]);
            }

        }

        public event Action<EventPropertySet> EventTriggered;
        public event Action<uint> EventFinished;
    }
}
