using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc;
using Xbim.Ifc4.GeometricConstraintResource;

namespace IfcAlignmentCreator.VerticalSegments
{
    class SegmentVerSegLine : SegmentVertical
    {
        public override double StartDistAlong { get; set; }
        public override double HorizontalLength { get; set; }
        public override double StartHeight { get; set; }
        public override double Gradient { get; set; }
        public override IfcAlignment2DVerticalSegment Segment { get; set; }

        public IfcAlignment2DVerticalSegment Create2DVerticalLineSeg2D(
                ref IfcStore model,
                double startDistAlong,
                double horizontalLength,
                double startHeight,
                double Gradient)
        {
            Segment = model.Instances.New<IfcAlignment2DVerSegLine>(sg =>
            {
                sg.StartDistAlong = startDistAlong;
                sg.HorizontalLength = horizontalLength;
                sg.StartHeight = startHeight;
                sg.StartGradient = Gradient;

            });

            return Segment;

        }

    }
}
