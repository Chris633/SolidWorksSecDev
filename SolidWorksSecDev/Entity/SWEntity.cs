using SolidWorks.Interop.sldworks;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolidWorksSecDev.Entity
{
    public interface SWEntity
    {
        public AssmblyEntity parent { get; set; }
        public int parentAmount { get; set; }
        public string parentId { get; set; }
        public string wid { get; set; }
        public string materialId { get; set; }
        public int amount { get; set; }
        public string ps { get; set; }
        public string size { get; set; }
        public string specification { get; set; }
        public string name { get; set; }
        public string weight { get; set; }
        public string bailment { get; set; }
        public string bomlv { get; set; }
        public string unit { get; set; }
        public string workshop { get; set; }
        public string storehouse { get; set; }

        public string propName { get; set; }

    }
}
