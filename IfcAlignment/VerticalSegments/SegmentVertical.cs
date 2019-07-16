using Xbim.IfcRail.GeometricConstraintResource;

namespace IfcAlignmentCreator.VerticalSegments
{
    abstract class SegmentVertical
    {
        public abstract double StartDistAlong { get; set; }
        public abstract double HorizontalLength { get; set; }
        public abstract double StartHeight { get; set; }
        public abstract double Gradient { get; set; }

        public abstract IfcAlignment2DVerticalSegment Segment { get; set; }
    }
}
