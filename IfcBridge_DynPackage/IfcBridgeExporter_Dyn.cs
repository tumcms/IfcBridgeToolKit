using System;
using IfcBridgeToolKit;
using Xbim.Ifc;
using Xbim.IfcRail.RailwayDomain;
using Xbim.IO;

namespace IfcBridge_DynPackage
{
    public class IfcBridgeExporter_Dyn
    {
        /// <summary>
        /// Define Credits for IfcModel interactions
        /// </summary>
        /// <param name="FamilyName">Family Name of the Editor</param>
        /// <param name="FirstName">First Name of the Editor</param>
        /// <param name="organization">Organization</param>
        /// <returns>credentials in an XBIM-based DataType</returns>
        public static XbimEditorCredentials Credentials(string FamilyName, string FirstName, string organization)
        {
            var credentials = new XbimEditorCredentials
            {
                ApplicationDevelopersName = "IfcBridgeToolKit",
                ApplicationFullName = "TUM_CMS_IfcBridgeToolkit",
                ApplicationVersion = "1.0",
                EditorsFamilyName = FamilyName,
                EditorsGivenName = FirstName,
                EditorsOrganisationName = organization
            };

            return credentials;
        }

        /// <summary>
        ///     Defines base content in IFC Model -> calls initModel
        /// </summary>
        /// <param name="projectName">Project Name inside the IfcModel</param>
        /// <param name="credentials">Editor Credentials - use node</param>
        /// <param name="directory">use 'directory path' node from Dynamo</param>
        /// <param name="FileName">simple string in the form of *.ifc</param>
        public static string InitIfcModel(string projectName, XbimEditorCredentials credentials, string directory, string FileName)
        {
            // build storage path of the model
            var storeFilePath = directory + "/" + FileName;

            try
            {
                var modelCreator = new CreateAndInitModel(); //ToDo: Correct Header -> IfcVersion 
                var model = modelCreator.CreateModel(projectName, credentials);

                //var bridgeCreator = new InitSpatialStructure();
                //bridgeCreator.AddIfcBridge(ref model, name, description);
                
                model.SaveAs(storeFilePath); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return storeFilePath; // return directory to Ifc Model
        }
        
        /// <summary>
        /// Add girders to IfcModel
        /// </summary>
        /// <param name="storeFilePath">Path to the already existing IfcModel</param>
        /// <param name="credentials">Editor credits</param>
        /// <returns></returns>
        public static string AddGirdersFromRevit(string storeFilePath, XbimEditorCredentials credentials)
        {
            // open Ifc model
            using (var model = IfcStore.Open(storeFilePath, credentials, null, null, XbimDBAccess.ReadWrite))
            {
                // do fancy transactions
                using (var txn = model.BeginTransaction("add a bridge item"))
                {
                    var bridge = model.Instances.New<IfcBridge>();
                    bridge.Name = "IfcBridge001";
                    bridge.Description = "I'm a fancy hello-world bridge";
                    bridge.PredefinedType = IfcBridgeTypeEnum.GIRDER;

                    // commit changes
                    txn.Commit();
                }

                // save model
                model.SaveAs(storeFilePath);
            }


            //foreach (var girder in girders)
            // {
            // Datenaufbereitung des internen Revit-Datentyps auf int, double, strings, ... ODER DataLayer (eigene Klasse!)

            //      var aufbereiteteGeometrie = new VonRevitMesh2IfcFacetedBRep();      // Klassendefinition ist in DataLayer.dll zu finden
            //      VonRevitMesh2IfcFacetedBRep.Schwerpunkt = x, y, z ( schon umgerechnet)
            //      VonRevitMesh2IfcFacetedBRep.MeshPunkte.Add(new Point3D(x,y,z));


            // rufe eine passende Funktion aus dem IfcBridgeToolkit auf (eigene dll, die die Bearbeitung des IFC Models vornimmt)
            // > IfcBridgeToolKit.addGirderToIfc(VonRevitMesh2IfcFacetedBRep); 

            // füge Relation zu entsprechender spatial Structure hinzu
            return storeFilePath;
        }
    }
}