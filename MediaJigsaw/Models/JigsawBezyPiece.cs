using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MediaJigsaw.Models
{
    public class JigsawBezyPiece : JigsawPiece
    {
        // Fields
        public double[] CurvyCoords;

        // Methods
        public JigsawBezyPiece(BitmapImage imageSource, int col, int row, double pieceSize)
            : base(imageSource, col, row, pieceSize)
        {
            this.CurvyCoords = new double[] { 
            0.0, 0.0, 35.0, 15.0, 37.0, 5.0, 37.0, 5.0, 40.0, 0.0, 38.0, -5.0, 38.0, -5.0, 20.0, -20.0, 
            50.0, -20.0, 50.0, -20.0, 80.0, -20.0, 62.0, -5.0, 62.0, -5.0, 60.0, 0.0, 63.0, 5.0, 63.0, 5.0, 
            65.0, 15.0, 100.0, 0.0
         };
        }

        protected override Geometry CreateGeometry()
        {
            ConnectionType[] types = GetConnectionType(base.CurrentRow, base.CurrentColumn, base.PieceSize);
            return CreatePolyBezierPath(types[0], types[1], types[2], types[3], base.PieceSize);
        }

        private static PathGeometry CreatePolyBezierPath(ConnectionType upperConnection, ConnectionType rightConnection, ConnectionType bottomConnection, ConnectionType leftConnection, int pieceSize)
    {
        var upperPoints = new List<Point>();
        var rightPoints = new List<Point>();
        var bottomPoints = new List<Point>();
        var leftPoints = new List<Point>();
        var upperSegment = new PolyBezierSegment(upperPoints, true);
        var rightSegment = new PolyBezierSegment(rightPoints, true);
        var bottomSegment = new PolyBezierSegment(bottomPoints, true);
        var leftSegment = new PolyBezierSegment(leftPoints, true);

        var pathFigure = new PathFigure {
            IsClosed = false,
            StartPoint = new Point(0.0, 0.0)
        };
        pathFigure.Segments.Add(upperSegment);
        pathFigure.Segments.Add(rightSegment);
        pathFigure.Segments.Add(bottomSegment);
        pathFigure.Segments.Add(leftSegment);
        return new PathGeometry { Figures = { pathFigure } };
    }

        protected override Rect CreateViewbox()
        {
            return new Rect((double)(base.CurrentColumn * base.PieceSize), (double)(base.CurrentRow * base.PieceSize), (double)base.PieceSize, (double)base.PieceSize);
        }

        protected override Rect CreateViewport()
        {
            return new Rect(0.0, 0.0, (double)base.PieceSize, (double)base.PieceSize);
        }

        private static ConnectionType[] GetConnectionType(int row, int column, int pieceSize)
        {
            return new ConnectionType[] { ConnectionType.Male };
        }
    }


}
