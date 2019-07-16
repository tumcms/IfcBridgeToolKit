using Xbim.Ifc;
using Xbim.IfcRail.GeometryResource;


namespace ElementRepresentation
{
   public class DistExp
    {
        public static IfcDistanceExpression CreateDistanceExpression(ref IfcStore model, double distanceAlong, double offsetLateral, double offsetVertical)
        {
            var ifcDistanceExpression = model.Instances.New<IfcDistanceExpression>(DisEx =>
            {
                DisEx.DistanceAlong = distanceAlong;
                DisEx.OffsetLateral = offsetLateral;
                DisEx.OffsetVertical = offsetVertical;
            });
            return ifcDistanceExpression;
        }
    }
}

