using IfcBridgeToolKit_DataLayer.GeometryConnector;
using System.Linq;
using Xbim.IfcRail.GeometricModelResource;
using Xbim.IfcRail.TopologyResource;
using Xbim.IfcRail.GeometryResource;
using Xbim.IfcRail.RepresentationResource;
using Xbim.Ifc;

namespace IfcBridgeToolKit
{
    class ProdGeometryService
    {
        /// <summary>
        /// Converts a DirectShapeMesh from Revit into an IfcFacetedBRep Entity
        /// </summary>
        /// <param name="model"></param>
        /// <param name="name"></param>
        /// <param name="ifcCartesianPoints"></param>
        /// <returns></returns>
        public static IfcProductDefinitionShape ConvertMyMeshToIfcFacetedBRep(ref IfcStore model, string name,
           DirectShapeToIfc ifcCartesianPoints)
        {
            var ifcFacetedBRep = model.Instances.New<IfcFacetedBrep>();
            var ifcClosedShell = model.Instances.New<IfcClosedShell>();
            ifcFacetedBRep.Outer = ifcClosedShell;

            foreach (var Point in ifcCartesianPoints.Facets)
            {
                var polyloop = model.Instances.New<IfcPolyLoop>();

                var pts = Point.vertices.ToList();

                // Übergibt Eckpunkte in den Polyloob 
                foreach (var pt in pts)
                {
                    var ifcCartesianPoint = model.Instances.New<IfcCartesianPoint>(iCP =>
                    {
                        iCP.X = pt.X;
                        iCP.Y = pt.Y;
                        iCP.Z = pt.Z;
                    });
                    polyloop.Polygon.Add(ifcCartesianPoint);
                }

                var ifcFaceOuterBound = model.Instances.New<IfcFaceOuterBound>(iFOB => iFOB.Bound = polyloop);

                var ifcFace = model.Instances.New<IfcFace>(iF => iF.Bounds.Add(ifcFaceOuterBound));
                //var ifcClosedShell = model.Instances.New<IfcClosedShell>();
                ifcClosedShell.CfsFaces.Add(ifcFace);
                //ifcFacetedBRep.Outer = ifcClosedShell;

                ifcFacetedBRep.Outer = ifcClosedShell;
            }

            var ifcShapeRepresentation = model.Instances.New<IfcShapeRepresentation>();
            var context = model.Instances.OfType<IfcGeometricRepresentationContext>().FirstOrDefault();
            ifcShapeRepresentation.ContextOfItems = context;
            ifcShapeRepresentation.RepresentationIdentifier = "Body";
            ifcShapeRepresentation.RepresentationType = "Brep";
            ifcShapeRepresentation.Items.Add(ifcFacetedBRep);

            //Erstellt IfcProductDefinitionShape 
            var ifcProductDefinitonShape = model.Instances.New<IfcProductDefinitionShape>();
            ifcProductDefinitonShape.Name = name;
            ifcProductDefinitonShape.Representations.Add(ifcShapeRepresentation);

            //  return ProductDefinitionShape
            return ifcProductDefinitonShape;
        }
    }
}
