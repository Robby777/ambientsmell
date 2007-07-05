using System;

namespace CodeModel.Components
{
	/// <summary>
	/// Summary description for FieldComponent.
	/// </summary>
	public class FieldComponent : AbstractComponent
	{
		public FieldComponent( string fullName, string name ) : base(fullName,name)
		{
		}

		public override void Visit(AbstractComponent component)
		{
		}
	}
}
