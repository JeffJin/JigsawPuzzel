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
            return new Rect(X, Y, ViewSize, ViewSize);
        }

        protected override Rect CreateViewport()
        {
            return new Rect(0.0, 0.0, ViewSize, ViewSize);
        }

        public override double X
        {
            get
            {
                var x = (base.CurrentColumn*base.PieceSize - base.PieceSize/2);
                return x > 0 ? x : 0;
            }
        }

        public override double Y
        {
            get
            {
                var y = (base.CurrentRow*base.PieceSize - base.PieceSize/2);
                return y > 0 ? y : 0;
            }
        }

        private double ViewSize
        {
            get { 
                double viewSize = (double) base.PieceSize*3;
                if (viewSize > MaxImageSize)
                    return MaxImageSize;
                return viewSize;
            }
        }
    }
}
