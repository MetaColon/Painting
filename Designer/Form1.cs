using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Painting.Types.Paint;

namespace Designer
{
    public partial class Designer : Form
    {
        private Color _mainColor;
        public Color MainColor
        {
            get
            {
                return _mainColor;
            }
            set
            {
                _mainColor = value;
                MainColorButton.Text = "Select Main Color (" + (!value.IsEmpty ? $"{value.ToString().Split('[').Last().Split(']').First()})" : "Invisible)");
            }
        }
        private Color _lineColor;

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
                    break;
                case "Rectangle":
                    break;
                default:
                    throw new Exception ("UnselectableItemSelected");
            }
            Refresh ();
            LineColor = Color.Empty;
            MainColor = Color.Empty;
        }

        private void AddShape (Shape s) => Collection.Shapes = new List<Shape> (new List<Shape> (Collection.Shapes) { s });

        private void Designer_Paint (object sender, PaintEventArgs e) => Collection.Paint (e.Graphics);

        private void SelectableShapes_SelectedIndexChanged (object sender, EventArgs e)
            => Edges.Enabled = Items[SelectableShapes.SelectedIndex] == "Polygon";

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
    }
}
