using System;
using System.Collections.Generic;
using System.Linq;
using IfcBridgeToolKit;
using IfcBridgeToolKit_DataLayer.GeometryConnector;
using Xbim.Ifc;
using Xbim.IO;
using DirectShape = Revit.Elements.DirectShape;
using Element = Revit.Elements.Element;



namespace IfcBridge_DynPackage
{
    /// <summary>
    ///     Compilation of IfcBridge related methods to create Ifc 4x2 models out of Dynamo
    /// </summary>
    public class IfcBridgeExporter_Dyn
    {
        #region InitData

        /// <summary>
        ///     Define Credits for IfcModel interactions
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
        /// <param name="directory">use 'Directory Path' node from Dynamo</param>
        /// <param name="fileName">simple string in the form of *.ifc</param>
        /// <param name="bridgeName">Bridge Name - could be empty</param>
        /// <param name="bridgeDescription">Bridge Description - could be empty</param>
        /// <returns>File Path to the IfcModel</returns>
        /// <search>
        ///     init, IfcBridge
        /// </search>
        public static string InitIfcModel(string projectName, XbimEditorCredentials credentials, string directory,
            string fileName, string bridgeName = "unknown", string bridgeDescription = "unknown")
        {
            // build storage path of the model
            var storeFilePath = directory + "/" + fileName;

            try
            {
                var modelCreator = new CreateAndInitModel(); //ToDo: Correct Header -> IfcVersion 
                var model = modelCreator.CreateModel(projectName, credentials);

                var bridgeCreator = new InitSpatialStructure();
                bridgeCreator.AddIfcBridge(ref model, bridgeName, bridgeDescription);

                model.SaveAs(storeFilePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return storeFilePath; // return directory to Ifc Model
        }

        #endregion

        #region Classifiable Items

        /// <summary>
        ///     Add girder items to IfcModel
        /// </summary>
        /// <param name="storeFilePath">Path to the already existing IfcModel</param>
        /// <param name="credentials">Editor credits from Credentials Node</param>
        /// <param name="elements">List of DirectShape geometries</param>
        /// <returns>File Path to the IfcModel</returns>
        /// <search>
        ///     girder, beam, IfcBridge
        /// </search>
        public static string AddGirdersFromRevit(
            string storeFilePath,
            XbimEditorCredentials credentials,
            List<Element> elements)
        {
            var counter = 0;

            foreach (var element in elements)
            {
                // init transporter for each girder
                var transporter = new DirectShapeToIfc();

                // --- GEOMETRIC SHAPE ---
                // every directShape consists of several faces. Each face has several corner points that are connected by a polyline
                foreach (var face in element.Faces)
                {
                    var transporterFacet =
                        new Facet(); // face in IfcToolKit understanding, contains several boundaryPoints

                    // error safety
                    if (face == null) continue;

                    // convert each vertexPt from Revit into an Pt3D of IfcToolKit definition
                    foreach (var revitVertex in face.Vertices)
                    {
                        var pt3D = new Point3D(
                            revitVertex.PointGeometry.X,
                            revitVertex.PointGeometry.Y,
                            revitVertex.PointGeometry.Z
                        );
                        // add point to current face
                        transporterFacet.vertices.Add(pt3D);
                    }

                    // add face to transporter (remember: one shape has several faces
                    transporter.Facets.Add(transporterFacet);
                }

                // --- PLACEMENT --- 
                var location = element.Solids.FirstOrDefault()?.Centroid();
               
                // insert Revit coordinates into transporter
                if (location != null)
                {
                    transporter.location.Position = new Point3D(location.X, location.Y, location.Z);
                }
                    
                //// serialize to JSON to check the contained data
                var myPath = @"C:\Users\Sebastian Esser\Desktop\tmpBridge\" + "meshJSON_girder_0" + counter.ToString() +
                             ".json";
                transporter.SerializeToJson(myPath);

                counter++;
            }


            //// open Ifc model
            //using (var model = IfcStore.Open(storeFilePath, credentials, null, null, XbimDBAccess.ReadWrite))
            //{
            //    // do fancy transactions
            //    using (var txn = model.BeginTransaction("add a bridge item"))
            //    {
            //        // commit changes
            //        txn.Commit();
            //    }

            //    // save model
            //    model.SaveAs(storeFilePath);
            //}

            return storeFilePath;
        }

        /// <summary>
        ///     Add foundation items to IfcModel
        /// </summary>
        /// <param name="storeFilePath"></param>
        /// <param name="credentials"></param>
        /// <param name="foundationMeshes"></param>
        /// <returns></returns>
        public static string AddFoundationsFromRevit(string storeFilePath, XbimEditorCredentials credentials,
            List<DirectShape> foundationMeshes)
        {
            return storeFilePath;
        }

        /// <summary>
        /// </summary>
        /// <param name="storeFilePath"></param>
        /// <param name="credentials"></param>
        /// <param name="bridgeDeckMeshes"></param>
        /// <returns></returns>
        public static string AddBridgeDecksFromRevit(string storeFilePath, XbimEditorCredentials credentials,
            List<DirectShape> bridgeDeckMeshes)
        {
            return storeFilePath;
        }

        /// <summary>
        /// </summary>
        /// <param name="storeFilePath"></param>
        /// <param name="credentials"></param>
        /// <param name="abutmentMeshes"></param>
        /// <returns></returns>
        public static string AddAbutmentsFromRevit(string storeFilePath, XbimEditorCredentials credentials,
            List<DirectShape> abutmentMeshes)
        {
            return storeFilePath;
        }

        /// <summary>
        /// </summary>
        /// <param name="storeFilePath"></param>
        /// <param name="credentials"></param>
        /// <param name="pierMeshes"></param>
        /// <returns></returns>
        public static string AddPiersFromRevit(string storeFilePath, XbimEditorCredentials credentials,
            List<DirectShape> pierMeshes)
        {
            return storeFilePath;
        }

        /// <summary>
        /// </summary>
        /// <param name="storeFilePath"></param>
        /// <param name="credentials"></param>
        /// <param name="bearingMeshes"></param>
        /// <returns></returns>
        public static string AddBearingsFromRevit(string storeFilePath, XbimEditorCredentials credentials,
            List<DirectShape> bearingMeshes)
        {
            return storeFilePath;
        }

        #endregion

        #region Unclassificable Elements

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeFilePath"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public static string AddCapsFromRevit(string storeFilePath, XbimEditorCredentials credentials)
        {
            // -> IfcBeam? 
            return storeFilePath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeFilePath"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public static string AddPavementFromRevit(string storeFilePath, XbimEditorCredentials credentials)
        {
            // will be defined by IfcRoad -> use IfcBldElementProxy for now
            return storeFilePath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="storeFilePath"></param>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public static string AddCurbstonesFromRevit(string storeFilePath, XbimEditorCredentials credentials)
        {
            // will be defined by IfcRoad -> use IfcBldElementProxy for now
            return storeFilePath;
        }

        #endregion
    }
}