using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MediaJigsaw.Models.Pieces
{
    public sealed class VideoJigsawRectPiece : VideoJigsawPieceBase
    {
        public VideoJigsawRectPiece(MediaElement mediaElement, int col, int row, double pieceSize)
            : base(mediaElement, col, row, pieceSize)
        {
            this._origin = new Point((col * pieceSize), (double)(row * pieceSize));
            this.Position = new Point((col * pieceSize), (double)(row * pieceSize));
            base.InitShapeProperties();
        }

        //TODO: Need to refactor with ImageJigsawRectPiece for duplication
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

        private readonly Point _origin;

        public override Point Origin
        {
            get { return _origin; }
        }

        public override Point Position { get; set; }
    }
}
