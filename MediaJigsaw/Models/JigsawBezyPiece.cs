using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MediaJigsaw.Helpers;

namespace MediaJigsaw.Models
{
    public class JigsawBezyPiece : JigsawPiece
    {
        public JigsawBezyPiece(BitmapImage imageSource, int col, int row, double pieceSize)
            : base(imageSource, col, row, pieceSize)
        {

        }

        protected override Geometry CreateGeometry()
        {
            PathGeometry geometry = CreatePolyBezierPath();
            return geometry;
        }

        private PathGeometry CreatePolyBezierPath()
        {
            var pathFigure = BezierCurveHelper.Create(base.CurrentColumn, base.CurrentRow);
            return new PathGeometry {Figures = {pathFigure}};
        }

        protected override Rect CreateViewbox()
        {
            return new Rect((double)(base.CurrentColumn * base.PieceSize), (double)(base.CurrentRow * base.PieceSize), (double)base.PieceSize, (double)base.PieceSize);
        }

        protected override Rect CreateViewport()
        {
            return new Rect(0.0, 0.0, (double)base.PieceSize, (double)base.PieceSize);
        }
    }
}
