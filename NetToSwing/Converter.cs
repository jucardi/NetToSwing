// <copyright file="Converter.cs"
//   Jucardi. All Rights Reserved.
// </copyright>
// <author>juan.diaz</author>
// <date>2/7/2012 12:50:14 PM</date>
//
// This software may be modified and distributed under the terms
// of the MIT license.  See the LICENSE file for details.

using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using System.ComponentModel;
using System.Text;
using System.Threading;

namespace Converter.NETdesigner_to_Java_Swing
{
	public class Converter
	{
		#region Constants

		private const string SCROLL_PANEL_TAG      = "SCROLL_PANEL_ADDED";
		private const string SCROLL_PANEL_ID       = "ADD_AS_JSCROLLPANE";
		private const string SCROLL_PANE_CONTAINER = "SCROLL_PANE_CONTAINER";

		#endregion

		#region Fields

		private TypeConverter converter = new TypeConverter();
		private Dictionary<string, string> controlsAdded = new Dictionary<string, string>();
		private Dictionary<string, int>  userControlList = new Dictionary<string, int>();
		private int noNameControlIndex = 0;

		#endregion

		#region Public Methods

		/// <summary>
		/// Converts the .NET type to a Java Swing designer file.
		/// </summary>
		/// <param name="path">The path.</param>
		/// <param name="type">The type.</param>
		public void ConvertNetToSwing(string path, Type type)
		{
			if (!type.IsSubclassOf(typeof(ContainerControl)))
			{
				MessageBox.Show("For now, the conversion only works with types that are subclass of 'ContainerControl'");
				return;
			}

			List<string> writer = new List<string>();

			object control = Activator.CreateInstance(type);

			this.WriteHeader(writer, type);
			this.AddControlToDesigner(writer, control, null, string.Empty);
			this.WriteEnd(writer);

			writer = this.AddImports(writer, type);

			File.WriteAllLines(path, writer.ToArray());
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Writes the header of the .java file.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="type">The type.</param>
		private void WriteHeader(List<string> writer, Type type)
		{
			writer.Add("import javax.swing.JFrame;");
			writer.Add("import javax.swing.SwingConstants;");
			writer.Add("import java.awt.Color;");
			writer.Add("import java.awt.EventQueue;");
			writer.Add("import java.awt.Font;");
			writer.Add(string.Empty);
			writer.Add(string.Format("public class {0}", type.Name));
			writer.Add("{");
			writer.Add(string.Empty);
			writer.Add("	/** Launch the application. */");
			writer.Add("	public static void main(String[] args)");
			writer.Add("	{");
			writer.Add("		EventQueue.invokeLater(new Runnable() {");
			writer.Add("			public void run()");
			writer.Add("			{");
			writer.Add("				try");
			writer.Add("				{");
			writer.Add("					/* Implement custom LookAndFeel here */");
			writer.Add("					javax.swing.UIManager.setLookAndFeel(\"com.sun.java.swing.plaf.windows.WindowsLookAndFeel\");");
			writer.Add("				}");
			writer.Add("				catch (Exception ex)");
			writer.Add("				{");
			writer.Add("					java.util.logging.Logger.getLogger(MainForm.class.getName()).log(java.util.logging.Level.SEVERE, null, ex);");
			writer.Add("				}");
			writer.Add(string.Empty);
			writer.Add("				try");
			writer.Add("				{");
			writer.Add(string.Format("					{0} window = new {0}();", type.Name));
			writer.Add("					window.frame.setVisible(true);");
			writer.Add("				}");
			writer.Add("				catch (Exception ex)");
			writer.Add("				{");
			writer.Add("					ex.printStackTrace();");
			writer.Add("				}");
			writer.Add("			}");
			writer.Add("		});");
			writer.Add("	}");
			writer.Add(string.Empty);
			writer.Add("	/** Create the application. */");
			writer.Add(string.Format("	public {0}()", type.Name));
			writer.Add("	{");
			writer.Add("		this.initialize();");
			writer.Add("	}");
			writer.Add(string.Empty);
			writer.Add("	/** Initialize the contents of the frame. */");
			writer.Add("	private void initialize()");
			writer.Add("	{");
		}

		/// <summary>
		/// Writes the end of the .java file.
		/// </summary>
		/// <param name="writer">The writer.</param>
		private void WriteEnd(List<string> writer)
		{
			writer.Add("	}");
			writer.Add(string.Empty);
			writer.Add("	// Controls declaration");
			writer.Add("	private JFrame frame = new JFrame();");

			foreach (KeyValuePair<string, string> item in this.controlsAdded)
				writer.Add(string.Format("	private {0} {1} = new {0}();", item.Value, item.Key));

			writer.Add(string.Empty);
			writer.Add("}");
		}

		/// <summary>
		/// Adds the imports used in the javax.swing package.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		private List<string> AddImports(List<string> writer, Type type)
		{
			List<string> ret = new List<string>();
			Dictionary<string, string> importsAdded = new Dictionary<string, string>();

			ret.Add(string.Format("package {0};", type.Namespace));
			ret.Add(string.Empty);

			foreach (KeyValuePair<string, string> item in this.controlsAdded)
			{
				if (importsAdded.ContainsKey(item.Value))
					continue;

				ret.Add(string.Format("import javax.swing.{0};", item.Value));

				importsAdded.Add(item.Value, string.Empty);
			}

			foreach (string item in writer)
				ret.Add(item);

			return ret;
		}

		/// <summary>
		/// Adds the control to the .java designer.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="obj">The obj.</param>
		/// <param name="parent">The parent.</param>
		private void AddControlToDesigner(List<string> writer, object obj, Control parent, string endID)
		{
			Type controlType = obj.GetType();

			Control tempControl = obj as Control;
			Console.WriteLine(tempControl.Name);

			if (tempControl is Form)
				tempControl.Name = "frame";

			if (tempControl == null)
				return;

			if (tempControl is UserControl)
			{
				if (this.userControlList.ContainsKey(controlType.Name))
					this.userControlList[controlType.Name]++;
				else
					this.userControlList.Add(controlType.Name, 0);

				endID = string.Format("{0}{1}", endID, this.userControlList[controlType.Name]);
			}

			if (!string.IsNullOrEmpty(endID))
				tempControl.Name = string.Format("{0}_{1}", tempControl.Name, endID);

			Control control = null;

			if (tempControl is Label)
				control = this.AddLabelBackColor(tempControl as Label, parent);
			else if (tempControl is ListBox || tempControl is DataGridView || tempControl is RichTextBox)
				control = this.AddScrollPanelToControl(tempControl, parent);
			else
				control = tempControl;

			if (string.IsNullOrEmpty(control.Name))
				control.Name = string.Format("NO_NAME_{0}", this.noNameControlIndex++);

			if (!this.AnalyzeControl(control))
				return;

			this.AddProperties(writer, control);
			this.SetParentingLines(writer, control, parent);

			writer.Add(string.Empty);

			if (control is StatusBar || control is StatusStrip)
				this.AddStatusBarSeparator(writer, control);

			if (control.Controls != null)
			{
				foreach (Control item in control.Controls)
					this.AddControlToDesigner(writer, item, control, endID);
			}

		}

		/// <summary>
		/// Adds the .NET object properties using the appropiate setters in the .java file.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="control">The control.</param>
		private void AddProperties(List<string> writer, Control control)
		{
			int witdh  = control.Location.X != 0 ? control.Size.Width  : control.Size.Width  - 1;
			int height = control.Location.Y != 0 ? control.Size.Height : control.Size.Height - 1;
			int x      = control.Location.X != 0 ? control.Location.X  : control.Location.X  + 1;
			int y      = control.Location.Y != 0 ? control.Location.Y  : control.Location.Y  + 1;

			writer.Add(string.Format("		this.{0}.setSize({1}, {2});", control.Name, witdh, height));
			writer.Add(string.Format("		this.{0}.setLocation({1}, {2});", control.Name, x, y));
			writer.Add(string.Format("		this.{0}.setForeground(new Color({1}, {2}, {3}));", control.Name, control.ForeColor.R, control.ForeColor.G, control.ForeColor.B, control.ForeColor.A));
			writer.Add(string.Format("		this.{0}.setBackground(new Color({1}, {2}, {3}));", control.Name, control.BackColor.R, control.BackColor.G, control.BackColor.B, control.BackColor.A));
			writer.Add(string.Format("		this.{0}.setFont(new Font(\"{1}\", Font.{2}, {3}));", control.Name, control.Font.Name, control.Font.Style != FontStyle.Regular ? control.Font.Style.ToString().ToUpper() : "PLAIN", (int)Math.Round(control.Font.Size) + 2));
			writer.Add(string.Format("		this.{0}.setEnabled({1});", control.Name, control.Enabled.ToString().ToLower()));
			//writer.Add(string.Format("		this.{0}.setVisible({1});", control.Name, control.Visible.ToString().ToLower()));

			this.SetBorder(writer, control);
			this.SetText(writer, control);
			this.SetTextAlignment(writer, control);
		}

		/// <summary>
		/// Analyzes the control to tell if it is a supported control to be converted to a java swing control.
		/// </summary>
		/// <param name="control">The control.</param>
		/// <returns><c>true</c> if it is a supported control; otherwise <c>false</c></returns>
		private bool AnalyzeControl(Control control)
		{
			if (control is Form)
				return true;

			string javaControlType = string.Empty;

			if (control is Button)
				javaControlType = "JButton";
			else if (control is CheckBox)
				javaControlType = "JCheckBox";
			else if (control is CheckedListBox)
				javaControlType = "JList";
			else if (control is ComboBox)
				javaControlType = "JComboBox";
			else if (control is DataGridView)
				javaControlType = "JTable";
			else if (control is GroupBox)
				javaControlType = "JPanel";
			else if (control is StatusBar)
				javaControlType = "JPanel";
			else if (control is Label)
				javaControlType = "JLabel";
			else if (control is ListBox)
				javaControlType = "JList";
			else if (control is MaskedTextBox)
				javaControlType = "JPasswordField";
			else if (control is PictureBox)
				javaControlType = "JPanel";
			else if (control is ProgressBar)
				javaControlType = "JProgressBar";
			else if (control is RadioButton)
				javaControlType = "JRadioButton";
			else if (control is RichTextBox)
				javaControlType = "JTextArea";
			else if (control is SplitContainer)
				javaControlType = "JSplitPane";
			else if (control is TabControl)
				javaControlType = "JTabbedPane";
			else if (control is TextBox)
				javaControlType = "JTextField";
			else if (control is VScrollBar)
				javaControlType = "JScrollBar";
			else if (control is TabPage)
				javaControlType = "JPanel";
			else if (control.GetType().IsSubclassOf(typeof(UserControl)))
				javaControlType = "JPanel";
			else if (control is StatusStrip)
				javaControlType = "JPanel";
			else if (control is Panel)
			{
				if (control.Tag as string == SCROLL_PANEL_ID)
					javaControlType = "JScrollPane";
				else
					javaControlType = "JPanel";
			}

			if (string.IsNullOrEmpty(javaControlType))
				return false;

			this.controlsAdded[control.Name] = javaControlType;

			return true;
		}

		/// <summary>
		/// Adds the Backcolor of the Label by creating a JPanel matching the Label backcolor to contain the JLabel.
		/// JLabels have a setBackground method but they do not implement it visually, their back color is always transparent,
		/// this is the reason to create a JPanel instead.
		/// </summary>
		/// <param name="control">The control.</param>
		/// <param name="parent">The parent.</param>
		/// <returns>A panel containing the Label if the Label has a custom Backcolor; otherwise returns the the Label itself</returns>
		private Control AddLabelBackColor(Label label, Control parent)
		{
			if (label.BackColor != parent.BackColor)
			{
				Panel tempPanel       = new Panel();
				tempPanel.Name        = string.Format("pnlBackcolor_{0}", label.Name);
				tempPanel.Size        = label.Size;
				tempPanel.Location    = label.Location;
				tempPanel.BackColor   = label.BackColor;
				tempPanel.Visible     = label.Visible;
				tempPanel.BorderStyle = label.BorderStyle;

				Label tempLabel       = this.CloneItem(label) as Label;
				tempLabel.Location    = new System.Drawing.Point(0, 0);
				tempLabel.BorderStyle = BorderStyle.None;

				tempPanel.Controls.Add(tempLabel);

				return tempPanel;
			}

			return label;
		}

		/// <summary>
		/// Adds the scroll panel to the list box.
		/// </summary>
		/// <param name="listBox">The list box.</param>
		/// <param name="parent">The parent.</param>
		/// <returns>The panel to be converted as JScrollPane with the ListBox inside.</returns>
		private Control AddScrollPanelToControl(Control control, Control parent)
		{
			if (control.Tag as string != SCROLL_PANEL_TAG)
			{
				Panel tempPanel     = new Panel();
				tempPanel.Name      = string.Format("pnlScroll_{0}", control.Name);
				tempPanel.Size      = control.Size;
				tempPanel.Location  = control.Location;
				tempPanel.BackColor = control.BackColor;
				tempPanel.Visible   = control.Visible;
				tempPanel.Tag       = SCROLL_PANEL_ID;

				PropertyInfo border = control.GetType().GetProperty("BorderStyle");

				if (border != null)
					tempPanel.BorderStyle = (BorderStyle)border.GetValue(control, null);

				Control tempControl = this.CloneItem(control) as Control;
				tempControl.Location = new System.Drawing.Point(0, 0);
				tempControl.Tag = SCROLL_PANEL_TAG;

				if (border != null)
					border.SetValue(control, BorderStyle.None, null);


				tempPanel.Controls.Add(tempControl);
				return tempPanel;
			}

			return control;
		}

		/// <summary>
		/// Adds the status bar separator.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="control">The control.</param>
		private void AddStatusBarSeparator(List<string> writer, Control control)
		{
			writer.Add(string.Format("		this.topSeparator_{0}.setSize({1}, {2});", control.Name, control.Width, 1));
			writer.Add(string.Format("		this.topSeparator_{0}.setLocation({1}, {1});", control.Name, 1));
			writer.Add(string.Format("		this.{0}.add(this.topSeparator_{0});", control.Name));

			this.controlsAdded[string.Format("topSeparator_{0}", control.Name)] = "JSeparator";
		}

		/// <summary>
		/// Sets the parenting code lines to add the given control to it's parent container.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="control">The control.</param>
		/// <param name="parent">The parent.</param>
		private void SetParentingLines(List<string> writer, Control control, Control parent)
		{
			if (parent != null)
			{
				if ((control is Panel && control.Tag as string != SCROLL_PANEL_ID) || control is TabPage || control is UserControl || control is GroupBox)
					writer.Add(string.Format("		this.{0}.setLayout(null);", control.Name));
				else if (control is Form)
					writer.Add(string.Format("		this.{0}.getContentPane().setLayout(null);", control.Name));

				if (parent is TabControl)
				{
					if (control is TabPage)
						writer.Add(string.Format("		this.{0}.addTab(\"{1}\", null, this.{2}, null);", parent.Name, control.Text, control.Name));

				}
				else if ((parent is Panel && parent.Tag as string != SCROLL_PANEL_ID) || parent is TabPage || parent is UserControl || parent is GroupBox)
				{
					writer.Add(string.Format("		this.{0}.add(this.{1});", parent.Name, control.Name));
				}
				else if (!(parent is Panel))
				{
					writer.Add(string.Format("		this.{0}.getContentPane().add(this.{1});", parent.Name, control.Name));
				}

				if (parent.Tag as string == SCROLL_PANEL_ID)
					writer.Add(string.Format("		this.{0}.setViewportView(this.{1});", parent.Name, control.Name));
			}
		}

		/// <summary>
		/// Sets the control border style.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="control">The control.</param>
		private void SetBorder(List<string> writer, Control control)
		{
			PropertyInfo borderStyleInfo = control.GetType().GetProperty("BorderStyle");

			if (borderStyleInfo != null && !(control is GroupBox))
			{
				BorderStyle style = (BorderStyle)borderStyleInfo.GetValue(control, null);

				switch (style)
				{
					case BorderStyle.Fixed3D:
						writer.Add(string.Format("		this.{0}.setBorder(javax.swing.BorderFactory.createLoweredBevelBorder());", control.Name));
						break;
					case BorderStyle.FixedSingle:
						writer.Add(string.Format("		this.{0}.setBorder(javax.swing.BorderFactory.createLineBorder(Color.BLACK));", control.Name));
						break;
					case BorderStyle.None:
					default:
						writer.Add(string.Format("		this.{0}.setBorder(null);", control.Name));
						break;
				}
			}

			if (control is GroupBox)
			{
				writer.Add(string.Format("		this.{0}.setBorder(javax.swing.BorderFactory.createTitledBorder(\"{1}\"));", control.Name, control.Text));
			}


		}

		/// <summary>
		/// Sets the control text.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="control">The control.</param>
		private void SetText(List<string> writer, Control control)
		{
			if (control is Form)
			{
				writer.Add(string.Format("		this.frame.setTitle(\"{0}\");", control.Text.Replace("\\", "\\\\").Replace("\"", "\\\"")));
			}
			else
			{
				if (control is Button)
					control.Text = control.Text.Replace("&", string.Empty).Replace("\\", "\\\\").Replace("\"", "\\\"");

				if (this.IsSetTextSupported(control))
					writer.Add(string.Format("		this.{0}.setText(\"{1}\");", control.Name, control.Text.Replace("\\", "\\\\").Replace("\"", "\\\"")));
			}
		}

		/// <summary>
		/// Sets the control text alignment.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="control">The control.</param>
		private void SetTextAlignment(List<string> writer, Control control)
		{
			PropertyInfo textAlignment = control.GetType().GetProperty("TextAlign");

			if (textAlignment != null)
			{
				object alignmentValue = textAlignment.GetValue(control, null);

				if (alignmentValue is HorizontalAlignment)
				{
					HorizontalAlignment alignment = (HorizontalAlignment)alignmentValue;
					writer.Add(string.Format("		this.{0}.setHorizontalAlignment(SwingConstants.{1});",  control.Name, alignmentValue.ToString().ToUpper()));
				}
				else if (alignmentValue is ContentAlignment)
				{
					ContentAlignment alignment = (ContentAlignment)alignmentValue;

					string verticalAlignment   = alignment.ToString().Replace("Center", string.Empty).Replace("Left", string.Empty).Replace("Right", string.Empty);
					string horizontalAlignment = alignment.ToString().Replace("Bottom", string.Empty).Replace("Middle", string.Empty).Replace("Top", string.Empty);

					verticalAlignment = verticalAlignment == "Middle" ? "Center" : verticalAlignment;

					writer.Add(string.Format("		this.{0}.setVerticalAlignment(SwingConstants.{1});",   control.Name, verticalAlignment.ToUpper()));
					writer.Add(string.Format("		this.{0}.setHorizontalAlignment(SwingConstants.{1});", control.Name, horizontalAlignment.ToUpper()));
				}
			}

		}

		/// <summary>
		/// Sets the control non basic properties.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="control">The control.</param>
		private void SetNonBasicProperties(List<string> writer, Control control)
		{

		}

		/// <summary>
		/// Determines whether the setText or setTitle methods are available to the specified control in java.
		/// </summary>
		/// <param name="control">The control.</param>
		/// <returns>
		/// 	<c>true</c> if is setText or setTitle is supported; otherwise, <c>false</c>.
		/// </returns>
		private bool IsSetTextSupported(Control control)
		{
			if (control is Panel       ||
				  control is TabPage     ||
				  control is TabControl  ||
				  control is UserControl ||
				  control is ListBox     ||
				  control is ComboBox    ||
				  control is GroupBox    ||
				  control is PictureBox  ||
				  control is StatusBar   ||
				  control is ToolBar     ||
				  control is StatusStrip ||
				  control is ToolStrip   ||
				  control is HScrollBar  ||
				  control is VScrollBar  ||
				  control is DataGrid)
				return false;

			return true;
		}

		/// <summary>
		/// Clones the item.
		/// </summary>
		/// <param name="obj">The obj.</param>
		/// <returns></returns>
		private object CloneItem(object obj)
		{
			Type objType = obj.GetType();
			PropertyInfo[] properties = objType.GetProperties();

			object ret = Activator.CreateInstance(objType);

			foreach (PropertyInfo property in properties)
			{
				if (!property.CanWrite || !property.CanRead || property.Name == "Controls")
					continue;

				try { property.SetValue(ret, property.GetValue(obj, null), null); } catch { }
			}

			return ret;
		}

		#endregion
	}
}
