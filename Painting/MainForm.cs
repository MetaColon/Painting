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
            i = new Coordinate(0,0);
            col = new ShapeCollection(new List<Shape>
            {
                new Polygon(8, 2, new Colour(Color.Red), new Coordinate(10, 10), new Coordinate(100, 100),
                    Colour.Invisible(), 0),
                new Ellipse(2, new Colour(Color.Blue), new Coordinate(60, 60), new Coordinate(100, 100),
                    Colour.Invisible())
            });
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            col.Paint(e.Graphics);
        }

        private void timer1_Tick (object sender, EventArgs e)
        {
            i = i.add(new Coordinate(1, 1));
            col.Position = i;
            label1.Text = i.ToString();
            Refresh();
        }
    }
}
