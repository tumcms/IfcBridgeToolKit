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
    public class EuclidianPlacement
    {
        public Point3D Position { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        public EuclidianPlacement(Point3D position)
        {
            Position = position;
        }

        public EuclidianPlacement()
        {
        }
    }
}
