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
        private BezierCurveModel _bezierCurveModel;
        public JigsawBezyPiece()
        {
            
        }

        public JigsawBezyPiece(BitmapImage imageSource, int col, int row, double pieceSize)
            : base(imageSource, col, row, pieceSize)
        {
            _bezierCurveModel = BezierCurveHelper.FindModel(row, col);
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
            return new Rect(_bezierCurveModel.ViewBoxPoint.X, _bezierCurveModel.ViewBoxPoint.Y, ViewSize, ViewSize);
        }

        protected override Rect CreateViewport()
        {
            return new Rect(_bezierCurveModel.ViewPortPoint.X, _bezierCurveModel.ViewPortPoint.Y, ViewSize, ViewSize);
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
