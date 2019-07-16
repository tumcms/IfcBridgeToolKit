using Xbim.Ifc;
using Xbim.Ifc4.GeometryResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.GeometricModelResource;
using Xbim.Ifc4.ProfileResource;
using System.Linq;
using Xbim.Common.Step21;

using Xbim.Ifc4.GeometricConstraintResource;

using Xbim.Ifc4.Kernel;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.ProductExtension;
using Xbim.Ifc4.RepresentationResource;
using Xbim.IO;
using IBTK_Basics;

namespace ElementCreator.ProfileTypes
{
   public class PointLists  
    {
        
    // Eckpunkte der "Deck"-Fläche erstellen  
        public IfcCartesianPointList2D PointlistDeck(ref IfcStore model)
        { 
        var PointList2D = model.Instances.New<IfcCartesianPointList2D>();

        var Point1 = model.Instances.New<IfcCartesianPoint>(p1 => p1.SetXY(-3.59925, 0.15));
        var Point2 = model.Instances.New<IfcCartesianPoint>(p2 => p2.SetXY(3.59925, 0.15));
        var Point3 = model.Instances.New<IfcCartesianPoint>(p3 => p3.SetXY(3.59925, -0.15));
        var Point4 = model.Instances.New<IfcCartesianPoint>(p4 => p4.SetXY(-3.59925, -0.15));


        PointList2D.CoordList.Add(Point1.Coordinates);
        PointList2D.CoordList.Add(Point2.Coordinates);
        PointList2D.CoordList.Add(Point3.Coordinates);
        PointList2D.CoordList.Add(Point4.Coordinates);
        PointList2D.CoordList.Add(Point1.Coordinates);

        return PointList2D;
        }
        // Eckpunkte der Trägerfläche erstellen 
        public IfcCartesianPointList2D PointListAASHTO_TypeVI (ref IfcStore model)
        { 
            
        var PointList2D = model.Instances.New<IfcCartesianPointList2D>();

        var Point1 = model.Instances.New<IfcCartesianPoint>(p1 => p1.SetXY(-0.5334, 0.9144));
        var Point2 = model.Instances.New<IfcCartesianPoint>(p2 => p2.SetXY(0.5334, 0.9144));
        var Point3 = model.Instances.New<IfcCartesianPoint>(p3 => p3.SetXY(0.5334, 0.8098));
        var Point4 = model.Instances.New<IfcCartesianPoint>(p4 => p4.SetXY(0.4174, 0.7844));
        var Point5 = model.Instances.New<IfcCartesianPoint>(p5 => p5.SetXY(0.254, -0.6604));
        var Point6 = model.Instances.New<IfcCartesianPoint>(p6 => p6.SetXY(0.3556, -0.7112));
        var Point7 = model.Instances.New<IfcCartesianPoint>(p7 => p7.SetXY(0.3556, -0.9144));
        var Point8 = model.Instances.New<IfcCartesianPoint>(p8 => p8.SetXY(-0.3556, -0.9144));
        var Point9 = model.Instances.New<IfcCartesianPoint>(p9 => p9.SetXY(-0.3556, -0.7112));
        var Point10 = model.Instances.New<IfcCartesianPoint>(p10 => p10.SetXY(-0.254, -0.6604));
        var Point11 = model.Instances.New<IfcCartesianPoint>(p11 => p11.SetXY(-0.4174, 0.7844));
        var Point12 = model.Instances.New<IfcCartesianPoint>(p12 => p12.SetXY(-0.5334, 0.8098));


        PointList2D.CoordList.Add(Point1.Coordinates);
        PointList2D.CoordList.Add(Point2.Coordinates);
        PointList2D.CoordList.Add(Point3.Coordinates);
        PointList2D.CoordList.Add(Point4.Coordinates);
        PointList2D.CoordList.Add(Point5.Coordinates);
        PointList2D.CoordList.Add(Point6.Coordinates);
        PointList2D.CoordList.Add(Point7.Coordinates);
        PointList2D.CoordList.Add(Point8.Coordinates);
        PointList2D.CoordList.Add(Point9.Coordinates);
        PointList2D.CoordList.Add(Point10.Coordinates);
        PointList2D.CoordList.Add(Point11.Coordinates);
        PointList2D.CoordList.Add(Point12.Coordinates);
        PointList2D.CoordList.Add(Point1.Coordinates);
            return PointList2D;
        }
        //Eckpunkte der linken Straßenseite erstellen
        public IfcCartesianPointList2D PointListLeftRoadpart(ref IfcStore model)
        {
            var PointList2D = model.Instances.New<IfcCartesianPointList2D>();

            var Point1 = model.Instances.New<IfcCartesianPoint>(p1 => p1.SetXY(1.5,0.145));
            var Point2 = model.Instances.New<IfcCartesianPoint>(p2 => p2.SetXY(-1.5,0.055));
            var Point3 = model.Instances.New<IfcCartesianPoint>(p3 => p3.SetXY(-1.5,-0.145));
            var Point4 = model.Instances.New<IfcCartesianPoint>(p4 => p4.SetXY(1.5, -0.055));


            PointList2D.CoordList.Add(Point1.Coordinates);
            PointList2D.CoordList.Add(Point2.Coordinates);
            PointList2D.CoordList.Add(Point3.Coordinates);
            PointList2D.CoordList.Add(Point4.Coordinates);
            PointList2D.CoordList.Add(Point1.Coordinates);

            return PointList2D;
        }

