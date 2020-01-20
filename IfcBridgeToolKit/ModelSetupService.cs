using System;
using System.Diagnostics;
using System.Linq;
using Xbim.Common.Step21;
using Xbim.Ifc;
using Xbim.IfcRail.GeometricConstraintResource;
using Xbim.IfcRail.GeometryResource;
using Xbim.IfcRail.Kernel;
using Xbim.IfcRail.MeasureResource;
using Xbim.IfcRail.ProductExtension;
using Xbim.IfcRail.RailwayDomain;
using Xbim.IfcRail.RepresentationResource;
using Xbim.IO;

namespace IfcBridgeToolKit
{
    /// <summary>
    /// Fügt IfcEntitäten für die Orientierung hinzu 
    /// </summary>
    public class ModelSetupService
    {
        /// <summary>
        /// Fügt IfcBridge in die Hierachie ein 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        public void CreateIfcBridgeEntity(ref IfcStore model, string name, string description)
        {
            using (var txn = model.BeginTransaction("Add IfcBridge to Instances"))
            {
                var bridge = model.Instances.New<IfcBridge>();
                bridge.Name = name;
                bridge.Description = description;

                // placement 
                var placementService = new PlacementService();
                bridge.ObjectPlacement = placementService.AddLocalPlacement(ref model, 0,0,0);
               
                // build relationship between bridge and Site
                var mySite = model.Instances.OfType<IfcSite>().FirstOrDefault();
                var spatial2Bridge = model.Instances.New<IfcRelAggregates>();

                spatial2Bridge.RelatingObject = mySite;
                spatial2Bridge.RelatedObjects.Add(bridge);
                
                txn.Commit();
            }
        }

        /// <summary>
        /// Create BridgePart Entities
        /// </summary>
        /// <param name="model"></param>
        /// <exception cref="Exception"></exception>
        public void CreateIfcBridgePartEntities(ref IfcStore model)
        {
            // other services needed for this method:
            var placementService = new PlacementService();

            using (var txn = model.BeginTransaction("Add Bridge Part structure"))
            {
                var superStructure = model.Instances.New<IfcBridgePart>();
                superStructure.Name = "Superstructure";

                superStructure.ObjectPlacement = placementService.AddLocalPlacement(ref model, 0,0,0);
                superStructure.CompositionType = IfcElementCompositionEnum.ELEMENT;
                superStructure.PredefinedType = IfcBridgePartTypeEnum.SUPERSTRUCTURE;
                
                var subStructure = model.Instances.New<IfcBridgePart>();
                subStructure.Name = "Substructure";
                subStructure.ObjectPlacement = placementService.AddLocalPlacement(ref model, 0,0,0);
                subStructure.CompositionType = IfcElementCompositionEnum.ELEMENT;
                subStructure.PredefinedType = IfcBridgePartTypeEnum.SUBSTRUCTURE;

                var surfaceStructure = model.Instances.New<IfcBridgePart>();
                surfaceStructure.Name = "Surfacestructure";
                surfaceStructure.ObjectPlacement = placementService.AddLocalPlacement(ref model,0,0,0);
                surfaceStructure.CompositionType = IfcElementCompositionEnum.ELEMENT;
                surfaceStructure.PredefinedType = IfcBridgePartTypeEnum.SURFACESTRUCTURE;

                // grab the existing bridge
                var myBridge = model.Instances.OfType<IfcBridge>().FirstOrDefault();
                if (myBridge == null)
                {
                    throw new Exception("No IfcBridge Item in the current model. ");
                }
                var spatial2Bridge = model.Instances.New<IfcRelAggregates>();

                spatial2Bridge.RelatingObject = myBridge;

                spatial2Bridge.RelatedObjects.Add(superStructure);
                spatial2Bridge.RelatedObjects.Add(subStructure); 
                spatial2Bridge.RelatedObjects.Add(surfaceStructure);
                
                txn.Commit();
            }
        }
        
