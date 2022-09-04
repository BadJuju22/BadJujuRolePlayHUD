using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using UnityEngine;

namespace BadJujuRPHUD.Models
{
    public class Region
    {
        [XmlAttribute]
        public uint Id { get; set; }
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public float Center_X { get; set; }
        [XmlAttribute]
        public float Center_Y { get; set; }
        [XmlAttribute]
        public float Center_Z { get; set; }
        [XmlAttribute]
        public float Radius { get; set; }

        public Vector3 ToVector3() => new Vector3(Center_X, Center_Y, Center_Z);

    }
}
