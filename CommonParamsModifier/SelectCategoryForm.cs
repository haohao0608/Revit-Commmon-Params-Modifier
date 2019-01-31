using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
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
        private List<Element> allElements = new List<Element>();

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
            List<int> catIds = new List<int>();
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
    }
}