        /// <summary>
        /// Sets up the basic parameters any model must provide, units, ownership etc
        /// </summary>
        /// <param name="projectName">Name of the project</param>
        /// <param name="familyName">FamilyName of Model Owner</param>
        /// <param name="firstName">FirstName of Model Owner</param>
        /// <returns>IfcStore model</returns>
        public IfcStore CreateModel(string projectName, string familyName, string firstName)
        {
            //first we need to set up some credentials for ownership of data in the new model
            var credentials = new XbimEditorCredentials
            {
                ApplicationDevelopersName = "IfcBridgeToolKit",
                ApplicationFullName = "TUM_CMS_IfcBridgeToolkit",
                ApplicationVersion = "1.1",
                EditorsFamilyName = familyName,
                EditorsGivenName = firstName,
                EditorsOrganisationName = "Technical University of Munich"
            };

            //now we can create an IfcStore, it is in Ifc4 format and will be held in memory rather than in a database
            //database is normally better in performance terms if the model is large >50MB of Ifc or if robust transactions are required
            
            Console.WriteLine("Create new IfcModel ... ");
            var model = IfcStore.Create(credentials, XbimSchemaVersion.IfcRail, XbimStoreType.InMemoryModel);

            //Begin a transaction as all changes to a model are ACID
            using (var txt = model.BeginTransaction("Initialize Model"))
            {
                Console.WriteLine("Create IfcProject Instance...");
                //create a project
                var project = model.Instances.New<IfcProject>();
                project.Name = projectName;

                Console.WriteLine("Set Units ...");
                // set units
                var unitAssignment = model.Instances.New<IfcUnitAssignment>();

                var lengthUnit = model.Instances.New<IfcSIUnit>();
                lengthUnit.UnitType = IfcUnitEnum.LENGTHUNIT;
                lengthUnit.Name = IfcSIUnitName.METRE;

                var angleUnit = model.Instances.New<IfcSIUnit>();
                angleUnit.UnitType = IfcUnitEnum.PLANEANGLEUNIT;
                angleUnit.Name = IfcSIUnitName.RADIAN;

                var areaUnit = model.Instances.New<IfcSIUnit>();
                areaUnit.UnitType = IfcUnitEnum.AREAUNIT;
                areaUnit.Name = IfcSIUnitName.SQUARE_METRE;

                var volumeUnit = model.Instances.New<IfcSIUnit>();
                volumeUnit.UnitType = IfcUnitEnum.VOLUMEUNIT;
                volumeUnit.Name = IfcSIUnitName.CUBIC_METRE;

                unitAssignment.Units.Add(lengthUnit);
                unitAssignment.Units.Add(angleUnit);
                unitAssignment.Units.Add(areaUnit);
                unitAssignment.Units.Add(volumeUnit);

                // assign to project instance
                project.UnitsInContext = unitAssignment;

                // local placement
                var placement = model.Instances.New<IfcLocalPlacement>();
                var axis2Placement3D = model.Instances.New<IfcAxis2Placement3D>();
                placement.RelativePlacement = axis2Placement3D;

                var point = model.Instances.New<IfcCartesianPoint>();
                point.X = 0;
                point.Y = 0;
                point.Z = 0;
                axis2Placement3D.Location = point;

                Console.WriteLine("Create GeometricRepresentation context... ");
                // IfcGeometricRepresentationContext -- necessary for TIN usw
                CreateGeometricPresentationContext(ref model);

                //now commit the changes, else they will be rolled back at the end of the scope of the using statement
                txt.Commit();
                Console.WriteLine("\n");
            }
            
            return model;
        }

        /// <summary>
        /// Creates a model
        /// </summary>
        /// <param name="projectName">desired project name</param>
        /// <param name="credentials">XBIM Credentials, used inside the Dyn package</param>
        /// <returns></returns>
        public IfcStore CreateModel(string projectName, XbimEditorCredentials credentials)
        {
            // extract names and call internal method
            var model = CreateModel(projectName, credentials.EditorsFamilyName, credentials.EditorsGivenName);

            return model;
        }
        
        /// <summary>
        /// Creates the IfcSite item
        /// </summary>
        /// <param name="model">XBIM model container, not in transaction</param>
        /// <param name="siteName">Site Name</param>
        /// <returns></returns>
        public void CreateIfcSite(ref IfcStore model, string siteName)
        {
            Console.WriteLine("Create IfcSite ...");
            using (var txn = model.BeginTransaction("Add IfcSite Item"))
            {
                // create an IfcSite instance
                var ifcSite = model.Instances.New<IfcSite>();
                ifcSite.Description = "SiteDescription";
                ifcSite.Name = siteName ?? "unknown";
                ifcSite.RefElevation = 0.00;

                Console.WriteLine("Build rel between project and Site...");
                // append ifcSite to the overall project instance
                var myProject = model.Instances.OfType<IfcProject>().FirstOrDefault();
                var spatial2Site = model.Instances.New<IfcRelAggregates>();

                spatial2Site.RelatingObject = myProject;
                spatial2Site.RelatedObjects.Add(ifcSite);

                // the following instances are already in the created project!
                var ifcAxis2Placement3D = model.Instances.OfType<IfcLocalPlacement>().FirstOrDefault();

                // place site at 000 
                ifcSite.ObjectPlacement = ifcAxis2Placement3D;
                
                // Coordinate Reference System
                txn.Commit();
            }

        }
      
        /// <summary>
        /// Creates a new Representation Context
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private void CreateGeometricPresentationContext(ref IfcStore model)
        {
            using (var txn = model.BeginTransaction("add geomContext"))
            {
                // new context 
                var geometricRepresentationContext = model.Instances.New<IfcGeometricRepresentationContext>();
                // set attributes
                geometricRepresentationContext.ContextType = "Model";
                geometricRepresentationContext.CoordinateSpaceDimension = 3;
                geometricRepresentationContext.Precision = 1e-05;

                var locationPoint = model.Instances.New<IfcCartesianPoint>();
                locationPoint.X = 0;
                locationPoint.Y = 0;
                locationPoint.Z = 0;

                var directionAxis = model.Instances.New<IfcDirection>(dA => dA.SetXYZ(0, 0, 1));
                var directionRefDirection = model.Instances.New<IfcDirection>(dRd => dRd.SetXYZ(1, 0, 0));

                var axis2Placement3D = model.Instances.New<IfcAxis2Placement3D>(a2P3D =>
                {
                    a2P3D.Location = locationPoint;
                    a2P3D.Axis = directionAxis;
                    a2P3D.RefDirection = directionRefDirection;
                });

                geometricRepresentationContext.WorldCoordinateSystem = axis2Placement3D;

                var direction = model.Instances.New<IfcDirection>(d => d.SetXY(0, 1));

                geometricRepresentationContext.TrueNorth = direction;

                txn.Commit();
               
            }
        }
    }
}


