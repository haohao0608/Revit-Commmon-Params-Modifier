/* -------------------------------------------------------------------------------------
 * 
 * Name: ApplicationMain.cs
 * 
 * Author: Zhonghao Lu
 * 
 * Company: University of Alberta
 * 
 * Description: The main entrance of ModifierX project.
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
    using System.Reflection;
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

        #region properties

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

        #endregion

        #region private methods

        /// <summary>
        /// Added Ribbon Panel to Revit UI;
        /// </summary>
        /// <param name="app"></param>
        private void AddRibbonPanel(UIControlledApplication app)
        {
            app.CreateRibbonTab("ModifierX");

            RibbonPanel modifierPanel = app.CreateRibbonPanel("ModifierX", "Modify");
            this.AddButtons(modifierPanel);

        }

        /// <summary>
        /// Added Button to RibbonPanel
        /// </summary>
        /// <param name="ribbonPanel"></param>
        private void AddButtons(RibbonPanel ribbonPanel)
        {
            PushButtonData pushButtonData = new PushButtonData("SelectCategoryCommand", "Select", Assembly.GetExecutingAssembly().Location, "CommonParamsModifier.SelectCategoryCommand");
            PushButton selectCategoryButton = ribbonPanel.AddItem(pushButtonData) as PushButton;
            //Stream stream = this.GetType().Assembly.GetManifestResourceStream("CommonParamsModifier.Resources.IntersectPath_16x_24.bmp");
            //var decoder = new System.Windows.Media.Imaging.
            selectCategoryButton.LargeImage = System.Windows.Interop.Imaging.
                                            CreateBitmapSourceFromHBitmap(CommonParamsModifier.Properties.Resources.IntersectPath_16x_24.GetHbitmap(),
                                                                                                          IntPtr.Zero, 
                                                                                                          Int32Rect.Empty, 
                                                                                                          BitmapSizeOptions.FromEmptyOptions());
            selectCategoryButton.ToolTip = "Select Categories";
        }

        #endregion
    }
}
