using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.DesignScript.Runtime;
using IfcBridgeToolKit;
using IfcBridgeToolKit_DataLayer.GeometryConnector;
using Revit.Elements;
using Xbim.Ifc;

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
        [MultiReturn(new[] { "credentials" })]
        public static  Dictionary<string, object> Credentials(string FamilyName, string FirstName, string organization)
        {
            var credentials = new XbimEditorCredentials
            {
                ApplicationDevelopersName = "IfcBridgeToolKit",
                ApplicationFullName = "TUM_CMS_IfcBridgeToolkit",
                ApplicationVersion = "1.1",
                EditorsFamilyName = FamilyName,
                EditorsGivenName = FirstName,
                EditorsOrganisationName = organization
            };

            return new Dictionary<string, object>
            {
                {"credentials", credentials }
            };
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
        [MultiReturn(new[] { "IfcModel" })]
        public static Dictionary<string, object> InitIfcModel(string projectName, XbimEditorCredentials credentials)
        {
           IfcStore model; 

            try
            {
                var modelCreator = new ModelSetupService(); 
                model = modelCreator.CreateModel(projectName, credentials);
                modelCreator.CreateIfcSite(ref model, "BridgeSite");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            // return model; // return Ifc Model
            return new Dictionary<string, object>
            {
                {"IfcModel", model }
            };
        }

        #endregion

        #region Classifiable Items
        ///<summary>
        /// Main Method to add a component to the IFC model
        /// </summary>
        /// <param name="model">Ifc model - already opened</param>
        /// <param name="credentials">Editor credits from Credentials Node</param>
        /// <param name="elements">List of DirectShape geometries</param>
        /// <param name="ifcElementType">Defines the target IfcClass -> use dropdown menu</param>
        /// <param name="ifcSpatialStructure">Choose desired spatial structure container</param>
        /// <returns>File Path to the IfcModel</returns>
        /// <search>
        ///     girder, beam, IfcBridge
        /// </search>
        [MultiReturn(new[] { "IfcModel" })]
        public static Dictionary<string, object> AddDirectShapeComponents(
            IfcStore model,
            XbimEditorCredentials credentials,
            List<Element> elements,
            string ifcElementType, 
            string ifcSpatialStructure)
        {
            var counter = 0;

            // Note: no transaction is required -> will be opened in the toolkit function
            foreach (var element in elements)
            {
                // init transporter for each element geometry
                var transporter = new DirectShapeToIfc();

                // --- add geometry to transporter ---
                InsertShape(element, ref transporter);

                // --- add placement to transporter --- 
                var location = element.Solids?.FirstOrDefault()?.Centroid();
                if (location != null)
                    transporter.location.Position =new Point3D(location.X, location.Y, location.Z); // insert Revit coordinates into transporter
                else
                    transporter.location.Position = new Point3D(0, 0, 0);

                // use IfcBridgeToolKit to generate a new IfcBuildingElement instance in the current model
                var productService = new ProductService();
                productService.AddBuildingElement(
                    ref model,                 // the Ifc Model
                    transporter,               // the container for all geometry-related content
                    "BuildingElement " + counter.ToString(),    // the bldElement's name
                    ifcElementType,            // desired IfcBuildingElement subclass
                    "local",       // placement method
                    ifcSpatialStructure);     // spatial structure element the component should belong to


                // increase counter
                counter++;
            }
          //  return model;
            return new Dictionary<string, object>
            {
                {"IfcModel", model }
            };
        }

        /// <summary>
        /// Creates an IfcBridge instance and IfcBridgeParts
        /// </summary>
        /// <param name="model">open Ifc Model</param>
        /// <param name="credentials"></param>
        /// <param name="bridgeName"></param>
        /// <param name="bridgeDescription"></param>
        /// <returns></returns>
        [MultiReturn(new[] { "IfcModel" })]
        public static Dictionary<string, object> AddBridgeStructure(IfcStore model, XbimEditorCredentials credentials, string bridgeName, string bridgeDescription)
        {
            try
            {
                var bridgeCreator = new ModelSetupService();
                bridgeCreator.CreateIfcBridgeEntity(ref model, bridgeName, bridgeDescription);
                bridgeCreator.CreateIfcBridgePartEntities(ref model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            // return model; // return directory to Ifc Model
            return new Dictionary<string, object>
            {
                {"IfcModel", model }
            };
        }

        /// <summary>
        /// Stores the IfcModel to a specified directory
        /// </summary>
        /// <param name="model"></param>
        /// <param name="directory">Use Directory node</param>
        /// <param name="fileName">The name of the model as a string (including .ifc)</param>
        public static void FinalizeModel(IfcStore model, string directory, string fileName)
        {
            // build storage path of the model
            var storeFilePath = directory + "/" + fileName;

            // save model
            model.SaveAs(storeFilePath);
            model.Close();

            // update IFC version
            var setupService = new ModelSetupService(); 
            setupService.ModifyHeader(storeFilePath); 
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
       
        // ToDo: add new class to handle mesh geometries

        #endregion
    }
}