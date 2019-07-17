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

namespace IfcBridgeToolKit
{
    class AddComponents
    {
        // ToDo: implementieren
        /// <summary>
        /// 
        /// </summary>
        /// <param name="meineAufbreiteteGeometrie"></param>
        /// <param name="Bauteilname"></param>
        public void addGirderToIfc(ref IfcStore model, VonRevitMesh2IfcFacetedBRep meineAufbreiteteGeometrie, string Bauteilname)
        {
            using (var txn = model.BeginTransaction("ich füge einen Träger ein"))
            {
                var beam = model.Instances.New<IfcBeam>();
                beam.Name = "HelloBeam";
                beam.Representation = ConvertMyMeshToIfcFacetedBRep();
                beam.ObjectPlacement = addMyLocalPlacement();
            }

        //    // öffne Transaktion auf Ifc Model

        //    // füge ein IfcBeam-Entity hinzu
        //    var beam = model.Instances.New<IfcBeam>();
        //    beam.name = Bauteilname; 

        //    // füge Placement ein - BEISPIEL!! noch nicht fertig implementiert
            addMyLocalPlacement();
        
        //var localPlacement = model.Instances.New<IfcLocalPlacement>();
            //localPlacement.X = meineAufbreiteteGeometrie.PlacementX;
            //meineAufbreiteteGeometrie.MeshPunkte.Add(new Point3D(1,2,3));
            //meineAufbreiteteGeometrie.M
            //    // füge Geometrie in IfcModel ein
            ConvertMyMeshToIfcFacetedBRep();
            //    // beende Transaktion

        }

        /// <summary>
        /// 
        /// </summary>
        public void addPileToIfc()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void addAbutment()
        {



        }

        /// <summary>
        /// 
        /// </summary>
        public void addBearing()
        {


        }

        /// <summary>
        /// 
        /// </summary>
        public void Covering()
        {


        }

        /// <summary>
        /// 
        /// </summary>
        public void addFoundation()
        {

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
        public void addProxyElement()
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        private IfcProductDefinitionShape ConvertMyMeshToIfcFacetedBRep()
        {
            // lege ein IfcFacetedBRep an
            
            // lege ShapeRepresentation an

            // voids...

            // übersetze empfangene Daten in Ifc Sprache
            // CartesianPointList
            // Edges

          //  return new IfcFacetedBrep(); // oder anderes "Level"
          return null;
        }

        /// <summary>
        /// 
        /// </summary>
        ///
      
        private void ConvertMySolidToIfcSectionedSolidHorizontal()
        {
            //not implemented yet
        }

        private IfcLocalPlacement addMyLocalPlacement()
        {
            // IfcCartesianPoint

            // Axes2D/3D

            return null; 
        }

        private void addMyLinearPlacement()
        {
            // IfcLinearPlacement()...

            // IfcDistanceExpressions
        }

    }

}
