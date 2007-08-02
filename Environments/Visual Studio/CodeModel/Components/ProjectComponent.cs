using System;
using System.Collections;

namespace CodeModel.Components
{
	/// <summary>
	/// Summary description for ProjectComponent.
	/// </summary>
	public class ProjectComponent : AbstractComponent
	{
		public ArrayList Files;
		public ProjectComponent( string fullName, string name ) : base( fullName, name )
		{
			Files = new ArrayList();
		}

		public override void Visit( AbstractComponent component )
		{
			this.Files.Add( component );
		}
	}
}
