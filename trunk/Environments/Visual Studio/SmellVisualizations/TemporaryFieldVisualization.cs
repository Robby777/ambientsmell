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
	public class TemporaryFieldVisualization : BaseVisualization
	{ 
		private MovieClip m_panelClip;

		public TemporaryFieldVisualization()
		{
			this.Width  = 1248;
			this.Height = 768;

			m_panelClip = new MovieClip( 0,0, this.Size );
			m_panelClip.Attach( this );
			
			m_panelClip.MouseClick+=new MouseEventHandler(OnMouseClickClass);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			m_panelClip.Render( e.Graphics );
		}

		public void LoadResults( ArrayList temporaryFields )
		{
			DoGraphics( temporaryFields );
		}

		Brush m_backBrush;
		
		protected void DoGraphics( ArrayList temporaryFields )
		{
			m_panelClip.Graphics.FillRectangle( Brushes.Black, m_panelClip.Rect );

			int w = 350;
			int h = 50;
			m_backBrush  = new SolidBrush( Color.FromArgb( 60, 60, 60 ) );
			
			foreach( TemporaryFieldComponent temp in temporaryFields )
			{
				CreateMethodClip( temp, 0, 0, w, h );
			}
			Space( 10, 10, m_panelClip.Children, this.Width );
		}
		protected void CreateMethodClip( TemporaryFieldComponent temp, 
			int x, int y, int w, int h )
		{
			MovieClip clip = m_panelClip.CreateSubMovieClip( x, y, w, h );
			clip.Object = temp.ClassComponent;
			// Back rectangle
			clip.Graphics.FillRectangle( m_backBrush, 0, 0, w, h);

			// Dots
			MovieClip dots = clip.CreateSubMovieClip( 0, 0, w, h/2 );
			dots.Object = temp.ClassComponent;
			for( int d = 0; d < temp.Uses.Count; d++ )
			{   // extra spacing in between disconnected chains?
				MovieClip dot = dots.CreateSubMovieClip( 0, 0, 10, 10 );
				dot.Graphics.FillEllipse( Brushes.Yellow, 0, 0, 9, 9 );
			}
			Space( 3, 3, dots.Children, dots.Width );

			// Dots 2
			MovieClip dots2 = clip.CreateSubMovieClip( 0, h/2, w, h/2 );
			dots2.Object = temp.ClassComponent;
			for( int d = 0; d < temp.Defs.Count; d++ )
			{   // extra spacing in between disconnected chains?
				MovieClip dot = dots2.CreateSubMovieClip( 0, 0, 10, 10 );
				dot.Graphics.FillEllipse( Brushes.LightBlue, 0, 0, 9, 9 );
				//Brushes.LemonChiffon
			}
			Space( 3, 3, dots2.Children, dots2.Width );

			// Text
			clip.CenteredString( temp.Name );
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
