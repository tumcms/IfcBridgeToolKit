using System;
using System.Linq;
using Xbim.Ifc;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.GeometryResource;
using Xbim.Ifc4.ProductExtension;

namespace IfcAlignmentCreator
{
   public class InitIfcAlignment
    {
        private string _name;
        private string _description; 

        public InitIfcAlignment()
        {
            _name = "alignmentName_notDefined";
            _description = "alignmentDefinition_notDefined"; 
        }

        public InitIfcAlignment(string name, string description)
        {
            _name = name;
            _description = description;
           
        }

        public void CreateAlignmentBaseData(ref IfcStore model, out IfcAlignment ifcAlignment)
        {
            ifcAlignment = model.Instances.New<IfcAlignment>();
            ifcAlignment.Name = _name;
            ifcAlignment.Description = _description;

            var css = model.Instances.OfType<IfcRelContainedInSpatialStructure>().FirstOrDefault();
            if (css != null) css.RelatedElements.Add(ifcAlignment);

         
            var ifcLocalPlacement = model.Instances.OfType<IfcLocalPlacement>().First() ?? throw new ArgumentNullException("model.Instances.OfType<IfcLocalPlacement>().First()");
            
            ifcAlignment.ObjectPlacement = ifcLocalPlacement;
          

           
            var ifcAlignmentCurve = model.Instances.New<IfcAlignmentCurve>();
            ifcAlignment.Axis = ifcAlignmentCurve;


            var ifcAlignment2DHorizontal = model.Instances.New<IfcAlignment2DHorizontal>();
            var ifcAlignment2DVertical = model.Instances.New<IfcAlignment2DVertical>();

          
            ifcAlignmentCurve.Horizontal = ifcAlignment2DHorizontal;
            ifcAlignmentCurve.Vertical = ifcAlignment2DVertical;
        }


            
    }
}