        //Eckpunkte der linken Straßenseite erstellen
        public IfcCartesianPointList2D PointListRightRoadpart(ref IfcStore model)
        {
            var PointList2D = model.Instances.New<IfcCartesianPointList2D>();

            var Point1 = model.Instances.New<IfcCartesianPoint>(p1 => p1.SetXY(-1.5, 0.145));
            var Point2 = model.Instances.New<IfcCartesianPoint>(p2 => p2.SetXY(1.5, 0.055));
            var Point3 = model.Instances.New<IfcCartesianPoint>(p3 => p3.SetXY(1.5, -0.145));
            var Point4 = model.Instances.New<IfcCartesianPoint>(p4 => p4.SetXY(-1.5, -0.055));


            PointList2D.CoordList.Add(Point1.Coordinates);
            PointList2D.CoordList.Add(Point2.Coordinates);
            PointList2D.CoordList.Add(Point3.Coordinates);
            PointList2D.CoordList.Add(Point4.Coordinates);
            PointList2D.CoordList.Add(Point1.Coordinates);

            return PointList2D;
        }

        //Eckpunkte der linken Straßenseite erstellen
        public IfcCartesianPointList2D PointListLeftSideline(ref IfcStore model)
        {
            var PointList2D = model.Instances.New<IfcCartesianPointList2D>();

            var Point1 = model.Instances.New<IfcCartesianPoint>(p1 => p1.SetXY(0.25, 0.0925));
            var Point2 = model.Instances.New<IfcCartesianPoint>(p2 => p2.SetXY(-0.25, 0.1075));
            var Point3 = model.Instances.New<IfcCartesianPoint>(p3 => p3.SetXY(-0.25, -0.0925));
            var Point4 = model.Instances.New<IfcCartesianPoint>(p4 => p4.SetXY(0.25, -0.1075));


            PointList2D.CoordList.Add(Point1.Coordinates);
            PointList2D.CoordList.Add(Point2.Coordinates);
            PointList2D.CoordList.Add(Point3.Coordinates);
            PointList2D.CoordList.Add(Point4.Coordinates);
            PointList2D.CoordList.Add(Point1.Coordinates);

            return PointList2D;
        }

        //Eckpunkte der rechten Straßenseite erstellen
        public IfcCartesianPointList2D PointListRightSideline(ref IfcStore model)
        {
            var PointList2D = model.Instances.New<IfcCartesianPointList2D>();

            var Point1 = model.Instances.New<IfcCartesianPoint>(p1 => p1.SetXY(-0.25,0.0925));
            var Point2 = model.Instances.New<IfcCartesianPoint>(p2 => p2.SetXY(0.25,0.1075));
            var Point3 = model.Instances.New<IfcCartesianPoint>(p3 => p3.SetXY(0.25,-0.0925));
            var Point4 = model.Instances.New<IfcCartesianPoint>(p4 => p4.SetXY(-0.25,-0.1075));


            PointList2D.CoordList.Add(Point1.Coordinates);
            PointList2D.CoordList.Add(Point2.Coordinates);
            PointList2D.CoordList.Add(Point3.Coordinates);
            PointList2D.CoordList.Add(Point4.Coordinates);
            PointList2D.CoordList.Add(Point1.Coordinates);

            return PointList2D;
        }

        //Eckpunkte des linken Straßenendes erstellen
        public IfcCartesianPointList2D PointListLeftEndline(ref IfcStore model)
        {
            var PointList2D = model.Instances.New<IfcCartesianPointList2D>();

            var Point1 = model.Instances.New<IfcCartesianPoint>(p1 => p1.SetXY(0.05,0.0985));
            var Point2 = model.Instances.New<IfcCartesianPoint>(p2 => p2.SetXY(-0.05,0.1015));
            var Point3 = model.Instances.New<IfcCartesianPoint>(p3 => p3.SetXY(-0.05,-0.0985));
            var Point4 = model.Instances.New<IfcCartesianPoint>(p4 => p4.SetXY(0.05,-0.1015));


            PointList2D.CoordList.Add(Point1.Coordinates);
            PointList2D.CoordList.Add(Point2.Coordinates);
            PointList2D.CoordList.Add(Point3.Coordinates);
            PointList2D.CoordList.Add(Point4.Coordinates);
            PointList2D.CoordList.Add(Point1.Coordinates);

            return PointList2D;
        }

        //Eckpunkte des rechten Straßenendes erstellen
        public IfcCartesianPointList2D PointListRightEndline(ref IfcStore model)
        {
            var PointList2D = model.Instances.New<IfcCartesianPointList2D>();

            var Point1 = model.Instances.New<IfcCartesianPoint>(p1 => p1.SetXY(0.05, 0.0985));
            var Point2 = model.Instances.New<IfcCartesianPoint>(p2 => p2.SetXY(-0.05, 0.1015));
            var Point3 = model.Instances.New<IfcCartesianPoint>(p3 => p3.SetXY(-0.05, -0.0985));
            var Point4 = model.Instances.New<IfcCartesianPoint>(p4 => p4.SetXY(0.05, -0.1015));


            PointList2D.CoordList.Add(Point1.Coordinates);
            PointList2D.CoordList.Add(Point2.Coordinates);
            PointList2D.CoordList.Add(Point3.Coordinates);
            PointList2D.CoordList.Add(Point4.Coordinates);
            PointList2D.CoordList.Add(Point1.Coordinates);

            return PointList2D;
        }

    }    
}
