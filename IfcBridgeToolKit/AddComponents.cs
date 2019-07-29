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
        /// <param name="meineAufbreiteteGeometrie"></param>
        /// <param name="Bauteilname"></param>
        public void addGirderToIfc(
            ref IfcStore model,
            DirectShapeToIfc meineAufbreiteteGeometrie,
            string Bauteilname,
            string NameRepräsentation)
        {
            using (var txn = model.BeginTransaction("ich füge einen Träger ein"))
            {
                var beam = model.Instances.New<IfcBeam>();
                beam.ObjectPlacement = addMyLocalPlacement(ref model, meineAufbreiteteGeometrie.location.Position);
                beam.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, NameRepräsentation, meineAufbreiteteGeometrie);
                beam.Name = Bauteilname;

                addSuperstructure(ref model, beam);
                
                txn.Commit();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="meineAufbereiteteGeometrie"></param>
        /// <param name="Bauteilname"></param>
        /// <param name="NameRepräsentation"></param>
        public void AddPileToIfc(
            ref IfcStore model,
            DirectShapeToIfc meineAufbereiteteGeometrie,
            string Bauteilname,
            string NameRepräsentation)
        {
            using (var txn = model.BeginTransaction("Füge einen Pfahl ein"))
            {
                    var pile = model.Instances.New<IfcPile>();
                    pile.ObjectPlacement = addMyLocalPlacement(ref model, meineAufbereiteteGeometrie.location.Position);
                    pile.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, NameRepräsentation, meineAufbereiteteGeometrie);
                    pile.Name = Bauteilname;

                    addSubstructure(ref model, pile);

                txn.Commit();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="meineAufbereiteteGeometrie"></param>
        /// <param name="Bauteilname"></param>
        /// <param name="NameRepräsentation"></param>
        public void AddAbutment(
            ref IfcStore model,
            DirectShapeToIfc meineAufbereiteteGeometrie,
            string Bauteilname,
            string NameRepräsentation)
        {
            using (var txn = model.BeginTransaction("Füge Endauflager hinzu ein"))
            {
                    var wingWall = model.Instances.New<IfcWall>();
                    wingWall.ObjectPlacement = addMyLocalPlacement(ref model, meineAufbereiteteGeometrie.location.Position);
                    wingWall.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, NameRepräsentation, meineAufbereiteteGeometrie);
                    wingWall.Name = Bauteilname;

                    addSubstructure(ref model, wingWall);

                txn.Commit();
            }
        }

        public void addSlabToIfc(
            ref IfcStore model,
            DirectShapeToIfc meineAufbereiteteGeometrie,
            string Bauteilname,
            string NameRepräsentation)
        {
            using (var txn = model.BeginTransaction("Füge Slab  ein"))
            {
                var slab = model.Instances.New<IfcSlab>();
                slab.ObjectPlacement = addMyLocalPlacement(ref model, meineAufbereiteteGeometrie.location.Position);
                slab.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, NameRepräsentation, meineAufbereiteteGeometrie);
                slab.Name = Bauteilname;

                addSurfacestructure(ref model, slab);

                txn.Commit();
            }
        }

        public void addPlateToIfc(
            ref IfcStore model,
            DirectShapeToIfc meineAufbereiteteGeometrie,
            string Bauteilname,
            string NameRepräsentation)
        {
            using (var txn = model.BeginTransaction("Füge Plate  ein"))
            {
                var plate = model.Instances.New<IfcPlate>();
                plate.ObjectPlacement = addMyLocalPlacement(ref model, meineAufbereiteteGeometrie.location.Position);
                plate.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, NameRepräsentation, meineAufbereiteteGeometrie);
                plate.Name = Bauteilname;

                addSurfacestructure(ref model, plate);

                txn.Commit();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="meineAufbereiteteGeometrie"></param>
        /// <param name="Bauteilname"></param>
        /// <param name="NameRepräsentation"></param>
        public void addBearingToIfc(
            ref IfcStore model,
            DirectShapeToIfc meineAufbereiteteGeometrie,
            string Bauteilname,
            string NameRepräsentation)
        {
            using (var txn = model.BeginTransaction("Füge ein Lager ein"))
            {
                    var bearing = model.Instances.New<IfcBearing>();
                    bearing.ObjectPlacement = addMyLocalPlacement(ref model, meineAufbereiteteGeometrie.location.Position);
                    bearing.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, NameRepräsentation, meineAufbereiteteGeometrie);
                    bearing.Name = Bauteilname;

                    addSuperstructure(ref model, bearing);

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
        public void addCovering(
            ref IfcStore model,
            DirectShapeToIfc meineAufbereiteteGeometrie,
            string Bauteilname,
            string NameRepräsentation)
        {
            using (var txn = model.BeginTransaction("Füge ein Covering ein"))
            {
                    var covering = model.Instances.New<IfcCovering>();
                    covering.Name = Bauteilname;
                    covering.ObjectPlacement = addMyLocalPlacement(ref model, meineAufbereiteteGeometrie.location.Position);
                    covering.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, NameRepräsentation, meineAufbereiteteGeometrie);

                    addSurfacestructure(ref model, covering);

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
                    foundation.ObjectPlacement = addMyLocalPlacement(ref model, meineAufbereiteteGeometrie.location.Position);
                    foundation.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, NameRepräsentation, meineAufbereiteteGeometrie);

                    addSubstructure(ref model, foundation);

                    txn.Commit();
                }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="meineAufbereiteteGeometrie"></param>
        /// <param name="Bauteilname"></param>
        /// <param name="NameRepräsentation"></param>
        public void addProxyElement(
            ref IfcStore model,
            DirectShapeToIfc meineAufbereiteteGeometrie,
            string Bauteilname,
            string NameRepräsentation)
        {
            
                using (var txn = model.BeginTransaction("Füge ein Fundament ein"))
                {
                    var buildingElementProxy = model.Instances.New<IfcBuildingElementProxy>();
                    buildingElementProxy.Name = Bauteilname;
                    buildingElementProxy.ObjectPlacement = addMyLocalPlacement(ref model, meineAufbereiteteGeometrie.location.Position);
                    buildingElementProxy.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, NameRepräsentation, meineAufbereiteteGeometrie);
                //ToDo: If-Loops für 3 Structuretypen, die auf Bauteiltypen referenzieren
                    txn.Commit();
                }
        }


     /// <summary>
     /// 
     /// </summary>
     /// <param name="model"></param>
     /// <param name="name"></param>
     /// <param name="ifcCartesianPoints"></param>
     /// <returns></returns>
        public static IfcProductDefinitionShape ConvertMyMeshToIfcFacetedBRep(ref IfcStore model, string name,
           DirectShapeToIfc ifcCartesianPoints)
        {


            var ifcFacetedBRep = model.Instances.New<IfcFacetedBrep>();
            var ifcClosedShell = model.Instances.New<IfcClosedShell>();
            ifcFacetedBRep.Outer = ifcClosedShell;
            //var ifcClosedShell = model.Instances.New<IfcClosedShell>();
            //ifcFacetedBRep.Outer = ifcClosedShell;


            foreach (var Point in ifcCartesianPoints.Facets)
            {
                var polyloob = model.Instances.New<IfcPolyLoop>();


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
                    polyloob.Polygon.Add(ifcCartesianPoint);
                }


                var ifcFaceOuterBound = model.Instances.New<IfcFaceOuterBound>(iFOB => iFOB.Bound = polyloob);

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
        /// SectionedSolidHorizontal kann Ideal verwendet werden, sobald in Dynamo die geometrische Repräsentation für Ifc verbessert wird 
        /// Es werden Solids, mit Informationen über die Oberfläche die gesweept wird, benötigt
        /// </summary>
        ///
        private void ConvertMySolidToIfcSectionedSolidHorizontal()
        {
            //not implemented yet
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

        private IfcBridgePart addSuperstructure(ref IfcStore model, IfcElement addElement)
        {
            var superstructure = model.Instances.OfType<IfcBridgePart>().FirstOrDefault(type => type.PredefinedType == IfcBridgePartTypeEnum.SUPERSTRUCTURE);
            model.Instances.New<IfcRelAggregates>(E2S => {
                E2S.RelatingObject = superstructure;
                E2S.RelatedObjects.Add(addElement);
            });
            return superstructure;
        }

        private IfcBridgePart addSubstructure(ref IfcStore model, IfcElement addElement)
        {
            var substructure = model.Instances.OfType<IfcBridgePart>().FirstOrDefault(type => type.PredefinedType == IfcBridgePartTypeEnum.SUBSTRUCTURE);
            model.Instances.New<IfcRelAggregates>(E2S => {
                E2S.RelatingObject = substructure;
                E2S.RelatedObjects.Add(addElement);
            });
            return substructure;
        }

        private IfcBridgePart addSurfacestructure(ref IfcStore model, IfcElement addElement)
        {
            var surfacestructure = model.Instances.OfType<IfcBridgePart>().FirstOrDefault(type => type.PredefinedType == IfcBridgePartTypeEnum.SURFACESTRUCTURE);
            model.Instances.New<IfcRelAggregates>(E2S => {
                E2S.RelatingObject = surfacestructure;
                E2S.RelatedObjects.Add(addElement);
            });
            return surfacestructure;
        }
    }
}
