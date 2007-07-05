using System;
using System.Collections;

namespace CodeModel.Components
{
	/// <summary>
	/// Summary description for FileComponent.
	/// </summary>
	public class FileComponent : AbstractComponent
	{
		public ArrayList Classes;
		public FileComponent( string fullName, string name ) : base( fullName, name )
		{
			Classes = new ArrayList();
		}

		public override void Visit( AbstractComponent component )
		{
			this.Classes.Add( component );
		}
	}
}
