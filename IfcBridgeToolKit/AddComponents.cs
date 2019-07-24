using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;
using IfcBridgeToolKit_DataLayer;
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

namespace IfcBridgeToolKit
{
    public class AddComponents
    {
        // ToDo: implementieren
        /// <summary>
        /// 
        /// </summary>
        /// <param name="meineAufbreiteteGeometrie"></param>
        /// <param name="Bauteilname"></param>
        public void addGirderToIfc(ref IfcStore model, VonRevitMesh2IfcFacetedBRep meineAufbreiteteGeometrie, string Bauteilname, string NameRepräsentation)
        {
            using (var txn = model.BeginTransaction("ich füge einen Träger ein"))
            {
                var beam = model.Instances.New<IfcBeam>();
               
             
                beam.Name = Bauteilname;
                //beam.Representation = ConvertMyMeshToIfcFacetedBRep(ref model,NameRepräsentation, list );
                //beam.ObjectPlacement = addMyLocalPlacement(refmodel,)
               // beam.ObjectPlacement = addMyLocalPlacement();
            }

        //    // öffne Transaktion auf Ifc Model

        //    // füge ein IfcBeam-Entity hinzu
        

        //    // füge Placement ein - BEISPIEL!! noch nicht fertig implementiert
            //addMyLocalPlacement();
        
        //var localPlacement = model.Instances.New<IfcLocalPlacement>();
            //localPlacement.X = meineAufbreiteteGeometrie.PlacementX;
            //meineAufbreiteteGeometrie.MeshPunkte.Add(new Point3D(1,2,3));
            //meineAufbreiteteGeometrie.M
            //    // füge Geometrie in IfcModel ein
          //  ConvertMyMeshToIfcFacetedBRep();
            //    // beende Transaktion

        }

