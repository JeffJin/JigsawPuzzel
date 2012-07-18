using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace MediaJigsaw.Models
{
    public class BezierCurveModel
    {
        public int Col { get; set; }
        public int Row { get; set; }
        public PathFigure Figure { get; set; }

        public BezierCurveModel(int col, int row)
        {
            Col = col;
            Row = row;
        }
    }
}
