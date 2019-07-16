using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc;
using Xbim.IO;
using Xbim.Common.Step21;
using Xbim.Ifc4;
using Xbim.Ifc4.Kernel;
using Xbim.Common;
using Xbim.Ifc4.SharedBldgElements;
using Xbim.Ifc4.PropertyResource;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.GeometryResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.ProductExtension;
using Xbim.Ifc4.RepresentationResource;
using Xbim.Ifc4.ActorResource;
using Xbim.Ifc4.DateTimeResource;
using Xbim.Ifc4.ExternalReferenceResource;
using Xbim.Ifc4.PresentationOrganizationResource;
using Xbim.Ifc4.GeometricModelResource;
using Xbim.Ifc4.ProfileResource;
using IBTK_Basics;
using IfcAlignmentCreator;
using IfcAlignmentCreator.HorizontalSegments;
using IfcAlignmentCreator.VerticalSegments;
using ElementRepresentation;
using ElementRepresentation.RepresentationMethode;
using ElementCreator.ProfileTypes;
using ElementCreator.FaceBasedSurfaceTypes;

namespace App_IBTK_Example
{
    internal class App_IfcBridge
    {
        private static void Main(string[] args)
        {
            //Inizialisiere bestehenden Baukasten 
            var ModelCreator = new CreateAndInitModel();
            var alignmentDesigner = new InitIfcAlignment("myCurve", "myDescription");            
            var SSH = new SectionSolidHorizontal();
            var CFS = new ConnectedFaceSet();
            var FBSM = new FaceBasedSurfaceModel();
            var PDS = new ProductShapeRepresentation();
            var PL = new PointLists();            
            var ACPD = new ArbitrayClosedProfileDef();
            //
            //
            //

            //Grundeigenschaften der Ifc-Datei werden erstellt 
            var model = ModelCreator.CreateModel("IfcBridgeTest_01");
            ModelCreator.CreateRequiredInstances(ref model, "project Site");
           
            using (var txn = model.BeginTransaction("add an IfcAlignment"))
            {

                // Grundwerte des Alignments  
                alignmentDesigner.CreateAlignmentBaseData(ref model, out var ifcAlignment);
                var ifcAlignmentCurve = ifcAlignment.Axis as IfcAlignmentCurve;
                //Horizontaler Anteil des Alignments 
                HorizontalSegmentFactory factory_H = new LineSegmentFactory(0, 455, 301.162,0);
                var line1 = factory_H.CreateSegment(ref model);
                ifcAlignmentCurve.Horizontal.Segments.Add(line1);
                // Vertikaler Anteil des Alignments
                VerticalSegmentFactory factory_V = new VerSeqLineFactory(0, 301.162, 455, 0);
                var Verseq1 = factory_V.CreateSegment(ref model);
                ifcAlignmentCurve.Vertical.Segments.Add(Verseq1);

               
                                          

                //SpartialStructure erweitern erstellen
                var ifcRelContainedInSpartialStructure = model.Instances.New<IfcRelContainedInSpatialStructure>();
                
                var ifcSite = model.Instances.OfType<IfcSite>().First();
                if (ifcSite == null)
                {
                    // error handle
                }

                ifcRelContainedInSpartialStructure.RelatingStructure = ifcSite;
                //Füge ifcAlignment der SpartialStructure hinzu 
                ifcRelContainedInSpartialStructure.RelatedElements.Add(ifcAlignment);



                // TestBeam 
                var StartDisExp = DistExp.CreateDistanceExpression(ref model, 0.4, -2.611940299, -1.2649);
                var EndDisExp = DistExp.CreateDistanceExpression(ref model, 61.19658962, -2.611940299, -1.2649);

                var a = PL.PointListAASHTO_TypeVI(ref model);
                ACPD.CreateArbitraryClosedProfileDef(ref model,a, out var test);
           
                SSH.CreateSectionedSolidHorizontal(ref model, ifcAlignmentCurve, test,test, StartDisExp, EndDisExp,true,out var Beamshape);
                var Beam = ShapeRepresentation.CreateIfcShapeRepresentation(ref model, "Body", "AdvancedSweptSolid", Beamshape);
                PDS.CreateIfcProductDefinitionShape(ref model, "AAHTO", "TypeVI", Beam,out var Element01);
                
                var Beam01 = model.Instances.New<IfcBeam>();
                Beam01.Name = "Girder A2";
                Beam01.Description = "Test";
                
                Beam01.Representation = Element01;
                
                var LocalPlacment = model.Instances.New<IfcLocalPlacement>();
                var Point = model.Instances.New<IfcCartesianPoint>(p => p.SetXYZ(0, 0, 0));
                //var Place = model.Instances.New<IfcAxis2Placement3D>();
                //Place.Location = Point;
                //var Placement = model.Instances.New<IfcLocalPlacement>();
                var Direction1 = model.Instances.New<IfcDirection>();
                Direction1.SetXYZ(0, 0, 1);
                var Direction2 = model.Instances.New<IfcDirection>();
                Direction2.SetXYZ(1, 0, 0);
                //Placement.RelativePlacement = Place;
                var Axis2Palcement3D = model.Instances.New<IfcAxis2Placement3D>();
                Axis2Palcement3D.Location = Point;
                Axis2Palcement3D.Axis = Direction1;
                Axis2Palcement3D.RefDirection = Direction2;
                LocalPlacment.RelativePlacement = Axis2Palcement3D;
                Beam01.ObjectPlacement = LocalPlacment;
                ifcRelContainedInSpartialStructure.RelatedElements.Add(Beam01);

                //

                //
                var cfs = CFS.FaceBasedSurfaceModelBearing(ref model);
                var assd = ShapeRepresentation.CreateIfcShapeRepresentation(ref model, "Test", "Test", cfs);
                PDS.CreateIfcProductDefinitionShape(ref model, "Test", "Test", assd, out var Element0101);

                var BearingPlacement_A1 = model.Instances.New<IfcLocalPlacement>();
                var BearingA2P3_A1 = model.Instances.New<IfcAxis2Placement3D>();
                var PointBearingA1 = model.Instances.New<IfcCartesianPoint>();

                PointBearingA1.SetXYZ(0.65, 452.3880597, 452.7307);
                BearingA2P3_A1.Location = PointBearingA1;
                BearingA2P3_A1.Axis = Direction1;
                BearingA2P3_A1.RefDirection = Direction2;
                BearingPlacement_A1.RelativePlacement = BearingA2P3_A1;

                var ifcBearingA1 = model.Instances.New<IfcBuildingElementProxy>();
                ifcBearingA1.Name = "BearingA1";
                ifcBearingA1.ObjectType = "Bearing";
                ifcBearingA1.ObjectPlacement = BearingPlacement_A1;
                ifcBearingA1.Representation = Element0101;
                ifcBearingA1.PredefinedType = IfcBuildingElementProxyTypeEnum.ELEMENT;
                ifcRelContainedInSpartialStructure.RelatedElements.Add(ifcBearingA1);
                //
                txn.Commit();
            }


                model.SaveAs("IfcBridgeToolKitExample_01", StorageType.Ifc, null);
        }
    }
}
