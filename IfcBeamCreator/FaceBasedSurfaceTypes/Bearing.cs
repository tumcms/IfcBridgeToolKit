using Xbim.Ifc;
using Xbim.IfcRail.GeometryResource;
using Xbim.IfcRail.TopologyResource;

namespace ElementCreator.FaceBasedSurfaceTypes
{
    public class ConnectedFaceSet
    {
        public IfcConnectedFaceSet FaceBasedSurfaceModelBearing(ref IfcStore model)
        { 
            

        var Point1 = model.Instances.New<IfcCartesianPoint>(p1 => p1.SetXYZ(-0.25, 0.25, 0.09));
        var Point2 = model.Instances.New<IfcCartesianPoint>(p2 => p2.SetXYZ(0.25, 0.25, 0.09));
        var Point3 = model.Instances.New<IfcCartesianPoint>(p3 => p3.SetXYZ(0.25, -0.25, 0.09));
        var Point4 = model.Instances.New<IfcCartesianPoint>(p4 => p4.SetXYZ(-0.25, -0.25, 0.09));
        var Point5 = model.Instances.New<IfcCartesianPoint>(p5 => p5.SetXYZ(-0.25, 0.25, 0.05));
        var Point6 = model.Instances.New<IfcCartesianPoint>(p6 => p6.SetXYZ(0.25, 0.25, 0.05));
        var Point7 = model.Instances.New<IfcCartesianPoint>(p7 => p7.SetXYZ(0.25, -0.25, 0.05));
        var Point8 = model.Instances.New<IfcCartesianPoint>(p8 => p8.SetXYZ(-0.25, -0.25, 0.05));        
        var Point9 = model.Instances.New<IfcCartesianPoint>(p9 => p9.SetXYZ(-0.2, 0.2, 0.05));        
        var Point10 = model.Instances.New<IfcCartesianPoint>(p10 => p10.SetXYZ(0.2, 0.2, 0.05));        
        var Point11 = model.Instances.New<IfcCartesianPoint>(p11 => p11.SetXYZ(0.2, -0.2, 0.05));        
        var Point12 = model.Instances.New<IfcCartesianPoint>(p12 => p12.SetXYZ(-0.2, -0.2, 0.05));
        var Point13 = model.Instances.New<IfcCartesianPoint>(p13 => p13.SetXYZ(-0.2, 0.2, -0.05));        
        var Point14 = model.Instances.New<IfcCartesianPoint>(p14 => p14.SetXYZ(0.2, 0.2, -0.05));        
        var Point15 = model.Instances.New<IfcCartesianPoint>(p15 => p15.SetXYZ(0.2, -0.2, -0.05));        
        var Point16 = model.Instances.New<IfcCartesianPoint>(p16 => p16.SetXYZ(-0.2, -0.2, -0.05));        
        var Point17 = model.Instances.New<IfcCartesianPoint>(p17 => p17.SetXYZ(-0.25, 0.25, -0.05));        
        var Point18 = model.Instances.New<IfcCartesianPoint>(p18 => p18.SetXYZ(0.25, 0.25, -0.05));        
        var Point19 = model.Instances.New<IfcCartesianPoint>(p19 => p19.SetXYZ(0.25, -0.25, -0.05));        
        var Point20 = model.Instances.New<IfcCartesianPoint>(p20 => p20.SetXYZ(-0.25, -0.25, -0.05));        
        var Point21 = model.Instances.New<IfcCartesianPoint>(p21 => p21.SetXYZ(-0.25, 0.25, -0.09));        
        var Point22 = model.Instances.New<IfcCartesianPoint>(p22 => p22.SetXYZ(0.25, 0.25, -0.09));        
        var Point23 = model.Instances.New<IfcCartesianPoint>(p23 => p23.SetXYZ(0.25, -0.25, -0.09));        
        var Point24 = model.Instances.New<IfcCartesianPoint>(p24 => p24.SetXYZ(-0.25, -0.25, -0.09));
        

                 
                    var PolyloopA1 = model.Instances.New<IfcPolyLoop>();
                    PolyloopA1.Polygon.Add(Point1);
                    PolyloopA1.Polygon.Add(Point2);
                    PolyloopA1.Polygon.Add(Point3);
                    PolyloopA1.Polygon.Add(Point4);

                    var PolyloopA2 = model.Instances.New<IfcPolyLoop>();
                    PolyloopA2.Polygon.Add(Point1);
                    PolyloopA2.Polygon.Add(Point4);
                    PolyloopA2.Polygon.Add(Point8);
                    PolyloopA2.Polygon.Add(Point5);

                    var PolyloopA3 = model.Instances.New<IfcPolyLoop>();
                    PolyloopA3.Polygon.Add(Point1);
                    PolyloopA3.Polygon.Add(Point2);
                    PolyloopA3.Polygon.Add(Point6);
                    PolyloopA3.Polygon.Add(Point5);

                    var PolyloopA4 = model.Instances.New<IfcPolyLoop>();
                    PolyloopA4.Polygon.Add(Point2);
                    PolyloopA4.Polygon.Add(Point3);
                    PolyloopA4.Polygon.Add(Point7);
                    PolyloopA4.Polygon.Add(Point6);

                    var PolyloopA5 = model.Instances.New<IfcPolyLoop>();
                    PolyloopA5.Polygon.Add(Point3);
                    PolyloopA5.Polygon.Add(Point4);
                    PolyloopA5.Polygon.Add(Point8);
                    PolyloopA5.Polygon.Add(Point7);

                    var PolyloopA6 = model.Instances.New<IfcPolyLoop>();
                    PolyloopA6.Polygon.Add(Point5);
                    PolyloopA6.Polygon.Add(Point9);
                    PolyloopA6.Polygon.Add(Point12);
                    PolyloopA6.Polygon.Add(Point8);

                    var PolyloopA7 = model.Instances.New<IfcPolyLoop>();
                    PolyloopA7.Polygon.Add(Point5);
                    PolyloopA7.Polygon.Add(Point6);
                    PolyloopA7.Polygon.Add(Point10);
                    PolyloopA7.Polygon.Add(Point9);

                    var PolyloopA8 = model.Instances.New<IfcPolyLoop>();
                    PolyloopA8.Polygon.Add(Point10);
                    PolyloopA8.Polygon.Add(Point6);
                    PolyloopA8.Polygon.Add(Point7);
                    PolyloopA8.Polygon.Add(Point11);

                    var PolyloopA9 = model.Instances.New<IfcPolyLoop>();
                    PolyloopA9.Polygon.Add(Point11);
                    PolyloopA9.Polygon.Add(Point7);
                    PolyloopA9.Polygon.Add(Point8);
                    PolyloopA9.Polygon.Add(Point12);

                    var PolyloopB1 = model.Instances.New<IfcPolyLoop>();
                    PolyloopB1.Polygon.Add(Point9);
                    PolyloopB1.Polygon.Add(Point10);
                    PolyloopB1.Polygon.Add(Point14);
                    PolyloopB1.Polygon.Add(Point13);

                    var PolyloopB2 = model.Instances.New<IfcPolyLoop>();
                    PolyloopB2.Polygon.Add(Point10);
                    PolyloopB2.Polygon.Add(Point14);
                    PolyloopB2.Polygon.Add(Point15);
                    PolyloopB2.Polygon.Add(Point11);

                    var PolyloopB3 = model.Instances.New<IfcPolyLoop>();
                    PolyloopB3.Polygon.Add(Point11);
                    PolyloopB3.Polygon.Add(Point15);
                    PolyloopB3.Polygon.Add(Point16);
                    PolyloopB3.Polygon.Add(Point12);

                    var PolyloopB4 = model.Instances.New<IfcPolyLoop>();
                    PolyloopB4.Polygon.Add(Point12);
                    PolyloopB4.Polygon.Add(Point16);
                    PolyloopB4.Polygon.Add(Point13);
                    PolyloopB4.Polygon.Add(Point9);

                    var PolyloopC1 = model.Instances.New<IfcPolyLoop>();
                    PolyloopC1.Polygon.Add(Point17);
                    PolyloopC1.Polygon.Add(Point18);
                    PolyloopC1.Polygon.Add(Point14);
                    PolyloopC1.Polygon.Add(Point13);

                    var PolyloopC2 = model.Instances.New<IfcPolyLoop>();
                    PolyloopC2.Polygon.Add(Point18);
                    PolyloopC2.Polygon.Add(Point14);
                    PolyloopC2.Polygon.Add(Point15);
                    PolyloopC2.Polygon.Add(Point19);

                    var PolyloopC3 = model.Instances.New<IfcPolyLoop>();
                    PolyloopC3.Polygon.Add(Point19);
                    PolyloopC3.Polygon.Add(Point15);
                    PolyloopC3.Polygon.Add(Point16);
                    PolyloopC3.Polygon.Add(Point20);

                    var PolyloopC4 = model.Instances.New<IfcPolyLoop>();
                    PolyloopC4.Polygon.Add(Point20);
                    PolyloopC4.Polygon.Add(Point16);
                    PolyloopC4.Polygon.Add(Point13);
                    PolyloopC4.Polygon.Add(Point17);

                    var PolyloopC5 = model.Instances.New<IfcPolyLoop>();
                    PolyloopC5.Polygon.Add(Point19);
                    PolyloopC5.Polygon.Add(Point18);
                    PolyloopC5.Polygon.Add(Point22);
                    PolyloopC5.Polygon.Add(Point23);

                    var PolyloopC6 = model.Instances.New<IfcPolyLoop>();
                    PolyloopC6.Polygon.Add(Point20);
                    PolyloopC6.Polygon.Add(Point19);
                    PolyloopC6.Polygon.Add(Point23);
                    PolyloopC6.Polygon.Add(Point24);

                    var PolyloopC7 = model.Instances.New<IfcPolyLoop>();
                    PolyloopC7.Polygon.Add(Point20);
                    PolyloopC7.Polygon.Add(Point24);
                    PolyloopC7.Polygon.Add(Point21);
                    PolyloopC7.Polygon.Add(Point17);

                    var PolyloopC8 = model.Instances.New<IfcPolyLoop>();
                    PolyloopC8.Polygon.Add(Point17);
                    PolyloopC8.Polygon.Add(Point21);
                    PolyloopC8.Polygon.Add(Point22);
                    PolyloopC8.Polygon.Add(Point18);

                    var PolyloopC9 = model.Instances.New<IfcPolyLoop>();
                    PolyloopC9.Polygon.Add(Point21);
                    PolyloopC9.Polygon.Add(Point22);
                    PolyloopC9.Polygon.Add(Point23);
                    PolyloopC9.Polygon.Add(Point24);

                    var FaceOuterBound1 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound1.Bound = PolyloopA1;
                    FaceOuterBound1.Orientation = true;

                    var FaceOuterBound2 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound2.Bound = PolyloopA2;
                    FaceOuterBound2.Orientation = true;

                    var FaceOuterBound3 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound3.Bound = PolyloopA3;
                    FaceOuterBound3.Orientation = true;

                    var FaceOuterBound4 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound4.Bound = PolyloopA4;
                    FaceOuterBound4.Orientation = true;

                    var FaceOuterBound5 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound5.Bound = PolyloopA5;
                    FaceOuterBound5.Orientation = true;

                    var FaceOuterBound6 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound6.Bound = PolyloopA6;
                    FaceOuterBound6.Orientation = true;

                    var FaceOuterBound7 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound7.Bound = PolyloopA7;
                    FaceOuterBound7.Orientation = true;

                    var FaceOuterBound8 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound8.Bound = PolyloopA8;
                    FaceOuterBound8.Orientation = true;

                    var FaceOuterBound9 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound9.Bound = PolyloopA9;
                    FaceOuterBound9.Orientation = true;

                    var FaceOuterBound10 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound10.Bound = PolyloopB1;
                    FaceOuterBound10.Orientation = true;

                    var FaceOuterBound11 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound11.Bound = PolyloopB2;
                    FaceOuterBound11.Orientation = true;

                    var FaceOuterBound12 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound12.Bound = PolyloopB3;
                    FaceOuterBound12.Orientation = true;

                    var FaceOuterBound13 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound13.Bound = PolyloopB4;
                    FaceOuterBound13.Orientation = true;

                    var FaceOuterBound14 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound14.Bound = PolyloopC1;
                    FaceOuterBound14.Orientation = true;

                    var FaceOuterBound15 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound15.Bound = PolyloopC2;
                    FaceOuterBound15.Orientation = true;
    
                    var FaceOuterBound16 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound16.Bound = PolyloopC3;
                    FaceOuterBound16.Orientation = true;

                    var FaceOuterBound17 = model.Instances.New<IfcFaceOuterBound>();
                   FaceOuterBound17.Bound = PolyloopC4;
                    FaceOuterBound17.Orientation = true;

                    var FaceOuterBound18 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound18.Bound = PolyloopC5;
                    FaceOuterBound18.Orientation = true;

                    var FaceOuterBound19 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound19.Bound = PolyloopC6;
                    FaceOuterBound19.Orientation = true;

                    var FaceOuterBound20 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound20.Bound = PolyloopC7;
                    FaceOuterBound20.Orientation = true;

                    var FaceOuterBound21 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound21.Bound = PolyloopC8;
                    FaceOuterBound21.Orientation = true;

                    var FaceOuterBound22 = model.Instances.New<IfcFaceOuterBound>();
                    FaceOuterBound22.Bound = PolyloopC9;
                    FaceOuterBound22.Orientation = true;
                   

        var ifcFace1 = model.Instances.New<IfcFace>();
        ifcFace1.Bounds.Add(FaceOuterBound1);

        var ifcFace2 = model.Instances.New<IfcFace>();
        ifcFace2.Bounds.Add(FaceOuterBound2);

        var ifcFace3 = model.Instances.New<IfcFace>();
        ifcFace3.Bounds.Add(FaceOuterBound3);

        var ifcFace4 = model.Instances.New<IfcFace>();
        ifcFace4.Bounds.Add(FaceOuterBound4);

        var ifcFace5 = model.Instances.New<IfcFace>();
        ifcFace5.Bounds.Add(FaceOuterBound5);

        var ifcFace6 = model.Instances.New<IfcFace>();
        ifcFace6.Bounds.Add(FaceOuterBound6);

        var ifcFace7 = model.Instances.New<IfcFace>();
        ifcFace7.Bounds.Add(FaceOuterBound7);

        var ifcFace8 = model.Instances.New<IfcFace>();
        ifcFace8.Bounds.Add(FaceOuterBound8);

        var ifcFace9 = model.Instances.New<IfcFace>();
        ifcFace9.Bounds.Add(FaceOuterBound9);

        var ifcFace10 = model.Instances.New<IfcFace>();
        ifcFace10.Bounds.Add(FaceOuterBound10);

        var ifcFace11 = model.Instances.New<IfcFace>();
        ifcFace11.Bounds.Add(FaceOuterBound11);

        var ifcFace12 = model.Instances.New<IfcFace>();
        ifcFace12.Bounds.Add(FaceOuterBound12);

        var ifcFace13 = model.Instances.New<IfcFace>();
        ifcFace13.Bounds.Add(FaceOuterBound13);

        var ifcFace14 = model.Instances.New<IfcFace>();
        ifcFace14.Bounds.Add(FaceOuterBound14);

        var ifcFace15 = model.Instances.New<IfcFace>();
        ifcFace15.Bounds.Add(FaceOuterBound15);

        var ifcFace16 = model.Instances.New<IfcFace>();
        ifcFace16.Bounds.Add(FaceOuterBound16);

        var ifcFace17 = model.Instances.New<IfcFace>();
        ifcFace17.Bounds.Add(FaceOuterBound17);

        var ifcFace18 = model.Instances.New<IfcFace>();
        ifcFace18.Bounds.Add(FaceOuterBound18);

        var ifcFace19 = model.Instances.New<IfcFace>();
        ifcFace19.Bounds.Add(FaceOuterBound19);

        var ifcFace20 = model.Instances.New<IfcFace>();
        ifcFace20.Bounds.Add(FaceOuterBound20);

        var ifcFace21 = model.Instances.New<IfcFace>();
        ifcFace21.Bounds.Add(FaceOuterBound21);

        var ifcFace22 = model.Instances.New<IfcFace>();
        ifcFace22.Bounds.Add(FaceOuterBound22);

        var ifcConnectedFaceSetBearing = model.Instances.New<IfcConnectedFaceSet>();
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace1);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace2);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace3);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace4);             
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace5);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace6);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace7);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace9);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace10);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace11);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace12);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace13);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace14);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace15);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace16);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace17);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace18);                
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace19);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace21);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace20);
        ifcConnectedFaceSetBearing.CfsFaces.Add(ifcFace22);

                   
        
        return ifcConnectedFaceSetBearing;
        }
    }
}
