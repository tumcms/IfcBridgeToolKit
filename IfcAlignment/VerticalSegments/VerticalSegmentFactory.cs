using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc;
using Xbim.IfcRail.GeometricConstraintResource;

namespace IfcAlignmentCreator.VerticalSegments
{
    public abstract class VerticalSegmentFactory
    {
        public abstract IfcAlignment2DVerticalSegment CreateSegment(ref IfcStore model);
    }
    public class VerSeqLineFactory : VerticalSegmentFactory
    {
    
        private readonly double _startDistAlong;
        private readonly double _horizontalLength;
        private readonly double _startHeight;
        private readonly double _gradient;

        /// <summary>
        /// Constructor for vertical line segment
        /// </summary>
        /// <param name="startDistAlong"></param>
        /// <param name="horizontalLength"></param>
        /// <param name="startHeight"></param>
        /// <param name="gradient"></param>
        public VerSeqLineFactory(double startDistAlong, double horizontalLength, double startHeight, double gradient)
        {
            _startDistAlong = startDistAlong;
            _horizontalLength = horizontalLength;
            _startHeight = startHeight;
            _gradient = gradient;
        }

        /// <summary>
        /// add entity to model
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public override IfcAlignment2DVerticalSegment CreateSegment(ref IfcStore model)
        {
            var designer = new SegmentVerSegLine();
            var segment = designer.Create2DVerticalLineSeg2D(ref model, _startDistAlong, _horizontalLength,
                _startHeight, _gradient);

            return segment;
        }
    }
}