        /// <summary>
        /// 
        /// </summary>
        public void addPileToIfc(ref IfcStore model,string fileName, string Bauteilname, string Repräsentationsname, IfcCartesianPoint PlacementPoint, List<IfcCartesianPoint> PointList)
        {
            string FileName = fileName;

            using (IfcStore.Open(FileName))
            { 
            using (var txn = model.BeginTransaction("Füge einen Pfahl ein"))
            {
                    var pile = model.Instances.New<IfcPile>();
                    pile.Name = Bauteilname;
                    pile.ObjectPlacement = addMyLocalPlacement(ref model, PlacementPoint);
                    pile.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, Repräsentationsname, PointList);



                txn.Commit();
            }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void addAbutment(ref IfcStore model, string fileName, string Bauteilname, string Repräsentationsname, IfcCartesianPoint PlacementPoint, List<IfcCartesianPoint> PointList)
        {
            string FileName = fileName;
            using (IfcStore.Open(FileName))
            {
                using (var txn = model.BeginTransaction("Füge einen Pfahl ein"))
                {
                    //var pile = model.Instances.New<>();
                    //pile.Name = Bauteilname;
                    //pile.ObjectPlacement = addMyLocalPlacement(ref model, PlacementPoint);
                    //pile.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, Repräsentationsname, PointList);



                    txn.Commit();
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void addBearing(ref IfcStore model, string fileName, string Bauteilname, string Repräsentationsname, IfcCartesianPoint PlacementPoint, List<IfcCartesianPoint> PointList)
        {
            string FileName = fileName;
            using (IfcStore.Open(FileName))
            {
                using (var txn = model.BeginTransaction("Füge ein Bearing ein"))
                {
                    var Bearing = model.Instances.New<IfcBearing>();
                    Bearing.Name = Bauteilname;
                    Bearing.ObjectPlacement = addMyLocalPlacement(ref model, PlacementPoint);
                    Bearing.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, Repräsentationsname, PointList);

                    txn.Commit();
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void Covering(ref IfcStore model, string fileName, string Bauteilname, string Repräsentationsname, IfcCartesianPoint PlacementPoint, List<IfcCartesianPoint> PointList)
        {
            string FileName = fileName;
            using (IfcStore.Open(FileName))
            {
                using (var txn = model.BeginTransaction("Füge ein Covering ein"))
                {
                    var Covering = model.Instances.New<IfcCovering>();
                    Covering.Name = Bauteilname;
                    Covering.ObjectPlacement = addMyLocalPlacement(ref model, PlacementPoint);
                    Covering.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, Repräsentationsname, PointList);

                    txn.Commit();
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void addFoundation(ref IfcStore model, string fileName, string Bauteilname, string Repräsentationsname, IfcCartesianPoint PlacementPoint, List<IfcCartesianPoint> PointList)
        {
            string FileName = fileName;
            using (IfcStore.Open(FileName))
            {
                using (var txn = model.BeginTransaction("Füge ein Fundament ein"))
                {
                    var Foundation = model.Instances.New<IfcDeepFoundation>();
                    Foundation.Name = Bauteilname;
                    Foundation.ObjectPlacement = addMyLocalPlacement(ref model, PlacementPoint);
                    Foundation.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, Repräsentationsname, PointList);

                    txn.Commit();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void addPier()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void addProxyElement(ref IfcStore model, string fileName, string Bauteilname, string Repräsentationsname, IfcCartesianPoint PlacementPoint, List<IfcCartesianPoint> PointList)
        {
            string FileName = fileName;
            using (IfcStore.Open(FileName))
            {
                using (var txn = model.BeginTransaction("Füge ein Fundament ein"))
                {
                    var buildingElementProxy = model.Instances.New<IfcBuildingElementProxy>();
                    buildingElementProxy.Name = Bauteilname;
                    buildingElementProxy.ObjectPlacement = addMyLocalPlacement(ref model, PlacementPoint);
                    buildingElementProxy.Representation = ConvertMyMeshToIfcFacetedBRep(ref model, Repräsentationsname, PointList);

                    txn.Commit();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static IfcProductDefinitionShape ConvertMyMeshToIfcFacetedBRep(ref IfcStore model, string name, List<IfcCartesianPoint> ifcCartesianPoints)
        {
                   
           //ToDo: Liste mit allen Eckpunkten einer Geometrie implementieren --> foreach schleife muss darauf ansprechen!
           //Frage an Sebastian: Herangehensweise ähneld deiner schreibweise. Es wird allerdings nur ein Facegeneriert, dass kann doch dann nicht die gesamte Geometrie beschreiben 
           
                
                var ifcFacetedBRep = model.Instances.New<IfcFacetedBrep>();
                              
            var polyloob = model.Instances.New<IfcPolyLoop>();
            //Frage an Sebastian: Es werden CaresianPoints generiert die ich nicht möchte --> Generiere Punkte in der ConsoleAppanwendung ... diese Übergebe ich zu ConvertMyMeshtoIfcFacetedBRep
            //Erstelle Liste die für den Zweiten foreach-loop verwendet werden kann 
            foreach (IfcCartesianPoint Point in ifcCartesianPoints)
            {
                List<IfcCartesianPoint> Eckpunkte = new List<IfcCartesianPoint> 
                { };
            Eckpunkte.Add(Point);
           
            
            // Übergibt Eckpunkte in den Polyloob 
            foreach (var Eckpunkt in Eckpunkte)
                {
                    var ifcCartesianPoint = model.Instances.New<IfcCartesianPoint>(iCP =>
                    {
                        iCP.X = Eckpunkt.X;
                        iCP.Y = Eckpunkt.Y;
                        iCP.Z = Eckpunkt.Z;

                    });
                    polyloob.Polygon.Add(ifcCartesianPoint);
                }
            }
            var ifcFaceOuterBound = model.Instances.New<IfcFaceOuterBound>(iFOB => iFOB.Bound = polyloob);
                
            var ifcFace = model.Instances.New<IfcFace>(iF => iF.Bounds.Add(ifcFaceOuterBound));

            var ifcClosedShell = model.Instances.New<IfcClosedShell>(iCS => iCS.CfsFaces.Add(ifcFace));
            //Erstellt Repräsentationsart "FacetedBRep" 
            ifcFacetedBRep.Outer = ifcClosedShell;
            //Erstellt IfcShapeRepresentation 
            var ifcShapeRepresentation = model.Instances.New<IfcShapeRepresentation>();
            //Iniziiere GeometricPresentationContext 
            var CreateModel = new CreateAndInitModel();
            var context = CreateModel.GetIfcGeometricPresentationContext(ref model);
            ifcShapeRepresentation.ContextOfItems = context;
            ifcShapeRepresentation.RepresentationIdentifier = "Body";
            ifcShapeRepresentation.RepresentationIdentifier = "FacetedbRep";
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
        /// LocalPlacement plaziert Komponenten absolut in Bezug auf der geometrischen Representaion
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Point"> Der Plazierungspunkt der afubereiteten Meshgeometrie muss verwendet werden, um eine korrekte Plazierung zu gewährleisten </param>
        /// <returns></returns>
        private IfcLocalPlacement addMyLocalPlacement(ref IfcStore model, IfcCartesianPoint Point )
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
            //Frage an Sebastian: Soll IfcDirection seperat erstellt werden --> Referenz auf Cambridge_4x2: Elemente habe unterschiedliche Directions 
            var directionAxis = model.Instances.New<IfcDirection>(dA => dA.SetXYZ(0, 0, 1));
            var directionRefDirection = model.Instances.New<IfcDirection>(dRD => dRD.SetXYZ(1, 0, 0));

            //Fülle den Hauptoperatoren mit den Benötigten Inputs 
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
        /// !!!Achtung: Wenn Alignment existiert, nutze LinearPlacement!!! 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="UseModelAlignmentCurve"> Die benötigte AlignmentCurve soll verwendet werden, um die Komponenten an die richtige Stelle zu platzieren </param>
        /// <param name="distancealong">Abstand Start zu Plazierungspunkt muss angegeben werden</param>
        /// <returns></returns>
        private IfcLinearPlacement addMyLinearPlacement(ref IfcStore model, IfcCurve UseModelAlignmentCurve, IfcLengthMeasure distancealong)
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

            //Frage an Sebastian: Soll ein IfcAxis2Placement3D implementiert werden... Ist ja nur notwendig, wenn App linear Placement nicht unterstützt 
            //linearPlacement.CartesianPosition = ;


            return linearPlacement;
        }

    }

}
