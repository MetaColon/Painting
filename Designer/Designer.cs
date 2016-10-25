using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private int[] _selectedDraggingShapeIndex;

        private int _selectedShapeInViewIndex;
        private int _layerIndex;

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
            _selectedShapeInViewIndex = -1;
            _selectedDraggingShapeIndex = new[] { -1, -1, -1 };
            _layerIndex = 0;
        }

        private void AddButton_Click (object sender, EventArgs e)
        {
            switch (Items[SelectableShapes.SelectedIndex])
            {
                case "Ellipse":
                    AddShape (
                        new Ellipse ((int) LineWidth.Value, !LineColor.IsEmpty ? new Colour (LineColor) : Colour.Invisible (),
                            new Coordinate (100, 100), new Coordinate (100, 100),
                            !MainColor.IsEmpty ? new Colour (MainColor) : Colour.Invisible ()));
                    break;
                case "Line":
                    AddShape (
                        new Line (new Coordinate (100, 100), new Coordinate (200, 200), !LineColor.IsEmpty ? new Colour (LineColor) : Colour.Invisible (), (int) LineWidth.Value));
                    break;
                case "Polygon":
                    AddShape (
                        new Polygon ((int) Edges.Value, (int) LineWidth.Value, !LineColor.IsEmpty ? new Colour (LineColor) : Colour.Invisible (), new Coordinate (100, 100), new Coordinate (100, 100), !MainColor.IsEmpty ? new Colour (MainColor) : Colour.Invisible (), 0));
                    break;
                case "Rectangle":
                    AddShape (
                        new Painting.Types.Paint.Rectangle ((int) LineWidth.Value, !LineColor.IsEmpty ? new Colour (LineColor) : Colour.Invisible (), new Coordinate (100, 100), new Coordinate (100, 100), !MainColor.IsEmpty ? new Colour (MainColor) : Colour.Invisible ()));
                    break;
                default:
                    throw new Exception ("Unselectable Item selected");
            }
            SelectShape();
            Refresh ();
            LineColor = Color.Empty;
            MainColor = Color.Empty;
        }

        private void AddShape (Shape s)
        {
            if (s is Polygon && Edges.Value < 2 || MainColor.IsEmpty && LineColor.IsEmpty || LineWidth.Value < 1 && MainColor.IsEmpty)
                return;
            Collection.Shapes = new ObservableCollection<Shape> (new List<Shape> (Collection.Shapes));
            Collection.Shapes.Insert(0, s);
            _selectedShapeInViewIndex = 0;
            _selectedDraggingShapeIndex = new[]{-1,-1,-1};
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
        }

        private void Designer_MouseDown (object sender, MouseEventArgs e)
        {
            _selectedShapeInViewIndex = GetClickedItem (e);
            SelectShape ();
            if (_selectedShapeInViewIndex == -1)
                _selectedDraggingShapeIndex = new[] { -1, -1, -1 };
            else
                _selectedDraggingShapeIndex = new[]
                    {_selectedShapeInViewIndex, e.X - (int) Collection.Shapes[_selectedShapeInViewIndex].Position.X, e.Y - (int) Collection.Shapes[_selectedShapeInViewIndex].Position.Y};
        }

        private void Designer_MouseMove (object sender, MouseEventArgs e)
        {
            if (_selectedDraggingShapeIndex[0] < 0 || _selectedDraggingShapeIndex[0] >= Collection.Shapes.Count)
                return;
            Collection.Shapes[_selectedDraggingShapeIndex[0]].Position = new Coordinate (e.X, e.Y).Sub (new Coordinate (_selectedDraggingShapeIndex[1], _selectedDraggingShapeIndex[2]));
            SelectShape ();
            Refresh ();
        }

        private void Designer_MouseUp (object sender, MouseEventArgs e)
        {
            if (_selectedDraggingShapeIndex[0] < 0 || _selectedDraggingShapeIndex[0] >= Collection.Shapes.Count)
                return;
            Collection.Shapes[_selectedDraggingShapeIndex[0]].Position = new Coordinate (e.X, e.Y).Sub (new Coordinate (_selectedDraggingShapeIndex[1], _selectedDraggingShapeIndex[2]));
            _selectedDraggingShapeIndex[0] = -1;
            SelectShape ();
            Refresh ();
        }

        private void Designer_MouseWheel (object sender, MouseEventArgs e)
        {
            if (_selectedShapeInViewIndex < 0)
                return;
            Collection.Shapes[_selectedShapeInViewIndex].Size = Collection.Shapes[_selectedShapeInViewIndex].Size.Add (new Coordinate (e.Delta/(float)15, e.Delta/(float)15));
            SelectShape ();
            Refresh ();
        }

        private void Designer_Paint (object sender, PaintEventArgs e) => Collection.Paint (e.Graphics);

        private void Designer_PreviewKeyDown (object sender, PreviewKeyDownEventArgs e)
        {
            if (_selectedShapeInViewIndex <= -1 || _selectedShapeInViewIndex >= Collection.Shapes.Count)
                return;
            var s = Collection.Shapes[_selectedShapeInViewIndex];
            var line = s as Line;
            switch (e.KeyCode)
            {
                case Keys.Right:
                    e.IsInputKey = true;
                    s.Position = s.Position.Add (new Coordinate (e.Shift ? 1 : 5, 0));
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.Left:
                    e.IsInputKey = true;
                    s.Position = s.Position.Add (new Coordinate (e.Shift ? -1 : -5, 0));
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.Up:
                    e.IsInputKey = true;
                    s.Position = s.Position.Add (new Coordinate (0, e.Shift ? -1 : -5));
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.Down:
                    e.IsInputKey = true;
                    s.Position = s.Position.Add (new Coordinate (0, e.Shift ? 1 : 5));
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.Delete:
                    e.IsInputKey = true;
                    _selectedShapeInViewIndex = -1;
                    Collection.Shapes.Remove (s);
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.D:
                    e.IsInputKey = true;
                    s.Size = s.Size.Add (new Coordinate (e.Shift ? 1 : 5, 0));
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.S:
                    e.IsInputKey = true;
                    s.Size = s.Size.Add (new Coordinate (0, e.Shift ? 1 : 5));
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.A:
                    e.IsInputKey = true;
                    s.Size = s.Size.Add (new Coordinate (e.Shift ? -1 : -5, 0));
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.W:
                    e.IsInputKey = true;
                    s.Size = s.Size.Add (new Coordinate (0, e.Shift ? -1 : -5));
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.T:
                    if (!(s is Polygon || s is Line))
                        break;
                    e.IsInputKey = true;
                    if (line != null)
                        line.Rotation += e.Shift ? 1 : 5;
                    else
                        ((Polygon) s).Rotation += e.Shift ? 1 : 5;
                    SelectShape ();
                    Refresh ();
                    break;
                case Keys.L:
                    e.IsInputKey = true;
                    _layerIndex = _layerIndex == 0 ? 1 : 0;
                    _selectedShapeInViewIndex = GetClickedItem (MousePosition.X, MousePosition.Y);
                    SelectShape ();
                    Refresh();
                    break;
                case Keys.F:
                    e.IsInputKey = true;
                    if (_selectedShapeInViewIndex < 1 || _selectedShapeInViewIndex > Collection.Shapes.Count)
                        break;
                    Collection.Shapes.Move(_selectedShapeInViewIndex, _selectedShapeInViewIndex-=1);
                    SelectShape();
                    Refresh();
                    break;
                case Keys.B:
                    e.IsInputKey = true;
                    if (_selectedShapeInViewIndex < 0 || _selectedShapeInViewIndex >= Collection.Shapes.Count)
                        break;
                    Collection.Shapes.Move(_selectedShapeInViewIndex, _selectedShapeInViewIndex+=1);
                    SelectShape();
                    Refresh();
                    break;
            }
        }

        private int GetClickedItem (MouseEventArgs e) => GetClickedItem (e.X, e.Y);

        private int GetClickedItem (float x, float y)
        {
            var enumerable =
                Collection.Shapes.Select((shape, i) => shape.IsCoordinateInThis(new Coordinate(x, y)) ? i : -1).ToList();
            if (!enumerable.Any(i => i > -1)) return -1;
            var f = enumerable.Where(i => i >= 0).ToList();
            return _layerIndex < f.Count && _layerIndex >= 0 ? f[_layerIndex] : f.First();
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

        private void SaveButton_Click (object sender, EventArgs e)
        {
            var c = 0;
            while (File.Exists ($"result{c}.txt"))
                c++;
            File.WriteAllText ($"result{c}.txt", Saving.GetSaveCode (Collection));
        }

        private void SelectableShapes_SelectedIndexChanged (object sender, EventArgs e)
            => Edges.Enabled = Items[SelectableShapes.SelectedIndex] == "Polygon";

        private void SelectShape ()
        {
            if (_selectedShapeInViewIndex <= -1 || _selectedShapeInViewIndex >= Collection.Shapes.Count)
            {
                Pointer.Text = string.Empty;
                return;
            }
            var sel = Collection.Shapes[_selectedShapeInViewIndex];
            var pos = sel.Position.Add (sel.Size.Div (new Coordinate (1, 2)).Add (new Coordinate (1, 1)));
            Pointer.Text = "<-";
            Pointer.Left = (int) Math.Ceiling (pos.X);
            Pointer.Top = (int) Math.Ceiling (pos.Y);
        }

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
