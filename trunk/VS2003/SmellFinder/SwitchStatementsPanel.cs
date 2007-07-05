using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;

using CodeModel.Components;

using SmellDetectors;
using SmellVisualizations;

namespace SmellFinder
{
	/// <summary>
	/// Summary description for TemporaryFieldPanel.
	/// </summary>
	public class SwitchStatementsPanel : AbstractSmellPanel
	{
		private System.Windows.Forms.Label label1;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SwitchStatementsPanel()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
		}

		public override void ProcessFindParameters(SolutionComponent solution)
		{
			ArrayList results = null;
			SwitchStatementDetector detect = new SwitchStatementDetector();
				
			results = detect.Search( solution );
	
			if( results != null && results.Count > 0 )
			{
				SwitchStatementVisualization vis = new SwitchStatementVisualization();
				vis.LoadResults( results );
				vis.Show();
			}
			else
			{
				MessageBox.Show( "No results found!" );
			}
		}


		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(40, 40);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(144, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "About Switch Statements";
			// 
			// SwitchStatementsPanel
			// 
			this.ClientSize = new System.Drawing.Size(288, 65);
			this.Controls.Add(this.label1);
			this.Name = "SwitchStatementsPanel";
			this.ResumeLayout(false);

		}
		#endregion
	}
}
