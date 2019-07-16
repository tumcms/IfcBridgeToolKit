using Xbim.Ifc;
using Xbim.IfcRail.GeometricConstraintResource;
using Xbim.IfcRail.GeometryResource;

namespace IfcAlignmentCreator.HorizontalSegments 
{
   public abstract class HorizontalSegmentFactory
    {
        public IfcAlignment2DHorizontalSegment CreateAlignment2DHorizontalSegment(
            ref IfcStore model,
            IfcCurveSegment2D geometry)
        {
            var segment = model.Instances.New<IfcAlignment2DHorizontalSegment>();
            segment.CurveGeometry = geometry;
            return segment;
        }

        // call this method whatever type you request
        // type is already set by the choice which factory should be used
        public abstract IfcAlignment2DHorizontalSegment CreateSegment(ref IfcStore model);
    }

    public class LineSegmentFactory : HorizontalSegmentFactory
    {
        private readonly double _startX;
        private readonly double _startY;
        private readonly double _segmentLength;
        private readonly double _startDirection;

        public LineSegmentFactory(double startX, double startY, double segmentLength, double startDirection)
        {
            _startX = startX;
            _startY = startY;
            _segmentLength = segmentLength;
            _startDirection = startDirection;
        }

        public override IfcAlignment2DHorizontalSegment CreateSegment(ref IfcStore model)
        {
            var geomDesigner = new SegmentLineSeg();
            var geometry =
                geomDesigner.CreateIfcLineSegment2D(ref model, _startX, _startY, _segmentLength, _startDirection);
            var segment = CreateAlignment2DHorizontalSegment(ref model, geometry);
            return segment;
        }
    }
}
