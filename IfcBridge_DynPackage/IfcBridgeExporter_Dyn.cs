using System;
using System.Collections.Generic;
using System.Linq;
using IfcBridgeToolKit;
using IfcBridgeToolKit_DataLayer.GeometryConnector;
using Revit.Elements;
using Xbim.Ifc;
using Xbim.IO;

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
                modelCreator.CreateRequiredInstances(ref model, "BridgeSite");
                

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
        
        /// <param name="credentials">Editor credits from Credentials Node</param>
        /// <param name="elements">List of DirectShape geometries</param>
        /// <param name="ifcElementType">Defines the target IfcClass -> use dropdown menu</param>
        /// <returns>File Path to the IfcModel</returns>
        /// <search>
        ///     girder, beam, IfcBridge
        /// </search>
        public static string AddDirectShapeComponents(
            string storeFilePath,
            XbimEditorCredentials credentials,
            List<Element> elements,
            string ifcElementType)
        {
            var counter = 0;
            // open Ifc model globally
            var model = IfcStore.Open(storeFilePath, credentials, null, null, XbimDBAccess.ReadWrite);
            // Note: no transaction is required -> will be opened in the toolkit function

            foreach (var element in elements)
            {
                // init transporter for each element geometry
                var transporter = new DirectShapeToIfc();

                // --- GEOMETRIC REPRESENTATION ---
                InsertShape(element, ref transporter);

                // --- PLACEMENT --- 
                var location = element.Solids.FirstOrDefault()?.Centroid();
                if (location != null)
                    transporter.location.Position =
                        new Point3D(location.X, location.Y, location.Z); // insert Revit coordinates into transporter

                // -- Control: serialize to JSON to check the contained data
                //var myPath = @"C:\Users\Sebastian Esser\Desktop\tmpBridge\" + "meshJSON_girder_0" + counter +
                //             ".json";
                //transporter.SerializeToJson(myPath);

                //counter++;

                // init class for interactions with IfcModel
                var toolkit = new AddComponents();

               
                    switch (ifcElementType) // ToDo: make use of enum
                    {
                        case "IfcBearing":
                        {
                            // call the bearing function in the toolkit
                            
                            //toolkit.addBearingToIfc(ref model, transporter, "Bearing" + counter, "BearingRepresentation");
                            break;
                        }
                        case "IfcBeam":
                        {
                            // call the girder function in the toolkit

                            toolkit.addGirderToIfc(ref model, transporter, "Girder" + counter, "GirderRepresentation");
                            break;
                        }
                        case "IfcColumn":
                        {
                            // call the bearing function in the toolkit

                            break;
                        }
                        case "IfcPile": 
                        {
                            // call the bearing function in the toolkit

                            //toolkit.AddPileToIfc(ref model, transporter, "Pile" + counter, "PileRepresentation");
                            break;
                        }
                        case "IfcFoundation":
                        {
                            // call the foundation function in the toolkit

                            //toolkit.addFoundationToIfc(ref model, transporter, "Foundation" + counter, "FoundationRepresentation");
                            break;
                        }

                        case "IfcSlab":
                        {
                            // call the Slab function in the toolkit

                            //toolkit.addSlabToIfc(ref model, transporter, "Slab" + counter, "SlabRepresentation");
                            break;
                        }

                        case "IfcAbutment": 
                        {
                            // call the Abutment function in the toolkit

                           // toolkit.AddAbutment(ref model, transporter, "Wall" + counter,"WallRepresentation");
                            break;
                        }

                        case "IfcPavement":
                        {
                            // call the bearing function in the toolkit

                           // toolkit.addSlabToIfc(ref model, transporter, "Pavement" + counter, "PavementRepresentation");
                            
                            break;
                        }

                      

                        // if nothing fits, make an IfcBuildingElementProxy out of it
                        default:
                          //  toolkit.addProxyElement(ref model,transporter, "Proxy" + counter,"ProxyRepresentation");
                            break;
                  
                }

                
            }

            // save model
            model.SaveAs(storeFilePath);
            model.Close();
            return storeFilePath;
        }

        public static string AddBridgeStructure(string storeFilePath, XbimEditorCredentials credentials, string bridgeName, string bridgeDescription)
        {
            var model = IfcStore.Open(storeFilePath, credentials, null, null, XbimDBAccess.ReadWrite);
            try
            {
               

                var bridgeCreator = new InitSpatialStructure();
                bridgeCreator.AddIfcBridge(ref model, bridgeName, bridgeDescription);
                bridgeCreator.AddIfcBridgepartSuperstructure(ref model);
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
        ///     converts direct shape representation into transporter container
        /// </summary>
        /// <param name="element"></param>
        /// <param name="transporter"></param>
        private static void InsertShape(Element element, ref DirectShapeToIfc transporter)
        {
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
        }

        #endregion
    }
}