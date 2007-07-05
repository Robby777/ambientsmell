using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using EnvDTE;
using CodeModel.Components;
using CodeModel.Representation;
using CodeModel.VSModel;


namespace SmellFinder
{
	/// <summary>
	/// Summary description for TestForm.
	/// </summary>
	public class TestForm : System.Windows.Forms.Form
	{
		#region UI

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnFind;
		private System.Windows.Forms.ComboBox cmbSmells;
		private System.Windows.Forms.GroupBox gbProperties;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

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

		#endregion

		private _DTE m_appInst;
		public _DTE AppObject
		{
			set { m_appInst = value; }
		}
		SolutionComponent m_solution;

		public TestForm()
		{
			InitializeComponent();

			cmbSmells.Items.Add( "Long Methods" );
			cmbSmells.Items.Add( "Message Chains" );
			cmbSmells.Items.Add( "Temporary Field" );
			cmbSmells.Items.Add( "Switch Statements" );
			cmbSmells.Items.Add( "Duplicated Code" );
			cmbSmells.Items.Add( "PIH");
			cmbSmells.Items.Add( "Hack" );
			cmbSmells.SelectedIndex = 0;
		}

		private void LoadSolution()
		{
			if( m_appInst != null )
			{
				ModelBuilder mb = new ModelBuilder();
				m_solution = mb.CreateModel( m_appInst );
			}
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.cmbSmells = new System.Windows.Forms.ComboBox();
			this.btnFind = new System.Windows.Forms.Button();
			this.gbProperties = new System.Windows.Forms.GroupBox();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(24, 34);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "Find what smell:";
			// 
			// cmbSmells
			// 
			this.cmbSmells.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbSmells.Location = new System.Drawing.Point(128, 32);
			this.cmbSmells.Name = "cmbSmells";
			this.cmbSmells.Size = new System.Drawing.Size(121, 21);
			this.cmbSmells.TabIndex = 1;
			this.cmbSmells.SelectedIndexChanged += new System.EventHandler(this.cmbSmells_SelectedIndexChanged);
			// 
			// btnFind
			// 
			this.btnFind.Location = new System.Drawing.Point(368, 32);
			this.btnFind.Name = "btnFind";
			this.btnFind.TabIndex = 5;
			this.btnFind.Text = "Find";
			this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
			// 
			// gbProperties
			// 
			this.gbProperties.Location = new System.Drawing.Point(24, 64);
			this.gbProperties.Name = "gbProperties";
			this.gbProperties.Size = new System.Drawing.Size(424, 168);
			this.gbProperties.TabIndex = 6;
			this.gbProperties.TabStop = false;
			this.gbProperties.Text = "Search Properties";
			// 
			// TestForm
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(464, 249);
			this.Controls.Add(this.gbProperties);
			this.Controls.Add(this.btnFind);
			this.Controls.Add(this.cmbSmells);
			this.Controls.Add(this.label1);
			this.Name = "TestForm";
			this.Text = "SmellFinder";
			this.ResumeLayout(false);

		}
		#endregion


		private void btnFind_Click(object sender, System.EventArgs e)
		{
			try
			{
				LoadSolution();
				if( m_panel != null )
				{
					m_panel.ProcessFindParameters( m_solution );
				}
			}
			catch( Exception ex )
			{
				MessageBox.Show( ex.Message +"\r\n"+ex.StackTrace );
			}
		}

		private AbstractSmellPanel SelectPanel()
		{
			AbstractSmellPanel panel = null;
			if( cmbSmells.SelectedItem.Equals( "Long Methods" ) )
			{
				panel = new LongMethodPanel();
			}
			else if( cmbSmells.SelectedItem.Equals( "Message Chains" ) )
			{
				panel = new MessageChainsPanel();
			}
			else if( cmbSmells.SelectedItem.Equals( "Temporary Field" ) )
			{
				panel = new TemporaryFieldPanel();
			}
			else if( cmbSmells.SelectedItem.Equals( "Switch Statements") )
			{
				panel = new SwitchStatementsPanel();
			}
			else if( cmbSmells.SelectedItem.Equals( "PIH") )
			{
				panel = new ParallelInheritanceHierarchyPanel();
			}
			else if( cmbSmells.SelectedItem.Equals( "Hack") )
			{
				panel = new HackPanel();
			}
			return panel;
		}

		private AbstractSmellPanel m_panel = null;
		private void cmbSmells_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			m_panel = SelectPanel();
			gbProperties.Controls.Clear();
			if( m_panel != null )
			{
				gbProperties.Controls.Add( m_panel );
			}
		}
	}
}
