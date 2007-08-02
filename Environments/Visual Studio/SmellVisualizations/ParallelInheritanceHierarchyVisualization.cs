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
	/// Summary description for MessageChainsVisualization.
	/// </summary>
	public class ParallelInheritanceHierarchyVisualization : BaseVisualization
	{
		private MovieClip m_panelClip;

		public ParallelInheritanceHierarchyVisualization()
		{
			this.Width  = 1248;
			this.Height = 768;

			m_panelClip = new MovieClip( 0,0, this.Size );
			m_panelClip.Attach( this );
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			m_panelClip.Render( e.Graphics );
		}

		public void LoadResults( ArrayList chains )
		{
			DoGraphics( chains );
		}

		Brush m_backBrush;
		
		
		

		protected void DoGraphics( ArrayList chains )
		{
			m_panelClip.Graphics.FillRectangle( Brushes.Black, m_panelClip.Rect );

			int w = 350;
			int h = 50;
			m_backBrush  = new SolidBrush( Color.FromArgb( 60, 60, 60 ) );
			

			string[] prefix = new string[]{"LongMethod", "TemporaryField", "MessageChains"};
			string[] baseCls = new string[]{"Detector", "Visualization", "Component", "Comparer"};

			int i = 0;

			MovieClip clip = m_panelClip.CreateSubMovieClip( 0, 0, w, h );
			foreach( string bass in baseCls)
			{
				CreateClassHier( clip, bass, prefix, 0, 0, w/4, h );
				i++;
			}
			MovieClip legend = clip.CreateSubMovieClip( (3*w)/4, 0, w/4, h );
			Brush[] brushes = new Brush[]{Brushes.LightGreen,Brushes.LightBlue, Brushes.LightPink};
			for( int d = 0; d < prefix.Length; d++ )
			{   // extra spacing in between disconnected chains?
				MovieClip index = legend.CreateSubMovieClip( 0, 0, w/4, h/3 );
				MovieClip dot = index.CreateSubMovieClip( 0, 0, 10, 10 );
				dot.Graphics.FillEllipse( brushes[d], 0, 0, 9, 9 );
				MovieClip text = index.CreateSubMovieClip( 10, 0, w/4 - 10, h/3 );
				text.LeftString( prefix[d], Brushes.White);
			}
			Space( 4, 1, legend.Children, w/4 );
			Space( 10, 10, clip.Children, this.Width );
		}
		protected void CreateClassHier( MovieClip parent, string pre, string[] baseCls,
			int x, int y, int w, int h )
		{
			MovieClip clip = parent.CreateSubMovieClip( x, y, w, h );
			//clip.Object = chain;
			// Back rectangle
			clip.Graphics.FillRectangle( m_backBrush, 0, 0, w, h);

			// Parent Class
			MovieClip par = clip.CreateSubMovieClip( 0, 0, w, h/2 );

			MovieClip dot = par.CreateSubMovieClip( w/2, h/4, 10, 10 );
			dot.Graphics.FillEllipse( Brushes.White, 0, 0, 9, 9 );
			par.LeftString( pre, Brushes.White );

			// Children
			MovieClip dots = clip.CreateSubMovieClip( 0, h/2, w, h/2 );
			//dots.Graphics.FillRectangle( Brushes.Wheat, 0, 0, w/4, h );

			int centerHack = 29;
			Brush[] brushes = new Brush[]{Brushes.LightGreen,Brushes.LightBlue, Brushes.LightPink};
			for( int d = 0; d < baseCls.Length; d++ )
			{   // extra spacing in between disconnected chains?
				MovieClip dd = dots.CreateSubMovieClip( 0, 0, 10, 10 );
				dd.Graphics.FillEllipse( brushes[d], 0, 0, 9, 9 );

				clip.Graphics.DrawLine( Pens.White, w/2+4, h/4+3, (centerHack-2) + ((1+d)*10), h/2 + 5 );
			}
			Space( centerHack, 3, 3, 3, dots.Children, dots.Width );

			
			// Text
			//MovieClip text = clip.CreateSubMovieClip( dots.Width, 0, w - dots.Width, h );
			//text.LeftString( "(" + chain.Count +")  " + chain.MessageChain, Brushes.White);
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