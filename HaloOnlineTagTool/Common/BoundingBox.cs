using System;

namespace HaloOnlineTagTool.Common
{
    public struct BoundingBox
    {
        public Range<float> XBounds, YBounds, ZBounds;

        public float Length =>
            (float)Math.Sqrt(
                Math.Pow(XBounds.Max - XBounds.Min, 2) +
                Math.Pow(YBounds.Max - YBounds.Min, 2) +
                Math.Pow(ZBounds.Max - ZBounds.Min, 2));
    }
}
