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
        public void AddIfcBridge(ref IfcStore model, string name, string description)
        {
            using (var txn = model.BeginTransaction("Add IfcBridge to Instances"))
            {
                var bridge = model.Instances.New<IfcBridge>();
                bridge.Name = name;
                bridge.Description = description;
                bridge.ObjectPlacement = GetIfcLocalPlacement(ref model);
                var myProject = model.Instances.OfType<IfcProject>().FirstOrDefault();

                var spatial2Bridge = model.Instances.New<IfcRelAggregates>();

                spatial2Bridge.RelatingObject = myProject;
                spatial2Bridge.RelatedObjects.Add(bridge);
                
                txn.Commit();
             

            }
        }
        /// <summary>
        /// Fügt IfcBridgepart in die Hierachie ein 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="Element"></param>
        /// <param name="NameOfElement"></param>
        public void AddIfcBridgepart(ref IfcStore model, string name, string description, IfcBridgePartTypeEnum NameOfElement)
        {
            using (var txn = model.BeginTransaction("Add IfcBridgepart to Instances"))
            {
                var bridgePart = model.Instances.New<IfcBridgePart>();
                bridgePart.Name = name;
                bridgePart.Description = description;
                bridgePart.ObjectPlacement = GetIfcLocalPlacement(ref model);
                bridgePart.CompositionType = IfcElementCompositionEnum.ELEMENT;
                bridgePart.PredefinedType = NameOfElement;
                

                var myBridge = model.Instances.OfType<IfcBridge>().FirstOrDefault();
                            
                var spatial2Bridge = model.Instances.New<IfcRelAggregates>();
                spatial2Bridge.RelatingObject = myBridge;
                spatial2Bridge.RelatedObjects.Add(bridgePart);
                
                txn.Commit();
            }
                                          
        }

        /// <summary>
        /// Verlinkt Element zu den zugehörigen IfcBrigeparts 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="BuildingElement"></param>
        // eventuell zugriffs modifizierung 
        public void AddIfcRelContainedInSpartialStructure( ref IfcStore model, IfcBuildingElement BuildingElement)
            {
            using (var txn = model.BeginTransaction("Add IfcRelContainedInSpartialStructure"))
            {
                var RelContainedInSpartialStructure = model.Instances.New<IfcRelContainedInSpatialStructure>();
                var myBridgePart = model.Instances.OfType<IfcBridgePart>().FirstOrDefault();
                RelContainedInSpartialStructure.RelatingStructure = myBridgePart;

                RelContainedInSpartialStructure.RelatedElements.Add(BuildingElement);

                txn.Commit();
            }
        }

        /// <summary>
        /// Jede Klasse der Spatial Structure benötigt das selbe Local Placement, die Optional miteinander verknüpft werden können
        /// </summary>
        internal static IfcLocalPlacement GetIfcLocalPlacement(ref IfcStore model)
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


