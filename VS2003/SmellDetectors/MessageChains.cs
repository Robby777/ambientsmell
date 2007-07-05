using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

using CodeModel.Components;
using CodeModel.Representation;

namespace SmellDetectors
{
	/// <summary>
	/// Summary description for MessageChains.
	/// </summary>
	public class DetectMessageChains : Detector
	{
		public DetectMessageChains()
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
			ArrayList list = new ArrayList( m_htMessageChains.Values );
			list.Sort( new MessageChainComponent.MessageChainComparer( MessageChainComponent.MessageChainComparer.SortDirection.Descending ) );
			return list;
		}

		public void Search( ClassComponent cls )
		{
			foreach( MethodComponent method in cls.Methods )
			{
				int i = 0;
				foreach( LineModel line in method.Lines )
				{
					if( line.HasCode )
					{
						string strLine = method.TextLines[i];
						//MakeMessageChainComponent( strLine );
						MakeChain( strLine );
					}
					i++;
				}
			}

			foreach( ClassComponent inner in cls.InnerClasses )
			{
				Search( inner );
			}
		}

		Hashtable m_htMessageChains = new Hashtable();
		public void MakeMessageChainComponent( string strLine )
		{
			string method = @"\w+\s*([(][^)]*[)])?";
			string chain = "(" + method + "[.])*" + method;
			Match m = Regex.Match( strLine, chain, RegexOptions.Compiled );
			if( m.Success )
			{
				string strChain = m.Groups[1].Value;

				int periodCount = CountPeriod( strChain );
				if( periodCount > 3 )
				{
					if( ! m_htMessageChains.ContainsKey( strChain ) )
					{
						m_htMessageChains[ strChain ] = new MessageChainComponent( strChain, periodCount );
					}
					MessageChainComponent comp = (MessageChainComponent)m_htMessageChains[ strChain ];
					comp.Count++;
				}
			}
		}

		public void MakeChain( string line )
		{
			//Match m = Regex.Match( strLine, chain, RegexOptions.Compiled );
			//if( m.Success )
			//{
			Hashtable bounds = Boundary.Boundaries( line );
			foreach( ArrayList depth in bounds.Values )
			{
				string dLine = "";
				int index = 0;
				int lastIndex = 0;
				foreach( Boundary b in depth )
				{
					dLine += line.Substring( index, (b.start-index)+1 );
					if( b.end > index )
					{
						index = b.end;
						lastIndex = b.end;
					}
				}
				if( lastIndex < line.Length - 1)
				{
					dLine += line.Substring( lastIndex, line.Length - lastIndex );
				}
				string chainLine = GetChainPart( dLine );
				int periodCount = CountPeriod( chainLine );
				if( periodCount > 3 )
				{
					if( ! m_htMessageChains.ContainsKey( chainLine ) )
					{
						m_htMessageChains[ chainLine ] = new MessageChainComponent( chainLine, periodCount );
					}
					MessageChainComponent comp = (MessageChainComponent)m_htMessageChains[ chainLine ];
					comp.Count++;
				}
			}
		}

		protected string GetChainPart( string strLine )
		{
			string method = @"\w+\s*([(][^)]*[)])?";
			string chain = "(" + method + "[.])*" + method;
			Match m = Regex.Match( strLine, chain, RegexOptions.Compiled );
			if( m.Success )
			{
				return m.Groups[0].Value;
			}
			return "";
		}

		// instead, just directly process string, assign each string to depth level.
		public class Boundary
		{
			public int start;
			public int end;
			public Boundary( int s, int e )
			{
				this.start = s;
				this.end   = e;
			}
			static public Hashtable Boundaries( string line )
			{
				Hashtable ht = new Hashtable();
				int depth = 0;
				int index = 0;
				Boundary b = null;
				foreach( char c in line )
				{
					if( c == '(' )
					{
						//ht[ depth ] =
						b = new Boundary(0,0);
						b.start = index;
						depth++;
					}
					if( c == ')' && b != null )
					{
						if( ht[depth] == null )
							ht[depth] = new ArrayList();
						b.end = index;
						((ArrayList)ht[depth]).Add( b );
						depth--;
						b = null;
					}
					index++;
				}
				return ht;
			}
		}

		public int CountPeriod( string strChain )
		{
			int count = 0;
			foreach( char c in strChain )
			{
				if( c == '.' )
					count++;
			}
			return count;
		}
	}
	

	public class MessageChainComponent : AbstractComponent
	{
		public MessageChainComponent( string component, int length ) : base( component, component )
		{
			this.MessageChain = component;
			this.Length = length;
		}
		public string MessageChain;
		public int Length = 0;
		public int Count = 0;
		public override void Visit(AbstractComponent component)
		{

		}

		public class MessageChainComparer : IComparer
		{
			public enum SortDirection
			{
				Ascending,
				Descending
			}

			SortDirection m_direction;
			public MessageChainComparer( SortDirection direction )
			{
				m_direction = direction;
			}
			#region IComparer Members

			public int Compare(object x, object y)
			{
				MessageChainComponent xx = (MessageChainComponent)x;
				MessageChainComponent yy = (MessageChainComponent)y;
				int value = xx.Count.CompareTo( yy.Count );
				if( value == 0 )
				{
					value = xx.Length.CompareTo( yy.Length );
				}
				return value * (m_direction == SortDirection.Ascending ? 1 : -1);
			}

			#endregion
		}
	}
}
