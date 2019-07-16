using Xbim.Ifc;
using Xbim.IfcRail.GeometricConstraintResource;
using Xbim.IfcRail.GeometryResource;

namespace IfcAlignmentCreator.HorizontalSegments
{
    public class SegmentLineSeg : SegmentHorizontal
    {
        private double _startX;
        private double _startY;
        private double _segmentLength;
        private double _startDirection;
        private IfcAlignment2DHorizontalSegment _segment;

        public override double StartX
        {
            get => _startX;
            set => _startX = value;
        }

        public override double StartY
        {
            get => _startY;
            set => _startY = value;
        }

        public override double SegmentLength
        {
            get => _segmentLength;
            set => _segmentLength = value;
        }

        public override double StartDirection
        {
            get => _startDirection;
            set => _startDirection = value;
        }

        public override IfcAlignment2DHorizontalSegment segment
        {
            get => _segment;
            set => _segment = value;
        }


        /// <summary>
        ///     Create and add a LineSegment to the model
        ///     Find the curve geometry with this.CurveGeometry
        ///     Model must be part of a running transaction!!
        /// </summary>
        /// <param name="model">Current IFC model</param>
        /// <param name="startX">startpoint x value</param>
        /// <param name="startY">startpoint y value</param>
        /// <param name="segmentLength">length of the desired element</param>
        /// <param name="startDirection">first derivation against global normal x </param>
        /// <returns></returns>
        public IfcCurveSegment2D CreateIfcLineSegment2D(
            ref IfcStore model,
            double startX,
            double startY,
            double segmentLength,
            double startDirection)
        {
            // init
            var segmentGeometry = model.Instances.New<IfcLineSegment2D>(sg =>
            {
                sg.SegmentLength = segmentLength;
                sg.StartDirection = startDirection;
            });
            segmentGeometry.StartPoint = model.Instances.New<IfcCartesianPoint>(
                pt =>
                {
                    pt.X = startX;
                    pt.Y = startY;
                });

            return segmentGeometry;
        }
    }
}
