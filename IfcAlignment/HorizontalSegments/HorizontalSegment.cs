using Xbim.Ifc4.GeometricConstraintResource;

namespace IfcAlignmentCreator.HorizontalSegments
{
    /// <summary>
    /// Factory Pattern: Abstract class for the requested object
    /// </summary>
    public abstract class SegmentHorizontal
    {
        public abstract double StartX { get; set; }
        public abstract double StartY { get; set; }
        public abstract double SegmentLength { get; set; }
        public abstract double StartDirection { get; set; }

        public abstract IfcAlignment2DHorizontalSegment segment { get; set; }
    }

    
}
