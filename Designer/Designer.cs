﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Painting.Types.Paint;

namespace Designer
{
    public partial class Designer : Form
    {
        private Color _lineColor;
        private Color _mainColor;

        public ShapeCollection Collection;
        public List<string> Items;

        public Designer ()
        {
            InitializeComponent ();
            Items = new List<string>
            {
                "Ellipse",
                "Line",
                "Polygon",
                "Rectangle"
            };
            SelectableShapes.DataSource = new BindingSource { DataSource = Items };
            SelectableShapes.DropDownStyle = ComboBoxStyle.DropDownList;
            Collection = new ShapeCollection (new List<Shape> ());
            MainColor = Color.Empty;
            LineColor = Color.Empty;
            SelectedShapeInViewIndex = -1;
        }

        private void AddButton_Click (object sender, EventArgs e)
        {
            switch (Items[SelectableShapes.SelectedIndex])
            {
                case "Ellipse":
                    AddShape (
                        new Ellipse ((int) Width.Value, !LineColor.IsEmpty ? new Colour (LineColor) : Colour.Invisible (),
                            new Coordinate (100, 100), new Coordinate (100, 100),
                            !MainColor.IsEmpty ? new Colour (MainColor) : Colour.Invisible ()));
                    break;
                case "Line":
                    AddShape (
                        new Line (new Coordinate (100, 100), new Coordinate (200, 200), !LineColor.IsEmpty ? new Colour (LineColor) : Colour.Invisible (), (int) Width.Value));
                    break;
                case "Polygon":
                    AddShape (
                        new Polygon ((int) Edges.Value, (int) Width.Value, !LineColor.IsEmpty ? new Colour (LineColor) : Colour.Invisible (), new Coordinate (100, 100), new Coordinate (100, 100), !MainColor.IsEmpty ? new Colour (MainColor) : Colour.Invisible (), 0));
                    break;
                case "Rectangle":
                    AddShape (
                        new Painting.Types.Paint.Rectangle ((int) Width.Value, !LineColor.IsEmpty ? new Colour (LineColor) : Colour.Invisible (), new Coordinate (100, 100), new Coordinate (100, 100), !MainColor.IsEmpty ? new Colour (MainColor) : Colour.Invisible ()));
                    break;
                default:
                    throw new Exception ("Unselectable Item selected");
            }
            Refresh ();
            LineColor = Color.Empty;
            MainColor = Color.Empty;
        }

        private void AddShape (Shape s)
        {
            if (s is Polygon && Edges.Value < 2 || MainColor.IsEmpty && LineColor.IsEmpty || Width.Value < 1 && MainColor.IsEmpty)
                return;
            Collection.Shapes = new List<Shape> (new List<Shape> (Collection.Shapes) { s });
        }

        private void CopyCodeButton_Click (object sender, EventArgs e) => Clipboard.SetText (Saving.GetSaveCode (Collection));

