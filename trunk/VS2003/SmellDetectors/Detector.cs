using System;
using System.Collections;

namespace SmellDetectors
{
	/// <summary>
	/// Summary description for Detector.
	/// </summary>
	public abstract class Detector
	{
		protected Detector()
		{
		}

		public abstract ArrayList Search( CodeModel.Components.SolutionComponent solution );
	}
}
