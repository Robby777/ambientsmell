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
	public class HackVisualization : BaseVisualization
	{
		private MovieClip m_panelClip;
		Random m_random = new Random();
		public HackVisualization()
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

			//int w = 150;
			//int h = 25;
			int w = 500;
			int h = 80;
			m_backBrush  = new SolidBrush( Color.FromArgb( 60, 60, 60 ) );
			
			//CreateClassHier( 0, 0, w, h );
			//CreateInInt( 0, 0, w, h );
			//RefusedBequest( 0, 0, w, h );
			TaskView( 0, 0, w, h );
			Space( 10, 10, m_panelClip.Children, this.Width );
		}

		public void AlignedString(MovieClip mc, string s, Brush brush, StringAlignment align, float size)
		{
			StringFormat sf = new StringFormat();
			sf.Alignment = align;
			sf.LineAlignment = align;
			Font f = new Font("Times", size);
			mc.Graphics.DrawString(s, f, brush, new Rectangle(0, 0, (int) (mc.Width * mc.Xscale), (int) (mc.Height * mc.Yscale)), sf);
		
		}
		public void AlignedString(MovieClip mc, string s, Brush brush, StringAlignment align)
		{
			AlignedString( mc, s, brush, align, 12f );
		}

		protected void TaskView( int x, int y, int w, int h )
		{
			MovieClip taskView = m_panelClip.CreateSubMovieClip( x, y, w, h );
			MovieClip taskSumm = taskView.CreateSubMovieClip( 0, 0, w, h/2 );
			MovieClip session  = taskView.CreateSubMovieClip( 0, h/2, w, h/2 );
			TaskSummary( taskSumm, w, h/2 );
			SessionSummary( session, w, h/2, 5 );

			m_panelClip.MouseClick+=new MouseEventHandler(session_MouseClick);
		}

		protected void TaskSummary( MovieClip clip, int w, int h )
		{
			clip.Graphics.FillRectangle( m_backBrush, 0, 0, w, h);
			clip.Graphics.DrawRectangle( Pens.SlateGray, 0, 0, w-1, h-1 );
			
			// ID Zone
			MovieClip idLabel  = clip.CreateSubMovieClip( 0  , 0, w/12, h );
			AlignedString( idLabel, "T1", Brushes.White, StringAlignment.Center );
			MovieClip editNav  = clip.CreateSubMovieClip( w/12, 0, w/12, h );
			MovieClip edit = editNav.CreateSubMovieClip( 0, 0, w/24, h );
			MovieClip nav = editNav.CreateSubMovieClip( w/24, 0, w/24, h );

			//edit.Graphics.DrawRectangle( Pens.Sienna, 0, 0, edit.Width-1, edit.Height-1 );
			//nav.Graphics.DrawRectangle( Pens.Sienna, 0, 0, nav.Width-1, nav.Height-1 );
			
			int eb_h = (int)(h*.45);
			int eb_w = (int)(h*.45);
			GradientPainter.Fill3DSphere( edit, Color.Green, Color.White, edit.Width/2-eb_w/2, edit.Height/2-eb_h/2, eb_w, eb_h );
			edit.CenteredString( "33", Brushes.White );
			edit.Graphics.DrawEllipse( Pens.DarkGreen, edit.Width/2-eb_w/2, edit.Height/2-eb_h/2, eb_w, eb_h );

			GradientPainter.Fill3DSphere( nav, Color.Blue, Color.White, nav.Width/2-eb_w/2, nav.Height/2-eb_h/2, eb_w, eb_h );
			nav.CenteredString( "15", Brushes.White );
			nav.Graphics.DrawEllipse( Pens.DarkGreen, nav.Width/2-eb_w/2, nav.Height/2-eb_h/2, eb_w, eb_h );

			// TAG CLOUD
			MovieClip tagCloud = clip.CreateSubMovieClip( w/6, 0, w/2, h );
			//tagCloud.LeftString( "lock(3) DB(2)", Brushes.White );
			AlignedString( tagCloud, "AUTOTAGS:", Brushes.LightGray, StringAlignment.Near, 7 );
			AlignedString( tagCloud, "Lock(3), Oracle(2)", Brushes.White, StringAlignment.Center );
			
			MovieClip sessions = clip.CreateSubMovieClip( 2*w/3, 0, w/3, h );
			AlignedString( sessions, "SESSIONS:", Brushes.LightGray, StringAlignment.Near, 7 );
			DrawSessionGraph( sessions, sessions.Width, sessions.Height );
		}

		protected void DrawSessionGraph( MovieClip sessions, int w, int h )
		{
			MovieClip sessionBar = sessions.CreateSubMovieClip( 0, h/3, w, h-(h/3) );

			// Date
			MovieClip dateBar    = sessionBar.CreateSubMovieClip( 0, sessionBar.Height/2, w, sessionBar.Height/2 );
			MovieClip dateFront  = dateBar.CreateSubMovieClip( 0, 0, dateBar.Width/2, dateBar.Height );
			dateFront.LeftString( "10/10/05", Brushes.White );
			MovieClip dateEnd    = dateBar.CreateSubMovieClip( dateBar.Width/2, 0, dateBar.Width/2, dateBar.Height );
			dateEnd.AlignedString( "12/03/05", Brushes.White, StringAlignment.Far );

			//dateBar.Graphics.DrawRectangle(Pens.Sienna, 0, 0, dateBar.Width-1, dateBar.Height-1 );
			//sessionBar.Graphics.DrawRectangle( Pens.Sienna, 0, 0, sessionBar.Width -1 , sessionBar.Height - 1);

			Pen wideGray = new Pen( Brushes.Gray, 2 );
			sessionBar.Graphics.DrawLine( wideGray, 0, sessionBar.Height/2, sessionBar.Width, sessionBar.Height/2 );
			wideGray.Dispose();
	
			MovieClip sessionThumbs = sessionBar.CreateSubMovieClip( 0, 0, sessionBar.Width, sessionBar.Height );
			for( int sIndex =0; sIndex < 5; sIndex++ )
			{
				MovieClip session = sessionThumbs.CreateSubMovieClip( 0, 0, 10, 10 );
				session.Graphics.FillRectangle( Brushes.Yellow , 0, 0, 9, 9 );
			}
			Space( 3, 2, sessionThumbs.Children, sessionThumbs.Width );
			MovieClip xx = sessionThumbs.Children[3];
			xx.X +=30;
		}

		protected void SessionSummary( MovieClip clip, int w, int h, int sessions )
		{
			clip.Graphics.FillRectangle( m_backBrush, 0, 0, w, h);

			MovieClip session = clip.CreateSubMovieClip( 0, 0, w, h/2 );
			MovieClip sessionWords = clip.CreateSubMovieClip(0,h/2,w,h/2);
			session.Graphics.FillRectangle( m_backBrush, 0, 0, w-1, h-1);

			AlignedString( sessionWords, "AUTOTAGS:", Brushes.LightGray, StringAlignment.Near, 7 );
			AlignedString( sessionWords, "                        Check(7), Lock(6), Acquire(4)", Brushes.White, StringAlignment.Near, 10 );
			sessionWords.AlignedString("author  10/16/05", Brushes.White, StringAlignment.Far );

			Pen wideGray = new Pen( Brushes.Gray, 2 );
			session.Graphics.DrawLine( wideGray, 0, session.Height, session.Width, session.Height );
			wideGray.Dispose();

			for( int s=1; s < session.Width; s++ )
			{
				if( s > session.Width/2 && m_random.Next(2) == 0 || 
					s < session.Width/4 && m_random.Next(2) == 0 )
					continue;
				if( s % 2 + m_random.Next(2) == 0 )
					session.Graphics.DrawLine( Pens.Blue, s, (session.Height-2), s, (session.Height-2)-m_random.Next((clip.Height-2)/3) );
				else if( s % 3+m_random.Next(1) == 0 )
				{
					session.Graphics.DrawLine( Pens.LightGreen, s, (session.Height-2), s, (session.Height-2)-m_random.Next((clip.Height-2)/3) );
				}
				else if( s % 5 == 0 || s > session.Width/2)
				{
					session.Graphics.DrawLine( Pens.Gray, s, (session.Height-2), s, (session.Height-2)-m_random.Next((session.Height-2)/3) );
				}
				else
				{
				}
			}
			//Space( 0, 0, clip.Children, clip.Width );
		}

		protected void RefusedBequest( int x, int y, int w, int h )
		{

			MovieClip clip = m_panelClip.CreateSubMovieClip( x, y, w, h );
			//clip.Object = chain;
			// Back rectangle
			clip.Graphics.FillRectangle( m_backBrush, 0, 0, w, h);
			// Dots

			Pen p = new Pen( Brushes.LightBlue, 3 );
			Pen b = new Pen( Brushes.LightCoral, 3 );
			int l = 10;
			for( int d = 0; d < 18; d++ )
			{
				MovieClip dot = clip.CreateSubMovieClip( 0, 0, l, 6 );
				
				if( d % 4 == 0 )
				{
					dot.Graphics.DrawLine( p, 0, 0, l-1, 0 );
				}
				else
				{
					dot.Graphics.DrawLine( b, 0, 0, l-1, 0 );
				}
			}

			Space( 10, 18, 10, 2, clip.Children, w );
			clip.LeftString( "StorageDB", Brushes.White );
		}

		protected void CreateInInt( int x, int y, int w, int h )
		{
			MovieClip clip = m_panelClip.CreateSubMovieClip( x, y, w, h );
			//clip.Object = chain;
			// Back rectangle
			clip.Graphics.FillRectangle( m_backBrush, 0, 0, w, h);
			// Dots
			MovieClip dots = clip.CreateSubMovieClip( w/2, 0, w, h/2 );
			MovieClip uptext = clip.CreateSubMovieClip( 0, 0, w/2, h/2 );
			for( int d = 0; d < 5; d++ )
			{
				MovieClip dot = dots.CreateSubMovieClip( 0, 0, 6, 6 );
				dot.Graphics.FillEllipse( Brushes.LightBlue, 0, 0, 5, 5 );
			}

			MovieClip others = clip.CreateSubMovieClip( w/2, h/2, w, h/2 );
			MovieClip downtext = clip.CreateSubMovieClip( 0, h/2, w/2, h/2 );
			for( int d = 0; d < 7; d++ )
			{
				MovieClip dot = others.CreateSubMovieClip( 0, 0, 6, 6 );
				dot.Graphics.FillEllipse( Brushes.LightGreen, 0, 0, 5, 5 );
			}

			LineTo( clip, 1, 0, w,h, Pens.White);
			LineTo( clip, 0, 2, w,h, Pens.Yellow );
			LineTo( clip, 1, 3, w,h, Pens.White );
			LineTo( clip, 2, 3, w,h, Pens.White );
			LineTo( clip, 0, 6, w,h, Pens.Yellow );
			LineTo( clip, 2, 2, w,h, Pens.White );
			LineTo( clip, 3, 5, w,h, Pens.White );
			LineTo( clip, 4, 1, w,h, Pens.Yellow );
			LineTo( clip, 3, 4, w,h, Pens.White );
			LineTo( clip, 4, 2, w,h, Pens.White );

			Space( 3, 3, dots.Children, dots.Width );
			Space( 3, 3, others.Children, others.Width );
			uptext.LeftString( "TreeView", Brushes.White );
			downtext.LeftString( "TreeNode", Brushes.White );
		}

		protected void LineTo( MovieClip clip, int d1, int d2, int w, int h, Pen pen )
		{
			int of = w/2 - 3;
			clip.Graphics.DrawLine( pen, (6+3)*(1+d1)+of, 6, (6+3)*(1+d2)+of, h/2 + 6 );
		}

		protected void CreateClassHier( 
			int x, int y, int w, int h )
		{
			MovieClip clip = m_panelClip.CreateSubMovieClip( x, y, w, h );
			//clip.Object = chain;
			// Back rectangle
			clip.Graphics.FillRectangle( m_backBrush, 0, 0, w, h);
			// Dots
			MovieClip dots = clip.CreateSubMovieClip( 74, 0, w, h );
			//dots.Graphics.FillRectangle( Brushes.Wheat, 0, 0, w/4, h );
			for( int d = 0; d < 7; d++ )
			{   // extra spacing in between disconnected chains?
				MovieClip dot = dots.CreateSubMovieClip( 0, 0, 6, 6 );
				if( d > 1 )
					dot.Graphics.DrawEllipse( Pens.LightGreen, 0, 0, 5, 5 );
				else
					dot.Graphics.FillEllipse( Brushes.LightGreen, 0, 0, 5, 5 );
			}
			Space( 2, 5, dots.Children, dots.Width );

			// Text
			MovieClip text = clip.CreateSubMovieClip( 0, 0, w, h );
			//text.LeftString( "string name (31):   +(17)  .Split(\",\")(14)", Brushes.White);
			//text.LeftString( "string name (31):   +(17)  .Split(\",\")(14)", Brushes.White);
			text.LeftString("CalculateTaxes(  ,  ,  ,  ,  ,  ,  )", Brushes.White);
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

		private void session_MouseClick(object sender, MouseEventArgs e)
		{
			MovieClip ses = (MovieClip)sender;
			ses.Graphics.FillRectangle( Brushes.White, 0, 0, 9, 9 );
			this.Invalidate();
		}
	}
}