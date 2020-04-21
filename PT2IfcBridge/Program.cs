using System;
using System.Globalization;
using System.Linq;
using IfcBridgeToolKit;
using Off_GeomLibrary;
using Xbim.IfcRail.GeometricConstraintResource;
using Xbim.IfcRail.GeometryResource;
using Xbim.IfcRail.ProductExtension;

namespace PT2IfcBridge
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(" --- PointCloud 2 IfcBridge Model --- \n");


            // --- Model Setup ---

            // create new Ifc4x2 Model
            var toolkit = new ModelSetupService();

            var model = toolkit.CreateModel("PT2IFC_Prototype", "Esser", "Sebastian");
            
            // create Site
            toolkit.CreateIfcSite(ref model, "SampleSite");

            // create bridge
            toolkit.CreateIfcBridgeEntity(ref model, "PTBridge", "ToolChain PT > Enrichment > IFC4x2 Model");

            // create Bridge Parts
            toolkit.CreateIfcBridgePartEntities(ref model);

            // get all files in the geometry folder
            var path = "geometryFiles/";
            var files = System.IO.Directory.GetFiles(path, "*.off").ToList();

            // init product service
            var productService = new ProductService();

            foreach (var file in files)
            {
                Console.WriteLine("add new product: " + file);
                // load geometry
                var offGeom = new OffGeometry(file);
                
                // add product to model
                productService.AddBuildingElement(ref model, offGeom, file, "IfcBuildingElementProxy", "local", "Superstructure");
            }
            
            Console.WriteLine("Save Model... \n");

            // set time stamp in file name
            var date = DateTime.Now;
            var dateStr = date.ToString("yy-MM-dd", CultureInfo.CreateSpecificCulture("en-US")); 
            var timeStr = date.ToString("hh-mm", CultureInfo.CreateSpecificCulture("de-DE"));
            var fileName = dateStr + "_" + timeStr + "_" + "PT2IFC_bridge_v01.ifc";
            Console.WriteLine("Filename is: " + fileName); 

            // save model
            model.SaveAs(fileName);

            // modify header
            toolkit.ModifyHeader(fileName);

            Console.WriteLine("Finished. Press button to exit. \n");

            var input = Console.ReadKey();
        }
    }
}
