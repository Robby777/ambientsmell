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
	/// Summary description for MessageChainsPanel.
	/// </summary>
	public class MessageChainsPanel : AbstractSmellPanel
	{
		private System.Windows.Forms.TextBox txtChainLength;
		private System.Windows.Forms.Label label1;
	
		public MessageChainsPanel()
		{
			InitializeComponent();
		}
		
		public override void ProcessFindParameters(SolutionComponent solution)
		{
			ArrayList results = null;
			DetectMessageChains detect = new DetectMessageChains();
				
			results = detect.Search( solution );
	
			if( results != null && results.Count > 0 )
			{
				MessageChainsVisualization vis = new MessageChainsVisualization();
				vis.LoadResults( results );
				vis.Show();
			}
			else
			{
				MessageBox.Show( "No results found!" );
			}	
		}


		private void InitializeComponent()
		{
			this.txtChainLength = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtChainLength
			// 
			this.txtChainLength.Location = new System.Drawing.Point(88, 22);
			this.txtChainLength.Name = "txtChainLength";
			this.txtChainLength.TabIndex = 0;
			this.txtChainLength.Text = "3";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(32, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(48, 16);
			this.label1.TabIndex = 1;
			this.label1.Text = "chain >";
			// 
			// MessageChainsPanel
			// 
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtChainLength);
			this.Name = "MessageChainsPanel";
			this.Size = new System.Drawing.Size(264, 88);
			this.ResumeLayout(false);

		}
	}
}