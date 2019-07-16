using Xbim.Ifc;
using Xbim.IfcRail.SharedBldgElements;
using Xbim.IfcRail.GeometricConstraintResource;
using Xbim.IfcRail.RepresentationResource;


namespace ElementCreator
{
    public class ElementWriter
    {
       

        public static  IfcBeam CreateifcBeam(ref IfcStore model, string name, string description, IfcObjectPlacement objectPlacement, IfcProductDefinitionShape productRepresentation )
        {
            var ifcBeam = model.Instances.New<IfcBeam>();
            ifcBeam.Name = name;
            ifcBeam.Description = description;
            ifcBeam.ObjectPlacement = objectPlacement;
            ifcBeam.Representation = productRepresentation; 
            return ifcBeam;
        }

        public static IfcSlab CreateifcSlab(ref IfcStore model, string name, string description, IfcObjectPlacement objectPlacement, IfcProductDefinitionShape productRepresentation)
        {
            var ifcSlab = model.Instances.New<IfcSlab>();
            ifcSlab.Name = name;
            ifcSlab.Description = description;
            ifcSlab.ObjectPlacement = objectPlacement;
            ifcSlab.Representation = productRepresentation;
            return ifcSlab;
        }
    
    }
}
