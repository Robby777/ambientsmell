using System;
using System.Collections;
using EnvDTE;
namespace CodeModel.Components
{
	/// <summary>
	/// Summary description for ClassComponent.
	/// </summary>
	public class ClassComponent : AbstractComponent
	{
		public ArrayList Methods;
		public ArrayList Fields;
		public ArrayList Properties;
		public ArrayList InnerClasses;
		public CodeClass CodeClass;
		public ClassComponent( string fullName, string name ) : base( fullName, name )
		{
			this.Methods      = new ArrayList();
			this.Fields       = new ArrayList();
			this.Properties   = new ArrayList();
			this.InnerClasses = new ArrayList();
		}

		public override void Visit( AbstractComponent component )
		{
			if( component is MethodComponent )
			{
				this.Methods.Add( component );
			}
			else if( component is FieldComponent )
			{
				this.Fields.Add( component );
			}
			else if( component is PropertyComponent )
			{
				this.Properties.Add( component );
			}
			else if( component is ClassComponent )
			{
				this.InnerClasses.Add( component );
			}
			else
			{
				throw new ApplicationException("Unexpected type" + component );
			}
		}
	}
}
