// <copyright file="MainForm.Designer.cs"
//   Jucardi. All Rights Reserved.
// </copyright>
// <author>juan.diaz</author>
// <date>2/7/2012 12:32:23 PM</date>
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.

namespace Converter.NETdesigner_to_Java_Swing
{
	partial class MainForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.btnSelectAssembly = new System.Windows.Forms.Button();
			this.btnCreateJava = new System.Windows.Forms.Button();
			this.treeviewClassExplorer = new System.Windows.Forms.TreeView();
			this.lblTitleClasses = new System.Windows.Forms.Label();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.button1 = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.SuspendLayout();
			//
			// textBox1
			//
			this.textBox1.Location = new System.Drawing.Point(12, 12);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(346, 20);
			this.textBox1.TabIndex = 0;
			this.textBox1.Text = "D:\\Smartmatic\\Visual Studio\\DEVSMS Source Code\\CNE API Biometrico Cliente\\.NET\\tr" +
					"unk\\API\\Build\\VisualStudio\\x86\\Samples\\Clients\\Capturer\\CNE.Capturer.Client.Samp" +
					"le.exe";
			//
			// btnSelectAssembly
			//
			this.btnSelectAssembly.Location = new System.Drawing.Point(461, 10);
			this.btnSelectAssembly.Name = "btnSelectAssembly";
			this.btnSelectAssembly.Size = new System.Drawing.Size(90, 23);
			this.btnSelectAssembly.TabIndex = 1;
			this.btnSelectAssembly.Text = "Load Assembly";
			this.btnSelectAssembly.UseVisualStyleBackColor = true;
			this.btnSelectAssembly.Click += new System.EventHandler(this.OnButtonLoadAssemblyClick);
			//
			// btnCreateJava
			//
			this.btnCreateJava.Location = new System.Drawing.Point(389, 199);
			this.btnCreateJava.Name = "btnCreateJava";
			this.btnCreateJava.Size = new System.Drawing.Size(162, 38);
			this.btnCreateJava.TabIndex = 3;
			this.btnCreateJava.Text = "Create .java";
			this.btnCreateJava.UseVisualStyleBackColor = true;
			this.btnCreateJava.Click += new System.EventHandler(this.OnButtonCreateJavaClick);
			//
			// treeviewClassExplorer
			//
			this.treeviewClassExplorer.Location = new System.Drawing.Point(12, 60);
			this.treeviewClassExplorer.Name = "treeviewClassExplorer";
			this.treeviewClassExplorer.Size = new System.Drawing.Size(539, 133);
			this.treeviewClassExplorer.TabIndex = 4;
			//
			// lblTitleClasses
			//
			this.lblTitleClasses.AutoSize = true;
			this.lblTitleClasses.Location = new System.Drawing.Point(9, 44);
			this.lblTitleClasses.Name = "lblTitleClasses";
			this.lblTitleClasses.Size = new System.Drawing.Size(43, 13);
			this.lblTitleClasses.TabIndex = 5;
			this.lblTitleClasses.Text = "Classes";
			//
			// saveFileDialog1
			//
			this.saveFileDialog1.FileName = "D:\\Jucardi\\Desktop\\temp.java";
			//
			// button1
			//
			this.button1.Location = new System.Drawing.Point(365, 10);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(90, 23);
			this.button1.TabIndex = 6;
			this.button1.Text = "Browse";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.OnButtonBrowseClick);
			//
			// openFileDialog1
			//
			this.openFileDialog1.FileName = "openFileDialog1";
			//
			// MainForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(563, 249);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.lblTitleClasses);
			this.Controls.Add(this.treeviewClassExplorer);
			this.Controls.Add(this.btnCreateJava);
			this.Controls.Add(this.btnSelectAssembly);
			this.Controls.Add(this.textBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = ".NET form to Java Swing";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button btnSelectAssembly;
		private System.Windows.Forms.Button btnCreateJava;
		private System.Windows.Forms.TreeView treeviewClassExplorer;
		private System.Windows.Forms.Label lblTitleClasses;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
	}
}
