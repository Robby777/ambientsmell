using System;
using System.Diagnostics;
using LCS;

namespace LCS
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	class Test
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			
			string s2="AA BB CC BB II CC KK DD H I K";
			string s1="AA BB CC DD E F G DD";
			//
			//string s3="A H B K C I D";

			Trace.WriteLine(LCSFinder.GetLCS(s1, s2) ) ;
			//Trace.WriteLine(LCSFinder.GetLCS(s1, s3) ) ;
		}
	}
}
