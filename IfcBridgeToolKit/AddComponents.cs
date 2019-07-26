using System.Collections.Generic;
using IfcBridgeToolKit_DataLayer;
using IfcBridgeToolKit_DataLayer.GeometryConnector;
using Xbim.Ifc;
using Xbim.IfcRail.SharedBldgElements;
using Xbim.IfcRail.GeometricConstraintResource;
using Xbim.IfcRail.GeometricModelResource;
using Xbim.IfcRail.RepresentationResource;
using Xbim.IfcRail.TopologyResource;
using Xbim.IfcRail.GeometryResource;
using Xbim.IfcRail.MeasureResource;
using Xbim.IfcRail.StructuralElementsDomain;
using Xbim.IfcRail.RailwayDomain;
using System.Linq;
using Xbim.IfcRail.Kernel;

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
               
                var myBridge = model.Instances.OfType < IfcBridgePart> ().FirstOrDefault();
                myBridge.PredefinedType = IfcBridgePartTypeEnum.SUPERSTRUCTURE;

                var spatial2Bridge = model.Instances.New<IfcRelAggregates>();
                spatial2Bridge.RelatingObject = myBridge;
                spatial2Bridge.RelatedObjects.Add(beam);


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
                    var pile = model.Instances.New<IfcWall>();
                    pile.ObjectPlacement = addMyLocalPlacement(ref model, meineAufbereiteteGeometrie.location.Position);
                    pile.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, NameRepräsentation, meineAufbereiteteGeometrie);
                    pile.Name = Bauteilname;

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
        public void addBearing(
            ref IfcStore model,
            DirectShapeToIfc meineAufbereiteteGeometrie,
            string Bauteilname,
            string NameRepräsentation)
        {
            using (var txn = model.BeginTransaction("Füge ein Lager ein"))
            {
                    var Bearing = model.Instances.New<IfcBearing>();
                    Bearing.ObjectPlacement = addMyLocalPlacement(ref model, meineAufbereiteteGeometrie.location.Position);
                    Bearing.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, NameRepräsentation, meineAufbereiteteGeometrie);
                    Bearing.Name = Bauteilname;

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
        public void Covering(
            ref IfcStore model,
            DirectShapeToIfc meineAufbereiteteGeometrie,
            string Bauteilname,
            string NameRepräsentation)
        {
            using (var txn = model.BeginTransaction("Füge ein Covering ein"))
            {
                    var Covering = model.Instances.New<IfcCovering>();
                    Covering.Name = Bauteilname;
                    Covering.ObjectPlacement = addMyLocalPlacement(ref model, meineAufbereiteteGeometrie.location.Position);
                    Covering.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, NameRepräsentation, meineAufbereiteteGeometrie);

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
        public void AddFoundation(
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
                ifcClosedShell.CfsFaces.Add(ifcFace);

            }

            var ifcShapeRepresentation = model.Instances.New<IfcShapeRepresentation>();
                var context = model.Instances.OfType<IfcGeometricRepresentationContext>().FirstOrDefault();
                ifcShapeRepresentation.ContextOfItems = context;
                ifcShapeRepresentation.RepresentationIdentifier = "Body";
                ifcShapeRepresentation.RepresentationType = "BRep";
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
            locationPoint.X = Point.X;
            locationPoint.Y = Point.Y;
            locationPoint.Z = Point.Z;

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
    }
}
