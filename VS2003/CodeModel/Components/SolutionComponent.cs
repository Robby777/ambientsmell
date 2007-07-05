using System;
using System.Collections;

namespace CodeModel.Components
{
	/// <summary>
	/// Summary description for SolutionComponent.
	/// </summary>
	public class SolutionComponent : AbstractComponent
	{
		public ArrayList Projects;
		public SolutionComponent( string fullName, string name ) : base( fullName, name )
		{
			this.Projects = new ArrayList();
		}

		public override void Visit( AbstractComponent component )
		{
			this.Projects.Add( component );
		}
	}
}
