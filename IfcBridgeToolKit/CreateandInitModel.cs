using System.Linq;
using Xbim.Common.Step21;
using Xbim.Ifc;
using Xbim.IfcRail.GeometricConstraintResource;
using Xbim.IfcRail.GeometryResource;
using Xbim.IfcRail.Kernel;
using Xbim.IfcRail.MeasureResource;
using Xbim.IfcRail.ProductExtension;
using Xbim.IfcRail.RepresentationResource;
using Xbim.IO;


namespace IfcBridgeToolKit
{/// <summary>
/// Provides basic methods to create a new IfcModel and sets up units, siteContext, ...
/// </summary>
    public class CreateAndInitModel
    {
        /// <summary>
        ///     Sets up the basic parameters any model must provide, units, ownership etc
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
                ApplicationVersion = "1.0",
                EditorsFamilyName = familyName,
                EditorsGivenName = firstName,
                EditorsOrganisationName = "Technical University of Munich"
            };

            //now we can create an IfcStore, it is in Ifc4 format and will be held in memory rather than in a database
            //database is normally better in performance terms if the model is large >50MB of Ifc or if robust transactions are required
            var model = IfcStore.Create(credentials, XbimSchemaVersion.IfcRail, XbimStoreType.InMemoryModel);

            //Begin a transaction as all changes to a model are ACID
            using (var txt = model.BeginTransaction("Initialize Model"))
            {
                //create a project
                var project = model.Instances.New<IfcProject>();
                project.Name = projectName;

                // set units (dont use xBIM's onboard tool if you want to create an alignment)
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

                //IfcGeometricRepresentationContext -- necessary for TIN usw
                var context = model.Instances.New<IfcGeometricRepresentationContext>();
                context.ContextType = "Model";
                context.CoordinateSpaceDimension = 3;
                context.WorldCoordinateSystem = axis2Placement3D;

                // link representationContext with project
                //var context = GetIfcGeometricPresentationContext(ref model);
                //project.RepresentationContexts.Add(context);

                //now commit the changes, else they will be rolled back at the end of the scope of the using statement
                txt.Commit();
            }
            // Speicherpfad
            //var path = @"C:\Benutzer\korbi\OneDrive\Dokumente\IfcBridge001";
            //model.SaveAs(path,StorageType.Ifc, null);
            return model;
           
        }

   


        /// <summary>
        ///     Creates IfcSite,
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void CreateRequiredInstances(ref IfcStore model, string siteName)
        {
            using (var txn = model.BeginTransaction("Add required Instances"))
            {
                // create an IfcSite instance
                var ifcSite = model.Instances.New<IfcSite>();
                ifcSite.Description = "SiteDescription";
                ifcSite.Name = siteName;
                ifcSite.RefElevation = 0.00;

                // append ifcSite to the overall project instance
                var myProject = model.Instances.OfType<IfcProject>().FirstOrDefault();
                var spatial2Site = model.Instances.New<IfcRelAggregates>();

                spatial2Site.RelatingObject = myProject;
                spatial2Site.RelatedObjects.Add(ifcSite);

                // the following instances are already in the created project!
                var ifcAxis2Placement3D = model.Instances.OfType<IfcLocalPlacement>().FirstOrDefault();

                // set placement for IfcSite instance 
                ifcSite.ObjectPlacement = ifcAxis2Placement3D;

                // add a relContainedInSpatialStructure
                var cisps = model.Instances.New<IfcRelContainedInSpatialStructure>(css =>
                {
                    css.RelatingStructure = ifcSite;
                });


                // Coordinate Reference System
                txn.Commit();
            }

        }
        public  IfcGeometricRepresentationContext GetIfcGeometricPresentationContext(ref IfcStore model)
        {
            var geometricRepresentationContext = model.Instances.New<IfcGeometricRepresentationContext>();
            geometricRepresentationContext.ContextType = "Model";
            geometricRepresentationContext.CoordinateSpaceDimension = 3;
            geometricRepresentationContext.Precision = (1e-05);


            var locationPoint = model.Instances.New<IfcCartesianPoint>();
            locationPoint.X = 0;
            locationPoint.Y = 0;
            locationPoint.Z = 0;
        
            var directionAxis = model.Instances.New<IfcDirection>(dA => dA.SetXYZ(0, 0, 1));
            var directionRefDirection = model.Instances.New<IfcDirection>(dRD => dRD.SetXYZ(1, 0, 0));

            var axis2Placement3D = model.Instances.New<IfcAxis2Placement3D>(a2P3D =>
            {
                a2P3D.Location = locationPoint;
                a2P3D.Axis = directionAxis;
                a2P3D.RefDirection = directionRefDirection;
            });

            geometricRepresentationContext.WorldCoordinateSystem = axis2Placement3D;

            var direction = model.Instances.New<IfcDirection>(d => d.SetXY(0, 1));

            geometricRepresentationContext.TrueNorth = direction;
            return geometricRepresentationContext;
        }
    }
}