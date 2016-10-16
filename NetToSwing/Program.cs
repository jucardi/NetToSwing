// <copyright file="Program.cs"
//   Jucardi. All Rights Reserved.
// </copyright>
// <author>juan.diaz</author>
// <date>2/7/2012 12:21:06 PM</date>
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.


﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Converter.NETdesigner_to_Java_Swing
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
		}

		static System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			throw new NotImplementedException();
		}
	}
}
