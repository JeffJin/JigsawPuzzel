using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MediaJigsaw.Models
{
    public class BezierCurveModel
    {
        public int Col { get; set; }
        public int Row { get; set; }
        public Point ViewBoxPoint { get; set; }
        public Point ViewPortPoint { get; set; }
        public PathFigure Figure { get; set; }
        public Point Position { get; set; }

        public BezierCurveModel(int row, int col)
        {
            Col = col;
            Row = row;
        }
    }
}
