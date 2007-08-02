using System;

using CodeModel.Components;

namespace SmellFinder
{
	/// <summary>
	/// Summary description for AbstractSmellPanel.
	/// </summary>
	public abstract class AbstractSmellPanel : System.Windows.Forms.UserControl
	{
		public AbstractSmellPanel()
		{
		}

		public abstract void ProcessFindParameters( SolutionComponent solution );

		protected int TryParse( string text )
		{
			try	{ return int.Parse( text ); }
			catch { return -1; }
		}
	}
}
