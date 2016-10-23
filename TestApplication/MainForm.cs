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

namespace TestApplication
{
    public partial class MainForm : Form
    {
        private Coordinate _i;
        private readonly ShapeCollection _col;

        public MainForm ()
        {
            InitializeComponent ();
            _i = new Coordinate (100, 100);
            _col = new ShapeCollection (new List<Shape>
            {
                new Polygon(8, 2, new Colour(Color.Red), new Coordinate(10, 10), new Coordinate(1, 1),
                    Colour.Invisible(), 0),
                new Ellipse(2, new Colour(Color.Blue), new Coordinate(60, 60), new Coordinate(1, 1),
                    Colour.Invisible()),
                new Line(new Coordinate(10,10), new Coordinate(11, 11), new Colour(Color.Green), 2)
            });
        }

        private void MainForm_Paint (object sender, PaintEventArgs e)
        {
            new ShapeCollection (new List<Shape> { new Polygon (6, 0, new Colour (Color.Empty), new Coordinate (490, 275), new Coordinate (100, 100), new Colour (Color.FromArgb (-16777216)), 0), new Ellipse (2, new Colour (Color.FromArgb (-65536)), new Coordinate (520, 300), new Coordinate (45, 45), new Colour (Color.Empty)), new Line (new Coordinate (495, 255), new Coordinate (545, 320), new Colour (Color.FromArgb (-8388353)), 5), }).Paint(e.Graphics);
        }

        private void timer1_Tick (object sender, EventArgs e)
        {
            _i = _i.Add (new Coordinate (1, 1));
            _col.Size = new Coordinate (_i.X, _i.Y);
            label1.Text = _i.ToString ();
            Refresh ();
        }
    }
}
