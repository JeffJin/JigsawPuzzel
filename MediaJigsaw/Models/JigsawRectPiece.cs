using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MediaJigsaw.Models
{
    public sealed class JigsawRectPiece : JigsawPiece
    {
        private Point _origin;
        public JigsawRectPiece(BitmapImage imageSource, int col, int row, double pieceSize)
            : base(imageSource, col, row, pieceSize)
        {
            this._origin = new Point((col * pieceSize), (double)(row * pieceSize));
            this.Position = new Point((col * pieceSize), (double)(row * pieceSize)); ;
            base.InitShapeProperties();
        }

        protected override Geometry CreateGeometry()
        {
            return new RectangleGeometry(new Rect(0.0, 0.0, (double)base.PieceSize, (double)base.PieceSize));
        }

        protected override Rect CreateViewbox()
        {
            return new Rect((double)(base.CurrentColumn * base.PieceSize), (double)(base.CurrentRow * base.PieceSize), (double)base.PieceSize, (double)base.PieceSize);
        }

        protected override Rect CreateViewport()
        {
            return new Rect(0.0, 0.0, (double)base.PieceSize, (double)base.PieceSize);
        }

        public override Point Origin
        {
            get { return _origin; }
        }

        public override Point Position { get; set; }
    }
}
