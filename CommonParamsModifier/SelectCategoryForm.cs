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
                if (categories.Equals(null))
                {
                    categories.Add(elem.Category);                    
                }
                if (!categories.Contains(elem.Category))
                {
                    categories.Add(elem.Category);
                }
            }
        }

        private void UpdateListView()
        {
            foreach (Category category in categories)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.SubItems.Add(category.ToString());
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
