using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

using CodeModel.Components;
using CodeModel.Representation;

using LCS;

namespace SmellDetectors
{
	/// <summary>
	/// Summary description for MessageChains.
	/// </summary>
	public class ParallelInheritanceHierarchyDetector : Detector
	{
		public ParallelInheritanceHierarchyDetector()
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
						Search( cls );
					}
				}
			}

			GatherParallel();
			//ArrayList list = new ArrayList( m_htMessageChains.Values );
			//list.Sort( new ParallelInheritanceHierarchyComponent.ParallelInheritanceHierarchyComparer( 
			//	ParallelInheritanceHierarchyComponent.ParallelInheritanceHierarchyComparer.SortDirection.Descending ) );
			return null;
		}

		public void GatherParallel()
		{
			// hierch
			Hashtable htPar = new Hashtable();

			LCS.LCSFinder f = new LCSFinder();

			foreach( string cls in m_htClassHierchs.Keys )
			{

				//ClassHierarchy ch = m_htClassHierchs[ cls ];
				
			}
		}

		public class ClassHierarchy
		{
			public string Parent;
			public ArrayList Children = new ArrayList();
		}

		Hashtable m_htClassHierchs = new Hashtable();

		public void Search( ClassComponent cls )
		{
			if( cls.CodeClass.Children.Count > 0 )
			{
				ClassHierarchy ch = new ClassHierarchy();
				ch.Parent = cls.Name;
				foreach( EnvDTE.CodeClass child in cls.CodeClass.Children )
				{
					ch.Children.Add( child.Name );
				}
				m_htClassHierchs[ cls.CodeClass ] = ch;
			}
		
			foreach( ClassComponent inner in cls.InnerClasses )
			{
				Search( inner );
			}
		}
	}
	

	public class ParallelInheritanceHierarchyComponent : AbstractComponent
	{
		public ParallelInheritanceHierarchyComponent( string component, int length ) : base( component, component )
		{
		}
		public int ClassCount;
		public int PrefixCount;
		public override void Visit(AbstractComponent component)
		{

		}

		public class ParallelInheritanceHierarchyComparer : IComparer
		{
			public enum SortDirection
			{
				Ascending,
				Descending
			}

			SortDirection m_direction;
			public ParallelInheritanceHierarchyComparer( SortDirection direction )
			{
				m_direction = direction;
			}
			#region IComparer Members

			public int Compare(object x, object y)
			{
				ParallelInheritanceHierarchyComponent xx = (ParallelInheritanceHierarchyComponent)x;
				ParallelInheritanceHierarchyComponent yy = (ParallelInheritanceHierarchyComponent)y;
				int value = xx.ClassCount.CompareTo( yy.ClassCount);
				if( value == 0 )
				{
					value = xx.PrefixCount.CompareTo( yy.PrefixCount );
				}
				return value * (m_direction == SortDirection.Ascending ? 1 : -1);
			}

			#endregion
		}
	}
}
