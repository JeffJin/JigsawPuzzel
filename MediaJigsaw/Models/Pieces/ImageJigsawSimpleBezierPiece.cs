using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MediaJigsaw.Helpers;

namespace MediaJigsaw.Models.Pieces
{
    public sealed class ImageJigsawSimpleBezierPiece : ImageJigsawPieceBase
    {
        private BezierCurveModel _bezierCurveModel;

        public ImageJigsawSimpleBezierPiece(BitmapImage imageSource, int col, int row, double pieceSize)
            : base(imageSource, col, row, pieceSize)
        {
            _bezierCurveModel = BezierCurveHelper.FindModel(col, row);
            if(_bezierCurveModel != null)
            {
                base.InitShapeProperties();
                this.Position = _bezierCurveModel.Position;
            }
        }

        protected override Geometry CreateGeometry()
        {
            PathGeometry geometry = CreatePolyBezierPath();
            return geometry;
        }

        private PathGeometry CreatePolyBezierPath()
        {
            return new PathGeometry {Figures = {_bezierCurveModel.Figure}};
        }

        protected override Rect CreateViewbox()
        {
            return new Rect(_bezierCurveModel.ViewBoxPoint.X, _bezierCurveModel.ViewBoxPoint.Y, ViewSize, ViewSize);
        }

        protected override Rect CreateViewport()
        {
            return new Rect(_bezierCurveModel.ViewPortPoint.X, _bezierCurveModel.ViewPortPoint.Y, ViewSize, ViewSize);
        }

        public override Point Origin
        {
            get { return _bezierCurveModel.Position; }
        }

        public override Point Position { get; set; }

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
