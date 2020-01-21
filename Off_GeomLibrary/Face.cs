using System.Collections.Generic;

namespace Off_GeomLibrary
{
    internal class Face
    {
        int NumVertices { get; set; }
        List<int> VertexIds { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="numVertices"></param>
        /// <param name="vertexIds"></param>
        public Face(int numVertices, List<int> vertexIds)
        {
            this.NumVertices = numVertices;
            this.VertexIds = vertexIds;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="numVertices"></param>
        public Face(int numVertices)
        {
            this.NumVertices = numVertices;
        }
    }
}