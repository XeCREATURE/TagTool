using System;
using System.Collections.Generic;
using HaloOnlineTagTool.Common;

namespace HaloOnlineTagTool.XboxCache.Definitions
{
    public abstract class scenario_structure_bsp
    {
        public CacheBase cache;
        public int geomRawID;
        public string BSPName;

        public Range<float> XBounds, YBounds, ZBounds;

        public List<Cluster> Clusters;
        public List<InstancedGeometry> GeomInstances;
        public List<render_model.Shader> Shaders;
        public List<render_model.ModelSection> ModelSections;
        public List<render_model.BoundingBox> BoundingBoxes;

        public int RawID1; //decorator vertex buffers
        public int RawID2; //???
        public int RawID3; //bsp raw

        public List<render_model.VertexBufferInfo> VertInfoList;
        public List<render_model.UnknownInfo1> Unknown1List;
        public List<render_model.IndexBufferInfo> IndexInfoList;
        public List<render_model.UnknownInfo2> Unknown2List;
        public List<render_model.UnknownInfo3> Unknown3List;

        public bool RawLoaded = false;

        public scenario_structure_bsp()
        {
            Clusters = new List<Cluster>();
            GeomInstances = new List<InstancedGeometry>();
            Shaders = new List<render_model.Shader>();
            ModelSections = new List<render_model.ModelSection>();
            BoundingBoxes = new List<render_model.BoundingBox>();

            VertInfoList = new List<render_model.VertexBufferInfo>();
            Unknown1List = new List<render_model.UnknownInfo1>();
            IndexInfoList = new List<render_model.IndexBufferInfo>();
            Unknown2List = new List<render_model.UnknownInfo2>();
            Unknown3List = new List<render_model.UnknownInfo3>();
        }

        public virtual void LoadRaw()
        {
            throw new NotImplementedException();
        }

        public abstract class Cluster
        {
            public Range<float> XBounds, YBounds, ZBounds;

            public int SectionIndex;
        }

        public abstract class InstancedGeometry
        {
            public float TransformScale;
            public Matrix4x3 TransformMatrix;
            public int SectionIndex;
            public string Name;

            public override string ToString()
            {
                return Name;
            }
        }
    }
}
