using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Designer.Properties;
using Painting.Types.Paint;
using Rectangle = Painting.Types.Paint.Rectangle;

namespace Designer
{
    public partial class Designer : Form
    {
        private int _layerIndex;
        private Color _lineColor;
        private Color _mainColor;
        private int[] _selectedDraggingShapeIndex;

        private int _selectedShapeInViewIndex;

        public ShapeCollection Collection;
        public List<string> Items;

        public Designer()
        {
            InitializeComponent();
            Items = new List<string>
            {
                "Ellipse",
                "Line",
                "Polygon",
                "Rectangle"
            };
            SelectableShapes.DataSource = new BindingSource { DataSource = Items };
            SelectableShapes.DropDownStyle = ComboBoxStyle.DropDownList;
            Collection = new ShapeCollection(new ObservableCollection<Shape>
                {
                    new Line(new Coordinate(525, 319), new Coordinate(617, 319),
                        new Colour(Color.FromArgb(-16711936)), 7),
                    new Polygon(4, 2, new Colour(Color.FromArgb(-65536)), new Coordinate(468, 262),
                        new Coordinate(116, 116), new Colour(Color.Empty), 0, 45),
                    new Ellipse(0, new Colour(Color.Empty), new Coordinate(475, 270), new Coordinate(100, 100),
                        new Colour(Color.FromArgb(-16777216)), 0f),
                })
                {Position = new Coordinate(0, 0)};
            MainColor = Color.Empty;
            LineColor = Color.Empty;
            _selectedShapeInViewIndex = -1;
            _selectedDraggingShapeIndex = new[] { -1, -1, -1 };
            _layerIndex = 0;
        }

        public Color LineColor
        {
            get { return _lineColor; }
            set
            {
                _lineColor = value;
                LineColourBuuton.Text = Resources.Designer_LineColor_Select_Line_Color__ +
                                        Util.GetColorString(value);
            }
        }

