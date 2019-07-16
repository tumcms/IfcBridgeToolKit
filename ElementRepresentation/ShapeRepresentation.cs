using Xbim.Ifc;
using Xbim.Ifc4.GeometryResource;
using Xbim.Ifc4.RepresentationResource;
using System.Linq;
using Xbim.Ifc4.ProductExtension;
using System;
using Xbim.Ifc4.MeasureResource;

namespace ElementRepresentation
{
    public class ShapeRepresentation

    {
        public static IfcShapeRepresentation CreateIfcShapeRepresentation (
            ref IfcStore model,
            IfcLabel Type,
            IfcLabel Identifier,
            IfcRepresentationItem item
            )
        {
           

            var shapeRepresentation = model.Instances.New<IfcShapeRepresentation>(ISP =>
            {
                               ISP.RepresentationIdentifier = Identifier;
                ISP.RepresentationType = Type;
                ISP.Items.Add(item);
            
                
            });
            // Modelcontext - Created when model was initialized
            var modelContext = model.Instances.OfType<IfcGeometricRepresentationContext>().FirstOrDefault();
            if (modelContext == null)
            {
                var ex = new Exception("Didn't find Model Context");
                throw ex;
            }
          
            shapeRepresentation.ContextOfItems = modelContext;
            return shapeRepresentation;
        }
    }
}
