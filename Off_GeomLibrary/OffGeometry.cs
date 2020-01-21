using System;
using System.Collections.Generic;
using System.IO;

namespace Off_GeomLibrary
{
    public class OffGeometry
    {
        public List<Point3D> Points { get; set; }

        List<Face> Faces { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public OffGeometry()
        { 
            Points = new List<Point3D>();
            Faces = new List<Face>(); 
        }

        /// <summary>
        /// Constructor with load
        /// </summary>
        public OffGeometry(string offFile)
        {
            Points = new List<Point3D>();
            Faces = new List<Face>();

            string[] rawTxt = File.ReadAllLines(offFile);

            



        }

    }
}
