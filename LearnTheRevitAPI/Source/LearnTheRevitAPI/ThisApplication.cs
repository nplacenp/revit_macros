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
//			this prompts you to select a revit element, and then it returns information
//			on the element name, ID, and assigned design option if applicable
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
//			this script prompts you to select an edge of a revit element
//			and returns the element name, ID, and length of thge edge
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
//			this script prompts you to select the face of a revit element
//			and returns the element name, ID, and the area of the face
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
//			this script grabs all of the wall elements in the file and adds them to your current selection in the document
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			ICollection<ElementId> walls = new FilteredElementCollector(doc).OfClass(typeof(Wall)).ToElementIds();
			uidoc.Selection.SetElementIds(walls);
			
		}
		public void selectedElements()
		{
//			this script takes your current in-view selection and gives you a list of how many elements
//			are selected, and then gives you the name of each.
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
			
////			This snippet will create a task dialog that shows the element names of existing
////			wall elements in the document
//			string wallInfo = "";
//			foreach(Element e in new FilteredElementCollector(doc).OfClass(typeof(Wall)))
//			{
//				wallInfo += e.Name + "\n";
//			}
//			TaskDialog.Show("Wall Elements", wallInfo);
//			
////			this snippet will create a task dialog that shows all of the existing 
////			wall types in the document
//			string wallTypeInfo = "";
//			foreach(Element e in new FilteredElementCollector(doc).OfClass(typeof(WallType)))
//			{
//				wallTypeInfo += e.Name + "\n";
//			}
//			TaskDialog.Show("Wall Types", wallTypeInfo);
//			
////			this snippet will create a task dialog that shows the family name, family symbol name and
////			family instance name for every door element in the document
//			string familyInfo = "";
//			foreach(Element e in new FilteredElementCollector(doc).OfClass(typeof(FamilyInstance))
//			        .OfCategory(BuiltInCategory.OST_Doors))
//			{
//				FamilyInstance fi = e as FamilyInstance;
//				FamilySymbol fs = fi.Symbol;
//				Family family = fs.Family;
//				familyInfo += family.Name + ": " + fs.Name + ": " + fi.Name + "\n";
//			}
//			TaskDialog.Show("Family names", familyInfo);

			
//			this will allow you to filter more than one category of elements

//			VARIOUS TYPES OF FILTERS EXAMPLES

////			door filter only
//			ElementCategoryFilter doorFilter = new ElementCategoryFilter(BuiltInCategory.OST_Doors);
////			window filter only
//			ElementCategoryFilter windowFilter = new ElementCategoryFilter(BuiltInCategory.OST_Windows);
////			combined filter for two filters above
//			LogicalOrFilter orFilter = new LogicalOrFilter(doorFilter, windowFilter);
			
			IList<BuiltInCategory> catList = new List<BuiltInCategory>();
			catList.Add(BuiltInCategory.OST_Doors);
			catList.Add(BuiltInCategory.OST_Windows);
			
			ElementMulticategoryFilter multiCatFilter = new ElementMulticategoryFilter(catList);
			
			string familyInfo = "";
//			You can also pass in the element active ID to filter by view with normal filteredelementcollectors
			foreach (Element e in new FilteredElementCollector(doc, doc.ActiveView.Id)
			         .OfClass(typeof(FamilyInstance))
			         .WherePasses(multiCatFilter))
			
			{
				FamilyInstance fi = e as FamilyInstance;
				FamilySymbol fs = fi.Symbol;
				Family family = fs.Family;
				familyInfo += family.Name + ": " + fs.Name + ": " + fi.Name + "\n";
			}
			TaskDialog.Show("Family names", familyInfo);
			
		}
		public void newFilter()
		{
			Document doc = this.ActiveUIDocument.Document;
//			ElementClassFilter classFilter = new ElementClassFilter(typeof(TextNote));
////			this next line will allow you to filter by active view, with an option of inverting the
////			filter to every view but your current active view -- can be used in other filters as well
//			ElementOwnerViewFilter viewFilter = new ElementOwnerViewFilter(doc.ActiveView.Id, true);
//			string text = "";
//			foreach (Element e in new FilteredElementCollector(doc)
//			         .WherePasses(classFilter)
//			         .WherePasses(viewFilter))
//			{
//				TextNote textnote = e as TextNote;
//				text += textnote.Text + "\n";
//			}
//			TaskDialog.Show("Text", text);

//			The following is a way to get around the limitations of the element class filter that does not support revit.db class types 
			ElementClassFilter classFilter = new ElementClassFilter(typeof(SpatialElement));
			string text = "";
			foreach (Element e in new FilteredElementCollector(doc).OfCategory(BuiltInCategory.OST_Rooms)
			         .WherePasses(classFilter))
			{
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
			         .Where(m => m.Symbol.Family.Name.Contains("Double"))
			        )
			{
//				FamilyInstance fi = e as FamilyInstance;
				FamilySymbol fs = fi.Symbol;
				Family family = fs.Family;
				info += family.Name + ": " + fs.Name + ": " + fi.Name + "\n";

//				the 'where' statement essentially replaces the if statement below with LINQ inquiries				
//				if (family.Name.Contains("Double")){
//					info += family.Name + ": " + fs.Name + ": " + fi.Name + "\n";
//				}
					
			}
			
			TaskDialog.Show("elements", info);
		}
		public void draftingViewsWithLinks()
		{
			Document doc = this.ActiveUIDocument.Document;
			
			IEnumerable<Element> allImports = new FilteredElementCollector(doc)
				.OfClass(typeof(ImportInstance));
//			Returns the count of all imported instances in the projects
			TaskDialog.Show("data", allImports.Count().ToString());
			
//			casts the elements to import instances to you can use the islinked property
			IEnumerable<Element> allImportsThatAreLinked = new FilteredElementCollector(doc)
				.OfClass(typeof(ImportInstance))
				.Cast<ImportInstance>()
				.Where(q => q.IsLinked);
			
			TaskDialog.Show("AllImportsThatAreLinked", allImportsThatAreLinked.Count().ToString());
			
//			Only returning import instances that are links and owned by drafting views
			IEnumerable<Element> allImportsThatAreLInkedAndOwnedByDraftingViews = new FilteredElementCollector(doc)
				.OfClass(typeof(ImportInstance))
				.Cast<ImportInstance>()
				.Where(q => q.IsLinked && doc.GetElement(q.OwnerViewId) is ViewDrafting);
			
			TaskDialog.Show("allImportsThatAreLinkedAndOwnedByDraftingViews", allImportsThatAreLInkedAndOwnedByDraftingViews.Count().ToString());
			
			IEnumerable<Element> viewsThatContainLinkedImports = new FilteredElementCollector(doc)
				.OfClass(typeof(ImportInstance))
				.Cast<ImportInstance>()
				.Where(q => q.IsLinked && doc.GetElement(q.OwnerViewId) is ViewDrafting)
				.Select(q => doc.GetElement(q.OwnerViewId));
			TaskDialog.Show("viewsThatContainLinkedImports", 
			                string.Join(",", viewsThatContainLinkedImports.Select(q => q.Name).Distinct()));
			
		}
