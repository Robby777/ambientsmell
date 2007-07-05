using System;
using System.Collections;

using CodeModel.Representation;

namespace CodeModel.Components
{
	/// <summary>
	/// Summary description for MethodComponent.
	/// </summary>
	public class MethodComponent : AbstractComponent
	{
		public ArrayList Lines;      // LineModel
		public ArrayList Statements; // ASTModel
		public string Text;
		public string[] TextLines;

		// Stats
		public int NumIterationStmts;
		public int NumConditionalStmts;
		public int NumComments;

		public object CodeFunction;

		protected bool m_inMultiLine;
		public MethodComponent( string fullName, string name ) : base( fullName, name )
		{
			Lines      = new ArrayList();
			Statements = new ArrayList();
		}

		public override void Visit( AbstractComponent component )
		{
		}

		public void CreateRepresentation( string text )
		{
			if( text.Length == 0 )
				return;

			this.TextLines = text.Split( new char[]{'\n' } );
			foreach( string rawLine in TextLines )
			{
				LineModel model = new LineModel( rawLine, m_inMultiLine );
				m_inMultiLine = model.InMultipleLineComment;

				if( model.HasIteration )
					this.NumIterationStmts++;
				if( model.HasConditional )
					this.NumConditionalStmts++;
				if( model.HasComment)
					this.NumComments++;

				Lines.Add( model );
			}
		}
	}
	
	public class MethodLengthComparer : IComparer
	{
		public enum SortDirection
		{
			Ascending,
			Descending
		}

		SortDirection m_direction;
		public MethodLengthComparer( SortDirection direction )
		{
			m_direction = direction;
		}
		#region IComparer Members

		public int Compare(object x, object y)
		{
			MethodComponent xx = (MethodComponent)x;
			MethodComponent yy = (MethodComponent)y;
			int value = xx.Lines.Count.CompareTo( yy.Lines.Count );
			return value * (m_direction == SortDirection.Ascending ? 1 : -1);
		}

		#endregion
	}
}