using IfcBridgeToolKit_DataLayer.GeometryConnector;
using System.Linq;
using Xbim.IfcRail.GeometricModelResource;
using Xbim.IfcRail.TopologyResource;
using Xbim.IfcRail.GeometryResource;
using Xbim.IfcRail.RepresentationResource;
using Xbim.Ifc;
using QuantumConcepts.Formats.StereoLithography;
using Xbim.IfcRail.MeasureResource;
using Off_GeomLibrary;

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

        /// <summary>
        /// Converts a given STL geometry into an IfcFacetedBRep representation
        /// </summary>
        /// <param name="model"></param>
        /// <param name="stlDocument"></param>
        /// <returns></returns>
        public static IfcFacetedBrep StlToIfc_FBRep(ref IfcStore model, STLDocument stlDocument)
        {
            // ToDo: introduce Scaling: mm - m and vice versa
            var scalingFactor = 1 / 1000000;

            // STL -> IfcFacetedBRep
            var ifcFacetedBRep = model.Instances.New<IfcFacetedBrep>();

            var ifcClosedShell = model.Instances.New<IfcClosedShell>();
            ifcFacetedBRep.Outer = ifcClosedShell;

            foreach (var facet in stlDocument.Facets.ToList())
            {
                // create IfcPolyLoop, which holds the vertex polygon
                var polyloop = model.Instances.New<IfcPolyLoop>();

                // get pts and map them into IfcCartesianPoints
                var pts = facet.Vertices.ToList();

                foreach (var pt in pts)
                {
                    var ifcPt = model.Instances.New<IfcCartesianPoint>(vtx =>
                    {
                        vtx.SetXYZ(pt.X, pt.Y, pt.Z);
                    });
                    // add vertex to polyloop
                    polyloop.Polygon.Add(ifcPt);
                }

                // create IfcFaceOuterBound instance and add the polyloop on it
                var outerBound = model.Instances.New<IfcFaceOuterBound>(ob => { ob.Bound = polyloop; });

                // Add Holes and Openings as IfcFaceBound and append it also to IfcFace


                // add bound to IfcFace
                var face = model.Instances.New<IfcFace>(fc => { fc.Bounds.Add(outerBound); });

                // Add IfcFace instance to IfcClosedShell Instance
                ifcClosedShell.CfsFaces.Add(face);
            }

            return ifcFacetedBRep;
        }

        /// <summary>
        /// Extracts BRep geometry out of a given STLDocument, adds required instances to the IfcModel and returns an fcTriangulatedFaceSet instance to the model
        /// </summary>
        /// <param name="model"></param>
        /// <param name="stlDocument"></param>
        /// <returns></returns>
        public static IfcProductDefinitionShape StlToIfc_TFS(ref IfcStore model, STLDocument stlDocument, double? scaling)
        {
            double scalingFactor = 0.001;
            if (scaling != null)
            {
                scalingFactor = (double)scaling;
            }

            var pointList = model.Instances.New<IfcCartesianPointList3D>();
            // https://github.com/xBimTeam/XbimEssentials/issues/70

            var tfs = model.Instances.New<IfcTriangulatedFaceSet>(fs =>
            {
                fs.Closed = true;
                fs.Coordinates = pointList;
            });

            foreach (var facet in stlDocument.Facets)
            {
                var facetPoints = facet.Vertices.Select(a => new { a.X, a.Y, a.Z }).ToList();

                // ToDo: check, if the point is already stored and get the indices
                var currentEndCoords = pointList.CoordList.Count;
                var currentEndIndex = tfs.CoordIndex.Count;

                var indices = new int[3];
                for (int i = 0; i < facetPoints.Count; i++)
                {
                    pointList.CoordList.GetAt(currentEndCoords + i).AddRange(new IfcLengthMeasure[]
                    {
                        facetPoints[i].X * scalingFactor,
                        facetPoints[i].Y * scalingFactor,
                        facetPoints[i].Z * scalingFactor
                    });

                    indices[i] = currentEndCoords + 1 + i;

                }
                tfs.CoordIndex.GetAt(currentEndIndex).AddRange(indices.Select(j => new IfcPositiveInteger(j)));
            }

            // https://github.com/xBimTeam/XbimEssentials/issues/182 

            var ifcShapeRepresentation = model.Instances.New<IfcShapeRepresentation>();
            var context = model.Instances.OfType<IfcGeometricRepresentationContext>().FirstOrDefault();
            ifcShapeRepresentation.ContextOfItems = context;
            ifcShapeRepresentation.RepresentationIdentifier = "Tesselation";
            ifcShapeRepresentation.RepresentationType = "Tesselation";
            ifcShapeRepresentation.Items.Add(tfs);


            //Erstellt IfcProductDefinitionShape 
            var ifcProductDefinitonShape = model.Instances.New<IfcProductDefinitionShape>();
            ifcProductDefinitonShape.Representations.Add(ifcShapeRepresentation);

            return ifcProductDefinitonShape;
        }
            
        /// <summary>
        /// Maps a given Off-Geometry into an IFC Representation
        /// </summary>
        /// <param name="model"></param>
        /// <param name="offGeometry"></param>
        /// <returns></returns>
        public static IfcProductDefinitionShape OffToIfc_TFS(ref IfcStore model, OffGeometry offGeometry)
        {
            var pointList = model.Instances.New<IfcCartesianPointList3D>();
           
            
            int i = 0;
            // map off geom points into Ifc point list
            foreach (var vertex in offGeometry.Vertices)
            {
                pointList.CoordList.GetAt(i).AddRange(new IfcLengthMeasure[]
                {
                        vertex.X,
                        vertex.Y,
                        vertex.Z
                });
            }
            
            // create TFS
            var tfs = model.Instances.New<IfcTriangulatedFaceSet>(fs =>
            {
                fs.Closed = false;
                fs.Coordinates = pointList;
            });

            // map off faces to IFC
            int j = 0; 
            foreach (var face in offGeometry.Faces)
            {
                // var indices = face.
                var indices = face.VertexIds;
                tfs.CoordIndex.GetAt(j).AddRange(indices.Select(k => new IfcPositiveInteger(k)));
                j++;
            }
            // https://github.com/xBimTeam/XbimEssentials/issues/182 
            // https://github.com/xBimTeam/XbimEssentials/issues/70

            // https://github.com/xBimTeam/XbimEssentials/issues/182 

            var ifcShapeRepresentation = model.Instances.New<IfcShapeRepresentation>();
            var context = model.Instances.OfType<IfcGeometricRepresentationContext>().FirstOrDefault();
            ifcShapeRepresentation.ContextOfItems = context;
            ifcShapeRepresentation.RepresentationIdentifier = "Tessellation";
            ifcShapeRepresentation.RepresentationType = "Tessellation";
            ifcShapeRepresentation.Items.Add(tfs);


            //Erstellt IfcProductDefinitionShape 
            var ifcProductDefinitonShape = model.Instances.New<IfcProductDefinitionShape>();
            ifcProductDefinitonShape.Representations.Add(ifcShapeRepresentation);

            return ifcProductDefinitonShape;
        }
    }
}
