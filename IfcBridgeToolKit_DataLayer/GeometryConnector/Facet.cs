using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfcBridgeToolKit_DataLayer.GeometryConnector
{
    /// <summary>
    /// 
    /// </summary>
    public class Facet
    {
        public List<Point3D> vertices { get; set; }

        /// <summary>
        /// Constructor of Facet class -> init List for vertices
        /// </summary>
        public Facet()
        {
            vertices = new List<Point3D>();
        }
    }
}
