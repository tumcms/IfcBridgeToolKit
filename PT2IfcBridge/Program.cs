using System;
using IfcBridgeToolKit;


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

            // load geometry



            Console.WriteLine("Save Model... \n");

            // set time stamp in file name
            var date = DateTime.Now;
            var dateStr = date.ToString("yy-mm-dd"); 
            var timeStr = date.ToString("hh-mm");
            var fileName = dateStr + "_" + timeStr + "_" + "PT2IFC_bridge_v01.ifc";
            Console.WriteLine("Filename is: " + fileName); 

            // save model
            model.SaveAs(fileName);

            Console.WriteLine("Done.");

            var input = Console.ReadKey();

        }
    }
}
