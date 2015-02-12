﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace IsoPixel
{
    public class DepthSprite : DepthBitmap
    {
        public IList<SpritePosition> sprites = new List<SpritePosition>();
        public string name;

        private DepthBitmap cache;

        public void ClearCache()
        {
            cache = null;
        }

        public DepthSprite()
        {

        }

        public DepthSprite(int width, int height)
            : base(width, height)
        {
            Clear(0, 0, 0, 0, 0);
        }

        private void update(IDictionary<string, DepthSprite> container)
        {
            if (cache == null)
            {
                cache = new DepthBitmap(width, height);
                cache.Draw(this, 0, 0, 0);

                foreach (var item in sprites)
                {
                    container[item.id].update(container);
                    cache.Draw(container[item.id].cache, item.x, item.y, item.z);
                }
            }
        }

        public void Clear(int r, int g, int b, int a, int z)
        {
            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i].Set(r, g, b, a, z);
            }
        }

        public void Clear(DepthPixel p)
        {
            Clear(p.r, p.g, p.b, p.a, p.z);
        }

        public void DrawTo(FastBitmap bitmap, IDictionary<string, DepthSprite> container)
        {
            update(container);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    bitmap.SetPixel(x, y, cache.PixelAt(x, y).GetColor());
                }
            }
        }

        public void From(FastBitmap bitmap)
        {
            Clear(0, 0, 0, 0, 0);
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Color c = bitmap.GetPixel(x, y);
                    PixelAt(x, y).Set(c.R, c.G, c.B, c.A, 0);
                }
            }
        }
    }
}