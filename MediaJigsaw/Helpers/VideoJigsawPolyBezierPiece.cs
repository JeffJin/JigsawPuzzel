using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MediaJigsaw.Models.Pieces;

namespace MediaJigsaw.Helpers
{
    internal class VideoJigsawPolyBezierPiece : VideoJigsawPieceBase
    {
        public VideoJigsawPolyBezierPiece(MediaElement mediaElement, int col, int row, double pieceSize)
            : base(mediaElement, col, row, pieceSize)
        {
            

        }

        protected override Geometry CreateGeometry()
        {
            throw new System.NotImplementedException();
        }

        protected override Rect CreateViewbox()
        {
            throw new System.NotImplementedException();
        }

        protected override Rect CreateViewport()
        {
            throw new System.NotImplementedException();
        }

        public override Point Origin
        {
            get { throw new System.NotImplementedException(); }
        }

        public override Point Position
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }
    }
}