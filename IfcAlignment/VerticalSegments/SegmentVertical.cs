using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc4.GeometricConstraintResource;

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
