using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HaloOnlineTagTool.Common
{
    /// <summary>
    /// Multipurpose quaternion, can be used as point or vector from 1 to 4 dimensions, or just as a collection of 1 to 4 real values.
    /// </summary>
    [Serializable]
    public class Vector
    {
        private float[] Values { get; }

        public int Dimensions =>
            Values.Length;

        public float A
        {
            get { return Values[0]; }
            set { Values[0] = value; }
        }

        public float B
        {
            get { return Values[1]; }
            set { Values[1] = value; }
        }

        public float C
        {
            get { return Values[2]; }
            set { Values[2] = value; }
        }

        public float D
        {
            get { return Values[3]; }
            set { Values[3] = value; }
        }

        public float X
        {
            get { return A; }
            set { A = value; }
        }

        public float Y
        {
            get { return B; }
            set { B = value; }
        }

        public float Z
        {
            get { return C; }
            set { C = value; }
        }

        public float W
        {
            get { return D; }
            set { D = value; }
        }

        public float I
        {
            get { return A; }
            set { A = value; }
        }

        public float J
        {
            get { return B; }
            set { B = value; }
        }

        public float K
        {
            get { return C; }
            set { C = value; }
        }

        public Vector()
        {
            Values = new float[4];
        }

        public Vector(float A)
        {
            Values = new float[1];
            this.A = A;
        }

        public Vector(float A, float B)
        {
            Values = new float[2];
            this.A = A;
            this.B = B;
        }

        public Vector(float A, float B, float C)
        {
            Values = new float[3];
            this.A = A;
            this.B = B;
            this.C = C;
        }

        public Vector(float A, float B, float C, float D)
        {
            Values = new float[4];
            this.A = A;
            this.B = B;
            this.C = C;
            this.D = D;
        }

        public float Length =>
            (float)Math.Sqrt(
                Math.Pow(A, 2) +
                Math.Pow(B, 2) +
                Math.Pow(C, 2) +
                Math.Pow(D, 2));

        public override string ToString()
        {
            string s = A.ToString("F6") + ", " + B.ToString("F6");
            if (Values.Length > 2) s += ", " + C.ToString("F6");
            if (Values.Length > 3) s += ", " + D.ToString("F6");
            return s;
        }

        public string ToString(int Dimensions, string Separator)
        {
            if (Dimensions < 1 || Dimensions > 4) throw new ArgumentOutOfRangeException("Dimensions", "Dimensions must be between 1 and 4 (inclusive).");
            string val = A.ToString("F6");

            if (Dimensions > 1) val += Separator + B.ToString("F6");
            if (Dimensions > 2) val += Separator + C.ToString("F6");
            if (Dimensions > 3) val += Separator + D.ToString("F6");

            return val;
        }

        public void Point3DTransform(Matrix4x3 Transform)
        {
            if (Transform.IsIdentity) return;

            float newX = (X * Transform.m11 + Y * Transform.m21 + Z * Transform.m31 + Transform.m41);
            float newY = (X * Transform.m12 + Y * Transform.m22 + Z * Transform.m32 + Transform.m42);
            float newZ = (X * Transform.m13 + Y * Transform.m23 + Z * Transform.m33 + Transform.m43);

            X = newX;
            Y = newY;
            Z = newZ;
        }

        public void Vector3DTransform(Matrix4x3 Transform)
        {
            if (Transform.IsIdentity) return;

            float newX = X * Transform.m11 + Y * Transform.m21 + Z * Transform.m31;
            float newY = X * Transform.m12 + Y * Transform.m22 + Z * Transform.m32;
            float newZ = X * Transform.m13 + Y * Transform.m23 + Z * Transform.m33;

            X = newX;
            Y = newY;
            Z = newZ;
        }
        
        public static Vector operator *(Vector q, float scalar)
        {
            if (q.Dimensions <= 2) return new Vector(q.A * scalar, q.B * scalar);
            else if (q.Dimensions == 3) return new Vector(q.A * scalar, q.B * scalar, q.C * scalar);
            else return new Vector(q.A * scalar, q.B * scalar, q.C * scalar, q.D * scalar);
        }

        public static Vector operator *(float scalar, Vector q) =>
            (q * scalar);

        public static Vector FromUDHenN3(uint UDHenN3) =>
            new Vector(
                (float)(UDHenN3 & 0x3FF) / (float)0x3FF,
                (float)((UDHenN3 >> 10) & 0x7FF) / (float)0x7FF,
                (float)((UDHenN3 >> 21) & 0x7FF) / (float)0x7FF);

        public static Vector FromUHenDN3(uint UHenDN3) =>
            new Vector(
                (float)(UHenDN3 & 0x7FF) / (float)0x7FF,
                (float)((UHenDN3 >> 11) & 0x7FF) / (float)0x7FF,
                (float)((UHenDN3 >> 22) & 0x3FF) / (float)0x3FF);

        public static Vector FromUDecN4(uint UDecN4) =>
            new Vector(
                (float)(UDecN4 & 0x3FF) / (float)0x3FF,
                (float)((UDecN4 >> 10) & 0x3FF) / (float)0x3FF,
                (float)((UDecN4 >> 20) & 0x3FF) / (float)0x3FF,
                (float)(UDecN4 >> 30) / (float)0x003);

        public static Vector FromUByte4(uint UByte4) =>
            new Vector(
                (float)(UByte4 >> 24),
                (float)((UByte4 >> 16) & 0xFF),
                (float)((UByte4 >> 8) & 0xFF),
                (float)(UByte4 & 0xFF));

        public static Vector FromUByteN4(uint UByteN4) =>
            new Vector(
                (float)(UByteN4 >> 24) / (float)0xFF,
                (float)((UByteN4 >> 16) & 0xFF) / (float)0xFF,
                (float)((UByteN4 >> 8) & 0xFF) / (float)0xFF,
                (float)(UByteN4 & 0xFF) / (float)0xFF);

        public static Vector FromDHenN3(uint DHenN3)
        {
            float a, b, c;
            uint[] SignExtendX = { 0x00000000, 0xFFFFFC00 };
            uint[] SignExtendYZ = { 0x00000000, 0xFFFFF800 };
            uint temp;

            temp = DHenN3 & 0x3FF;
            a = (float)(short)(temp | SignExtendX[temp >> 9]) / (float)0x1FF;

            temp = (DHenN3 >> 10) & 0x7FF;
            b = (float)(short)(temp | SignExtendYZ[temp >> 10]) / (float)0x3FF;

            temp = (DHenN3 >> 21) & 0x7FF;
            c = (float)(short)(temp | SignExtendYZ[temp >> 10]) / (float)0x3FF;

            return new Vector(a, b, c);
        }

        public static Vector FromHenDN3(uint HenDN3)
        {
            float a, b, c;
            uint[] SignExtendXY = { 0x00000000, 0xFFFFF800 };
            uint[] SignExtendZ = { 0x00000000, 0xFFFFFC00 };
            uint temp;

            temp = HenDN3 & 0x7FF;
            a = (float)(short)(temp | SignExtendXY[temp >> 10]) / (float)0x3FF;

            temp = (HenDN3 >> 11) & 0x7FF;
            b = (float)(short)(temp | SignExtendXY[temp >> 10]) / (float)0x3FF;

            temp = (HenDN3 >> 22) & 0x3FF;
            c = (float)(short)(temp | SignExtendZ[temp >> 9]) / (float)0x1FF;

            return new Vector(a, b, c);
        }

        public static Vector FromDecN4(uint DecN4)
        {
            uint[] SignExtend = { 0x00000000, 0xFFFFFC00 };
            uint[] SignExtendW = { 0x00000000, 0xFFFFFFFC };
            uint temp;

            temp = DecN4 & 0x3FF;
            var a = (float)(short)(temp | SignExtend[temp >> 9]) / 511.0f;

            temp = (DecN4 >> 10) & 0x3FF;
            var b = (float)(short)(temp | SignExtend[temp >> 9]) / 511.0f;

            temp = (DecN4 >> 20) & 0x3FF;
            var c = (float)(short)(temp | SignExtend[temp >> 9]) / 511.0f;

            temp = DecN4 >> 30;
            var d = (float)(short)(temp | SignExtendW[temp >> 1]);

            return new Vector(a, b, c, d);
        }

    }
}
