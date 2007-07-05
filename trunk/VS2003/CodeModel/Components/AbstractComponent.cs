using System;

namespace CodeModel.Components
{
	/// <summary>
	/// Summary description for AbstractComponent.
	/// </summary>
	public abstract class AbstractComponent
	{
		public string FullName;
		public string Name;
		protected AbstractComponent( string fullName, string name )
		{
			this.FullName = fullName;
			this.Name     = name;
		}

		public abstract void Visit( AbstractComponent component );
	}
}
