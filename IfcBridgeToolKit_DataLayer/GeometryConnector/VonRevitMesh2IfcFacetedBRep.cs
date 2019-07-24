using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.IfcRail.GeometryResource;

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

        public  Point3D(double coordX, double coordY, double coordZ)
        {
            this.coordX = coordX;
            this.coordY = coordY;
            this.coordZ = coordZ;
            
        }
    }
    public class VonRevitApitoIfc
    {
        public void GetList(List<(double x, double y, double z)> RevitApiPoints)
        {
            var ListforTransformation = new List<Point3D>();
            foreach (var RevitPoint in RevitApiPoints)
            {
                var X = RevitPoint.x;
                var Y = RevitPoint.y;
                var Z = RevitPoint.z;
                var Point = new Point3D(X, Y, Z);
                ListforTransformation.Add(Point);
            }
        }


        public void GetTransformationVector(List<Point3D> ListforTransformation, Point3D MinPoint, Point3D PointofOrigin)
        {
            var X = MinPoint.coordX - PointofOrigin.coordX;
            var Y = MinPoint.coordY - PointofOrigin.coordY;
            var Z = MinPoint.coordZ - PointofOrigin.coordZ;
            var VectorPoint = new Point3D(X, Y, Z);

        }



    }
    public class GetPlacementPoint
    {
        public  PointTransform PlacementPoint(List<object> ListforTransformation)
        {
            
            var Transform = ListforTransformation.Min();
            var X = Transform.coordX;
            var Y = Transform.coordY;
            var Z = Transform.coordZ;
            var MinPoint = new PointTransform(X, Y, Z);
            return MinPoint;

        }
      
    }

    public class PointTransform
    {
        public double coordX { get; set; }
        public double coordY { get; set; }
        public double coordZ { get; set; }

        public PointTransform(double coordX, double coordY, double coordZ)
        {
            this.coordX = coordX;
            this.coordY = coordY;
            this.coordZ = coordZ;

        }
    }

}
