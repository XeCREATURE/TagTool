using TagTool.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TagTool.Sounds
{
    /// <summary>
    /// Provides methods for identifying Wwise sound formats.
    /// </summary>
    public static class SoundFormatDetection
    {
        /// <summary>
        /// Identifies the format of a Wwise sound based upon RIFX header information.
        /// </summary>
        /// <param name="rifx">The RIFX header information for the sound.</param>
        /// <returns>A SoundFormat value indicating the type of the sound, or SoundFormat.Unknown if identification failed.</returns>
        public static SoundFormat DetectFormat(RIFX rifx)
        {
            if (rifx.FormatMagic == (int)RIFFFormat.XWMA)
            {
                if (rifx.Codec == (short)RIFXCodec.WMA || rifx.Codec == (short)RIFXCodec.WMAPro)
                    return SoundFormat.XWMA;
            }
            else if (rifx.FormatMagic == (int)RIFFFormat.WAVE)
            {
                switch (rifx.Codec)
                {
                    case (short)RIFXCodec.WwiseOGG:
                        return SoundFormat.WwiseOGG;
                    case (short)RIFXCodec.XMA:
                        return SoundFormat.XMA;
                }
            }

            return SoundFormat.Unknown;
        }
    }
}
