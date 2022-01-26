/*
 * Created by SharpDevelop.
 * User: nplac
 * Date: 11/16/2021
 * Time: 6:34 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB.Architecture;

namespace LearnTheRevitAPI
{
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
    [Autodesk.Revit.DB.Macros.AddInId("8E4F35CF-414F-4C4F-ACC2-2F18C152EC17")]
	public partial class ThisApplication
	{
		private void Module_Startup(object sender, EventArgs e)
		{

		}

		private void Module_Shutdown(object sender, EventArgs e)
		{

		}

		#region Revit Macros generated code
		private void InternalStartup()
		{
			this.Startup += new System.EventHandler(Module_Startup);
			this.Shutdown += new System.EventHandler(Module_Shutdown);
		}
		#endregion
		public void SimpleDialog()
		{
			TaskDialog.Show("This is the title", "Welcome to your first dialog!");
		}
		public void selectElement()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Reference myRef = uidoc.Selection.PickObject(ObjectType.Element);
			
			Element e = doc.GetElement(myRef);
			
			string designOptionName = "<none>";
			
			if (e.DesignOption != null){
				designOptionName = e.DesignOption.Name;
			}
			
			TaskDialog.Show("Element Info", e.Name + Environment.NewLine + e.Id + "\n" + designOptionName);
		}
		public void selectEdge()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Reference myRef = uidoc.Selection.PickObject(ObjectType.Edge);
			
			Element e = doc.GetElement(myRef);
			
			GeometryObject geomObj = e.GetGeometryObjectFromReference(myRef);
			
			Edge edge = geomObj as Edge;
			
			TaskDialog.Show("Element Info", e.Name + "\n" + e.Id + "\n" + edge.ApproximateLength);
		}
		public void selectFace()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Reference myRef = uidoc.Selection.PickObject(ObjectType.Face);
			
			Element e = doc.GetElement(myRef);
			
			GeometryObject geomObj = e.GetGeometryObjectFromReference(myRef);
			
			Face f = geomObj as Face;
			
			TaskDialog.Show("Element Info", e.Name + "\n" + e.Id + "\n" + f.Area);
		}
		public void setSelectedElements()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			ICollection<ElementId> walls = new FilteredElementCollector(doc).OfClass(typeof(Wall)).ToElementIds();
			uidoc.Selection.SetElementIds(walls);
			
		}
		public void selectedElements()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			ICollection<ElementId> selectedIds = uidoc.Selection.GetElementIds();
			string s  = "";
			foreach (ElementId id in selectedIds)
			{
				Element e = doc.GetElement(id);
				s += e.Name + "\n";
			}
			TaskDialog.Show("Elements", selectedIds.Count + "\n" + s);
			
			
		}
		public void findElements()
		{
			Document doc = this.ActiveUIDocument.Document;
			string info = "";
			
			ElementCategoryFilter doorFilter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);
			ElementCategoryFilter windowFilter = new ElementCategoryFilter(BuiltInCategory.OST_Windows);
			LogicalOrFilter orFilter = new LogicalOrFilter(doorFilter, windowFilter);
			
			
			IList<BuiltInCategory> catList = new List<BuiltInCategory>();
			catList.Add(BuiltInCategory.OST_Doors);
			catList.Add(BuiltInCategory.OST_Windows);
			
			ElementMulticategoryFilter multiCatFilter = new ElementMulticategoryFilter(catList);
			
			foreach(Element e in new FilteredElementCollector(doc)
			        .OfClass(typeof(FamilyInstance))
			        .WherePasses(multiCatFilter))
			{
				FamilyInstance fi = e as FamilyInstance;
				FamilySymbol fs = fi.Symbol;
				Family family = fs.Family;
				info += family.Name + ": " + fs.Name + ": " + fi.Name + "\n";
			}
			
			TaskDialog.Show("elements", info);
			
		}
		public void newFilter()
		{
			Document doc = this.ActiveUIDocument.Document;
			ElementClassFilter classFilter = new ElementClassFilter(typeof(SpatialElement));
//			ElementOwnerViewFilter viewFilter = new ElementOwnerViewFilter(doc.ActiveView.Id);
			
			
			string text = "";
			foreach (Element e in new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Rooms)
			         .WherePasses(classFilter))
			{
//				TextNote textnote = e as TextNote;
				text += e.Name + "\n";
			}
			TaskDialog.Show("Text", text);
		}
		public void boundingBoxFilter()
		{
			Document doc = this.ActiveUIDocument.Document;
			UIDocument uidoc = this.ActiveUIDocument;
			
			XYZ pt1 = uidoc.Selection.PickPoint();
			XYZ pt2 = uidoc.Selection.PickPoint();
			
			Outline outline = new Outline(pt1, pt2);
			
			BoundingBoxIntersectsFilter bboxFilter = new BoundingBoxIntersectsFilter(outline);
			
			string text = "";
			foreach (Element e in new FilteredElementCollector(doc, doc.ActiveView.Id).WherePasses(bboxFilter))
			{
				text += e.Name + "\n";
			}
			TaskDialog.Show("Text", text);
		}
		public void findDoorElements()
		{
			Document doc = this.ActiveUIDocument.Document;
			string info = "";
			
			foreach (FamilyInstance fi in new FilteredElementCollector(doc)
			         .OfClass(typeof(FamilyInstance))
			         .OfCategory(BuiltInCategory.OST_Doors)
			         .Cast<FamilyInstance>()
			         .Where(m => m.Symbol.Family.Name.Contains("Double")))
			{
				FamilySymbol fs = fi.Symbol;
				Family family = fs.Family;
				info += family.Name + ": " + fs.Name + ": " + fi.Name + "\n";
				
			}
			TaskDialog.Show("Elements", info);
		}
	}
}