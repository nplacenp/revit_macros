/*
 * Created by SharpDevelop.
 * User: nplac
 * Date: 11/16/2021
 * Time: 6:34 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB.Architecture;
using System.IO;
using Autodesk.Revit.DB.ExtensibleStorage;

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
		
		public void AllFilesInFolder()
		{
			string files = "";
			string folder = @"C:\Users\nplac\Documents\RevitFamilies";
				foreach(string filename in Directory.GetFiles(folder, "*.rfa"))
			{
				files += filename + Environment.NewLine;
			}
			TaskDialog.Show("Files", files);
		}
		
		public void WriteTextFile()
		{
			
			string tempPath = Path.GetTempPath();
			
			string myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			
			string file = "test.txt";
			string filename = Path.Combine(tempPath, file);
			
			if (File.Exists(filename))
			{
				File.Delete(filename);
			}
			
			using (StreamWriter writer = new StreamWriter(filename, true))
			{
				writer.WriteLine("First Line");
				writer.WriteLine("Second Line");
			}

		}
		public void ReadTextFileFirstLine()
		{
			string filename = @"C:\Users\nplac\Documents\RevitApi\test.txt";

			using (StreamReader reader = new StreamReader(filename, true))
			{
				TaskDialog.Show(filename, reader.ReadLine());
			}

		}
		public void ReadTextFileAll()
		{
			string filename = @"C:\Users\nplac\Documents\RevitApi\test.txt";	
			string fileContents = "";			

			using (StreamReader reader = new StreamReader(filename, true))
			{
				string thisLine = "";
				
				while (thisLine != null)
				{
					thisLine = reader.ReadLine();
					fileContents += thisLine + Environment.NewLine;
				}
				TaskDialog.Show(filename, fileContents);
			}
		}
			
		private string getFaceInfo(HostObject ho, IList<Reference> refList)
		{
			string s = "";
			foreach (Reference r in refList)
			{
				Face f = ho.GetGeometryObjectFromReference(r) as Face;
				s += "Face area = " + Math.Round(f.Area) + ", " + Environment.NewLine;
				s += "Edge Lengths = ";
				foreach (EdgeArray ea in f.EdgeLoops)
				{
					foreach(Edge e in ea)
					{
						s += Math.Round(e.ApproximateLength, 2) + ", ";
					}
				}
				s += Environment.NewLine;
			}
			return s;
		}
		
		public void HostFaces()
		{
			Document doc = this.ActiveUIDocument.Document;
			string info = "";
			foreach (HostObject hostObj in new FilteredElementCollector(doc).OfClass(typeof(HostObject)))
			{
				info += hostObj.Name + Environment.NewLine;
				try
				{
					info += getFaceInfo(hostObj, HostObjectUtils.GetSideFaces(hostObj, ShellLayerType.Exterior));
				}
				catch (Autodesk.Revit.Exceptions.ArgumentException)
				{}
				
				try
				{
					info += getFaceInfo(hostObj, HostObjectUtils.GetTopFaces(hostObj));
				}
				catch (Autodesk.Revit.Exceptions.ArgumentException)
				{}
				try
				{
					info += getFaceInfo(hostObj, HostObjectUtils.GetBottomFaces(hostObj));
				}
				catch (Autodesk.Revit.Exceptions.ArgumentException)
				{}
				
				info += Environment.NewLine;
				info += Environment.NewLine;

			}
			TaskDialog.Show("Host Objects", info);
		}
		
		public void DeleteElement()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;

			using (Transaction t = new Transaction(doc, "Delete Element"))
			{
			t.Start();
			doc.Delete(uidoc.Selection.PickObject(ObjectType.Element).ElementId);
			t.Commit();
			}
		}
		
		public void CreateTextNote()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			XYZ point = uidoc.Selection.PickPoint();
			
			using (Transaction t = new Transaction(doc, "Create Text Note"))
			{
				t.Start();
				TextNoteOptions options = new TextNoteOptions();
				options.HorizontalAlignment = HorizontalTextAlignment.Center;
				options.TypeId = (new FilteredElementCollector(doc).OfClass(typeof(TextNoteType)).FirstOrDefault()).Id;
				TextNote note = TextNote.Create(doc, doc.ActiveView.Id, point, 1,
				                                "I am an API created text note" + Environment.NewLine + "Line 2 of the Text",
				                                options);
				
				Parameter arcParameter = note.GetParameter(ParameterTypeId.ArcLeaderParam);
				arcParameter.Set(1);
				
				Leader l = note.AddLeader(TextNoteLeaderTypes.TNLT_ARC_L);
				
				t.Commit();
			}
		}
		
		public void TextColor()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			TextNote tn = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element)) as TextNote;
			TextNoteType tnType = tn.TextNoteType;
			Parameter tnColor = tnType.GetParameter(ParameterTypeId.LineColor);
			
			System.Drawing.Color color = System.Drawing.Color.FromArgb(255,0,0);
			
			int colorInt = System.Drawing.ColorTranslator.ToWin32(color);
			
			using (Transaction t = new Transaction(doc, "Color"))
			{
				t.Start();
				tnColor.Set(colorInt);
				t.Commit();
			}
		}
		
		public void SetParamForGenericAnnotations()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			string parameterTypeError = "";
			using (Transaction t = new Transaction(doc, "Set Annotation Parameters"))
			       {
						t.Start();
						
						foreach (FamilyInstance fi in new FilteredElementCollector(doc)
						         .OfClass(typeof(FamilyInstance))
						         .OfCategory(BuiltInCategory.OST_DetailComponents)
						         .Cast<FamilyInstance>())
						{
							Parameter p = fi.LookupParameter("view");
							if (p == null)
								break;
							
							if (p.StorageType != StorageType.String)
							{
								parameterTypeError += fi.Symbol.Family.Name + " - " + p.StorageType.ToString() + "/n";
								break;
							}
							Element ownerView = doc.GetElement(fi.OwnerViewId);
							p.Set(ownerView.Name);
						}
						if (parameterTypeError != "")
						{
							TaskDialog.Show("Error", "Parameter must be a string.\n" + parameterTypeError);
						}
							
						
						t.Commit();
			       }
		}
		
		public void BuiltInParamsForElement()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Element e = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element));
			string data = "";
			foreach (BuiltInParameter bip in Enum.GetValues(typeof(BuiltInParameter)))
			{
				try
				{
					Parameter p = e.GetParameter(ParameterUtils.GetParameterTypeId(bip)); //api difference with typeid
					data += bip.ToString() + ": " + p.Definition.Name + ": ";
					
					if (p.StorageType == StorageType.String)
						data += p.AsString();
					else if (p.StorageType == StorageType.Integer)
						data += p.AsInteger();
					else if (p.StorageType == StorageType.Double)
						data += p.AsDouble();
					else if (p.StorageType == StorageType.ElementId)
						data += "ID " + p.AsElementId().IntegerValue;
				}
				catch
				{
					
				}
			}
			TaskDialog.Show("BI Params", data);
		}
		
		public void KeynoteArea()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			List<Element> elements = new FilteredElementCollector(doc).WhereElementIsNotElementType().ToList();
			List<Tuple<ElementId, double, string>> results = new List<Tuple<ElementId, double, string>>();
			
			foreach (Element e in elements)
			{
				Parameter areaParam = e.GetParameter(ParameterTypeId.HostAreaComputed);
				if (areaParam == null)
					continue;
				
				ElementId typeId = e.GetTypeId();
				Element typeElement = doc.GetElement(typeId);
				
				Parameter keynoteParam = typeElement.GetParameter(ParameterTypeId.KeynoteParam);
				
				Tuple<ElementId, double, string> thisElementData = 
					new Tuple<ElementId, double, string>(e.Id, areaParam.AsDouble(), keynoteParam.AsString());
				
				results.Add(thisElementData);
			}
			string resultText = "";
			
			foreach(Tuple<ElementId, double, string> t in results)
			{
				resultText += t.ToString();
			}
			
			TaskDialog.Show("Results", resultText);//Watch is not working in SharpDevelop...
		
		}
		
		public void CreateWall()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Line l = Line.CreateBound(XYZ.Zero, new XYZ(10,10,0));
			ElementId levelId = uidoc.Selection.PickObject(ObjectType.Element, "Select a level").ElementId;
			
			using (Transaction t = new Transaction(doc, "Create Wall"))
			{
				t.Start();
				Wall w = Wall.Create(doc, l, levelId, false);
				t.Commit();
			}
		}
		
		public void NewDesk()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Level l = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().OrderBy(q => q.Elevation).First();
			Family f = new FilteredElementCollector(doc).OfClass(typeof(Family)).FirstOrDefault(q => q.Name == "Desk") as Family;
			FamilySymbol fs = f.GetFamilySymbolIds().Select(q => doc.GetElement(q)).First(q => q.Name == "72\" x 36\"") as FamilySymbol;
			XYZ point = uidoc.Selection.PickPoint("Pick Point");
			
			using (Transaction t = new Transaction(doc, "Create Family Instance"))
			{
				t.Start();
				
				if (!fs.IsActive)
					fs.Activate();
				
				FamilyInstance fi = doc.Create.NewFamilyInstance(point, fs, l, Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
				
				t.Commit();
			}
		}
		
		public void NewDoor()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Level l = new FilteredElementCollector(doc).OfClass(typeof(Level)).Cast<Level>().OrderBy(q => q.Elevation).First();
			Family f = new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>()
				.FirstOrDefault(q => q.FamilyCategoryId.IntegerValue == (int)BuiltInCategory.OST_Doors && q.Name == "Single-Flush") as Family;
			FamilySymbol fs = f.GetFamilySymbolIds().Select(q => doc.GetElement(q)).Cast<FamilySymbol>().First(Queryable => Queryable.Name =="36\" x 84\"");
			Reference r = uidoc.Selection.PickObject(ObjectType.Element, "Pick point on wall");
			XYZ point = r.GlobalPoint;
			Element host = doc.GetElement(r);
			
			using (Transaction t = new Transaction(doc, "Create Family Instance"))
			{
				t.Start();
				
				FamilyInstance fi = doc.Create.NewFamilyInstance(point, fs, host, l,
                                             Autodesk.Revit.DB.Structure.StructuralType.NonStructural);
				
				t.Commit();
			}
						                                                    
		}
		
		public void Rotate()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Element e = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element));
			
			XYZ point = ((LocationPoint)e.Location).Point;
			
			XYZ point2 = point.Add(XYZ.BasisZ);
			
			Line axis = Line.CreateBound(point, point2);
			
			using (Transaction t = new Transaction(doc, "Rotate"))
			{
				t.Start();
				
				ElementTransformUtils.RotateElement(doc, e.Id, axis, DegreesToRadians(45));
				
				t.Commit();
			}
		}
		
		private double DegreesToRadians(double degrees)
		{
			return degrees * Math.PI / 180;
		}
		
		public void GetElementWorkset()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Element e = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element));
			WorksetId workId = e.WorksetId;
			WorksetTable table = doc.GetWorksetTable();
			Workset workset = table.GetWorkset(workId);
			TaskDialog.Show("Element", e.Name + Environment.NewLine + e.Id +
			                Environment.NewLine + workset.Name + " - " + workset.Owner);
		}
		
		public void Location()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Element e = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element));
			
			Location location = e.Location;
			if (location is LocationCurve)
			{
				LocationCurve lc = location as LocationCurve;
				Curve c = lc.Curve;
				XYZ end0 = c.GetEndPoint(0);
				XYZ end1 = c.GetEndPoint(1);
				TaskDialog.Show("Curve length + endpoints", "Curve Length - " + c.ApproximateLength.ToString()
				               + Environment.NewLine + "Curve Endpoints:" + Environment.NewLine +
					"Start Point - " + end0.ToString() + Environment.NewLine +
					"End Point - " + end1.ToString());
				
//				this following line is just to show an example of a journal file comment implementation
				doc.Application.WriteJournalComment("LocationMacro: " + end0.ToString() + " - " + end1.ToString(), false);
				
				if (c is Line)
				{
					Line line = c as Line;
					TaskDialog.Show("Direction", line.Direction.ToString());
				}
				else
				{
					Transform t = c.ComputeDerivatives(0.5, true);
					XYZ tangent = t.BasisX;
					XYZ tangentNormal = tangent.Normalize();
					TaskDialog.Show("Direction", "Tangent = " + tangent.ToString()
					                + Environment.NewLine + "Tangent Normalized = "
					                + tangentNormal.ToString());
				}
			}
			else
			{
				LocationPoint lp = location as LocationPoint;
				XYZ point = lp.Point;
				TaskDialog.Show("Point", point.ToString());
			}
		}
		
		public void Volume()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Options options = new Options();
			
			foreach (Element e in new FilteredElementCollector(doc).OfClass(typeof(Stairs)))
			{
				CurveArray curves = new CurveArray();
				List<Solid> solids = new List<Solid>();
				AddCurvesAndSolids(e.get_Geometry(options), ref curves, ref solids);
				foreach (Solid s in solids)
				{
					TaskDialog.Show("e", e.Name + " " + e.Id.IntegerValue + " " + s.Volume);
				}
			}
		}
		
		private void AddCurvesAndSolids(Autodesk.Revit.DB.GeometryElement geomElem,
                                ref Autodesk.Revit.DB.CurveArray curves,
                                ref System.Collections.Generic.List<Autodesk.Revit.DB.Solid> solids)
		{
		    foreach (Autodesk.Revit.DB.GeometryObject geomObj in geomElem)
		    {
		        Autodesk.Revit.DB.Curve curve = geomObj as Autodesk.Revit.DB.Curve;
		        if (null != curve)
		        {
		            curves.Append(curve);
		            continue;
		        }
		        Autodesk.Revit.DB.Solid solid = geomObj as Autodesk.Revit.DB.Solid;
		        if (null != solid)
		        {
		            solids.Add(solid);
		            continue;
		        }
		        //If this GeometryObject is Instance, call AddCurvesAndSolids
		        Autodesk.Revit.DB.GeometryInstance geomInst = geomObj as Autodesk.Revit.DB.GeometryInstance;
		        if (null != geomInst)
		        {
		            Autodesk.Revit.DB.GeometryElement transformedGeomElem
		              = geomInst.GetInstanceGeometry(geomInst.Transform);
		            AddCurvesAndSolids(transformedGeomElem, ref curves, ref solids);
		        }
		    }
		}
	
		public void setUnits()
		{
			Document doc = this.ActiveUIDocument.Document;
			
			Units units = doc.GetUnits();
			FormatOptions foLength = units.GetFormatOptions(SpecTypeId.Length);
			foLength.SetUnitTypeId(UnitTypeId.Millimeters);
			foLength.SetSymbolTypeId(SymbolTypeId.Mm);
			foLength.Accuracy = 10;
			
			FormatOptions foVolume = units.GetFormatOptions(SpecTypeId.Volume);
			foVolume.SetUnitTypeId(UnitTypeId.CubicMeters);
//			foVolume.SetSymbolTypeId(SymbolTypeId.MCaret3); // i cannot figure out why this doesn't work...
			
			
			units.SetFormatOptions(SpecTypeId.Length, foLength);
			units.SetFormatOptions(SpecTypeId.Volume, foVolume);
			
			using (Transaction t = new Transaction(doc, "Set Units"))
			       {
				t.Start();
				
				doc.SetUnits(units);
				
				t.Commit();
			       }
		}
		public void SelectElementWithFilter()
		{
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Reference myRefWall = uidoc.Selection.PickObject(ObjectType.Element, new GenericSelectionFilter("Walls"), "Select a Wall");
			Reference myRefFloor = uidoc.Selection.PickObject(ObjectType.Element, new GenericSelectionFilter("Floors"), "Select a Floor");


			
			Element e = doc.GetElement(myRefWall);
			Element ef = doc.GetElement(myRefFloor);
			
			string designOptionName = "<none>";
			if (e.DesignOption != null)
			{
				designOptionName = e.DesignOption.Name;
			}
			TaskDialog.Show("Element Info", e.Name + Environment.NewLine + e.Id + Environment.NewLine + designOptionName);
			
			if (ef.DesignOption != null)
			{
				designOptionName = ef.DesignOption.Name;
			}
			TaskDialog.Show("Element Info", ef.Name + Environment.NewLine + ef.Id + Environment.NewLine + designOptionName);
		
		}
		
		public class GenericSelectionFilter : ISelectionFilter
		{
			
			static string CategoryName = "";
			
			public GenericSelectionFilter(string name)
			{
				CategoryName = name;
			}
			
			public bool AllowElement(Element e)
			{
				if (e.Category.Name == CategoryName)
					return true;
				
				return false;
			}
			public bool AllowReference(Reference r, XYZ point)
			{
				return true;
			}
		}
		
		public void registerSaveEvent()
		{
			Document doc = this.ActiveUIDocument.Document;
			doc.DocumentSaving += new EventHandler<DocumentSavingEventArgs>(myDocumentSaving);
		}
		
		private void myDocumentSaving(object sender, DocumentSavingEventArgs args)
		{
			Document doc = sender as Document;
			string user = doc.Application.Username;
			string filename = doc.PathName;
			string filenameShort = Path.GetFileNameWithoutExtension(filename);
			string tempFolder = Path.GetTempPath();
			string outputFile = Path.Combine(tempFolder,filenameShort + ".txt");
			
			using (StreamWriter sw = new StreamWriter(outputFile, true))
			{
				sw.WriteLine(DateTime.Now + ": " + user);
			}
		}
		
		public void ExtensibleStorage_SetData()
		{
			string schemaName = "PurchasingInfo";
			
			// check to see if schema exists
			Schema mySchema = Schema.ListSchemas().FirstOrDefault(q => q.SchemaName == schemaName);
			
			if (mySchema == null)
			{
				Guid myGuid = new Guid("6e402ce9-66fd-456a-9c55-701008547f73");
				SchemaBuilder sb = new SchemaBuilder(myGuid);
				sb.SetSchemaName(schemaName);
				
				// add fields (properties) to schema, defining the name and data type for each field
				FieldBuilder fbCost = sb.AddSimpleField("Cost", typeof(Int32));
				FieldBuilder fbShippingWeight = sb.AddSimpleField("ShippingWeight", typeof(double));
				// set the Unit Type of the Shipping Weight
				fbShippingWeight.SetSpec(SpecTypeId.Mass);
				
				//conclude the creation of the schema
				mySchema = sb.Finish();
			}
			
			//create entity of schema (like an instance of a class)
			Entity myEntity = new Entity(mySchema);
			
			//get the fields from the schema
			Field myCostField = mySchema.GetField("Cost");
			Field myShipWeightField = mySchema.GetField("ShippingWeight");
			
			// set value for this entity for each field
			myEntity.Set<Int32>(myCostField, 125);
			myEntity.Set<double>(myShipWeightField, 1, UnitTypeId.Kilograms);
			
			//prompt user to select an element
			
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Element element = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element));
			
			// store the data in the element
			using (Transaction t = new Transaction(doc, "Store Data"))
			{
				t.Start();
				
				element.SetEntity(myEntity);
				
				t.Commit();
			}
			
		}
		
		public void ExtensibleStorage_GetData()
		{
			//get the schema with ListSchemas
			string schemaName = "PurchasingInfo";
			Schema mySchema = Schema.ListSchemas().FirstOrDefault(q => q.SchemaName == schemaName);
			
			//diagnostics in the event that the desired schema was not found
			if (mySchema == null)
			{
				string allSchema = "";
				foreach(Schema s in Schema.ListSchemas())
				{
					allSchema += s.SchemaName + Environment.NewLine;
				}
				TaskDialog.Show("error", "Schema not found " + schemaName);
				TaskDialog.Show("All Schemas", allSchema);
				return;
			}
			
			// get the fields from the schema
			Field myCostField = mySchema.GetField("Cost");
			Field myShipWeightField = mySchema.GetField("ShippingWeight");
			
			//prompt user to select an element
			UIDocument uidoc = this.ActiveUIDocument;
			Document doc = uidoc.Document;
			
			Element element = doc.GetElement(uidoc.Selection.PickObject(ObjectType.Element));
			
			//get the entity from the element
			Entity myEntity = element.GetEntity(mySchema);
			
			//get the data from the entity
			Int32 cost = myEntity.Get<Int32>("Cost");
			double weight = myEntity.Get<double>(myShipWeightField, UnitTypeId.PoundsMass);
			
			TaskDialog.Show(schemaName, cost + Environment.NewLine + weight);
		}
		
		public void referenceIntersector()
		{
			Document doc = this.ActiveUIDocument.Document;
			View3D view3D = doc.ActiveView as View3D;
			if (view3D == null)
			{
				TaskDialog.Show("Error", "Active view must be a 3d view");
				return;
			}
			ReferenceIntersector ri = new ReferenceIntersector(view3D);
			IList<ReferenceWithContext> refWithContextList = ri.Find(XYZ.Zero, XYZ.BasisY);
			string data = "";
			foreach (ReferenceWithContext rwc in refWithContextList)
			{
				Reference r = rwc.GetReference();
				double d = rwc.Proximity;
				Element e = doc.GetElement(r);

				
				GeometryObject o = e.GetGeometryObjectFromReference(r);
				string oType;
				try
				{
					oType = o.GetType().ToString();
				}
				catch
				{
					continue;
				}
				
				data += oType + " - " + e.Name + " - " + d + Environment.NewLine;
			}
			TaskDialog.Show("Elements Hit", data);
		}
		
		public class TextTypeUpdater : IUpdater
		{
			static AddInId m_appId;
			static UpdaterId m_updaterId;
			//constructor takes the AddInId for the add-in associated with this updater
			public TextTypeUpdater(AddInId id)
			{
				m_appId = id;
				// every updater must have a unique ID
				m_updaterId = new UpdaterId(m_appId, new Guid("6dc5c664-fc17-4be3-942a-d0f097379f39"));
			}
			
			public void Execute(UpdaterData data)
			{
				Document doc = data.GetDocument();
				
				// loop through the list of added elements
				foreach (ElementId addedElemId in data.GetAddedElementIds())
				{
					TextNoteType textNoteType = doc.GetElement(addedElemId) as TextNoteType;
					string name = textNoteType.Name;
					doc.Delete(addedElemId);
					TaskDialog.Show("New Element Deleted!", "Text type '" + name + "' has been deleted. Please use existing text types.");
				}
			}
			public string GetAdditionalInformation(){return "Text note type check";}
			public ChangePriority GetChangePriority(){return ChangePriority.FloorsRoofsStructuralWalls;}
			public UpdaterId GetUpdaterId(){return m_updaterId;}
			public string GetUpdaterName(){return "Text note type";}
		}
		
		public void RegisterUpdater()
		{
			TextTypeUpdater updater = new TextTypeUpdater(this.Application.ActiveAddInId);
			UpdaterRegistry.RegisterUpdater(updater);
			
			// Trigger will occur only for TextNoteType elements
			ElementClassFilter textNoteTypeFilter = new ElementClassFilter(typeof(TextNoteType));
			
			// GetChangeTypeElementAddition specifies that the trigger will occur when elements are added
			//Other options are GetChangeTypeAny, GetChangeTypeElementDeletion, GetChangeTypeGeometry, GetChangeTypeParameter
			UpdaterRegistry.AddTrigger(updater.GetUpdaterId(), textNoteTypeFilter, Element.GetChangeTypeElementAddition());
		}
		
		public void UnregisterUpdater()
		{
			TextTypeUpdater updater = new TextTypeUpdater(this.Application.ActiveAddInId);
			UpdaterRegistry.UnregisterUpdater(updater.GetUpdaterId());
		}
	}
}
