using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc;
using Xbim.IO;
using Xbim.Common.Step21;
using Xbim.Ifc4;
using Xbim.Ifc4.Kernel;
using Xbim.Common;
using Xbim.Ifc4.SharedBldgElements;
using Xbim.Ifc4.PropertyResource;
using Xbim.Ifc4.MeasureResource;
using Xbim.Ifc4.GeometricConstraintResource;
using Xbim.Ifc4.GeometryResource;
using Xbim.Ifc4.Interfaces;
using Xbim.Ifc4.ProductExtension;
using Xbim.Ifc4.RepresentationResource;
using Xbim.Ifc4.ActorResource;
using Xbim.Ifc4.DateTimeResource;
using Xbim.Ifc4.ExternalReferenceResource;
using Xbim.Ifc4.PresentationOrganizationResource;
using Xbim.Ifc4.GeometricModelResource;
using Xbim.Ifc4.ProfileResource;



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
