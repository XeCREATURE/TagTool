using System.IO;

namespace HaloOnlineTagTool.Common
{
    public struct Matrix4x3
    {
        public float m11 { get; }
        public float m12 { get; }
        public float m13 { get; }
        public float m21 { get; }
        public float m22 { get; }
        public float m23 { get; }
        public float m31 { get; }
        public float m32 { get; }
        public float m33 { get; }
        public float m41 { get; }
        public float m42 { get; }
        public float m43 { get; }

        public bool IsIdentity =>
            (m11 == 1.0f && m12 == 0.0f && m13 == 0.0f &&
             m21 == 0.0f && m22 == 1.0f && m23 == 0.0f &&
             m31 == 0.0f && m32 == 0.0f && m33 == 1.0f &&
             m41 == 0.0f && m42 == 0.0f && m43 == 0.0f);

        public static Matrix4x3 Identity =>
            new Matrix4x3(
                1.0f, 0.0f, 0.0f,
                0.0f, 1.0f, 0.0f,
                0.0f, 0.0f, 1.0f,
                0.0f, 0.0f, 0.0f);

        public bool Equals(Matrix4x3 M) =>
            (m11 == M.m11 &&
             m12 == M.m12 &&
             m13 == M.m13 &&
             m21 == M.m21 &&
             m22 == M.m22 &&
             m23 == M.m23 &&
             m31 == M.m31 &&
             m32 == M.m32 &&
             m33 == M.m33 &&
             m41 == M.m41 &&
             m42 == M.m42 &&
             m43 == M.m43);

        public Matrix4x3(
            float M11, float M12, float M13,
            float M21, float M22, float M23,
            float M31, float M32, float M33,
            float M41, float M42, float M43)
        {
            m11 = M11; m12 = M12; m13 = M13;
            m21 = M21; m22 = M22; m23 = M23;
            m31 = M31; m32 = M32; m33 = M33;
            m41 = M41; m42 = M42; m43 = M43;
        }

        public static Matrix4x3 operator *(Matrix4x3 matrix1, Matrix4x3 matrix2)
        {
            if (matrix1.IsIdentity) return matrix2;
            if (matrix2.IsIdentity) return matrix1;

            return new Matrix4x3(
                matrix1.m11 * matrix2.m11 + matrix1.m12 * matrix2.m21 +
                matrix1.m13 * matrix2.m31 + 0 * matrix2.m41,
                matrix1.m11 * matrix2.m12 + matrix1.m12 * matrix2.m22 +
                matrix1.m13 * matrix2.m32 + 0 * matrix2.m42,
                matrix1.m11 * matrix2.m13 + matrix1.m12 * matrix2.m23 +
                matrix1.m13 * matrix2.m33 + 0 * matrix2.m43,
                matrix1.m21 * matrix2.m11 + matrix1.m22 * matrix2.m21 +
                matrix1.m23 * matrix2.m31 + 0 * matrix2.m41,
                matrix1.m21 * matrix2.m12 + matrix1.m22 * matrix2.m22 +
                matrix1.m23 * matrix2.m32 + 0 * matrix2.m42,
                matrix1.m21 * matrix2.m13 + matrix1.m22 * matrix2.m23 +
                matrix1.m23 * matrix2.m33 + 0 * matrix2.m43,
                matrix1.m31 * matrix2.m11 + matrix1.m32 * matrix2.m21 +
                matrix1.m33 * matrix2.m31 + 0 * matrix2.m41,
                matrix1.m31 * matrix2.m12 + matrix1.m32 * matrix2.m22 +
                matrix1.m33 * matrix2.m32 + 0 * matrix2.m42,
                matrix1.m31 * matrix2.m13 + matrix1.m32 * matrix2.m23 +
                matrix1.m33 * matrix2.m33 + 0 * matrix2.m43,
                matrix1.m41 * matrix2.m11 + matrix1.m42 * matrix2.m21 +
                matrix1.m43 * matrix2.m31 + 1 * matrix2.m41,
                matrix1.m41 * matrix2.m12 + matrix1.m42 * matrix2.m22 +
                matrix1.m43 * matrix2.m32 + 1 * matrix2.m42,
                matrix1.m41 * matrix2.m13 + matrix1.m42 * matrix2.m23 +
                matrix1.m43 * matrix2.m33 + 1 * matrix2.m43);
        }

        public static Matrix4x3 Read(BinaryReader reader) =>
            new Matrix4x3(
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(),
                reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

        public static Matrix4x3 FromBounds(BoundingBox bb)
        {
            var bbXLength = bb.XBounds.Max - bb.XBounds.Min;
            var bbYLength = bb.YBounds.Max - bb.YBounds.Min;
            var bbZLength = bb.ZBounds.Max - bb.ZBounds.Min;

            return new Matrix4x3(
                bbXLength != 0.0f ? bbXLength : 1.0f, 0.0f, 0.0f,
                0.0f, bbYLength != 0.0f ? bbYLength : 1.0f, 0.0f,
                0.0f, 0.0f, bbZLength != 0.0f ? bbZLength : 1.0f,
                bbXLength, bbYLength, bbZLength);
        }

        public static Matrix4x3 FromBounds(Vector min, Vector max)
        {
            var bbXLength = max.X - min.X;
            var bbYLength = max.Y - min.Y;
            var bbZLength = max.Z - min.Z;

            return new Matrix4x3(
                bbXLength != 0.0f ? bbXLength : 1.0f, 0.0f, 0.0f,
                0.0f, bbYLength != 0.0f ? bbYLength : 1.0f, 0.0f,
                0.0f, 0.0f, bbZLength != 0.0f ? bbZLength : 1.0f,
                bbXLength, bbYLength, bbZLength);
        }
    }
}
