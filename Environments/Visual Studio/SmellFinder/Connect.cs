namespace SmellFinder
{
	using System;
	using System.Reflection;
	
	using Microsoft.Office.Core;
	using Extensibility;
	using System.Runtime.InteropServices;
	using EnvDTE;

	using CodeModel.Components;
	using CodeModel.Representation;
	using CodeModel.VSModel;

	#region Read me for Add-in installation and setup information.
	// When run, the Add-in wizard prepared the registry for the Add-in.
	// At a later time, if the Add-in becomes unavailable for reasons such as:
	//   1) You moved this project to a computer other than which is was originally created on.
	//   2) You chose 'Yes' when presented with a message asking if you wish to remove the Add-in.
	//   3) Registry corruption.
	// you will need to re-register the Add-in by building the MyAddin21Setup project 
	// by right clicking the project in the Solution Explorer, then choosing install.
	#endregion
	
	/// <summary>
	///   The object for implementing an Add-in.
	/// </summary>
	/// <seealso class='IDTExtensibility2' />
	[GuidAttribute("220ABBA2-E279-451E-8EFF-F451ACE2D42F"), ProgId("SmellFinder.Connect")]
	public class Connect : Object, Extensibility.IDTExtensibility2, IDTCommandTarget
	{
		/// <summary>
		///		Implements the constructor for the Add-in object.
		///		Place your initialization code within this method.
		/// </summary>
		public Connect()
		{
		}

		/// <summary>
		///      Implements the OnConnection method of the IDTExtensibility2 interface.
		///      Receives notification that the Add-in is being loaded.
		/// </summary>
		/// <param term='application'>
		///      Root object of the host application.
		/// </param>
		/// <param term='connectMode'>
		///      Describes how the Add-in is being loaded.
		/// </param>
		/// <param term='addInInst'>
		///      Object representing this Add-in.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, Extensibility.ext_ConnectMode connectMode, object addInInst, ref System.Array custom)
		{
			applicationObject = (_DTE)application;
			addInInstance = (AddIn)addInInst;
			//if(connectMode == Extensibility.ext_ConnectMode.ext_cm_UISetup)
			{
				object []contextGUIDS = new object[] { };
				Commands commands = applicationObject.Commands;
				_CommandBars commandBars = applicationObject.CommandBars;

				// When run, the Add-in wizard prepared the registry for the Add-in.
				// At a later time, the Add-in or its commands may become unavailable for reasons such as:
				//   1) You moved this project to a computer other than which is was originally created on.
				//   2) You chose 'Yes' when presented with a message asking if you wish to remove the Add-in.
				//   3) You add new commands or modify commands already defined.
				// You will need to re-register the Add-in by building the SmellFinderSetup project,
				// right-clicking the project in the Solution Explorer, and then choosing install.
				// Alternatively, you could execute the ReCreateCommands.reg file the Add-in Wizard generated in
				// the project directory, or run 'devenv /setup' from a command prompt.
				try
				{
					Command command = Find( commands );
					if( command == null )
					{
						command = commands.AddNamedCommand(addInInstance, "SmellFinder", "SmellFinder", "Executes the command for SmellFinder", true, 59, ref contextGUIDS, (int)vsCommandStatus.vsCommandStatusSupported+(int)vsCommandStatus.vsCommandStatusEnabled);
						CommandBar commandBar = (CommandBar)commandBars["Tools"];
						CommandBarControl commandBarControl = command.AddControl(commandBar, 1);
					}
				}
				catch(System.Exception /*e*/)
				{
				}
			}
		}

		protected Command Find( Commands commands )
		{
			Command cmd = null;
			try
			{
				cmd = commands.Item( "SmellFinder.Connect.SmellFinder", -1 );
			}
			catch{}
			return cmd;
		}

		/// <summary>
		///     Implements the OnDisconnection method of the IDTExtensibility2 interface.
		///     Receives notification that the Add-in is being unloaded.
		/// </summary>
		/// <param term='disconnectMode'>
		///      Describes how the Add-in is being unloaded.
		/// </param>
		/// <param term='custom'>
		///      Array of parameters that are host application specific.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(Extensibility.ext_DisconnectMode disconnectMode, ref System.Array custom)
		{
		}

		/// <summary>
		///      Implements the OnAddInsUpdate method of the IDTExtensibility2 interface.
		///      Receives notification that the collection of Add-ins has changed.
		/// </summary>
		/// <param term='custom'>
		///      Array of parameters that are host application specific.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnAddInsUpdate(ref System.Array custom)
		{
		}

		/// <summary>
		///      Implements the OnStartupComplete method of the IDTExtensibility2 interface.
		///      Receives notification that the host application has completed loading.
		/// </summary>
		/// <param term='custom'>
		///      Array of parameters that are host application specific.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref System.Array custom)
		{
		}

		/// <summary>
		///      Implements the OnBeginShutdown method of the IDTExtensibility2 interface.
		///      Receives notification that the host application is being unloaded.
		/// </summary>
		/// <param term='custom'>
		///      Array of parameters that are host application specific.
		/// </param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref System.Array custom)
		{
		}
		
		/// <summary>
		///      Implements the QueryStatus method of the IDTCommandTarget interface.
		///      This is called when the command's availability is updated
		/// </summary>
		/// <param term='commandName'>
		///		The name of the command to determine state for.
		/// </param>
		/// <param term='neededText'>
		///		Text that is needed for the command.
		/// </param>
		/// <param term='status'>
		///		The state of the command in the user interface.
		/// </param>
		/// <param term='commandText'>
		///		Text requested by the neededText parameter.
		/// </param>
		/// <seealso class='Exec' />
		public void QueryStatus(string commandName, EnvDTE.vsCommandStatusTextWanted neededText, ref EnvDTE.vsCommandStatus status, ref object commandText)
		{
			if(neededText == EnvDTE.vsCommandStatusTextWanted.vsCommandStatusTextWantedNone)
			{
				if(commandName == "SmellFinder.Connect.SmellFinder")
				{
					status = (vsCommandStatus)vsCommandStatus.vsCommandStatusSupported|vsCommandStatus.vsCommandStatusEnabled;
				}
			}
		}

		/// <summary>
		///      Implements the Exec method of the IDTCommandTarget interface.
		///      This is called when the command is invoked.
		/// </summary>
		/// <param term='commandName'>
		///		The name of the command to execute.
		/// </param>
		/// <param term='executeOption'>
		///		Describes how the command should be run.
		/// </param>
		/// <param term='varIn'>
		///		Parameters passed from the caller to the command handler.
		/// </param>
		/// <param term='varOut'>
		///		Parameters passed from the command handler to the caller.
		/// </param>
		/// <param term='handled'>
		///		Informs the caller if the command was handled or not.
		/// </param>
		/// <seealso class='Exec' />
		public void Exec(string commandName, EnvDTE.vsCommandExecOption executeOption, ref object varIn, ref object varOut, ref bool handled)
		{
			handled = false;
			if(executeOption == EnvDTE.vsCommandExecOption.vsCommandExecOptionDoDefault)
			{
				if(commandName == "SmellFinder.Connect.SmellFinder")
				{
					handled = true;
					
					//ShowToolWindow( applicationObject, addInInstance );
					TestForm testForm = new TestForm();
					testForm.AppObject = applicationObject;
					testForm.Show();

					return;
				}
			}
		}

		private _DTE applicationObject;
		private AddIn addInInstance;
		
		static Window windowToolWindow;
		private static VSUserControlHostLib.IVSUserControlHostCtl objControl;
		//private static Prototype.Controls.Class_Container.SourceVisPanel m_visPanel;
		TestForm m_frm;

		// Need to execute: "regsvr32 VSUserControlHost.dll"

		public void ShowToolWindow(_DTE applicationObject, AddIn addInInstance)
		{
			try
			{
				object objTemp = null;

				String guidstr = "{FDB416EB-2AC8-4714-96BC-8D39E8C9E063}";

				if( windowToolWindow == null )
				{
					windowToolWindow = applicationObject.Windows.CreateToolWindow (addInInstance, "VSUserControlHost.VSUserControlHostCtl", "C# Tool window", guidstr, ref objTemp);

					//When using the hosting control, you must set visible to true before calling HostUserControl,
					// otherwise the UserControl cannot be hosted properly.
					windowToolWindow.Visible    = true;
					windowToolWindow.IsFloating = false;
					windowToolWindow.Linkable   = true;
					if( windowToolWindow.LinkedWindowFrame == null )
					{
						windowToolWindow.Width      = 1024;
						windowToolWindow.Height     = 768;
					}
					objControl = (VSUserControlHostLib.IVSUserControlHostCtl)objTemp;
					
					//System.Reflection.Assembly asm = System.Reflection.Assembly.LoadFrom( @"C:\Documents and Settings\chris\Desktop\classes\InfoViz\SourceVis\Erato\bin\Debug\Prototype.dll" );
					//Prototype.Controls.Class_Container.SourceVisPanel ctrl = (Prototype.Controls.Class_Container.SourceVisPanel)objControl.HostUserControl(asm.Location, "Prototype.Controls.Class_Container.SourceVisPanel");
					//ctrl.InitSource( applicationObject );
					//m_visPanel = ctrl;

					Assembly asm = Assembly.GetExecutingAssembly();
					m_frm = (TestForm)objControl.HostUserControl( asm.Location, "SmellFinder.TestForm");
				}
				else
				{
//					windowToolWindow.Visible    = true;
//					windowToolWindow.IsFloating = false;
//					windowToolWindow.Linkable   = true;
//					windowToolWindow.Width      = 1024;
//					windowToolWindow.Height     = 768;
				}
			}
			catch( Exception ex )
			{
				System.Windows.Forms.MessageBox.Show( ex.Message + ex.StackTrace );
			}
		}

	}
}