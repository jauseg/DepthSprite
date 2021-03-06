﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace IsoPixel
{
    public class DepthContainer : Dictionary<string, DepthSprite>
    {
        public static DepthContainer Parse(string json)
        {
            var ser = new JavaScriptSerializer();
            ser.MaxJsonLength = Int32.MaxValue;
            var list = ser.Deserialize<Dictionary<string, DepthSprite>>(json);

            var container = new DepthContainer(list);

            foreach (var sprite in list)
            {
                sprite.Value.SetContainerAndId(container, sprite.Key);
            }
            return container;
        }

        public DepthContainer() : base() { }

        public DepthContainer(Dictionary<string, DepthSprite> list) : base(list) { }

        public string ToJsonSring()
        {
            return JsonStringFormatter.FormatOutput(new JavaScriptSerializer().Serialize(this));
        }

        public bool CanAddSpriteToSprite(string addId, string recvId)
        {
            if (addId.Equals(recvId))
            {
                return false;
            }

            foreach (var subSprite in this[addId].subSprites)
            {
                if (!CanAddSpriteToSprite(subSprite.id, recvId))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
