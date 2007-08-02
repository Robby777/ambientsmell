using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using SwiftVisLib;

using CodeModel.Components;
using CodeModel.Representation;
using SmellDetectors;

using EnvDTE;
using Extensibility;


namespace SmellVisualizations
{
	/// <summary>
	/// Summary description for TemporaryFieldVisualization.
	/// </summary>
	public class SwitchStatementVisualization : BaseVisualization
	{ 
		private MovieClip m_panelClip;

		public SwitchStatementVisualization()
		{
			this.Width  = 1248;
			this.Height = 768;

			m_panelClip = new MovieClip( 0,0, this.Size );
			m_panelClip.Attach( this );
			
			m_panelClip.MouseClick+=new MouseEventHandler(OnMouseClickMethod);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			m_panelClip.Render( e.Graphics );
		}

		public void LoadResults( ArrayList switchStatements )
		{
			DoGraphics( switchStatements );
		}

		Brush m_backBrush;
		
		protected void DoGraphics( ArrayList switchStatements )
		{
			m_panelClip.Graphics.FillRectangle( Brushes.Black, m_panelClip.Rect );

			int w = 150;
			int h = 25;
			m_backBrush  = new SolidBrush( Color.FromArgb( 60, 60, 60 ) );
			
			foreach( SwitchStatementComponent swtch in switchStatements )
			{
				CreateMethodClip( swtch, 0, 0, w, h );
			}
			Space( 10, 10, m_panelClip.Children, this.Width );
		}
		protected void CreateMethodClip( SwitchStatementComponent swtch, 
			int x, int y, int w, int h )
		{
			MovieClip clip = m_panelClip.CreateSubMovieClip( x, y, w, h );
			clip.Object = swtch.MethodComponent;
			// Back rectangle
			clip.Graphics.FillRectangle( m_backBrush, 0, 0, w, h);

			// Dots
			MovieClip dots = clip.CreateSubMovieClip( (3*w)/4, 0, w/4, h );
			dots.Object = swtch.MethodComponent;
			for( int d = 0; d < swtch.CaseCount; d++ )
			{   // extra spacing in between disconnected chains?
				MovieClip dot = dots.CreateSubMovieClip( 0, 0, 4, 4 );
				dot.Graphics.FillEllipse( Brushes.Yellow, 0, 0, 3, 3 );
			}
			Space( 2, 2, dots.Children, dots.Width );

			// Text
			clip.LeftString( swtch.Name + " (" + swtch.Count + ")", Brushes.White );
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose (disposing);
			if( m_backBrush != null )
			{
				m_backBrush.Dispose();
				m_backBrush = null;
			}
		}
	}
}
