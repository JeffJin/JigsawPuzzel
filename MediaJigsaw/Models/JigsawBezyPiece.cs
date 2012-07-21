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
        public JigsawBezyPiece()
        {
            
        }

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
            var pathFigure = BezierCurveHelper.Create(base.CurrentRow, base.CurrentColumn);
            return new PathGeometry {Figures = {pathFigure}};
        }

        protected override Rect CreateViewbox()
        {
            return new Rect((base.CurrentColumn * base.PieceSize / 2d), (base.CurrentRow * base.PieceSize / 2d), (double)base.PieceSize * 2, (double)base.PieceSize * 2);
        }

        protected override Rect CreateViewport()
        {
            return new Rect(0.0, 0.0, (double)base.PieceSize * 2, (double)base.PieceSize * 2);
        }
    }
}
