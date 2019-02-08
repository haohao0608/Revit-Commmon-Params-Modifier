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

namespace CommonParamsModifier
{
    [Transaction(TransactionMode.Manual)]
    public partial class SelectCategoryForm : System.Windows.Forms.Form
    {
        private UIDocument uiDoc;
        private Document doc;
        private List<Category> categories = new List<Category>();
        private List<int> catIds = new List<int>();
        private List<Element> allElements = new List<Element>();
        private List<Category> selectedCats = new List<Category>();
        private List<Element> selectedEles = new List<Element>();
        private List<string> commonParamsDefNames = new List<string>();
        private Parameter chosenPara;
        private ModifierXEventHandler modifierXEventHandler;

        private ManualResetEvent manualResetEvent; 

        public SelectCategoryForm(ExternalCommandData exCmdData, ModifierXEventHandler modifierXEventHandler)
        {
            InitializeComponent();
            this.uiDoc = exCmdData.Application.ActiveUIDocument;
            this.doc = uiDoc.Document;
            this.modifierXEventHandler = modifierXEventHandler;

            InitCategories();
            UpdateCheckedListBox();
            SetElementHost();
            SetPanel1();
            
        }

        private void SetPanel1()
        {
            string combobox1InitStr = "--Select--";
            comboBox1.Items.Add(combobox1InitStr);
            comboBox1.SelectedItem = combobox1InitStr;

        }

        public void StorageTypeStringModifierEvent(UIApplication uiApp, object args)
        {
            try
            {
                Transaction t = new Transaction(doc, "Modifying Attributes");
                t.Start();
                foreach (Element element in selectedEles)
                {
                    element.GetParameters(comboBox1.Text)[0].Set(textBox4.Text);
                }
                t.Commit();
            }
            catch(Exception exp)
            {
                Trace.WriteLine(exp);
                MessageBox.Show("ModifierX cannot finish the transacation.");
            }
            
            
        }

        public void StorageTypeDoubleModifierEvent(UIApplication uiApp, object args)
        {
            double tempDouble;
            if (double.TryParse(textBox4.Text, out tempDouble))
            {
                try
                {
                    tempDouble = Autodesk.Revit.DB.UnitUtils.Convert(tempDouble, DisplayUnitType.DUT_MILLIMETERS, DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES);
                    Transaction t = new Transaction(doc, "Modifying Attributes");
                    t.Start();
                    foreach (Element element in selectedEles)
                    {
                        element.GetParameters(comboBox1.Text)[0].Set(tempDouble);
                    }
                    t.Commit();
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
            
        }

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
                    checkedListBox1.Items.Add(category.Name);
                }
            }
        }

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

        private void CheckedListBoxCheckedItemsChanged(object sender, EventArgs e)
        {
            selectedCats.Clear();
            selectedEles.Clear();
            for (int i=0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                Category category = categories.Find(x => x.Name.Equals(checkedListBox1.CheckedItems[i].ToString()));
                if (category!= null)
                {
                    selectedCats.Add(category);
                }    
            }
            highLight();
            updateCommonParamsDefNames();
            updateComboBox();
        }

        private void SetElementHost()
        {
            elementHost1.Child = new PreviewControl(doc, doc.ActiveView.Id);
        }

        private void testCheck(object sender, EventArgs e)
        {
            MessageBox.Show("There you go");
        }

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

        private void updateComboBox()
        {
            comboBox1.Items.Clear();
            string select = "--Select--";
            comboBox1.Items.Add(select);
            comboBox1.SelectedItem = select;

            if (selectedEles.Count == 0) { return; }

            comboBox1.Items.AddRange(commonParamsDefNames.ToArray());
        }
        
