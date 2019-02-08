/* -------------------------------------------------------------------------------------
 * 
 * Name: SelectCategoryCommand.cs
 * 
 * Author: Zhonghao Lu
 * 
 * Company: University of Alberta
 * 
 * Description: A Class that implement IExternalCommand interface, called when pushButton 
 * 
 *              is clicked, it will create a new modifierX form.
 * 
 * Copyright © University of Alberta 2016
 * 
 * -------------------------------------------------------------------------------------
 */
namespace CommonParamsModifier
{
    #region namespaces

    using Autodesk.Revit.Attributes;
    using Autodesk.Revit.DB;
    using Autodesk.Revit.UI;
    using System;
    using System.Diagnostics;
    [Transaction(TransactionMode.Manual)]

    #endregion

    class SelectCategoryCommand : IExternalCommand
    {
        /// <summary>
        /// IExternalCommand interface method
        /// </summary>
        /// <param name="commandData"></param>
        /// <param name="message"></param>
        /// <param name="elements"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Create Modifier form when pushButton is clicked.
        /// </summary>
        /// <param name="exCmdData"></param>
        private void createForm(ExternalCommandData exCmdData)
        {
            ApplicationMain.modifierXEventHandler = new ModifierXEventHandler();
            ApplicationMain.externalEvent = ExternalEvent.Create(ApplicationMain.modifierXEventHandler);
            ApplicationMain.modifierXEventHandler.ModifierXEvent = ApplicationMain.externalEvent;

            ModifierXForm form = new ModifierXForm(exCmdData, ApplicationMain.modifierXEventHandler);
            form.Show();
        }
    }
}