//		public void FamilyTypesParameters()
//		{
//		}
		public void FamilyTypesParameters()
		{
			Document doc = this.ActiveUIDocument.Document;
			
			if (!doc.IsFamilyDocument)
			{
				return;
			}
			
			using (Transaction t = new Transaction(doc, "family test"))
			{
				t.Start();
				FamilyManager mgr = doc.FamilyManager;
				
				FamilyParameter param = mgr.AddParameter("New Parameter", ParameterTypeId.TextText, ParameterTypeId.TextText, false);
				
				for (int i = 1; i < 5; i++)
				{
					FamilyType newType = mgr.NewType(i.ToString());
					mgr.CurrentType = newType;
					mgr.Set(param, "this value " + i);
				}
				
				t.Commit();
			}
			
		}
		public void lineLength()
		{
			Document doc = this.ActiveUIDocument.Document;
			UIDocument uidoc = this.ActiveUIDocument;
			
			ICollection<ElementId> ids = uidoc.Selection.GetElementIds();
			IEnumerable<Element> elements = ids.Select(q => doc.GetElement(q));
			
			double total = 0;
			foreach (Element e in elements)
			{
				Parameter lengthParam = e.get_Parameter(BuiltInParameter.CURVE_ELEM_LENGTH);
				if (lengthParam == null)
					continue;
				double length = lengthParam.AsDouble();
				total += length;
			}
			

			
			FormatOptions fo = doc.GetUnits().GetFormatOptions(SpecTypeId.Length);
			
			ForgeTypeId dut = fo.GetUnitTypeId();
			
//			doc.GetUnits().GetFormatOptions(SpecTypeId.Length).GetUnitTypeId())
//			string unitLbl = LabelUtils.GetLabelForSpec(dut);
			
			double doubleConverted = UnitUtils.ConvertFromInternalUnits(
			total, dut);
			
			
			TaskDialog.Show("Total Length", total.ToString() + "\n" + doubleConverted);
		}
	}
}
