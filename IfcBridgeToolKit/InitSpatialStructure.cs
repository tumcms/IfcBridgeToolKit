using System.Linq;
using Xbim.Ifc;
using Xbim.IfcRail.GeometricConstraintResource;
using Xbim.IfcRail.GeometryResource;
using Xbim.IfcRail.Kernel;
using Xbim.IfcRail.ProductExtension;
using Xbim.IfcRail.RailwayDomain;

namespace IfcBridgeToolKit
{
    /// <summary>
    /// Fügt IfcEntitäten für die Orientierung hinzu 
    /// </summary>
    public class InitSpatialStructure
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
                bridge.ObjectPlacement = GetIfcLocalPlacement(ref model);
                var mySite = model.Instances.OfType<IfcSite>().FirstOrDefault();

                var spatial2Bridge = model.Instances.New<IfcRelAggregates>();

                spatial2Bridge.RelatingObject = mySite;
                spatial2Bridge.RelatedObjects.Add(bridge);
                
                txn.Commit();
            }
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void CreateIfcBridgePartEntities(ref IfcStore model)
        {
            using (var txn = model.BeginTransaction("Add Bridge Part structure"))
            {
                var superstructure = model.Instances.New<IfcBridgePart>();
                superstructure.Name = "Superstructure";
                superstructure.ObjectPlacement = GetIfcLocalPlacement(ref model);
                superstructure.CompositionType = IfcElementCompositionEnum.ELEMENT;
                superstructure.PredefinedType = IfcBridgePartTypeEnum.SUPERSTRUCTURE;
                

                var substructure = model.Instances.New<IfcBridgePart>();
                substructure.Name = "Substructure";
                substructure.ObjectPlacement = GetIfcLocalPlacement(ref model);
                substructure.CompositionType = IfcElementCompositionEnum.ELEMENT;
                substructure.PredefinedType = IfcBridgePartTypeEnum.SUBSTRUCTURE;

                var surfacestructure = model.Instances.New<IfcBridgePart>();
                surfacestructure.Name = "Surfacestructure";
                surfacestructure.ObjectPlacement = GetIfcLocalPlacement(ref model);
                surfacestructure.CompositionType = IfcElementCompositionEnum.ELEMENT;
                surfacestructure.PredefinedType = IfcBridgePartTypeEnum.SURFACESTRUCTURE;

                var myBridge = model.Instances.OfType<IfcBridge>().FirstOrDefault();
                var spatial2Bridge = model.Instances.New<IfcRelAggregates>();

                spatial2Bridge.RelatingObject = myBridge;

                spatial2Bridge.RelatedObjects.Add(superstructure);
                spatial2Bridge.RelatedObjects.Add(substructure); 
                spatial2Bridge.RelatedObjects.Add(surfacestructure);
                
                txn.Commit();
            }
        }

        
        /// <summary>
        /// Jede Klasse der Spatial Structure benötigt das selbe Local Placement, die Optional miteinander verknüpft werden können
        /// </summary>
        private static IfcLocalPlacement GetIfcLocalPlacement(ref IfcStore model)
        {
            var localPlacement = model.Instances.New<IfcLocalPlacement>();
            var axis2Placement3D = model.Instances.New<IfcAxis2Placement3D>();
            var locationPoint = model.Instances.New<IfcCartesianPoint>(lP => lP.SetXYZ(0, 0, 0));
            var directionAxis = model.Instances.New<IfcDirection>(dA => dA.SetXYZ(0, 0, 1));
            var directionRefDirection = model.Instances.New<IfcDirection>(dRD => dRD.SetXYZ(1, 0, 0));
            axis2Placement3D.Location = locationPoint;
            axis2Placement3D.Axis = directionAxis;
            axis2Placement3D.RefDirection = directionRefDirection;
            localPlacement.RelativePlacement = axis2Placement3D;

            return localPlacement;
        }
    }
}


