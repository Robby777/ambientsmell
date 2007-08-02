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
	public class SwitchStatementDetector : Detector
	{
		public SwitchStatementDetector()
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

			m_list.AddRange( m_htTypeCodes.Values );
			m_list.Sort( new SwitchStatementComparer( SwitchStatementComparer.SortDirection.Descending ) );
			return m_list;
		}
	
		ArrayList m_list = new ArrayList();
		Hashtable m_htTypeCodes = new Hashtable();

		public void ProcessStuff( ClassComponent cls )
		{
			foreach( MethodComponent method in cls.Methods )
			{
				int i = 0;
				
				SwitchStatementComponent current = null;
				foreach( LineModel line in method.Lines )
				{
					if( line.HasSwitch )
					{
						string sw = method.TextLines[i];
						int start = sw.IndexOf( "(" ) + 1;
						int end   = sw.LastIndexOf( ")" );
						string typeCode = sw.Substring( start, end - start );
						if( ! m_htTypeCodes.ContainsKey( typeCode ) )
						{
							m_htTypeCodes[ typeCode ] = new SwitchStatementComponent( method, typeCode );
						}
						current = (SwitchStatementComponent)m_htTypeCodes[ typeCode ];
						current.Count++;
						
					}
					if( line.HasCase && current != null && current.Count == 1)
					{
						current.CaseCount++;
					}
					i++;
				}
			}
		
			foreach( ClassComponent inner in cls.InnerClasses )
			{
				ProcessStuff( inner );
			}
		}
	}

	public class SwitchStatementComponent : AbstractComponent
	{
		public MethodComponent MethodComponent;
		public int CaseCount = 0;
		public int Count = 0;
		public SwitchStatementComponent( MethodComponent unit, string name) : base( name, name )
		{
			this.MethodComponent = unit;
		}
		public override void Visit(AbstractComponent component)
		{
		}
	}

	public class SwitchStatementComparer : IComparer
	{
		public enum SortDirection
		{
			Ascending,
			Descending
		}

		SortDirection m_direction;
		public SwitchStatementComparer( SortDirection direction )
		{
			m_direction = direction;
		}
		#region IComparer Members

		public int Compare(object x, object y)
		{
			SwitchStatementComponent xx = (SwitchStatementComponent)x;
			SwitchStatementComponent yy = (SwitchStatementComponent)y;
			int value = xx.Count.CompareTo( yy.Count );
			if( value == 0 )
				value = xx.CaseCount.CompareTo( yy.CaseCount );
			return value * (m_direction == SortDirection.Ascending ? 1 : -1);
		}

		#endregion
	}
}
