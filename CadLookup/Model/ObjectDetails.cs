using System;

namespace CADSnoop.Model
{
    /// <summary>
    /// Information Of Object 
    /// </summary>
    public class ObjectDetails
    {
        public string GroupName { get; set; }
        public string PropName { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public Object LinkObject { get; set; }
    }
}
