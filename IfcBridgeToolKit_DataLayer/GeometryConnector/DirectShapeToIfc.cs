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
        }

        #region methods

        /// <summary>
        /// Serializes the given transporter instance into a JSON to check the content
        /// </summary>
        /// <param name="path">FilePath to text file</param>
        public void SerializeToJson(string path)
        {
            // serialize transporter item into a JSON
            string json = JsonConvert.SerializeObject(this);
            using (StreamWriter myFile = new StreamWriter(path))
            {
                myFile.WriteLine(JsonConvert.SerializeObject(json));
            }
        }

        #endregion
    }
}
