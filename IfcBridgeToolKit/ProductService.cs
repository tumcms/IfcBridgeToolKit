using System;
using IfcBridgeToolKit_DataLayer.GeometryConnector;
using System.Linq;
using Xbim.Ifc;
using Xbim.IfcRail.GeometricConstraintResource;
using Xbim.IfcRail.GeometryResource;
using Xbim.IfcRail.Kernel;
using Xbim.IfcRail.ProductExtension;
using Xbim.IfcRail.RailwayDomain;
using Xbim.IfcRail.SharedBldgElements;
using Xbim.IfcRail.StructuralElementsDomain;

namespace IfcBridgeToolKit
{
    /// <summary>
    /// Contains methods to add and modify components inside a given IfcModel. All public methods create a new transaction, all private methods require a running transaction
    /// </summary>
    public class ProductService
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public ProductService()
        {
        }

        /// <summary>
        /// Adds a product to the IFC file
        /// </summary>
        /// <param name="model">current XBIM model, must not be in running transaction</param>
        /// <param name="rawGeometry">product geometry</param>
        /// <param name="name">name of product</param>
        /// <param name="ifcElementType">desired Ifc class (choose one out of IfcBuildingElement</param>
        /// <param name="placementType">either "local", "linear" or "span"</param>
        /// <param name="spatialStructure">choose spatial container the product should be added to</param>
        public void AddBuildingElement(ref IfcStore model, DirectShapeToIfc rawGeometry, string name, string ifcElementType, string placementType, string spatialStructure)
        {
            // handle null inputs
            if (placementType == null)
            {
                placementType = "localPlacement";
            }
            
            using (var txn = model.BeginTransaction("insert a product"))
            {
                IfcBuildingElement buildingElement;

                switch (ifcElementType) // ToDo: make use of enum
                {
                    case "IfcBeam":
                    {
                        buildingElement = model.Instances.New<IfcBeam>();
                        break;
                    }

                    case "IfcBearing":
                    {
                        buildingElement = model.Instances.New<IfcBearing>();
                        break;
                    }

                    case "IfcChimney":
                    {
                        buildingElement = model.Instances.New<IfcChimney>();
                        break;
                    }

                    case "IfcColumn":
                    {
                        // call the bearing function in the toolkit
                        buildingElement = model.Instances.New<IfcColumn>();
                        break;
                    }

                    case "IfcCovering":
                    {
                        // call the bearing function in the toolkit
                        buildingElement = model.Instances.New<IfcCovering>();
                        break;
                    }

                    case "IfcCurtainWall":
                    {
                        // call the bearing function in the toolkit
                        buildingElement = model.Instances.New<IfcCurtainWall>();
                        break;
                    }

                    case "IfcDeepFoundation":
                    {
                        buildingElement = model.Instances.New<IfcDeepFoundation>();
                        break;
                    }

                    case "IfcDoor":
                    {
                        buildingElement = model.Instances.New<IfcDoor>();
                        break;
                    }

                    case "IfcFooting":
                    {
                        buildingElement = model.Instances.New<IfcFooting>();
                        break;
                    }

                    case "IfcMember":
                    {
                        buildingElement = model.Instances.New<IfcMember>();
                        break;
                    }

                    case "IfcPlate":
                    {
                        buildingElement = model.Instances.New<IfcPlate>();
                        break;
                    }

                    case "IfcRailing":
                    {
                        buildingElement = model.Instances.New<IfcRailing>();
                        break;
                    }

                    case "IfcRamp":
                    {
                        buildingElement = model.Instances.New<IfcRamp>();
                        break;
                    }

                    case "IfcRampFlight":
                    {
                        buildingElement = model.Instances.New<IfcRampFlight>();
                        break;
                    }

                    case "IfcRoof":
                    {
                        buildingElement = model.Instances.New<IfcRoof>();
                        break;
                    }

                    case "IfcShadingDevice":
                    {
                        buildingElement = model.Instances.New<IfcShadingDevice>();
                        break;
                    }

                    case "IfcSlab":
                    {
                        buildingElement = model.Instances.New<IfcSlab>();
                        break;
                    }

                    case "IfcStair":
                    {
                        buildingElement = model.Instances.New<IfcStair>();
                        break;
                    }

                    case "IfcWall":
                    {
                        buildingElement = model.Instances.New<IfcWall>();
                        break;
                    }

                    case "IfcWindow":
                    {
                        buildingElement = model.Instances.New<IfcWindow>();
                        break;
                    }

                    // if nothing fits, make an IfcBuildingElementProxy out of it
                    default:
                        buildingElement = model.Instances.New<IfcBuildingElementProxy>();
                        break;
                }

                // fill name property
                buildingElement.Name = name; 
                
                // add product placement (localPlacement or linearPlacement - Span is not supported by Ifc 4x2!)
                switch (placementType)
                {
                    case "local":
                        buildingElement.ObjectPlacement = addLocalPlacement(ref model, rawGeometry.location.Position);
                        break;

                    case "linear":
                    {
                        buildingElement.ObjectPlacement = AddLinearPlacement(ref model, null, 0);
                        break;
                    }

                    case "span":
                    {
                        var e = new Exception("Span placement was not introduced by IfcBridge!");
                        throw e;
                    }

                    default:
                    {
                        var e = new Exception("Placement method was not specified correctly .");
                        throw e;
                    }
                }

                // add product representation              
                buildingElement.Representation = ProdGeometryService.ConvertMyMeshToIfcFacetedBRep(ref model, "representation", rawGeometry);
               
                // add product to spatial structure
                AddToSpatialStructure(ref model, buildingElement, spatialStructure);

                txn.Commit();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="name">Name of ifcProductDefinitionShape</param>
        /// <param name="ifcCartesianPoints"></param>
        /// <returns></returns>
        /// <summary>
        /// LocalPlacement plaziert Komponenten absolut in Bezug auf die geometrischen Representaion
        /// </summary>
        /// <param name="model">Current model, already in transaction</param>
        /// <param name="point">xyz placement</param>
        /// <returns></returns>
        private IfcLocalPlacement addLocalPlacement(ref IfcStore model, Point3D point)
        {
            // localPlacement instance
            var localPlacement = model.Instances.New<IfcLocalPlacement>();
            //Orientation
            var axis2Placement3D = model.Instances.New<IfcAxis2Placement3D>();

            // CartesianPoint
            var locationPoint = model.Instances.New<IfcCartesianPoint>();
            locationPoint.X = 0;
            locationPoint.Y = 0;
            locationPoint.Z = 0;

            // Directions  
            var directionAxis = model.Instances.New<IfcDirection>(dA => dA.SetXYZ(0,
                0,
                1)
            );
            var directionRefDirection = model.Instances.New<IfcDirection>(
                dRd => dRd.SetXYZ(
                    1,
                    0,
                    0)
                );

            
            axis2Placement3D.Location = locationPoint;
            axis2Placement3D.Axis = directionAxis;
            axis2Placement3D.RefDirection = directionRefDirection;

            //Link with parent placement 
            localPlacement.RelativePlacement = axis2Placement3D;

            return localPlacement;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="AlignmentCurve"> Die benötigte AlignmentCurve soll verwendet werden, um die Komponenten an die richtige Stelle zu platzieren </param>
        /// <param name="distancealong">Abstand Start zu Plazierungspunkt muss angegeben werden</param>
        /// <returns></returns>
        private IfcLinearPlacement AddLinearPlacement(ref IfcStore model, IfcCurve AlignmentCurve, double distancealong)
        {
            // ToDo: Identify chosen alignment by its GUID
            var linearPlacement = model.Instances.New<IfcLinearPlacement>();

            // Füge DistanceExpression hinzu 
            var distanceExpression = model.Instances.New<IfcDistanceExpression>();
            distanceExpression.DistanceAlong = distancealong;
            distanceExpression.OffsetLateral = 1;
            distanceExpression.OffsetVertical = 0;
            distanceExpression.OffsetLongitudinal = 0;
            distanceExpression.AlongHorizontal = true;

            var orientationExpression = model.Instances.New<IfcOrientationExpression>();

            // Füge Informationen für OrientationExpression hinzu 
            var lateralAxisDirection = model.Instances.New<IfcDirection>(lAD => lAD.SetXYZ(1,
                0,
                0));
            var verticalAxisDirection = model.Instances.New<IfcDirection>(vAD => vAD.SetXYZ(0,
                0,
                1));

            //Fülle OrientationExpression 
            orientationExpression.LateralAxisDirection = lateralAxisDirection;
            orientationExpression.VerticalAxisDirection = verticalAxisDirection;

            //Fülle den Hauptoperator mit den benötigten Inputs
            linearPlacement.PlacementMeasuredAlong = AlignmentCurve;
            linearPlacement.Distance = distanceExpression;
            linearPlacement.Orientation = orientationExpression;

            return linearPlacement;
        }

        /// <summary>
        /// Builds the objectified relationship between a product and the desired spatial structure container
        /// </summary>
        /// <param name="model"></param>
        /// <param name="product"></param>
        /// <param name="structure"></param>
        /// <returns></returns>
        private void AddToSpatialStructure(ref IfcStore model, IfcProduct product, string structure)
        {
            structure = structure.ToUpper();

            IfcSpatialStructureElement spatialStructure;

            // bridge parts were already added to the model
            spatialStructure = model.Instances.OfType<IfcBridgePart>().FirstOrDefault(type => type.PredefinedType.ToString() == structure);

            // if desired bridge part doesnt exist, add the component to the bridge itself
            if (spatialStructure == null)
            {
                spatialStructure = model.Instances.OfType<IfcBridge>().FirstOrDefault();
            }

            // add new relationship between desired spatial structure container and current product
            model.Instances.New<IfcRelAggregates>(E2S =>
            {
                E2S.RelatingObject = spatialStructure;
                E2S.RelatedObjects.Add(product);
            });
            
        }
        
    }
}