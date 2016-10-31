using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;
using Painting.Types.Paint;

namespace TestApplication
{
    public partial class MainForm : Form
    {
        private readonly ShapeCollection _col;
        private float _i;
        private ShapeCollection foo;

        public MainForm()
        {
            InitializeComponent();
            foo = new ShapeCollection (new ObservableCollection<Shape> { new Ellipse (0, new Colour (Color.Empty), new Coordinate (644, 380), new Coordinate (50, 120), new Colour (Color.FromArgb (-16777216)), 90), new Polygon (3, 0, new Colour (Color.Empty), new Coordinate (600, 389), new Coordinate (70, 100), new Colour (Color.FromArgb (-65536)), 90, 30), }) { Position = new Coordinate (0, 0) };
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            foo.Paint(e.Graphics, foo.Position.Add(foo.Size.Div(2)));
            Console.WriteLine(foo.ToString());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _i++;
            foo.Rotation = _i;
            foo.Position = foo.Position.Add(new Coordinate(0,10));
            Refresh();
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            label1.Text = "X: " + e.X + "; Y: " + e.Y;
        }
    }
}