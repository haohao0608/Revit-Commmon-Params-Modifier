/* -------------------------------------------------------------------------------------
 * 
 * Name: CommonParamsModifier.cs
 * 
 * Author: Zhonghao Lu
 * 
 * Company: University of Alberta
 * 
 * Description: A class to execute functions with Revit API context when we otherwise wouldn't have it.
 * 
 * Copyright © University of Alberta 2016
 * 
 * -------------------------------------------------------------------------------------
 */

namespace CommonParamsModifier
{
    #region namespaces

    using Autodesk.Revit.UI;
    using System;
    using System.Windows;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Interop;
    using System.Windows.Media.Imaging;

    #endregion
    public class ApplicationMain : IExternalApplication
    {
        #region Fields

        private static UIControlledApplication uiCtrApp;
        private static ApplicationMain activeApp;
        public static ModifierXEventHandler modifierXEventHandler;
        public static ExternalEvent externalEvent;

        #endregion

        public static UIControlledApplication UICtrApp
        {
            get { return ApplicationMain.uiCtrApp; }
        }

        public static ApplicationMain ActiveApp
        {
            get { return ApplicationMain.activeApp; }
        }

        public static ExternalEvent ExternalEvent
        {
            get { return ApplicationMain.externalEvent; }
            set { ApplicationMain.externalEvent = value; }
        }

        public static ModifierXEventHandler ModifierXEventHandler
        {
            get { return ApplicationMain.modifierXEventHandler; }
            set { ApplicationMain.modifierXEventHandler = value; }
        }

        public Result OnShutdown(UIControlledApplication app)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication app)
        {
            ApplicationMain.uiCtrApp = app;
            ApplicationMain.activeApp = this;
            this.AddRibbonPanel(app);

            return Result.Succeeded;
        }

        private void AddRibbonPanel(UIControlledApplication app)
        {
            app.CreateRibbonTab("ModifierX");

            RibbonPanel modifierPanel = app.CreateRibbonPanel("ModifierX", "Modify");
            this.AddButtons(modifierPanel);

        }

        private void AddButtons(RibbonPanel ribbonPanel)
        {
            PushButtonData pushButtonData = new PushButtonData("SelectCategoryCommand", "Select", Assembly.GetExecutingAssembly().Location, "CommonParamsModifier.SelectCategoryCommand");
            PushButton selectCategoryButton = ribbonPanel.AddItem(pushButtonData) as PushButton;
            //Stream stream = this.GetType().Assembly.GetManifestResourceStream("CommonParamsModifier.Resources.IntersectPath_16x_24.bmp");
            //var decoder = new System.Windows.Media.Imaging.
            selectCategoryButton.LargeImage = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(CommonParamsModifier.Properties.Resources.IntersectPath_16x_24.GetHbitmap(),
                                                                                                            IntPtr.Zero, 
                                                                                                            Int32Rect.Empty, 
                                                                                                                BitmapSizeOptions.FromEmptyOptions());
            selectCategoryButton.ToolTip = "Select Categories";
    
        }
    }
}
