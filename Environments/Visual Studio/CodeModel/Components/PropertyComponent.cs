using System;

namespace CodeModel.Components
{
	/// <summary>
	/// Summary description for PropertyComponent.
	/// </summary>
	public class PropertyComponent : AbstractComponent
	{
		public PropertyComponent( string fullName, string name ) : base(fullName, name)
		{
		}

		public override void Visit(AbstractComponent component)
		{
		}
	}
}
