using System;
using System.Collections.Generic;
using IfcBridgeToolKit;
using Xbim.IfcRail.RailwayDomain;
using System.Linq.Expressions;

namespace IfcBridge_DynPackage
{
    public class IfcBridgeExporter_Dyn
    {
        /// <summary>
        /// Defines base content in IFC Model -> calls initModel
        /// </summary>
        /// <param name="filepath"></param>
        public static bool InitIfcModel(string projectName)
        {
            try
            {
                var modelCreator = new CreateAndInitModel();    //ToDo: Correct Header -> IfcVersion 
                var model = modelCreator.CreateModel(projectName);
              
                
               // using (var txn = model.BeginTransaction("add a bridge item"))
               // {
               //     var bridge = model.Instances.New<IfcBridge>();
               //     bridge.Name = "IfcBridge001";
               //     bridge.Description = "I'm a fancy hello-world bridge";
               //     bridge.PredefinedType = IfcBridgeTypeEnum.GIRDER;

               //    txn.Commit();
               //}

                //var bridgeCreator = new InitSpatialStructure();
                //bridgeCreator.AddIfcBridge(ref model, name, description);

                //model.SaveAs(@"C:\Users\Korbi\OneDrive\Desktop\TestModel001.ifc"); // dont do it that way...
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return true; // everything went well - otherwise receive a false
        }

        // ToDo: weitere Funktionen definieren, die von Dynamo aus aufrufbar sein sollen

        /// <summary>
        /// Fügt einen Brückenträger zu einem bestehenden IfcModel hinzu
        /// </summary>
        /// <returns></returns>
        public static bool AddGirdersFromRevit(List<int> girders)
        {
            foreach (var girder in girders)
            {
                // Datenaufbereitung des internen Revit-Datentyps auf int, double, strings, ... ODER DataLayer (eigene Klasse!)

                //      var aufbereiteteGeometrie = new VonRevitMesh2IfcFacetedBRep();      // Klassendefinition ist in DataLayer.dll zu finden
                //      VonRevitMesh2IfcFacetedBRep.Schwerpunkt = x, y, z ( schon umgerechnet)
                //      VonRevitMesh2IfcFacetedBRep.MeshPunkte.Add(new Point3D(x,y,z));


                // rufe eine passende Funktion aus dem IfcBridgeToolkit auf (eigene dll, die die Bearbeitung des IFC Models vornimmt)
                // > IfcBridgeToolKit.addGirderToIfc(VonRevitMesh2IfcFacetedBRep); 

                // füge Relation zu entsprechender spatial Structure hinzu
            }
            return true; 
        }

    }
}