        public Color MainColor
        {
            get { return _mainColor; }
            set
            {
                _mainColor = value;
                MainColorButton.Text = Resources.Designer_MainColor_Select_Main_Color__ +
                                       Util.GetColorString(value);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            switch (Items[SelectableShapes.SelectedIndex])
            {
                case "Ellipse":
                    AddShape(
                        new Ellipse((int)LineWidth.Value,
                            Util.GetColourToColor(LineColor),
                            new Coordinate(100, 100), new Coordinate(100, 100),
                            Util.GetColourToColor(MainColor), 0f));
                    break;
                case "Line":
                    AddShape(
                        new Line(new Coordinate(100, 100), new Coordinate(200, 200),
                            Util.GetColourToColor(LineColor), (int)LineWidth.Value));
                    break;
                case "Polygon":
                    AddShape(
                        new Polygon((int)Edges.Value, (int)LineWidth.Value,
                            Util.GetColourToColor(LineColor), new Coordinate(100, 100),
                            new Coordinate(100, 100), Util.GetColourToColor(MainColor), 0, 0));
                    break;
                case "Rectangle":
                    AddShape(
                        new Rectangle((int)LineWidth.Value,
                            Util.GetColourToColor(LineColor), new Coordinate(100, 100),
                            new Coordinate(100, 100), Util.GetColourToColor(MainColor), 0));
                    break;
                default:
                    throw new Exception("Unselectable Item selected");
            }
            SelectShape();
            Refresh();
            LineColor = Color.Empty;
            MainColor = Color.Empty;
        }

        private void AddShape(Shape s)
        {
            if ((s is Polygon && (Edges.Value < 2)) || (MainColor.IsEmpty && LineColor.IsEmpty) ||
                ((LineWidth.Value < 1) && MainColor.IsEmpty))
                return;
            Collection.Shapes = new ObservableCollection<Shape>(new List<Shape>(Collection.Shapes));
            Collection.Shapes.Insert(0, s);
            _selectedShapeInViewIndex = 0;
            _selectedDraggingShapeIndex = new[] { -1, -1, -1 };
        }

        private void CopyCodeButton_Click(object sender, EventArgs e)
            => Clipboard.SetText(Saving.GetSaveCode(Collection));

        private void Designer_FormClosing(object sender, FormClosingEventArgs e)
        {
            var res = MessageBox.Show(Resources.Designer_Designer_FormClosing_Do_you_want_to_save_your_latest_Changes_,
                Resources.Designer_Designer_FormClosing_Warning,
                MessageBoxButtons.YesNoCancel);
            switch (res)
            {
                case DialogResult.Yes:
                    SaveButton_Click(sender, e);
                    break;
                case DialogResult.No:
                    break;
                case DialogResult.Abort:
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }

        private void Designer_Load(object sender, EventArgs e)
        {
            foreach (Control control in Controls)
                control.PreviewKeyDown += Designer_PreviewKeyDown;
        }

        private void Designer_MouseDown(object sender, MouseEventArgs e)
        {
            _selectedShapeInViewIndex = GetClickedItem(e);
            SelectShape();
            if (_selectedShapeInViewIndex == -1)
                _selectedDraggingShapeIndex = new[] { -1, -1, -1 };
            else
                _selectedDraggingShapeIndex = new[]
                {
                    _selectedShapeInViewIndex, e.X - (int) Collection.Shapes[_selectedShapeInViewIndex].Position.X,
                    e.Y - (int) Collection.Shapes[_selectedShapeInViewIndex].Position.Y
                };
        }

        private void Designer_MouseMove(object sender, MouseEventArgs e)
        {
            if ((_selectedDraggingShapeIndex[0] < 0) || (_selectedDraggingShapeIndex[0] >= Collection.Shapes.Count))
                return;
            Collection.Shapes[_selectedDraggingShapeIndex[0]].Position =
                new Coordinate(e.X, e.Y).Sub(new Coordinate(_selectedDraggingShapeIndex[1],
                    _selectedDraggingShapeIndex[2]));
            SelectShape();
            Refresh();
        }

        private void Designer_MouseUp(object sender, MouseEventArgs e)
        {
            if ((_selectedDraggingShapeIndex[0] < 0) || (_selectedDraggingShapeIndex[0] >= Collection.Shapes.Count))
                return;
            Collection.Shapes[_selectedDraggingShapeIndex[0]].Position =
                new Coordinate(e.X, e.Y).Sub(new Coordinate(_selectedDraggingShapeIndex[1],
                    _selectedDraggingShapeIndex[2]));
            _selectedDraggingShapeIndex[0] = -1;
            SelectShape();
            Refresh();
        }

        private void Designer_MouseWheel(object sender, MouseEventArgs e)
        {
            if (_selectedShapeInViewIndex < 0)
                return;
            Collection.Shapes[_selectedShapeInViewIndex].Size =
                Collection.Shapes[_selectedShapeInViewIndex].Size.Add(new Coordinate(e.Delta / (float)15,
                    e.Delta / (float)15));
            SelectShape();
            Refresh();
        }

        private void Designer_Paint(object sender, PaintEventArgs e)
        {
            Collection.Paint(e.Graphics, Collection.Size.Div(2));
        }

        private void Designer_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if ((_selectedShapeInViewIndex <= -1) || (_selectedShapeInViewIndex >= Collection.Shapes.Count))
                return;
            var s = Collection.Shapes[_selectedShapeInViewIndex];
            var line = s as Line;
            var o = e.IsInputKey;
            e.IsInputKey = true;
            var dif = e.Shift ? 1 : 5;
            switch (e.KeyCode)
            {
                case Keys.Right:
                    s.Position = s.Position.Add(new Coordinate(dif, 0));
                    break;
                case Keys.Left:
                    s.Position = s.Position.Add(new Coordinate(-dif, 0));
                    break;
                case Keys.Up:
                    s.Position = s.Position.Add(new Coordinate(0, -dif));
                    break;
                case Keys.Down:
                    s.Position = s.Position.Add(new Coordinate(0, dif));
                    break;
                case Keys.Delete:
                    _selectedShapeInViewIndex = -1;
                    Collection.Shapes.Remove(s);
                    break;
                case Keys.D:
                    s.Size = s.Size.Add(new Coordinate(dif, 0));
                    break;
                case Keys.S:
                    s.Size = s.Size.Add(new Coordinate(0, dif));
                    break;
                case Keys.A:
                    s.Size = s.Size.Add(new Coordinate(-dif, 0));
                    break;
                case Keys.W:
                    s.Size = s.Size.Add(new Coordinate(0, -dif));
                    break;
                case Keys.R:
                    if (!(s is Polygon || s is Line || s is Ellipse || s is Rectangle))
                        break;
                    if (line != null)
                        line.Rotation += e.Shift ? 1 : 5;
                    else if (s is Polygon)
                        ((Polygon) s).Rotation += dif;
                    else if (s is Rectangle)
                        ((Rectangle) s).Rotation += dif;
                    else
                        ((Ellipse) s).Rotation += dif;
                    break;
                case Keys.T:
                    if(!(s is Polygon))
                        break;
                    ((Polygon) s).TurningAngle += dif;
                    break;
                case Keys.L:
                    _layerIndex = _layerIndex == 0 ? 1 : 0;
                    _selectedShapeInViewIndex = GetClickedItem(MousePosition.X, MousePosition.Y);
                    break;
                case Keys.F:
                    if ((_selectedShapeInViewIndex < 1) || (_selectedShapeInViewIndex >= Collection.Shapes.Count))
                        break;
                    Collection.Shapes.Move(_selectedShapeInViewIndex, _selectedShapeInViewIndex -= 1);
                    break;
                case Keys.B:
                    if ((_selectedShapeInViewIndex < 0) || (_selectedShapeInViewIndex >= Collection.Shapes.Count - 1))
                        break;
                    Collection.Shapes.Move(_selectedShapeInViewIndex, _selectedShapeInViewIndex += 1);
                    break;
                default:
                    e.IsInputKey = o;
                    return;
            }
            SelectShape();
            Refresh();
        }

        private int GetClickedItem(MouseEventArgs e) => GetClickedItem(e.X, e.Y);

        private int GetClickedItem(float x, float y)
        {
            var enumerable =
                Collection.Shapes.Select((shape, i) => shape.IsCoordinateInThis(new Coordinate(x, y)) ? i : -1).ToList();
            if (!enumerable.Any(i => i > -1))
                return -1;
            var f = enumerable.Where(i => i >= 0).ToList();
            return (_layerIndex < f.Count) && (_layerIndex >= 0) ? f[_layerIndex] : f.First();
        }

        private void LineColourButton_Click(object sender, EventArgs e)
        {
            ColorPicker.Color = LineColor;
            ColorPicker.ShowDialog();
            LineColor = ColorPicker.Color;
        }

        private void MainColorButton_Click(object sender, EventArgs e)
        {
            ColorPicker.Color = MainColor;
            ColorPicker.ShowDialog();
            MainColor = ColorPicker.Color;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var c = 0;
            while (File.Exists($"result{c}.txt"))
                c++;
            File.WriteAllText($"result{c}.txt", Saving.GetSaveCode(Collection));
        }

        private void SelectableShapes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Edges.Enabled = Items[SelectableShapes.SelectedIndex] == "Polygon";
            MainColorButton.Enabled = Items[SelectableShapes.SelectedIndex] != "Line";
        }

        private void SelectShape()
        {
            if ((_selectedShapeInViewIndex <= -1) || (_selectedShapeInViewIndex >= Collection.Shapes.Count))
            {
                Pointer.Text = string.Empty;
                return;
            }
            var sel = Collection.Shapes[_selectedShapeInViewIndex];
            var pos = sel.Position.Add(sel.Size.Div(new Coordinate(1, 2)).Add(new Coordinate(1, 1)));
            Pointer.Text = Resources.Designer_SelectShape___;
            Pointer.Left = (int)Math.Ceiling(pos.X);
            Pointer.Top = (int)Math.Ceiling(pos.Y);
        }
    }
}