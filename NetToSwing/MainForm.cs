// <copyright file="MainForm.cs"
//   Jucardi. All Rights Reserved.
// </copyright>
// <author>juan.diaz</author>
// <date>2/7/2012 12:32:23 PM</date>
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.


﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace Converter.NETdesigner_to_Java_Swing
{
	public partial class MainForm : Form
	{
		private Assembly currentAssembly;

		public MainForm()
		{
			this.InitializeComponent();
		}

		private void OnButtonLoadAssemblyClick(object sender, EventArgs e)
		{

			string[] dllAssemblies = Directory.GetFiles(Path.GetDirectoryName(textBox1.Text), "*.dll");
			string[] exeAssemblies = Directory.GetFiles(Path.GetDirectoryName(textBox1.Text), "*.exe");

			foreach (string file in dllAssemblies)
				try { Assembly.LoadFile(file); } catch { }

			foreach (string file in exeAssemblies)
				try { Assembly.LoadFile(file); } catch { }

			if (!File.Exists(this.textBox1.Text))
			{
				MessageBox.Show("Assembly not found.", "Error");
				return;
			}

			try
			{
				this.currentAssembly = Assembly.LoadFile(this.textBox1.Text);
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format("Unable to load assembly.\r\n\r\n{0}", ex.ToString()));
				return;
			}

			Type[] types = this.currentAssembly.GetTypes();
			TreeNode node = new TreeNode(this.currentAssembly.FullName);

			foreach (Type type in types)
			{
				TreeNode newNode = new TreeNode(type.Name);
				newNode.Tag = type;
				node.Nodes.Add(newNode);
			}

			this.treeviewClassExplorer.Nodes.Add(node);
		}

		private void OnButtonCreateJavaClick(object sender, EventArgs e)
		{
			if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
				return;

			Converter converter = new Converter();
			converter.ConvertNetToSwing(this.saveFileDialog1.FileName, treeviewClassExplorer.SelectedNode.Tag as Type);
			MessageBox.Show("Done.");
		}

		private void OnButtonBrowseClick(object sender, EventArgs e)
		{
			if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
				return;

			this.textBox1.Text = this.openFileDialog1.FileName;
		}
	}
}