        private void Designer_FormClosing (object sender, FormClosingEventArgs e)
        {
            var res = MessageBox.Show ("Do you want to save your latest Changes?", "Warning",
                MessageBoxButtons.YesNoCancel);
            switch (res)
            {
                case DialogResult.Yes:
                    SaveButton_Click (sender, e);
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.Abort:
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }

        private void Designer_Load (object sender, EventArgs e)
        {
            foreach (Control control in Controls)
                control.PreviewKeyDown += Designer_PreviewKeyDown;
        }

        private void Designer_MouseClick (object sender, MouseEventArgs e)
        {
            SelectedShapeInViewIndex = GetClickedItem (e);
            SelectShape ();
        }

        private void Designer_Paint (object sender, PaintEventArgs e)
        {
            Collection.Paint (e.Graphics);
            new ShapeCollection (new List<Shape> { new Ellipse (0, new Colour (Color.Empty), new Coordinate (185, 225), new Coordinate (85, 110), new Colour (Color.FromArgb (-128))), new Polygon (5, 3, new Colour (Color.FromArgb (-16711808)), new Coordinate (210, 165), new Coordinate (100, 100), new Colour (Color.Empty), 25), new Line (new Coordinate (175, 195), new Coordinate (280, 295), new Colour (Color.FromArgb (-65408)), 3), }).Paint (e.Graphics);
        }

        private void Designer_PreviewKeyDown (object sender, PreviewKeyDownEventArgs e)
        {
            if (SelectedShapeInViewIndex <= -1 || SelectedShapeInViewIndex >= Collection.Shapes.Count)
                return;
            var s = Collection.Shapes[SelectedShapeInViewIndex];
            var line = s as Line;
            switch (e.KeyCode)
            {
                case Keys.Right:
                    e.IsInputKey = true;
                    s.Position = s.Position.Add (new Coordinate (5, 0));
                    if (line != null)
                        line.End = line?.End.Add (new Coordinate (5, 0));
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.Left:
                    e.IsInputKey = true;
                    s.Position = s.Position.Add (new Coordinate (-5, 0));
                    if (line != null)
                        line.End = line?.End.Add (new Coordinate (-5, 0));
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.Up:
                    e.IsInputKey = true;
                    s.Position = s.Position.Add (new Coordinate (0, -5));
                    if (line != null)
                        line.End = line?.End.Add (new Coordinate (0, -5));
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.Down:
                    e.IsInputKey = true;
                    s.Position = s.Position.Add (new Coordinate (0, 5));
                    if (line != null)
                        line.End = line?.End.Add (new Coordinate (0, 5));
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.Delete:
                    e.IsInputKey = true;
                    SelectedShapeInViewIndex = -1;
                    Collection.Shapes.Remove (s);
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.D:
                    e.IsInputKey = true;
                    s.Size = s.Size.Add (new Coordinate (5, 0));
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.S:
                    e.IsInputKey = true;
                    s.Size = s.Size.Add (new Coordinate (0, 5));
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.A:
                    e.IsInputKey = true;
                    s.Size = s.Size.Add (new Coordinate (-5, 0));
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.W:
                    e.IsInputKey = true;
                    s.Size = s.Size.Add (new Coordinate (0, -5));
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.T:
                    if (!(s is Polygon))
                        break;
                    e.IsInputKey = true;
                    ((Polygon) s).TurningAngle += 5;
                    Refresh ();
                    break;
            }
        }

        private int GetClickedItem (MouseEventArgs e)
        {
            var enumerable = Collection.Shapes.Select ((shape, i) => shape.IsCoordinateInThis (new Coordinate (e.X, e.Y)) ? i : -1).ToList ();
            if (enumerable.Any (i => i > -1))
                return enumerable.First (i => i > -1);
            return -1;
        }

        private void LineColourButton_Click (object sender, EventArgs e)
        {
            ColorPicker.Color = LineColor;
            ColorPicker.ShowDialog ();
            LineColor = ColorPicker.Color;
        }

        private void MainColorButton_Click (object sender, EventArgs e)
        {
            ColorPicker.Color = MainColor;
            ColorPicker.ShowDialog ();
            MainColor = ColorPicker.Color;
        }

        private void SaveButton_Click (object sender, EventArgs e) => File.WriteAllText ("result.txt", Saving.GetSaveCode (Collection));

        private void SelectableShapes_SelectedIndexChanged (object sender, EventArgs e)
            => Edges.Enabled = Items[SelectableShapes.SelectedIndex] == "Polygon";

        private void SelectShape ()
        {
            if (SelectedShapeInViewIndex <= -1 || SelectedShapeInViewIndex >= Collection.Shapes.Count)
            {
                Pointer.Text = string.Empty;
                return;
            }
            var sel = Collection.Shapes[SelectedShapeInViewIndex];
            var pos = sel.Position.Add (sel.Size.Div (new Coordinate (1, 2)).Add (new Coordinate (1, 1)));
            Pointer.Text = "<-";
            Pointer.Left = (int) Math.Ceiling (pos.X);
            Pointer.Top = (int) Math.Ceiling (pos.Y);
        }

        private int SelectedShapeInViewIndex { get; set; }

        public Color LineColor
        {
            get
            {
                return _lineColor;
            }
            set
            {
                _lineColor = value;
                LineColourBuuton.Text = "Select Line Color (" + (!value.IsEmpty ? $"{value.ToString ().Split ('[').Last ().Split (']').First ()})" : "Invisible)");
            }
        }
        public Color MainColor
        {
            get
            {
                return _mainColor;
            }
            set
            {
                _mainColor = value;
                MainColorButton.Text = "Select Main Color (" + (!value.IsEmpty ? $"{value.ToString ().Split ('[').Last ().Split (']').First ()})" : "Invisible)");
            }
        }
    }
}
