
using Xbim.Ifc;
using Xbim.Ifc4.RepresentationResource;


namespace ElementRepresentation
{
    public class ProductShapeRepresentation
    {
       public void CreateIfcProductDefinitionShape (ref IfcStore model,
           string name,
           string description,
           IfcRepresentation Representation,                                 
           out IfcProductDefinitionShape NameforElement)
        {
       
            NameforElement = model.Instances.New<IfcProductDefinitionShape>(IPDS =>
            {
                IPDS.Name = name;
                IPDS.Description = description;
                IPDS.Representations.Add(Representation);

            });
        }
    }
    
}
