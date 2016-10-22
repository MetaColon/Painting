using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using Painting.Types.Paint;
using Rectangle = Painting.Types.Paint.Rectangle;

namespace Painting
{
    public partial class MainForm : Form
    {
        private Coordinate i;
        private ShapeCollection col;

        public MainForm ()
        {
            InitializeComponent ();
            i = new Coordinate(100,100);
            col = new ShapeCollection(new List<Shape>
            {
                new Polygon(8, 2, new Colour(Color.Red), new Coordinate(10, 10), new Coordinate(1, 1),
                    Colour.Invisible(), 0),
                new Ellipse(2, new Colour(Color.Blue), new Coordinate(60, 60), new Coordinate(1, 1),
                    Colour.Invisible()),
                new Line(new Coordinate(10,10), new Coordinate(11, 11), new Colour(Color.Green), 2)
            });
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            col.Paint(e.Graphics);
        }

        private void timer1_Tick (object sender, EventArgs e)
        {
            i = i.Add(new Coordinate(1, 1));
            col.Size = new Coordinate(i.X, i.Y);
            label1.Text = i.ToString();
            Refresh();
        }
    }
}
