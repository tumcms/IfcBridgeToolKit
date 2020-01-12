using System;
using System.Collections.Generic;
using IfcBridgeToolKit_DataLayer.GeometryConnector;
using System.Linq;
using Autodesk.Revit.DB;
using Xbim.Ifc;
using Xbim.IfcRail.GeometricConstraintResource;
using Xbim.IfcRail.GeometryResource;
using Xbim.IfcRail.Kernel;
using Xbim.IfcRail.MeasureResource;
using Xbim.IfcRail.ProductExtension;
using Xbim.IfcRail.RailwayDomain;
using Xbim.IfcRail.SharedBldgElements;
using Xbim.IfcRail.StructuralElementsDomain;
using static IfcBridgeToolKit.BridgePartEnum;

namespace IfcBridgeToolKit
{
    /// <summary>
    /// 
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
        /// <param name="placementType">either "localPlacement" or "linearPlacement"</param>
        /// <param name="spatialStructure">choose spatial container the product should be added to</param>
        public void AddProduct(ref IfcStore model, DirectShapeToIfc rawGeometry, string name, string ifcElementType, string placementType, string spatialStructure)
        {
            // handle null inputs
            if (placementType == null)
            {
                placementType = "localPlacement";
            }
            
            using (var txn = model.BeginTransaction("insert a product"))
            {
                IfcProduct product;

                switch (ifcElementType) // ToDo: make use of enum
                {
                    case "IfcBeam":
                    {
                        product = model.Instances.New<IfcBeam>();
                        break;
                    }

                    case "IfcBearing":
                    {
                        product = model.Instances.New<IfcBearing>();
                        break;
                    }

                    case "IfcChimney":
                    {
                        product = model.Instances.New<IfcChimney>();
                        break;
                    }

                    case "IfcColumn":
                    {
                        // call the bearing function in the toolkit
                        product = model.Instances.New<IfcColumn>();
                        break;
                    }

                    case "IfcCovering":
                    {
                        // call the bearing function in the toolkit
                        product = model.Instances.New<IfcCovering>();
                        break;
                    }

                    case "IfcCurtainWall":
                    {
                        // call the bearing function in the toolkit
                        product = model.Instances.New<IfcCurtainWall>();
                        break;
                    }

                    case "IfcDeepFoundation":
                    {
                        product = model.Instances.New<IfcDeepFoundation>();
                        break;
                    }

                    case "IfcDoor":
                    {
                        product = model.Instances.New<IfcDoor>();
                        break;
                    }

                    case "IfcFooting":
                    {
                        product = model.Instances.New<IfcFooting>();
                        break;
                    }

                    case "IfcMember":
                    {
                        product = model.Instances.New<IfcMember>();
                        break;
                    }

                    case "IfcPlate":
                    {
                        product = model.Instances.New<IfcPlate>();
                        break;
                    }

                    case "IfcRailing":
                    {
                        product = model.Instances.New<IfcRailing>();
                        break;
                    }

                    case "IfcRamp":
                    {
                        product = model.Instances.New<IfcRamp>();
                        break;
                    }

                    case "IfcRampFlight":
                    {
                        product = model.Instances.New<IfcRampFlight>();
                        break;
                    }

                    case "IfcRoof":
                    {
                        product = model.Instances.New<IfcRoof>();
                        break;
                    }

                    case "IfcShadingDevice":
                    {
                        product = model.Instances.New<IfcShadingDevice>();
                        break;
                    }

                    case "IfcSlab":
                    {
                        product = model.Instances.New<IfcSlab>();
                        break;
                    }

                    case "IfcStair":
                    {
                        product = model.Instances.New<IfcStair>();
                        break;
                    }

                    case "IfcWall":
                    {
                        product = model.Instances.New<IfcWall>();
                        break;
                    }

                    case "IfcWindow":
                    {
                        product = model.Instances.New<IfcWindow>();
                        break;
                    }

                    // if nothing fits, make an IfcBuildingElementProxy out of it
                    default:
                        product = model.Instances.New<IfcBuildingElementProxy>();
                        break;
                }

                // fill name property
                product.Name = name; 


                // add product placement (localPlacement or linearPlacement - Span is not supported by Ifc 4x2!)
                switch (placementType)
                {
                    case "local":
                        product.ObjectPlacement = addMyLocalPlacement(ref model,
                            rawGeometry.location.Position);
                        break;

                    case "linearPlacement":
                        var e = new Exception("Linear placement is not implemented yet.");
                        throw e;
                }

                // add product representation              
                product.Representation =
                    ProdGeometryService.ConvertMyMeshToIfcFacetedBRep(ref model,
                        "representation",
                        rawGeometry);
                product.Name = name;

                // add product to spatial structure
                spatialStructure = spatialStructure.ToUpper();
               
                txn.Commit();
            }
        }

       


        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawGeometry"></param>
        /// <param name="Name"></param>
        public void AddGirderToIfc(
            ref IfcStore model,
            DirectShapeToIfc rawGeometry,
            string Name,
            string Representation)
        {
            using (var txn = model.BeginTransaction("insert beam"))
            {
                var beam = model.Instances.New<IfcBeam>();
                beam.ObjectPlacement = addMyLocalPlacement(ref model,
                    rawGeometry.location.Position);
                beam.Representation =
                    ProdGeometryService.ConvertMyMeshToIfcFacetedBRep(ref model,
                        Representation,
                        rawGeometry);
                beam.Name = Name;

                AddToSuperstructure(ref model,
                    beam);

                txn.Commit();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="rawGeometry"></param>
        /// <param name="Name"></param>
        /// <param name="Representation"></param>
        public void AddPileToIfc(
            ref IfcStore model,
            DirectShapeToIfc rawGeometry,
            string Name,
            string Representation)
        {
            using (var txn = model.BeginTransaction("insert pile"))
            {
                var pile = model.Instances.New<IfcPile>();
                pile.ObjectPlacement = addMyLocalPlacement(ref model,
                    rawGeometry.location.Position);
                pile.Representation =
                    ProdGeometryService.ConvertMyMeshToIfcFacetedBRep(ref model,
                        Representation,
                        rawGeometry);
                pile.Name = Name;

                // insert in spatial structure
                AddToSubstructure(ref model,
                    pile);

                txn.Commit();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="rawGeometry"></param>
        /// <param name="name"></param>
        /// <param name="representation"></param>
        public void AddAbutment(
            ref IfcStore model,
            DirectShapeToIfc rawGeometry,
            string name,
            string representation)
        {
            using (var txn = model.BeginTransaction("insert abutment"))
            {
                var wingWall = model.Instances.New<IfcWall>();
                wingWall.ObjectPlacement = addMyLocalPlacement(ref model,
                    rawGeometry.location.Position);
                wingWall.Representation =
                    ProdGeometryService.ConvertMyMeshToIfcFacetedBRep(ref model,
                        representation,
                        rawGeometry);
                wingWall.Name = name;

                // insert in spatial structure
                AddToSubstructure(ref model,
                    wingWall);

                txn.Commit();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="rawGeometry"></param>
        /// <param name="Name"></param>
        /// <param name="Representation"></param>
        public void addSlabToIfc(
            ref IfcStore model,
            DirectShapeToIfc rawGeometry,
            string Name,
            string Representation)
        {
            using (var txn = model.BeginTransaction("insert slab"))
            {
                var slab = model.Instances.New<IfcSlab>();
                slab.ObjectPlacement = addMyLocalPlacement(ref model,
                    rawGeometry.location.Position);
                slab.Representation =
                    ProdGeometryService.ConvertMyMeshToIfcFacetedBRep(ref model,
                        Representation,
                        rawGeometry);
                slab.Name = Name;

                // insert in spatial structure
                AddToSurfacestructure(ref model,
                    slab);

                txn.Commit();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="rawGeometry"></param>
        /// <param name="name"></param>
        /// <param name="representation"></param>
        public void addBearingToIfc(
            ref IfcStore model,
            DirectShapeToIfc rawGeometry,
            string name,
            string representation)
        {
            using (var txn = model.BeginTransaction("insert bearing"))
            {
                var bearing = model.Instances.New<IfcBearing>();
                bearing.ObjectPlacement = addMyLocalPlacement(ref model,
                    rawGeometry.location.Position);
                bearing.Representation =
                    ProdGeometryService.ConvertMyMeshToIfcFacetedBRep(ref model,
                        representation,
                        rawGeometry);
                bearing.Name = name;

                // insert in spatial structure
                AddToSuperstructure(ref model,
                    bearing);

                txn.Commit();
            }
        }

        //ToDo: verify use of IfcCovering 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fileName"></param>
        /// <param name="Bauteilname"></param>
        /// <param name="Repräsentationsname"></param>
        /// <param name="PlacementPoint"></param>
        /// <param name="PointList"></param>
        public void addCovering(
            ref IfcStore model,
            DirectShapeToIfc meineAufbereiteteGeometrie,
            string Bauteilname,
            string NameRepräsentation)
        {
            using (var txn = model.BeginTransaction("insertCovering"))
            {
                var covering = model.Instances.New<IfcCovering>();
                covering.Name = Bauteilname;
                covering.ObjectPlacement = addMyLocalPlacement(ref model,
                    meineAufbereiteteGeometrie.location.Position);
                covering.Representation =
                    ProdGeometryService.ConvertMyMeshToIfcFacetedBRep(ref model,
                        NameRepräsentation,
                        meineAufbereiteteGeometrie);

                // insert in spatial structure
                AddToSurfacestructure(ref model,
                    covering);

                txn.Commit();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fileName"></param>
        /// <param name="Bauteilname"></param>
        /// <param name="Repräsentationsname"></param>
        /// <param name="PlacementPoint"></param>
        /// <param name="PointList"></param>
        public void addFoundationToIfc(
            ref IfcStore model,
            DirectShapeToIfc meineAufbereiteteGeometrie,
            string Bauteilname,
            string NameRepräsentation)
        {
            using (var txn = model.BeginTransaction("Füge ein Fundament ein"))
            {
                var foundation = model.Instances.New<IfcDeepFoundation>();
                foundation.Name = Bauteilname;
                foundation.ObjectPlacement =
                    addMyLocalPlacement(ref model,
                        meineAufbereiteteGeometrie.location.Position);
                foundation.Representation =
                    ProdGeometryService.ConvertMyMeshToIfcFacetedBRep(ref model,
                        NameRepräsentation,
                        meineAufbereiteteGeometrie);

                // insert in spatial structure
                AddToSubstructure(ref model,
                    foundation);

                txn.Commit();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="rawGeometry"></param>
        /// <param name="Bauteilname"></param>
        /// <param name="NameRepräsentation"></param>
        public void addProxyElement(
            ref IfcStore model,
            DirectShapeToIfc rawGeometry,
            string Bauteilname,
            string NameRepräsentation)
        {
            using (var txn = model.BeginTransaction("Füge ein Fundament ein"))
            {
                var buildingElementProxy = model.Instances.New<IfcBuildingElementProxy>();
                buildingElementProxy.Name = Bauteilname;
                buildingElementProxy.ObjectPlacement = addMyLocalPlacement(ref model,
                    rawGeometry.location.Position);
                buildingElementProxy.Representation =
                    ProdGeometryService.ConvertMyMeshToIfcFacetedBRep(ref model,
                        NameRepräsentation,
                        rawGeometry);
                //ToDo: If-Loops für 3 Structuretypen, die auf Bauteiltypen referenzieren

                AddToSuperstructure(ref model,
                    buildingElementProxy);

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
        /// <param name="model"></param>
        /// <param name="Point">
        /// <returns></returns>
        private IfcLocalPlacement addMyLocalPlacement(ref IfcStore model,
            Point3D Point)
        {
            var localPlacement = model.Instances.New<IfcLocalPlacement>();
            //Axis3D
            var axis2Placement3D = model.Instances.New<IfcAxis2Placement3D>();

            // CartesianPoint has get the coordinates for the choosen Placement 
            var locationPoint = model.Instances.New<IfcCartesianPoint>();
            locationPoint.X = 0;
            locationPoint.Y = 0;
            locationPoint.Z = 0;

            //Grundlegende definition der Achsen und der referenzierten Richtung    
            var directionAxis = model.Instances.New<IfcDirection>(dA => dA.SetXYZ(0,
                0,
                1));
            var directionRefDirection = model.Instances.New<IfcDirection>(dRD => dRD.SetXYZ(1,
                0,
                0));

            //Fülle den Hauptoperatoren mit den benötigten Inputs 
            axis2Placement3D.Location = locationPoint;
            axis2Placement3D.Axis = directionAxis;
            axis2Placement3D.RefDirection = directionRefDirection;

            //Erstelle LocalPlacement 
            localPlacement.RelativePlacement = axis2Placement3D;

            return localPlacement;
        }

        /// <summary>
        /// LinearPlacement wird verwendet, wenn ein Alignment verwendet wird 
        /// Aktueller Status: AlignmentCurve wird nicht verwendet, da IW diese nicht nach Revit exportiert 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="UseModelAlignmentCurve"> Die benötigte AlignmentCurve soll verwendet werden, um die Komponenten an die richtige Stelle zu platzieren </param>
        /// <param name="distancealong">Abstand Start zu Plazierungspunkt muss angegeben werden</param>
        /// <returns></returns>
        private IfcLinearPlacement addMyLinearPlacement(ref IfcStore model,
            IfcCurve UseModelAlignmentCurve,
            IfcLengthMeasure distancealong)
        {
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
            linearPlacement.PlacementMeasuredAlong = UseModelAlignmentCurve;
            linearPlacement.Distance = distanceExpression;
            linearPlacement.Orientation = orientationExpression;

            return linearPlacement;
        }

        private IfcBridgePart AddToSuperstructure(ref IfcStore model,
            IfcProduct addElement)
        {
            var superstructure = model.Instances.OfType<IfcBridgePart>()
                .FirstOrDefault(type => type.PredefinedType == IfcBridgePartTypeEnum.SUPERSTRUCTURE);
            model.Instances.New<IfcRelAggregates>(E2S =>
            {
                E2S.RelatingObject = superstructure;
                E2S.RelatedObjects.Add(addElement);
            });
            return superstructure;
        }

        private IfcBridgePart AddToSubstructure(ref IfcStore model,
            IfcElement addElement)
        {
            var substructure = model.Instances.OfType<IfcBridgePart>()
                .FirstOrDefault(type => type.PredefinedType == IfcBridgePartTypeEnum.SUBSTRUCTURE);
            model.Instances.New<IfcRelAggregates>(E2S =>
            {
                E2S.RelatingObject = substructure;
                E2S.RelatedObjects.Add(addElement);
            });
            return substructure;
        }

        private IfcBridgePart AddToSurfacestructure(ref IfcStore model,
            IfcElement addElement)
        {
            var surfacestructure = model.Instances.OfType<IfcBridgePart>()
                .FirstOrDefault(type => type.PredefinedType == IfcBridgePartTypeEnum.SURFACESTRUCTURE);
            model.Instances.New<IfcRelAggregates>(E2S =>
            {
                E2S.RelatingObject = surfacestructure;
                E2S.RelatedObjects.Add(addElement);
            });
            return surfacestructure;
        }
    }
}