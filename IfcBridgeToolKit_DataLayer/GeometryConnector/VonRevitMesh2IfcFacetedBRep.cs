using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IfcBridgeToolKit_DataLayer
{
    public class VonRevitMesh2IfcFacetedBRep
    {
        //    private Placement Einfügepunkt; 
        public double PlacementX { get; set; }
        public double PlacementY { get; set; }
        public double PlacementZ { get; set; }

        public List<Point3D> MeshPunkte { get; set; }

    }

    public class Point3D
    {
        public double coordX { get; set; }
        public double coordY { get; set; }
        public double coordZ { get; set; }

        public Point3D(double coordX, double coordY, double coordZ)
        {
            this.coordX = coordX;
            this.coordY = coordY;
            this.coordZ = coordZ;
        }
    }

    //class Placement
    //{
       
    //}
}
