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
                AddComponents.ConvertMyMeshToIfcFacetedBRep(ref model, "Testprodukt", CartesianPoints(ref model));
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
        private static List<IfcCartesianPoint> CartesianPoints(ref IfcStore model)
        {
            // Liste die von Funktion abgerufen werden soll 
            List<IfcCartesianPoint> cartesianPoints = new List<IfcCartesianPoint>();

            // Erstellt Variable, die VonRevitMesh2IfcFacetedBRep abruft 
            var aufbereiteteGeometrie = new DirectShapeToIfc();

            // Übergebe neuer Variabele die Test Koordinaten 
            var ptList = ifcCartesianPoints(ref model);

            // --- Geometrische Repräsentation ---
            foreach (var punkt in ptList)
            {
                // ToDo: Facetten bzw. PolyLoop Definition erstellen
            }

            // --- Placement ---
            aufbereiteteGeometrie.location.Position = new Point3D(0, 0, 0);


            return cartesianPoints;
        }
    }
}
