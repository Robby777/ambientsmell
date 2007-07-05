using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

using CodeModel.Components;
using CodeModel.Representation;

namespace SmellDetectors
{
	/// <summary>
	/// Summary description for TemporaryFieldDetector.
	/// </summary>
	public class TemporaryFieldDetector : Detector
	{
		public TemporaryFieldDetector()
		{
		}

		public override ArrayList Search( SolutionComponent solution )
		{
			foreach( ProjectComponent project in solution.Projects )
			{
				foreach( FileComponent file in project.Files )
				{
					foreach( ClassComponent cls in file.Classes )
					{
						ProcessStuff( cls );
					}
				}
			}

			m_list.Sort( new TemporaryFieldComparer( TemporaryFieldComparer.SortDirection.Descending ) );
			return m_list;
		}

		public void ProcessStuff( ClassComponent cls )
		{
			Hashtable htUses = SearchUses( cls );
			Hashtable htDefs = SearchDefs( cls, htUses );
			GenerateForClass( htUses, htDefs, cls );

			foreach( ClassComponent inner in cls.InnerClasses )
			{
				ProcessStuff( inner );
			}
		}

		ArrayList m_list = new ArrayList();
		protected void GenerateForClass( Hashtable htDefs, Hashtable htUses, ClassComponent cls )
		{
			ArrayList uses = new ArrayList();
			ArrayList defs = new ArrayList();
			TemporaryFieldComponent temp = new TemporaryFieldComponent( cls, cls.Name, cls.FullName );
			foreach( FieldComponent field in htUses.Keys )
			{
				if( (ArrayList)htUses[ field ] != null )
				{
					foreach( object obj in (ArrayList)htUses[ field ] )
					{
						if( ! uses.Contains( obj ) )
						{
							uses.Add( obj );
						}
					}
				}
				if( (ArrayList)htDefs[ field ] != null )
				{
					foreach( object obj in (ArrayList)htDefs[ field ] )
					{
						if( ! defs.Contains( obj ) )
						{
							defs.Add( obj );
						}
					}
				}
			}
			temp.Uses = uses;
			temp.Defs = defs;
			if( temp.Uses.Count > 0 || temp.Defs.Count > 0 )
                m_list.Add( temp );
		}

		#region Uses

		public Hashtable SearchUses( ClassComponent cls )
		{
			Hashtable htUses = new Hashtable();
			foreach( MethodComponent method in cls.Methods )
			{
				int i = 0;
				foreach( LineModel line in method.Lines )
				{
					CollectTemporaryFieldUses( htUses, cls, method, line, method.TextLines[i] );
					i++;
				}
			}
			return htUses;
		}

		protected void CollectTemporaryFieldUses( Hashtable htUses, ClassComponent cls, MethodComponent m, LineModel line, string strLine )
		{
			if( line.HasConditional )
			{
				foreach( FieldComponent field in cls.Fields )
				{
					if( strLine.IndexOf( field.Name ) > -1)
					{
						if( htUses[ field ] == null )
						{
							htUses[ field ] = new ArrayList();
						}
						if( ! ((ArrayList)htUses[ field ]).Contains( m ) )
						{
							((ArrayList)htUses[ field ]).Add( m );
						}
					}
				}
			}
		}
		#endregion

		#region Defs

		public Hashtable SearchDefs( ClassComponent cls, Hashtable htUses )
		{
			Hashtable htDefs = new Hashtable();
			foreach( MethodComponent method in cls.Methods )
			{
				int i = 0;
				foreach( LineModel line in method.Lines )
				{
					CollectTemporaryFieldDefs( htDefs, htUses, method, line, method.TextLines[i] );
					i++;
				}
			}
			return htDefs;
		}

		protected void CollectTemporaryFieldDefs( Hashtable htDefs, Hashtable htUses, MethodComponent m, LineModel line, string strLine )
		{
			if( line.HasCode && line.HasAssignment )
			{
				foreach( FieldComponent tempfield in htUses.Keys )
				{
					int idx = strLine.IndexOf( tempfield.Name );
					if( idx > -1 && idx < strLine.IndexOf("=") )
					{
						if( htDefs[ tempfield ] == null )
						{
							htDefs[ tempfield ] = new ArrayList();
						}
						if( ! ((ArrayList)htDefs[ tempfield ] ).Contains( m ) )
						{
							((ArrayList)htDefs[ tempfield ]).Add( m );
						}
					}
				}
			}
		}
		#endregion
	}

	public class TemporaryFieldComponent : AbstractComponent
	{
		public ClassComponent ClassComponent;
		public ArrayList Uses = new ArrayList();
		public ArrayList Defs = new ArrayList();
		public TemporaryFieldComponent( ClassComponent unit, string name, string fullName ) : base( fullName, name )
		{
			this.ClassComponent = unit;
		}
		public override void Visit(AbstractComponent component)
		{
		}
	}

	public class TemporaryFieldComparer : IComparer
	{
		public enum SortDirection
		{
			Ascending,
			Descending
		}

		SortDirection m_direction;
		public TemporaryFieldComparer( SortDirection direction )
		{
			m_direction = direction;
		}
		#region IComparer Members

		public int Compare(object x, object y)
		{
			TemporaryFieldComponent xx = (TemporaryFieldComponent)x;
			TemporaryFieldComponent yy = (TemporaryFieldComponent)y;
			int left = xx.Uses.Count + xx.Defs.Count;
			int value = left.CompareTo( yy.Uses.Count + yy.Defs.Count );
			return value * (m_direction == SortDirection.Ascending ? 1 : -1);
		}

		#endregion
	}
}
