using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CommonParamsModifier
{
    public class ModifierXEventHandler : IExternalEventHandler
    {

        #region Fields
        private ExternalEvent modifierXEvent;
        private Tuple<Action<UIApplication, object>, object> actionToExecute;
        private ManualResetEvent resetEvent;
        private bool executing;
        #endregion

        #region Properties

        public ExternalEvent ModifierXEvent
        {
            get { return this.modifierXEvent; }
            set { this.modifierXEvent = value; }
        }

        public Tuple<Action<UIApplication, object>, object> ActiontoExecute
        {
            get { return this.actionToExecute; }
            set { this.actionToExecute = value; }
        }

        public ManualResetEvent ResetEvent
        {
            get { return this.resetEvent; }
            set { this.resetEvent = value;  }
        }

        public bool Executing
        {
            get { return this.executing; }
            set { this.executing = value; }
        }

        #endregion

        #region Public Methods
        public void Execute(UIApplication app)
        {
            try
            {
                if (this.actionToExecute != null)
                {
                    this.executing = true;
                    this.actionToExecute.Item1.Invoke(app, this.actionToExecute.Item2);
                }
            }
            catch(Exception ex)
            {
                Trace.WriteLine(ex);
            }
            finally
            {
                this.executing = false;
                this.actionToExecute = null;

                this.resetEvent.Set();
            }
        }

        public void SetActionAndRaise(Action<UIApplication, object> actionToExecute, ManualResetEvent resetEvent, object args = null )
        {
            this.ActiontoExecute = new Tuple<Action<UIApplication, object>, object>(actionToExecute, args);
            this.ResetEvent = resetEvent;

            //SetForegroundWindow(Autodesk.Windows.ComponentManager.ApplicationWindow);

            this.modifierXEvent.Raise();

            // Try to force immediate execution of event by "jiggling" the mouse.
            // Adapted from Jo Ye, ACE DevBlog.
            // http://adndevblog.typepad.com/aec/2013/07/tricks-to-force-trigger-idling-event.html
            Cursor.Position = new Point(Cursor.Position.X + 1, Cursor.Position.Y + 1);
            Cursor.Position = new Point(Cursor.Position.X - 1, Cursor.Position.Y - 1);
        }

        public string GetName()
        {
            return "ModifierXEventHandler";
        }
        #endregion

        #region External Methods

        // Revit application window must be focused window for event to execute.
        // Use external Windows user32 method to set foreground window.
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);
        #endregion
    }
}
