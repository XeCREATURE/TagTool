using System;
using HaloOnlineTagTool.Endian;
using HaloOnlineTagTool.Resources.Bitmaps;

namespace HaloOnlineTagTool.S3D
{
    public class Texture
    {
        public bool isLittleEndian;

        public int Width;
        public int Height;
        public BitmapType Type;
        public BitmapFormat Format;
        public int DataAddress;

        public int VirtualWidth
        {
            get
            {
                int var;
                switch (Format)
                {
                    case BitmapFormat.A8:
                    case BitmapFormat.Y8:
                    case BitmapFormat.AY8:
                    case BitmapFormat.A8Y8:
                    case BitmapFormat.A8R8G8B8:
                    case BitmapFormat.A4R4G4B4:
                    case BitmapFormat.R5G6B5:
                        var = 32;
                        break;

                    default:
                        var = 128;
                        break;
                }

                if (isLittleEndian) var = 4; //little endian used on mipmaps where blocksize is 4

                return (Width % var == 0) ? Width : Width + (var - (Width % var));
            }
        }
        public int VirtualHeight
        {
            get
            {
                int var;
                switch (Format)
                {
                    case BitmapFormat.A8:
                    case BitmapFormat.Y8:
                    case BitmapFormat.AY8:
                    case BitmapFormat.A8Y8:
                    case BitmapFormat.A8R8G8B8:
                    case BitmapFormat.A4R4G4B4:
                    case BitmapFormat.R5G6B5:
                        var = 32;
                        break;
                    //return Height;

                    default:
                        var = 128;
                        break;
                }

                if (isLittleEndian) var = 4; //little endian used on mipmaps where blocksize is 4

                return (Height % var == 0) ? Height : Height + (var - (Height % var));
            }
        }
        public int RawSize
        {
            get
            {
                int size = 0;
                switch (Format)
                {
                    case BitmapFormat.Ctx1:
                    case BitmapFormat.Dxt1:
                    case BitmapFormat.Dxt3aMono:
                    case BitmapFormat.Dxt3aAlpha:
                    case BitmapFormat.DXT5a:
                    case BitmapFormat.Dxt5aMono:
                    case BitmapFormat.Dxt5aApha:
                        size = VirtualWidth * VirtualHeight / 2;
                        break;
                    case BitmapFormat.A8:
                    case BitmapFormat.Y8:
                    case BitmapFormat.AY8:
                    case BitmapFormat.Dxt3:
                    case BitmapFormat.Dxt5:
                    case BitmapFormat.Dxn:
                    case BitmapFormat.DxnMonoAlpha:
                        size = VirtualWidth * VirtualHeight;
                        break;
                    case BitmapFormat.A4R4G4B4:
                    case BitmapFormat.A1R5G5B5:
                    case BitmapFormat.A8Y8:
                    case BitmapFormat.R5G6B5:
                        size = VirtualWidth * VirtualHeight * 2;
                        break;
                    case BitmapFormat.A8R8G8B8:
                    case BitmapFormat.X8R8G8B8:
                        size = VirtualWidth * VirtualHeight * 4;
                        break;
                    default:
                        return 0;
                }

                if (Type == BitmapType.CubeMap)
                    size *= 6;

                return size;
            }
        }

        public Texture(PakFile Pak, PakFile.PakTag Item)
        {
            var reader = Pak.Reader;
            reader.Format = EndianFormat.Little;
            reader.SeekTo(Item.Offset + 6);

            isLittleEndian = reader.ReadInt32() == 1346978644; //PICT
            if (!isLittleEndian) reader.Format = Endian.EndianFormat.Big;

            reader.SeekTo(Item.Offset + (isLittleEndian ? 16 : 12));
            Width = reader.ReadInt32();
            Height = reader.ReadInt32();

            reader.SeekTo(Item.Offset + (isLittleEndian ? 38 : 32));
            Format = BitmapFormat.Dxt5;
            var intFormat = reader.ReadInt32();
            switch (intFormat)
            {
                case 0:
                    Format = BitmapFormat.A8R8G8B8;
                    break;
                case 10:
                    Format = BitmapFormat.A8Y8;
                    break;
                case 12:
                    Format = BitmapFormat.Dxt1;
                    break;
                case 13:
                    Format = BitmapFormat.Dxt1;
                    break;
                case 15:
                    Format = BitmapFormat.Dxt3;
                    break;
                case 17:
                    Format = BitmapFormat.Dxt5;
                    break;
                case 22:
                    Format = BitmapFormat.X8R8G8B8;
                    break;
                case 36:
                    Format = BitmapFormat.Dxn;
                    break;
                case 37:
                    Format = BitmapFormat.DXT5a;
                    break;
                default:
                    throw new Exception("CHECK THIS");
            }

            reader.SeekTo(Item.Offset + (isLittleEndian ? 28 : 24));
            int mapCount = reader.ReadInt32();
            if (mapCount == 6) Type = BitmapType.CubeMap;
            else Type = BitmapType.Texture2D;
            
            if (mapCount > 1 && mapCount != 6)
                throw new Exception("CHECK THIS");

            DataAddress = Item.Offset + (isLittleEndian ? 58 : 4096);
            reader.Format = Endian.EndianFormat.Little; //in case it was PICT
        }
    }
}
