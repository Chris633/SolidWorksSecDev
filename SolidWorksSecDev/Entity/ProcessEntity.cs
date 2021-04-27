using System.Diagnostics;

namespace SolidWorksSecDev.Entity
{
    internal class ProcessEntity
    {
        public string name { get; set; }
        public string pid { get; set; }
        public string mt { get; set; }
        public string isBinded { get; set; }
        public Process process { get; set; }
    }
}