using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

using SwiftVisLib;

using CodeModel.Components;
using CodeModel.Representation;

using EnvDTE;
using Extensibility;

namespace SmellVisualizations
{
	/// <summary>
	/// Summary description for BaseVisualization.
	/// </summary>
	public class BaseVisualization : Form
	{
		public BaseVisualization()
		{
		}

		protected void Space( int x_spacing, int y_spacing, ICollection clips, int ContainerWidth )
		{
			int x = x_spacing;
			int y = y_spacing;
			Space( x, y, x_spacing, y_spacing, clips, ContainerWidth );
		}
		protected void Space( int x, int y, int x_spacing, int y_spacing, ICollection clips, int ContainerWidth )
		{		
			foreach( MovieClip clip in clips )
			{
				clip.X = x;
				clip.Y = y;
				x += clip.Width + x_spacing;
				if( x + clip.Width > ContainerWidth )
				{
					x = x_spacing;
					y += clip.Height + y_spacing;
				}
			}
		}

		protected void OnMouseClickMethod(object sender, MouseEventArgs e)
		{
			MovieClip clip = (MovieClip)sender;
			if( clip.Object != null )
			{
				MethodComponent mc = (MethodComponent)clip.Object;
				if( mc.CodeFunction != null )
				{
					CodeFunction cf = (CodeFunction) mc.CodeFunction;
					SelectObjectMethod( cf );
				}
			}
		}
		protected void OnMouseClickClass(object sender, MouseEventArgs e)
		{
			MovieClip clip = (MovieClip)sender;
			if( clip.Object != null )
			{
				ClassComponent mc = (ClassComponent)clip.Object;
				if( mc.CodeClass != null )
				{
					CodeClass cc = (CodeClass) mc.CodeClass;
					SelectObjectClass( cc );
				}
			}
		}

		protected void SelectObjectClass( CodeClass cc )
		{
			ProjectItem p = cc.ProjectItem;
			//  Open the file as a source code file
			EnvDTE.Window theWindow = p.Open(Constants.vsViewKindCode);

			//Get a handle to the new document in the open window
			TextDocument objTextDoc = (TextDocument)theWindow.Document.Object("TextDocument");
			EditPoint objEditPoint = (EditPoint)objTextDoc.StartPoint.CreateEditPoint();

			theWindow.Visible = true;

			TextSelection ts = (TextSelection) theWindow.Selection;
			ts.StartOfDocument(false);
			objEditPoint.MoveToLineAndOffset(cc.StartPoint.Line,1);
			ts.MoveToPoint( objEditPoint, false );
		}

		protected void SelectObjectMethod( CodeFunction cf )
		{
			ProjectItem p = cf.ProjectItem;
			//  Open the file as a source code file
			EnvDTE.Window theWindow = p.Open(Constants.vsViewKindCode);

			//Get a handle to the new document in the open window
			TextDocument objTextDoc = (TextDocument)theWindow.Document.Object("TextDocument");
			EditPoint objEditPoint = (EditPoint)objTextDoc.StartPoint.CreateEditPoint();

			theWindow.Visible = true;

			TextSelection ts = (TextSelection) theWindow.Selection;
			ts.StartOfDocument(false);
			objEditPoint.MoveToLineAndOffset(cf.StartPoint.Line,1);
			ts.MoveToPoint( objEditPoint, false );
		}
	}
}
