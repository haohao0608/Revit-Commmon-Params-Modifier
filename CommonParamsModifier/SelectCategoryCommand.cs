using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonParamsModifier
{
    [Transaction(TransactionMode.Manual)]
    class SelectCategoryCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                createForm(commandData);
                return Result.Succeeded;
            }
            catch(Exception exp)
            {
                Trace.WriteLine(exp.ToString());
                return Result.Failed;
            }
        }

        private void createForm(ExternalCommandData exCmdData)
        {
            SelectCategoryForm form = new SelectCategoryForm(exCmdData);
            form.Show();
        }
    }
}
