using IfcBridgeToolKit_DataLayer.GeometryConnector;
using Xbim.Ifc;
using Xbim.IfcRail.GeometricConstraintResource;
using Xbim.IfcRail.GeometryResource;

namespace IfcBridgeToolKit
{
    /// <summary>
    /// This services manages everything which is related to placements inside an IfcModel
    /// </summary>
    class PlacementService
    {
        /// <summary>
        /// Creates a local placement at a given position and default directions. Running transaction is required
        /// </summary>
        public IfcLocalPlacement AddLocalPlacement(ref IfcStore model, Point3D point)
        {
            return AddLocalPlacement(ref model, point.X, point.Y,point.Z);
        }

        /// <summary>
        /// Creates a local placement at a given position and default directions. Running transaction is required
        /// </summary>
        public IfcLocalPlacement AddLocalPlacement(ref IfcStore model, double? xIn, double? yIn, double? zIn)
        {
            double x, y, z; 
            // check input
            if (xIn == null) 
                x = 0;
            else
                x = (double) xIn;

            if (yIn == null)
                y = 0;
            else
                y = (double)yIn;

            if (zIn == null)
                z = 0;
            else
                z = (double)zIn;



            // build new Ifc entities
            var localPlacement = model.Instances.New<IfcLocalPlacement>();

            // relationship to parenting placement system
            var axis2Placement3D = model.Instances.New<IfcAxis2Placement3D>();

            var locationPoint = model.Instances.New<IfcCartesianPoint>(lP => lP.SetXYZ(x, y, z));
            
            // direction
            var directionAxis = model.Instances.New<IfcDirection>(dA => dA.SetXYZ(0, 0, 1));
            var directionRefDirection = model.Instances.New<IfcDirection>(dRD => dRD.SetXYZ(1, 0, 0));
            
            // build relationships
            axis2Placement3D.Location = locationPoint;
            axis2Placement3D.Axis = directionAxis;
            axis2Placement3D.RefDirection = directionRefDirection;
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
        public IfcLinearPlacement AddLinearPlacement(ref IfcStore model, IfcCurve AlignmentCurve, double distancealong)
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


    }
}
