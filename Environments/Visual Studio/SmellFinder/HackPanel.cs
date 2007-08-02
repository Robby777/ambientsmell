using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;


using CodeModel.Components;
using CodeModel.Representation;
using SmellDetectors;

using EnvDTE;
using Extensibility;

using SmellVisualizations;
namespace SmellFinder
{
	/// <summary>
	/// Summary description for HackPanel.
	/// </summary>
	public class HackPanel : AbstractSmellPanel
	{
		public HackPanel() 
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override void ProcessFindParameters(SolutionComponent solution)
		{
			HackDetector det = new HackDetector();
			ArrayList list = det.Search( solution );

			HackVisualization vis = new HackVisualization();
			vis.LoadResults( null /*list*/ );
			vis.Show();
		}
	}
}
