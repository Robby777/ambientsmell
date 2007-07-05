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
	/// Summary description for LongMethodPanel.
	/// </summary>
	public class LongMethodPanel : AbstractSmellPanel
	{
		private System.Windows.Forms.TextBox txtIterations;
		private System.Windows.Forms.TextBox txtConditionals;
		private System.Windows.Forms.TextBox txtMinLen;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ComboBox cmbLocations;
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LongMethodPanel()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			txtMinLen.Text       = "100";
			txtConditionals.Text = "1";
			txtIterations.Text   = "1";

			cmbLocations.Items.Add( "Entire Solution" );
			cmbLocations.Items.Add( "Current Project" );
			cmbLocations.Items.Add( "Current File" );
			cmbLocations.Items.Add( "Current Context" );
			cmbLocations.SelectedIndex = 0;
		}

		public override void ProcessFindParameters( SolutionComponent solution )
		{
			int min = TryParse( txtMinLen.Text );
			int iter = TryParse( txtIterations.Text );
			int cond = TryParse( txtConditionals.Text );

			if( min < 0 || iter < 0 || cond < 0 )
			{
				MessageBox.Show( "Enter an integer greater than 0" );
				return;
			}

			ArrayList results = null;
			DetectLongMethod detect = new DetectLongMethod( min, iter, cond );
			
			//if( cmbLocations.SelectedItem.Equals( "Entire Solution" ) )
			{		
				results = detect.Search( solution );
			}
			
			if( results != null && results.Count > 0 )
			{
				LongMethodVisualization vis = new LongMethodVisualization();
				SortMethods( results );
				int maxLength = ((MethodComponent)results[0]).Lines.Count;
				vis.LoadResults( results, maxLength );
				vis.Show();
			}
			else
			{
				MessageBox.Show( "No results found!" );
			}
		}

		protected void SortMethods( ArrayList results )
		{
			results.Sort( new MethodLengthComparer( MethodLengthComparer.SortDirection.Descending ) );
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
			this.txtIterations = new System.Windows.Forms.TextBox();
			this.txtConditionals = new System.Windows.Forms.TextBox();
			this.txtMinLen = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.cmbLocations = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// txtIterations
			// 
			this.txtIterations.Location = new System.Drawing.Point(272, 48);
			this.txtIterations.Name = "txtIterations";
			this.txtIterations.Size = new System.Drawing.Size(64, 20);
			this.txtIterations.TabIndex = 20;
			this.txtIterations.Text = "";
			// 
			// txtConditionals
			// 
			this.txtConditionals.Location = new System.Drawing.Point(112, 48);
			this.txtConditionals.Name = "txtConditionals";
			this.txtConditionals.Size = new System.Drawing.Size(56, 20);
			this.txtConditionals.TabIndex = 17;
			this.txtConditionals.Text = "";
			// 
			// txtMinLen
			// 
			this.txtMinLen.Location = new System.Drawing.Point(112, 8);
			this.txtMinLen.Name = "txtMinLen";
			this.txtMinLen.Size = new System.Drawing.Size(120, 20);
			this.txtMinLen.TabIndex = 15;
			this.txtMinLen.Text = "";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(192, 48);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(64, 16);
			this.label5.TabIndex = 19;
			this.label5.Text = "Iterations >";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 48);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(80, 16);
			this.label4.TabIndex = 18;
			this.label4.Text = "Conditionals >";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 8);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(100, 16);
			this.label3.TabIndex = 16;
			this.label3.Text = "Method Length >";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(8, 88);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.TabIndex = 14;
			this.label2.Text = "Look in:";
			// 
			// cmbLocations
			// 
			this.cmbLocations.ItemHeight = 13;
			this.cmbLocations.Location = new System.Drawing.Point(112, 88);
			this.cmbLocations.Name = "cmbLocations";
			this.cmbLocations.Size = new System.Drawing.Size(121, 21);
			this.cmbLocations.TabIndex = 13;
			// 
			// LongMethodPanel
			// 
			this.Controls.Add(this.txtIterations);
			this.Controls.Add(this.txtConditionals);
			this.Controls.Add(this.txtMinLen);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cmbLocations);
			this.Name = "LongMethodPanel";
			this.Size = new System.Drawing.Size(352, 120);
			this.ResumeLayout(false);

		}
		#endregion
	}
}
