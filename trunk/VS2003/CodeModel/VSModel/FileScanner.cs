using System;
using System.IO;

using EnvDTE;

using CodeModel.Components;

namespace CodeModel.VSModel
{
	public class ModelBuilder
	{
		protected _DTE m_app;
		public ModelBuilder()
		{
		}

		public SolutionComponent CreateModel( _DTE app )
		{
			m_app = app;

			string name = Path.GetFileNameWithoutExtension(app.Solution.FileName);
			SolutionComponent sol = new SolutionComponent( app.Solution.FullName, name );

			foreach( Project p in app.Solution )
			{
				ProjectComponent project = new ProjectComponent( p.FullName, p.Name );
				if( p.ProjectItems != null )
				{
					WalkProjectItems( project, p.ProjectItems );
					sol.Visit( project );
				}
			}
			return sol;
		}

		protected void WalkProjectItems( ProjectComponent project,  ProjectItems itemList )
		{
			if( itemList != null )
			{
				foreach( ProjectItem item in itemList )
				{
					VisitProject( project, item );
				
					WalkProjectItems( project, item.ProjectItems ); // Get children.
				}
			}
		}

		protected void VisitProject( ProjectComponent project, ProjectItem item )
		{
			if( item.SubProject == null )
			{
				FileComponent file = VisitFile( item );
				if( file != null )
					project.Visit( file );
			}
		}

		protected FileComponent VisitFile( ProjectItem item )
		{
			FileComponent file = null;

			if( Filter( item ) )
			{
				m_app.StatusBar.Text = "building stats for " + item.Name;

				file = new FileComponent( item.get_FileNames(0), item.Name );

				Walker walker = new Walker();
				walker.WalkFile( file, item );
			}

			return file;
		}

		// true if good, false is non-file
		public bool Filter( ProjectItem item )
		{
			return item.FileCodeModel != null && item.FileCodeModel.CodeElements != null;
		}
	}

	internal class Walker
	{
		public void WalkFile( FileComponent file, ProjectItem item )
		{
			if( item.FileCodeModel.CodeElements != null && 
				item.FileCodeModel.CodeElements.Count > 0 )
			{
				foreach( CodeElement ce in item.FileCodeModel.CodeElements )
				{
					WalkElements( ce, file );
				}
			}
		}
	
		protected void WalkElements( CodeElement cein, AbstractComponent parent )
		{
			CodeElements ces;
			switch( cein.Kind )
			{
				// Handle namespaces
				case EnvDTE.vsCMElement.vsCMElementNamespace:
				{
					CodeNamespace cn = (CodeNamespace) cein;
										
					ces = cn.Members;
					foreach (CodeElement ce in ces)
						WalkElements(ce, parent );
					break;
				}
				// Handle classes
				case EnvDTE.vsCMElement.vsCMElementClass:
				{
					CodeClass cc = (CodeClass) cein;
					
					ClassComponent cls = new ClassComponent( cc.FullName, cc.Name );
					cls.CodeClass = cc;
					parent.Visit( cls );

					ces = cc.Members;
					foreach (CodeElement ce in ces)
						WalkElements(ce, cls );
					break;	
				}
				// Handle interfaces
				case EnvDTE.vsCMElement.vsCMElementInterface:
				{
					CodeInterface ci = (CodeInterface) cein;

					// nothing for now.
					
					break;
				}
				// Handle methods (functions)
				case  EnvDTE.vsCMElement.vsCMElementFunction:
				{
					CodeFunction cf = (CodeFunction) cein;

					MethodComponent mc = new MethodComponent( cf.FullName, cf.Name );
					parent.Visit( mc );
					mc.CreateRepresentation( GetFunctionText( cf ) );
					mc.CodeFunction = cf;

					break;
				}
				// Handle properties
				case EnvDTE.vsCMElement.vsCMElementProperty:
				{
					CodeProperty cp = (CodeProperty) cein;

					PropertyComponent pc = new PropertyComponent( cp.FullName, cp.Name );
					parent.Visit( pc );

					break;
				}
				// Handle fields (variables)
				case EnvDTE.vsCMElement.vsCMElementVariable:
				{
					CodeVariable cv = (CodeVariable) cein;

					FieldComponent fc = new FieldComponent( cv.FullName, cv.Name );
					parent.Visit( fc );
					
					break;
				}
			}
		}

		protected string GetFunctionText( CodeFunction elem )
		{
			EditPoint aEditPoint = elem.StartPoint.CreateEditPoint();
			return aEditPoint.GetText(elem.EndPoint);
		}
	}
}
