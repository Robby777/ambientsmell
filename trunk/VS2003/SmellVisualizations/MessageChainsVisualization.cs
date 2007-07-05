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
	public class MessageChainsVisualization : BaseVisualization
	{
		private MovieClip m_panelClip;

		public MessageChainsVisualization()
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
			
			foreach( MessageChainComponent chain in chains )
			{
				CreateMethodClip( chain, 0, 0, w, h );
			}
			Space( 10, 10, m_panelClip.Children, this.Width );
		}
		protected void CreateMethodClip( MessageChainComponent chain, 
			int x, int y, int w, int h )
		{
			MovieClip clip = m_panelClip.CreateSubMovieClip( x, y, w, h );
			clip.Object = chain;
			// Back rectangle
			clip.Graphics.FillRectangle( m_backBrush, 0, 0, w, h);

			// Dots
			MovieClip dots = clip.CreateSubMovieClip( 0, 0, w/4, h );
			//dots.Graphics.FillRectangle( Brushes.Wheat, 0, 0, w/4, h );
			for( int d = 0; d < chain.Length; d++ )
			{   // extra spacing in between disconnected chains?
				MovieClip dot = dots.CreateSubMovieClip( 0, 0, 10, 10 );
				dot.Graphics.FillEllipse( Brushes.LightGreen, 0, 0, 9, 9 );
			}
			Space( 3, 3, dots.Children, dots.Width );

			// Text
			MovieClip text = clip.CreateSubMovieClip( dots.Width, 0, w - dots.Width, h );
			text.LeftString( "(" + chain.Count +")  " + chain.MessageChain, Brushes.White);
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