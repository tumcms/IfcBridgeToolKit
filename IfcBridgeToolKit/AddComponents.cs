using IfcBridgeToolKit_DataLayer.GeometryConnector;
using System.Linq;
using Xbim.Ifc;
using Xbim.IfcRail.GeometricConstraintResource;
using Xbim.IfcRail.GeometricModelResource;
using Xbim.IfcRail.GeometryResource;
using Xbim.IfcRail.Kernel;
using Xbim.IfcRail.MeasureResource;
using Xbim.IfcRail.ProductExtension;
using Xbim.IfcRail.RailwayDomain;
using Xbim.IfcRail.RepresentationResource;
using Xbim.IfcRail.SharedBldgElements;
using Xbim.IfcRail.StructuralElementsDomain;
using Xbim.IfcRail.TopologyResource;

namespace IfcBridgeToolKit
{
    /// <summary>
    /// 
    /// </summary>
    public class AddComponents
    {
        /// <summary>
        /// default constructor
        /// </summary>
        public AddComponents()
        {
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
                beam.ObjectPlacement = addMyLocalPlacement(ref model, rawGeometry.location.Position);
                beam.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, Representation, rawGeometry);
                beam.Name = Name;

                AddToSuperstructure(ref model, beam);

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
                pile.ObjectPlacement = addMyLocalPlacement(ref model, rawGeometry.location.Position);
                pile.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, Representation, rawGeometry);
                pile.Name = Name;

                // insert in spatial structure
                AddToSubstructure(ref model, pile);

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
                wingWall.ObjectPlacement = addMyLocalPlacement(ref model, rawGeometry.location.Position);
                wingWall.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, representation, rawGeometry);
                wingWall.Name = name;

                // insert in spatial structure
                AddToSubstructure(ref model, wingWall);

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
                slab.ObjectPlacement = addMyLocalPlacement(ref model, rawGeometry.location.Position);
                slab.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, Representation, rawGeometry);
                slab.Name = Name;

                // insert in spatial structure
                AddToSurfacestructure(ref model, slab);

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
                bearing.ObjectPlacement = addMyLocalPlacement(ref model, rawGeometry.location.Position);
                bearing.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, representation, rawGeometry);
                bearing.Name = name;

                // insert in spatial structure
                AddToSuperstructure(ref model, bearing);

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
                covering.ObjectPlacement = addMyLocalPlacement(ref model, meineAufbereiteteGeometrie.location.Position);
                covering.Representation =
                    ConvertMyMeshToIfcFacetedBRep(ref model, NameRepräsentation, meineAufbereiteteGeometrie);

                // insert in spatial structure
                AddToSurfacestructure(ref model, covering);

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
                    addMyLocalPlacement(ref model, meineAufbereiteteGeometrie.location.Position);
                foundation.Representation =
                    ConvertMyMeshToIfcFacetedBRep(ref model, NameRepräsentation, meineAufbereiteteGeometrie);

                // insert in spatial structure
                AddToSubstructure(ref model, foundation);

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
                buildingElementProxy.ObjectPlacement = addMyLocalPlacement(ref model, rawGeometry.location.Position);
                buildingElementProxy.Representation =
                    ConvertMyMeshToIfcFacetedBRep(ref model, NameRepräsentation, rawGeometry);
                //ToDo: If-Loops für 3 Structuretypen, die auf Bauteiltypen referenzieren

                AddToSuperstructure(ref model, buildingElementProxy);

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
        public static IfcProductDefinitionShape ConvertMyMeshToIfcFacetedBRep(ref IfcStore model, string name,
            DirectShapeToIfc ifcCartesianPoints)
        {
            var ifcFacetedBRep = model.Instances.New<IfcFacetedBrep>();
            var ifcClosedShell = model.Instances.New<IfcClosedShell>();
            ifcFacetedBRep.Outer = ifcClosedShell;

            foreach (var Point in ifcCartesianPoints.Facets)
            {
                var polyloop = model.Instances.New<IfcPolyLoop>();

                var pts = Point.vertices.ToList();

                // Übergibt Eckpunkte in den Polyloob 
                foreach (var pt in pts)
                {
                    var ifcCartesianPoint = model.Instances.New<IfcCartesianPoint>(iCP =>
                    {
                        iCP.X = pt.X;
                        iCP.Y = pt.Y;
                        iCP.Z = pt.Z;
                    });
                    polyloop.Polygon.Add(ifcCartesianPoint);
                }

                var ifcFaceOuterBound = model.Instances.New<IfcFaceOuterBound>(iFOB => iFOB.Bound = polyloop);

                var ifcFace = model.Instances.New<IfcFace>(iF => iF.Bounds.Add(ifcFaceOuterBound));
                //var ifcClosedShell = model.Instances.New<IfcClosedShell>();
                ifcClosedShell.CfsFaces.Add(ifcFace);
                //ifcFacetedBRep.Outer = ifcClosedShell;

                ifcFacetedBRep.Outer = ifcClosedShell;
            }

            var ifcShapeRepresentation = model.Instances.New<IfcShapeRepresentation>();
            var context = model.Instances.OfType<IfcGeometricRepresentationContext>().FirstOrDefault();
            ifcShapeRepresentation.ContextOfItems = context;
            ifcShapeRepresentation.RepresentationIdentifier = "Body";
            ifcShapeRepresentation.RepresentationType = "Brep";
            ifcShapeRepresentation.Items.Add(ifcFacetedBRep);

            //Erstellt IfcProductDefinitionShape 
            var ifcProductDefinitonShape = model.Instances.New<IfcProductDefinitionShape>();
            ifcProductDefinitonShape.Name = name;
            ifcProductDefinitonShape.Representations.Add(ifcShapeRepresentation);

            //  return ProductDefinitionShape
            return ifcProductDefinitonShape;
        }

        /// <summary>
        /// LocalPlacement plaziert Komponenten absolut in Bezug auf die geometrischen Representaion
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Point">
        /// <returns></returns>
        private IfcLocalPlacement addMyLocalPlacement(ref IfcStore model, Point3D Point)
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
            var directionAxis = model.Instances.New<IfcDirection>(dA => dA.SetXYZ(0, 0, 1));
            var directionRefDirection = model.Instances.New<IfcDirection>(dRD => dRD.SetXYZ(1, 0, 0));

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
        private IfcLinearPlacement addMyLinearPlacement(ref IfcStore model, IfcCurve UseModelAlignmentCurve,
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
            var lateralAxisDirection = model.Instances.New<IfcDirection>(lAD => lAD.SetXYZ(1, 0, 0));
            var verticalAxisDirection = model.Instances.New<IfcDirection>(vAD => vAD.SetXYZ(0, 0, 1));

            //Fülle OrientationExpression 
            orientationExpression.LateralAxisDirection = lateralAxisDirection;
            orientationExpression.VerticalAxisDirection = verticalAxisDirection;

            //Fülle den Hauptoperator mit den benötigten Inputs
            linearPlacement.PlacementMeasuredAlong = UseModelAlignmentCurve;
            linearPlacement.Distance = distanceExpression;
            linearPlacement.Orientation = orientationExpression;

            return linearPlacement;
        }

        private IfcBridgePart AddToSuperstructure(ref IfcStore model, IfcElement addElement)
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

        private IfcBridgePart AddToSubstructure(ref IfcStore model, IfcElement addElement)
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

        private IfcBridgePart AddToSurfacestructure(ref IfcStore model, IfcElement addElement)
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
