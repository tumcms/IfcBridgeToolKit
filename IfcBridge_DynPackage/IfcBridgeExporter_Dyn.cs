using System;
using IfcBridgeToolKit;

namespace IfcBridge_DynPackage
{
    public class IfcBridgeExporter_Dyn
    {
        /// <summary>
        /// Defines base content in IFC Model -> calls initModel
        /// </summary>
        /// <param name="filepath"></param>
        public static bool InitIfcModel(string projectName)
        {
            try
            {
                var modelCreator = new CreateAndInitModel();
                var model = modelCreator.CreateModel(projectName);
                model.SaveAs(@"C:\Users\Sebastian Esser\Desktop\TestModel001.ifc"); // dont do it that way...
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return true; // everything went well - otherwise receive a false
        }
    }
}