        private void comboBox1TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null) { return; }
            
            string chosenStr = comboBox1.Text;
            if (chosenStr != "--Select--")
            {
                button4.Visible = true;
                chosenPara = selectedEles[0].GetParameters(chosenStr)[0];
                if (chosenPara.StorageType == StorageType.Double)
                {
                    comboBox4.Visible = label4.Visible = textBox3.Visible = false;
                    comboBox2.Visible = comboBox3.Visible = textBox1.Visible = textBox2.Visible = true;
                    comboBox2.SelectedIndex = 0;
                    comboBox3.SelectedIndex = 0;
                    button4.Location = new System.Drawing.Point(button4.Location.X, textBox2.Location.Y + textBox2.Size.Height + 6);
                }
                else if (chosenPara.StorageType == StorageType.String)
                {
                    textBox3.Visible = label4.Visible = true;
                    comboBox4.Visible = false;
                    comboBox2.Visible = comboBox3.Visible = textBox1.Visible = textBox2.Visible = false;
                    button4.Location = new System.Drawing.Point(button4.Location.X, textBox3.Location.Y + textBox3.Size.Height + 6);
                }
                else
                {
                    comboBox4.Visible = label4.Visible = textBox3.Visible = false;
                    comboBox2.Visible = comboBox3.Visible = textBox1.Visible = textBox2.Visible = false;
                    button4.Location = new System.Drawing.Point(button4.Location.X, comboBox1.Location.Y + comboBox1.Size.Height + 6);
                }
            }
            else
            {
                chosenPara = null;
                comboBox4.Visible = label4.Visible = textBox3.Visible = false;
                comboBox2.Visible = comboBox3.Visible = textBox1.Visible = textBox2.Visible = false;
                button4.Visible = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            List<Element> FilteredElement = new List<Element>();

            foreach (Category category in selectedCats)
            {
                selectedEles.AddRange(allElements.FindAll(x => x.Category.Id.IntegerValue.Equals(category.Id.IntegerValue)));
            }

            if (chosenPara.StorageType == StorageType.Double)
            {
                double more, less;
                if (!(double.TryParse(textBox1.Text, out more) && double.TryParse(textBox2.Text, out less))) {
                    MessageBox.Show("Invalid input.", "ModifierX");
                    return;
                }

                if (comboBox2.SelectedIndex == 0 && comboBox3.SelectedIndex == 0)
                {
                    foreach (Element element in selectedEles)
                    {
                        double value = element.GetParameters(comboBox1.Text)[0].AsDouble();
                        value = Autodesk.Revit.DB.UnitUtils.Convert(value, DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES, DisplayUnitType.DUT_MILLIMETERS);
                        if ((value > more) &&(value < less))
                        {
                            FilteredElement.Add(element);
                        }
                    }
                }
                else if(comboBox2.SelectedIndex == 1 && comboBox3.SelectedIndex == 0)
                {
                    foreach (Element element in selectedEles)
                    {
                        double value = element.GetParameters(comboBox1.Text)[0].AsDouble();
                        value = Autodesk.Revit.DB.UnitUtils.Convert(value, DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES, DisplayUnitType.DUT_MILLIMETERS);
                        if ((value >= more) && (value < less))
                        {
                            FilteredElement.Add(element);
                        }
                    }
                }
                else if (comboBox2.SelectedIndex == 0 && comboBox3.SelectedIndex == 1)
                {
                    foreach (Element element in selectedEles)
                    {
                        double value = element.GetParameters(comboBox1.Text)[0].AsDouble();
                        value = Autodesk.Revit.DB.UnitUtils.Convert(value, DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES, DisplayUnitType.DUT_MILLIMETERS);
                        if ((value > more) && (value <= less))
                        {
                            FilteredElement.Add(element);
                        }
                    }
                }
                else if (comboBox2.SelectedIndex == 1 && comboBox3.SelectedIndex == 1)
                {
                    foreach (Element element in selectedEles)
                    {
                        double value = element.GetParameters(comboBox1.Text)[0].AsDouble();
                        value = Autodesk.Revit.DB.UnitUtils.Convert(value, DisplayUnitType.DUT_FEET_FRACTIONAL_INCHES, DisplayUnitType.DUT_MILLIMETERS);
                        if ((value >= more) && (value <= less))
                        {
                            FilteredElement.Add(element);
                        }
                    }
                }
            }
            else if(chosenPara.StorageType == StorageType.String)
            {
                foreach(Element element in selectedEles)
                {
                    string tempStr = element.GetParameters(comboBox1.Text)[0].AsString();
                    if (tempStr == null) { continue; }
                    if (element.GetParameters(comboBox1.Text)[0].AsString().Contains(textBox3.Text))
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (chosenPara == null) { return;  }
            this.manualResetEvent = new ManualResetEvent(false);
            if (chosenPara.StorageType == StorageType.Double)
            {
                this.modifierXEventHandler.SetActionAndRaise(this.StorageTypeDoubleModifierEvent, this.manualResetEvent);
            }else if(chosenPara.StorageType == StorageType.String)
            {
                this.modifierXEventHandler.SetActionAndRaise(this.StorageTypeStringModifierEvent, this.manualResetEvent);
            }
            else
            {
                MessageBox.Show("Selected parameter is not supported.", "ModifierX");
                return;
            }
        }
    }
}
