using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MediaJigsaw.Models
{
    public class JigsawRectPiece : JigsawPiece
    {
        public JigsawRectPiece()
        {
            
        }

        public JigsawRectPiece(BitmapImage imageSource, int col, int row, double pieceSize)
            : base(imageSource, col, row, pieceSize)
        {
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

        public override double X
        {
            get { return this.CurrentColumn*this.PieceSize; }
        }

        public override double Y
        {
            get { return this.CurrentRow*this.PieceSize; }
        }
    }
}
