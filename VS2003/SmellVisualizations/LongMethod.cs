using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using SwiftVisLib;

using CodeModel.Components;
using CodeModel.Representation;

namespace SmellVisualizations
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class LongMethodVisualization : BaseVisualization
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		private MovieClip m_panelClip;

		private Brush m_backBrush;
		private Brush m_smellBrush;

		public void LoadResults( ArrayList results, int maxLength )
		{
			DoGraphics( results, maxLength );
		}

		public LongMethodVisualization()
		{
			InitializeComponent();
			this.Width  = 800;
			this.Height = 600;

			m_panelClip = new MovieClip( 0,0, this.Size );
			m_panelClip.Attach( this );
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			m_panelClip.Render( e.Graphics );
		}

		protected void DoGraphics( ArrayList methods, int maxLength )
		{
			m_panelClip.Graphics.FillRectangle( Brushes.Black, m_panelClip.Rect );

			m_panelClip.MouseClick+=new MouseEventHandler(OnMouseClickMethod);

			int w = 150;
			int h = 25;
			int x_spacing = 10;
			int y_spacing = 10;
			int x = x_spacing;
			int y = y_spacing;
			
			m_backBrush  = new SolidBrush( Color.FromArgb( 60, 60, 60 ) );
			m_smellBrush = new SolidBrush( Color.FromArgb( 120, 120, 120 ) );

			foreach( MethodComponent method in methods )
			{
				CreateMethodClip( method, maxLength, x, y, w, h );
				x += w + x_spacing;
				if( x + w > this.Width )
				{
					x = x_spacing;
					y += h + y_spacing;
				}
			}
		}

		protected void CreateMethodClip( MethodComponent method, int maxLength, 
			int x, int y, int w, int h )
		{
			MovieClip clip = m_panelClip.CreateSubMovieClip( x, y, w, h );
			clip.Object = method;
			// Back rectangle
			clip.Graphics.FillRectangle( m_backBrush, 0, 0, w, h);

			double length = method.Lines.Count;
			double smell_width = w *(length / maxLength);
			
			// Smell rectangle
			clip.Graphics.FillRectangle( m_smellBrush, 0, 0, (int)smell_width, h );

			// Iteration/Conditional/Comment markings
			int lineCount = 0;
			foreach( LineModel model in method.Lines )
			{
				Pen pen = GetPen( model );
				// Hack
				if( pen != null )
				{
					double ratio = lineCount/length;
					int mark_x = (int)(ratio*smell_width);
					clip.Graphics.DrawLine( pen, mark_x, 0, mark_x, h );
				}
				lineCount++;
			}
			clip.CenteredString( method.Name + "() " + method.Lines.Count );
		}

		protected Pen GetPen( LineModel model )
		{
			if( model.HasComment )
				return Pens.LightGreen;
			if( model.HasIteration )
				return Pens.LightBlue;
			if( model.HasConditional )
				return Pens.Blue;
			return null;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}

			if( m_backBrush != null )
			{
				m_backBrush.Dispose();
				m_backBrush = null;
			}
			if( m_smellBrush != null )
			{
				m_smellBrush.Dispose();
				m_smellBrush = null;
			}

			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.Size = new System.Drawing.Size(300,300);
			this.Text = "Form1";
		}
		#endregion
	}
}
