/* -------------------------------------------------------------------------------------
 * 
 * Name: ModifierXForm.cs
 * 
 * Author: Zhonghao Lu
 * 
 * Company: University of Alberta
 * 
 * Description: Main form for selecting categories,  filtering, and modify corresponding parameters.
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
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// Main form for selecting categories,  filtering, and modify corresponding parameters
    /// </summary>
    [Transaction(TransactionMode.Manual)]
    public partial class ModifierXForm : System.Windows.Forms.Form
    {
        #region variables

        private UIDocument uiDoc = null;
        private Document doc = null;
        private Parameter chosenPara = null;
        private ModifierXEventHandler modifierXEventHandler = null;
        private ManualResetEvent manualResetEvent = null;

        private List<Category> categories = new List<Category>();
        private List<int> catIds = new List<int>();
        private List<Element> allElements = new List<Element>();
        private List<Category> selectedCats = new List<Category>();
        private List<Element> selectedEles = new List<Element>();
        private List<string> commonParamsDefNames = new List<string>();

        #endregion

        #region public methods

        /// <summary>
        /// Constructor of this class, Initialize a new instance of the Form Class
        /// </summary>
        /// <param name="exCmdData"></param>
        /// <param name="modifierXEventHandler"></param>
        public ModifierXForm(ExternalCommandData exCmdData, ModifierXEventHandler modifierXEventHandler)
        {
            InitializeComponent();
            this.uiDoc = exCmdData.Application.ActiveUIDocument;
            this.doc = uiDoc.Document;
            this.modifierXEventHandler = modifierXEventHandler;

            InitCategories();
            UpdateCheckedListBox();
            SetElementHost();
            SetFilterPanel();    
        }
        
        /// <summary>
        /// An Event method which trying to modify the doc by transcations and raised when the user click modify button
        /// and chosen parameter storage type is String.
        /// </summary>
        /// <param name="uiApp"></param>
        /// <param name="args"></param>
        public void StorageTypeStringModifierEvent(UIApplication uiApp, object args)
        {
            try
            {
                Transaction t = new Transaction(doc, "Modifying Attributes");
                t.Start();
                foreach (Element element in selectedEles)
                {
                    element.GetParameters(parametersComboBox.Text)[0].Set(modifyTextBox.Text);
                }
                t.Commit();
            }
            catch(Exception exp)
            {
                Trace.WriteLine(exp);
                MessageBox.Show("ModifierX cannot finish the transacation.");
            }
            this.BringToFront();
        }

        /// <summary>
        /// An Event method which trying to modify the doc by transcations and raised when the user click modify button
        /// and chosen parameter storage type is Double.
        /// </summary>
        /// <param name="uiApp"></param>
        /// <param name="args"></param>
        public void StorageTypeDoubleModifierEvent(UIApplication uiApp, object args)
        {
            double tempDouble;
            if (double.TryParse(modifyTextBox.Text, out tempDouble))
            {
                try
                {
                    tempDouble = Autodesk.Revit.DB.UnitUtils.Convert(tempDouble, DisplayUnitType.DUT_MILLIMETERS, DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES);
                    Transaction t = new Transaction(doc, "Modifying Attributes");
                    t.Start();
                    foreach (Element element in selectedEles)
                    {
                        element.GetParameters(parametersComboBox.Text)[0].Set(tempDouble);
                    }
                    var transcation_CommitStatus = t.Commit();
                    if (transcation_CommitStatus != TransactionStatus.Committed)
                    {
                        this.Dispose();
                    }
                }
                catch (Exception exp)
                {
                    Trace.WriteLine(exp);
                    MessageBox.Show("ModifierX cannot finish the transacation.");
                }
            }
            else
            {
                MessageBox.Show("Invalid input for this parameter.");
            }
            this.BringToFront();
        }

        #endregion

        #region private methods

        /// <summary>
        /// Get the all the visible categories in the doc.
        /// </summary>
        private void InitCategories()
        {
            allElements = getAllElements();
            foreach (Element elem in allElements)
            {
                if (!categories.Contains(elem.Category))
                {
                    categories.Add(elem.Category);
                }
            }
        }

        /// <summary>
        /// Initialize FilterPanel;
        /// </summary>
        private void SetFilterPanel()
        {
            string combobox1InitStr = "--Select--";
            parametersComboBox.Items.Add(combobox1InitStr);
            parametersComboBox.SelectedItem = combobox1InitStr;
        }

        /// <summary>
        /// Initialize the categoryCheckedListBox;
        /// </summary>
        private void UpdateCheckedListBox()
        {
            foreach (Category category in categories)
            {
                catIds.Add(category.Id.IntegerValue);
            }
            catIds = catIds.Distinct().ToList();
            foreach (int catId in catIds)
            {
                if (categories.Exists(x => x.Id.IntegerValue.Equals(catId))){
                    Category category = categories.Find(x => x.Id.IntegerValue.Equals(catId));
                    categoryCheckedListBox.Items.Add(category.Name);
                }
            }
        }

        /// <summary>
        /// get all visible elements from the doc. 
        /// </summary>
        /// <returns>
        /// return all visible elements.
        /// </returns>
        private List<Element> getAllElements()
        {
            List<Element> elements = new List<Element>();

            FilteredElementCollector collector
              = new FilteredElementCollector(doc)
                .WhereElementIsNotElementType();

            foreach (Element e in collector)
            {
                if (null != e.Category
                  && e.Category.HasMaterialQuantities)
                {
                    elements.Add(e);
                }
            }
            return elements;
        }

        /// <summary>
        /// An event uupdate the parametersDropBox, view and common parameters. 
        /// Raised when CategoryCheckedListBox CheckedItems Changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckedListBoxCheckedItemsChanged(object sender, EventArgs e)
        {
            selectedCats.Clear();
            selectedEles.Clear();
            for (int i=0; i < categoryCheckedListBox.CheckedItems.Count; i++)
            {
                Category category = categories.Find(x => x.Name.Equals(categoryCheckedListBox.CheckedItems[i].ToString()));
                if (category!= null)
                {
                    selectedCats.Add(category);
                }    
            }
            highLight();
            updateCommonParamsDefNames();
            updateComboBox();
        }

        /// <summary>
        /// Initialize the ViewElementHost for a view of model; 
        /// </summary>
        private void SetElementHost()
        {
            ViewElementHost.Child = new PreviewControl(doc, doc.ActiveView.Id);
        }

        /// <summary>
        /// Highlight Elemends in the doc;
        /// </summary>
        private void highLight()
        {
            foreach (Category category in selectedCats)
            {
                selectedEles.AddRange(allElements.FindAll(x => x.Category.Id.IntegerValue.Equals(category.Id.IntegerValue)));
            }
            List<ElementId> elementIds = selectedEles.Select(o => o.Id).ToList();
            uiDoc.Selection.SetElementIds(elementIds);
            uiDoc.RefreshActiveView();
        }

        /// <summary>
        /// update common parameters of selected elements;
        /// </summary>
        private void updateCommonParamsDefNames()
        {
            commonParamsDefNames.Clear();

            if (selectedEles.Count == 0) { return; }

            commonParamsDefNames = Util.RawConvertSetToList<Parameter>(selectedEles[0].Parameters).Select(x=>x.Definition.Name).ToList();
            
            foreach(Element elem in selectedEles)
            {
                List<string> tempParamsNames = Util.RawConvertSetToList<Parameter>(elem.Parameters).Select(x => x.Definition.Name).ToList();
                commonParamsDefNames = commonParamsDefNames.Intersect(tempParamsNames).ToList();
            }
            commonParamsDefNames.Sort();
        }

        /// <summary>
        /// update ComboBox when common parameters changed.
        /// </summary>
        private void updateComboBox()
        {
            parametersComboBox.Items.Clear();
            string select = "--Select--";
            parametersComboBox.Items.Add(select);
            parametersComboBox.SelectedItem = select;

            if (selectedEles.Count == 0) { return; }

            parametersComboBox.Items.AddRange(commonParamsDefNames.ToArray());
        }

        /// <summary>
        /// An event that refresh the filterPanel based on the parameter storage type, 
        /// raised when the user chooses a new parameter
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void parametersComboBoxTextChanged(object sender, EventArgs e)
        {
            if (parametersComboBox.SelectedItem == null) { return; }
            
            string chosenStr = parametersComboBox.Text;
            if (chosenStr != "--Select--")
            {
                selectParameterConfirmButton.Visible = true;
                chosenPara = selectedEles[0].GetParameters(chosenStr)[0];
                if (chosenPara.StorageType == Autodesk.Revit.DB.StorageType.Double)
                {
                    filterContainsLabel.Visible = stringFilterTextBox.Visible = false;
                    morethanComboBox.Visible = lessthanComboBox.Visible = morethanTextBox.Visible = lessthanTextBox.Visible =
                        morethanUnit.Visible = lessthanUnit.Visible= true;
                    morethanComboBox.SelectedIndex = 0;
                    lessthanComboBox.SelectedIndex = 0;
                    selectParameterConfirmButton.Location = new System.Drawing.Point(selectParameterConfirmButton.Location.X, lessthanTextBox.Location.Y + lessthanTextBox.Size.Height + 6);
                }
                else if (chosenPara.StorageType == Autodesk.Revit.DB.StorageType.String)
                {
                    stringFilterTextBox.Visible = filterContainsLabel.Visible = true;
                    morethanComboBox.Visible = lessthanComboBox.Visible = morethanTextBox.Visible = lessthanTextBox.Visible =
                        morethanUnit.Visible = lessthanUnit.Visible = false;
                    selectParameterConfirmButton.Location = new System.Drawing.Point(selectParameterConfirmButton.Location.X, stringFilterTextBox.Location.Y + stringFilterTextBox.Size.Height + 6);
                }
                else
                {
                    filterContainsLabel.Visible = stringFilterTextBox.Visible = false;
                    morethanComboBox.Visible = lessthanComboBox.Visible = morethanTextBox.Visible = lessthanTextBox.Visible =
                        morethanUnit.Visible = lessthanUnit.Visible = false;
                    selectParameterConfirmButton.Location = new System.Drawing.Point(selectParameterConfirmButton.Location.X, StorageTypeLabel.Location.Y + StorageTypeLabel.Size.Height + 6);
                }
                StorageTypeLabel.Visible = true;
                StorageTypeLabel.Text = "Storage Type: "+chosenPara.StorageType.ToString();
            }
            else
            {
                chosenPara = null;
                filterContainsLabel.Visible = stringFilterTextBox.Visible = false;
                morethanComboBox.Visible = lessthanComboBox.Visible = morethanTextBox.Visible = lessthanTextBox.Visible =
                    morethanUnit.Visible = lessthanUnit.Visible = false;
                selectParameterConfirmButton.Visible = false;
                StorageTypeLabel.Visible = false;
            }
        }

        /// <summary>
        /// An click event of confirmButton that trys to filter the element in the doc based on the condition 
        /// specified by user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void confirmButton_Click(object sender, EventArgs e)
        {
            List<Element> FilteredElement = new List<Element>();

            foreach (Category category in selectedCats)
            {
                selectedEles.AddRange(allElements.FindAll(x => x.Category.Id.IntegerValue.Equals(category.Id.IntegerValue)));
            }

            if (chosenPara.StorageType == Autodesk.Revit.DB.StorageType.Double)
            {
                if ((morethanTextBox.Text.Equals("")) && (lessthanTextBox.Text.Equals("")))
                {
                    MessageBox.Show("Empty Input", "ModifierX");
                    return;
                }
                double more, less;
                List<Element> FilteredElementMoreThan = new List<Element>();
                if (morethanTextBox.Text != "")
                {
                    if (!(double.TryParse(morethanTextBox.Text, out more)))
                    {
                        MessageBox.Show("Invalid Input", "ModifierX");
                        return;
                    }
                    if (morethanComboBox.SelectedIndex == 0)
                    {
                        foreach (Element element in selectedEles)
                        {
                            double value = element.GetParameters(parametersComboBox.Text)[0].AsDouble();
                            value = Autodesk.Revit.DB.UnitUtils.Convert(value, DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES, DisplayUnitType.DUT_MILLIMETERS);
                            if (value > more)
                            {
                                FilteredElementMoreThan.Add(element);
                            }
                        }
                    }
                    else if (morethanComboBox.SelectedIndex == 1)
                    {
                        foreach (Element element in selectedEles)
                        {
                            double value = element.GetParameters(parametersComboBox.Text)[0].AsDouble();
                            value = Autodesk.Revit.DB.UnitUtils.Convert(value, DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES, DisplayUnitType.DUT_MILLIMETERS);
                            if (value >= more)
                            {
                                FilteredElementMoreThan.Add(element);
                            }
                        }
                    }
                }
                else
                {
                    FilteredElementMoreThan = selectedEles;
                }

                if (!(lessthanTextBox.Text.Equals("")))
                {
                    if (!(double.TryParse(lessthanTextBox.Text, out less)))
                    {
                        MessageBox.Show("Invalid Input", "ModifierX");
                        return;
                    }
                    if (lessthanComboBox.SelectedIndex == 0)
                    {
                        foreach (Element element in FilteredElementMoreThan)
                        {
                            double value = element.GetParameters(parametersComboBox.Text)[0].AsDouble();
                            value = Autodesk.Revit.DB.UnitUtils.Convert(value, DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES, DisplayUnitType.DUT_MILLIMETERS);
                            if (value < less)
                            {
                                FilteredElement.Add(element);
                            }
                        }
                    }
                    else if (lessthanComboBox.SelectedIndex == 1)
                    {
                        foreach (Element element in FilteredElementMoreThan)
                        {
                            double value = element.GetParameters(parametersComboBox.Text)[0].AsDouble();
                            value = Autodesk.Revit.DB.UnitUtils.Convert(value, DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES, DisplayUnitType.DUT_MILLIMETERS);
                            if (value <= less)
                            {
                                FilteredElement.Add(element);
                            }
                        }
                    }
                }
                else
                {
                    FilteredElement = FilteredElementMoreThan;
                }
            }
            else if(chosenPara.StorageType == Autodesk.Revit.DB.StorageType.String)
            {
                foreach(Element element in selectedEles)
                {
                    string tempStr = element.GetParameters(parametersComboBox.Text)[0].AsString();
                    if (tempStr == null) { continue; }
                    if (element.GetParameters(parametersComboBox.Text)[0].AsString().Contains(stringFilterTextBox.Text))
                    {
                        FilteredElement.Add(element);
                    }
                }

            }
            else
            {
                MessageBox.Show("Selected parameter is not supported.", "ModifierX");
                return;
            }
            
            selectedEles.Clear();
            selectedEles.AddRange(FilteredElement);
            List<ElementId> elementIds = selectedEles.Select(o => o.Id).ToList();
            uiDoc.Selection.SetElementIds(elementIds);
            uiDoc.RefreshActiveView();
        }

        /// <summary>
        /// An click event of modifyButton try to call modifacation functions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modifyButton_Click(object sender, EventArgs e)
        {
            if (chosenPara == null) { return;  }
            this.manualResetEvent = new ManualResetEvent(false);
            if (chosenPara.StorageType == Autodesk.Revit.DB.StorageType.Double)
            {
                this.modifierXEventHandler.SetActionAndRaise(this.StorageTypeDoubleModifierEvent, this.manualResetEvent);
            }else if(chosenPara.StorageType == Autodesk.Revit.DB.StorageType.String)
            {
                this.modifierXEventHandler.SetActionAndRaise(this.StorageTypeStringModifierEvent, this.manualResetEvent);
            }
            else
            {
                MessageBox.Show("Selected parameter is not supported.", "ModifierX");
                return;
            }
        }

        #endregion
    }
}
