﻿using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CommonParamsModifier
{
    public partial class SelectCategoryForm : System.Windows.Forms.Form
    {
        private UIDocument uiDoc;
        private Document doc;
        private List<Category> categories = new List<Category>();
        private List<int> catIds = new List<int>();
        private List<Element> allElements = new List<Element>();
        private List<Category> selectedCats = new List<Category>();
        List<Element> selectedEles = new List<Element>();

        public SelectCategoryForm(ExternalCommandData exCmdData)
        {
            InitializeComponent();
            this.uiDoc = exCmdData.Application.ActiveUIDocument;
            this.doc = uiDoc.Document;
            

            InitCategories();
     
            UpdateListView();

            
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

        private void UpdateListView()
        {
            //foreach (Category category in categories)
            //{
            //    ListViewItem listViewItem = new ListViewItem(category.Name.ToString());
            //    listViewItem.SubItems.Add(category.Id.ToString());
            //    if (!listView1.Items.ContainsKey(category.Name.ToString()))
            //    {
            //        listView1.Items.Add(listViewItem);
            //    }
            //}
            foreach (Category category in categories)
            {
                catIds.Add(category.Id.IntegerValue);
            }
            catIds = catIds.Distinct().ToList();
            foreach (int catId in catIds)
            {
                if (categories.Exists(x => x.Id.IntegerValue.Equals(catId))){
                    Category category = categories.Find(x => x.Id.IntegerValue.Equals(catId));
                    ListViewItem listViewItem = new ListViewItem(category.Name.ToString());
                    listViewItem.SubItems.Add(category.Id.ToString());
                    listView1.Items.Add(listViewItem);
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

        private void button1_Click(object sender, EventArgs e)
        {
            selectedCats.Clear();
            selectedEles.Clear();
            for (int i=0; i < listView1.SelectedItems.Count; i++)
            {
                int Id = Convert.ToInt32(listView1.SelectedItems[i].SubItems[1].Text);
                Category category = categories.Find(x => x.Id.IntegerValue.Equals(Id));
                if (category!= null)
                {
                    selectedCats.Add(category);
                }    
            }
            //string str = "";
            //foreach (Category category in selectedCats)
            //{
            //    str += category.Name;
            //}
            //label2.Text = str;


            highLight();
            getCommonParams();
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

        private void getCommonParams()
        {   
            List<Parameter> commonParams = Util.RawConvertSetToList<Parameter>(selectedEles[0].Parameters);
            foreach (Element elem in selectedEles)
            {
                List<Parameter> params2 = Util.RawConvertSetToList<Parameter>(elem.Parameters);
                //commonParams = commonParams.Intersect(params2).ToList();
                MessageBox.Show("a "+selectedEles[0].Id.ToString() + " "+ commonParams[1].Definition.ParameterGroup.ToString());
                MessageBox.Show("b " + elem.Id.ToString() + " " + params2[1].Definition.ParameterGroup.ToString());
            }
            
        }

    }
}
