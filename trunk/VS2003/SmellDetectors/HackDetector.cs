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
	public class HackDetector : Detector
	{
		public HackDetector()
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

		public int ClassCount = 0;
		public int Lines = 0;
		public void ProcessStuff( ClassComponent cls )
		{
			ClassCount++;
			foreach( MethodComponent meth in cls.Methods )
			{
				Lines += meth.Lines.Count;
			}
			foreach( ClassComponent inner in cls.InnerClasses )
			{
				ProcessStuff( inner );
				ClassCount++;
			}
		}

		ArrayList m_list = new ArrayList();
	}

	public class HackComponent : AbstractComponent
	{
		public ArrayList Uses = new ArrayList();
		public ArrayList Defs = new ArrayList();
		public HackComponent( string name, string fullName ) : base( fullName, name )
		{

		}
		public override void Visit(AbstractComponent component)
		{
		}
	}

	public class HackComparer : IComparer
	{
		public enum SortDirection
		{
			Ascending,
			Descending
		}

		SortDirection m_direction;
		public HackComparer( SortDirection direction )
		{
			m_direction = direction;
		}
		#region IComparer Members

		public int Compare(object x, object y)
		{
			HackComponent xx = (HackComponent)x;
			HackComponent yy = (HackComponent)y;
			int left = xx.Uses.Count + xx.Defs.Count;
			int value = left.CompareTo( yy.Uses.Count + yy.Defs.Count );
			return value * (m_direction == SortDirection.Ascending ? 1 : -1);
		}

		#endregion
	}
}
