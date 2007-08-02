using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;

using CodeModel.Components;
using CodeModel.Representation;

namespace SmellDetectors
{
	/// <summary>
	/// Summary description for Detect.
	/// </summary>
	public class DetectLongMethod : Detector
	{
		public DetectLongMethod( int methodLength, int iterationStmts, int condtionalStmts )
		{
			this.MethodLength     = methodLength;
			this.IterationStmts   = iterationStmts;
			this.ConditionalStmts = condtionalStmts;
		}

		public int MethodLength;
		public int IterationStmts;
		public int ConditionalStmts;

		public override ArrayList Search( SolutionComponent solution )
		{
			ArrayList list = new ArrayList();
			foreach( ProjectComponent project in solution.Projects )
			{
				foreach( FileComponent file in project.Files )
				{
					foreach( ClassComponent cls in file.Classes )
					{
						list.AddRange( Search( cls ) );
					}
				}
			}
			return list;
		}

		public ArrayList Search( ClassComponent cls )
		{
			ArrayList list = new ArrayList();
			foreach( MethodComponent method in cls.Methods )
			{
				if( method.Lines.Count > this.MethodLength  &&
					method.NumConditionalStmts > this.ConditionalStmts &&
					method.NumIterationStmts   > this.IterationStmts )
				{
					list.Add( method );
				}
			}

			foreach( ClassComponent inner in cls.InnerClasses )
			{
				list.AddRange( Search( inner ) );
			}

			return list;
		}

		// Update status
	}
}
