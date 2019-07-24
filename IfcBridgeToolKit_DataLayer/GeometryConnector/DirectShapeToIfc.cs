using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace IfcBridgeToolKit_DataLayer.GeometryConnector
{
    /// <summary>
    /// 
    /// </summary>
    public class DirectShapeToIfc
    {
        // private Placement Einfügepunkt; 
        public EuclidianPlacement location { get; set; }

        // geometry
        public List<Facet> Facets { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public DirectShapeToIfc()
        {
            Facets = new List<Facet>();
            location = new EuclidianPlacement();
        }

        #region methods

        /// <summary>
        /// Serializes the given transporter instance into a JSON to check the content
        /// </summary>
        /// <param name="path">FilePath to text file</param>
        public void SerializeToJson(string path)
        {
            using (StreamWriter file = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, this);
            }
        }

        #endregion
    }
}
