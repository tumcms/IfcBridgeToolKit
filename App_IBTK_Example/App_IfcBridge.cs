using System.Linq;
using Xbim.IO;
using Xbim.IfcRail.SharedBldgElements;
using Xbim.IfcRail.GeometryResource;
using Xbim.IfcRail.ProductExtension;
using IfcBridgeToolKit;
using IfcBridgeToolKit_DataLayer;
using Xbim.IfcRail.GeometricConstraintResource;
using System.Collections.Generic;
using IfcBridgeToolKit_DataLayer.GeometryConnector;
using Xbim.Ifc;
using Xbim.Common;

namespace App_IBTK_Example
{
    internal class App_IfcBridge
    {
        private static void Main(string[] args)
        {
            //Erstellt Variabele, die CreateAndInitModel abruft 
            var ModelCreator = new CreateAndInitModel();
            //Fügt Grundstrucktur der Ifc-Datei hinzu 
            var model = ModelCreator.CreateModel("IfcBridgeTest_01", "Aicher", "Korbinian");
            ModelCreator.CreateRequiredInstances(ref model, "project Site");


            using (var txn = model.BeginTransaction("add an IfcAlignment"))
            {
                AddComponents.ConvertMyMeshToIfcFacetedBRep(ref model, "Testprodukt", ifcCartesianPoints(ref model));
                txn.Commit();

                

            }

            model.SaveAs("IfcBridgeToolKitExample_03.ifc");
        }


        /// <summary>
        /// Testkoordinaten 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static List<IfcCartesianPoint> ifcCartesianPoints(ref IfcStore model)
        {
            List<IfcCartesianPoint> ifcCartesianPoints = new List<IfcCartesianPoint>();
            var Point1 = model.Instances.New<IfcCartesianPoint>(p1 => p1.SetXYZ(0, 0, 0));
            ifcCartesianPoints.Add(Point1);
            ifcCartesianPoints.Add(model.Instances.New<IfcCartesianPoint>(p2 => p2.SetXYZ(1, 0, 0)));
            ifcCartesianPoints.Add(model.Instances.New<IfcCartesianPoint>(p3 => p3.SetXYZ(1, 1, 0)));
            ifcCartesianPoints.Add(model.Instances.New<IfcCartesianPoint>(p4 => p4.SetXYZ(0, 1, 0)));
            ifcCartesianPoints.Add(model.Instances.New<IfcCartesianPoint>(p5 => p5.SetXYZ(0, 1, 1)));
            ifcCartesianPoints.Add(model.Instances.New<IfcCartesianPoint>(p6 => p6.SetXYZ(0, 0, 1)));
            ifcCartesianPoints.Add(model.Instances.New<IfcCartesianPoint>(p7 => p7.SetXYZ(1, 0, 1)));
            ifcCartesianPoints.Add(model.Instances.New<IfcCartesianPoint>(p8 => p8.SetXYZ(1, 1, 1)));
            ifcCartesianPoints.Add(model.Instances.New<IfcCartesianPoint>(p1 => p1.SetXYZ(0, 0, 0)));
            return ifcCartesianPoints;
        }


        /// <summary>
        /// Testtransformation, die an einigen Stellen noch Fehlerhast ist.
        /// Zeile 91: System.NullReferenceExeption --> Der Objectverweis wurde nicht auf eine Objektinstanz festgelegt  
        /// Minimum ist definitiv flasch.. da jedes mal  ein neuer erzeugt wird
        /// Todo: foreach schleife für die Korrekte plazierung aus den schleifen nehmen und im nachhinein abändern --> Neue Funktion in DataLayer eventuell sinnvoller 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        //private static List<IfcCartesianPoint> CartesianPoints(ref IfcStore model)
        //{
        //    // Liste die von Funktion abgerufen werden soll 
        //    List<IfcCartesianPoint> cartesianPoints = new List<IfcCartesianPoint>();
        //    // Erstellt Variable, die VonRevitMesh2IfcFacetedBRep abruft 
        //    var aufbereiteteGeometrie = new VonRevitMesh2IfcFacetedBRep();
        //    // Übergebe neuer Variabele die Test Koordinaten 
        //    var Punktliste = ifcCartesianPoints(ref model);

        //    // Loop übergibt Input-Geometrie 
        //        foreach (var punkt in Punktliste)
        //        {
        //        (aufbereiteteGeometrie.PlacementX)  = punkt.X;
        //         var _X = aufbereiteteGeometrie.PlacementX;
        //         var _Y = aufbereiteteGeometrie.PlacementY;
        //         var _Z = aufbereiteteGeometrie.PlacementZ;
        //         _X = punkt.X;
        //         _Y = punkt.Y;
        //         _Z = punkt.Z;

        //            var Point = new Point3D(_X, _Y, _Z);
                
        //            aufbereiteteGeometrie.MeshPunkte.Add(Point);
        //            var meshPunkte = aufbereiteteGeometrie.MeshPunkte;
        //            var Minimum = meshPunkte.Min();
        //        // Loop transformiert geometrie im Bezug auf einen Referenzpunkt 
        //            foreach (var meshPunkt in meshPunkte)
        //            {
        //                var n_X = aufbereiteteGeometrie.PlacementX;
        //                var n_Y = aufbereiteteGeometrie.PlacementY;
        //                var n_Z = aufbereiteteGeometrie.PlacementZ;
        //                n_X = Point.coordX - Minimum.coordX;
        //                n_Y = Point.coordY - Minimum.coordY;
        //                n_Z = Point.coordZ - Minimum.coordZ;
        //                var transformedPoint = new Point3D(n_X, n_Y, n_Z);

        //                aufbereiteteGeometrie.MeshPunkte.Add(Point);
        //                var transformendPoints = aufbereiteteGeometrie.MeshPunkte;

                    
                
        //        // Loop transformiert doubles in verwendbare IfcPunkte 
        //                foreach (var tPoints in transformendPoints)
        //                {
        //                    var ifcPoint = model.Instances.New<IfcCartesianPoint>();
        //                    ifcPoint.X = tPoints.coordX;
        //                    ifcPoint.Y = tPoints.coordY;
        //                    ifcPoint.Z = tPoints.coordZ;
        //                    cartesianPoints.Add(ifcPoint);


        //                }
        //            }

                   
        //        }
        //    return cartesianPoints;
        //}
        //public class VonRevitApitoIfc
        //{
        //    public void GetList(List<(double x,double y, double z)> RevitApiPoints)
        //    {
        //        var ListforTransformation = new List<Point3D>();
        //        foreach (var RevitPoint in RevitApiPoints)
        //        {
        //          var X = RevitPoint.x;
        //          var Y = RevitPoint.y;
        //          var Z = RevitPoint.z;
        //          var Point  = new Point3D(X,Y,Z);
        //          ListforTransformation.Add(Point);  
        //        }               
        //    }


        //    public void GetPlacementPoint(List<Point3D> ListforTransformation)
        //    {
        //        var MinPoint = ListforTransformation.Min();                                
        //    }

        //    public GetTransformationVector(List<Point3D> ListforTransformation, Point3D MinPoint, Point3D PointofOrigin)
        //    {
        //        var X = MinPoint.coordX - PointofOrigin.coordX;
        //        var Y = MinPoint.coordY - PointofOrigin.coordY;
        //        var Z = MinPoint.coordZ - PointofOrigin.coordZ;
        //        var VectorPoint = new Point3D(X, Y, Z);
                
        //    }



        //}

   }
    
}

