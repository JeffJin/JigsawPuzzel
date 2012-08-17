using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MediaJigsaw.Models.Pieces
{
    public class JigsawPolyBezierPiece : ImageJigsawPieceBase
    {

        public JigsawPolyBezierPiece(BitmapImage imageSource, int col, int row, double pieceSize)
            : base(imageSource, col, row, pieceSize)
        {
        }

        protected override Geometry CreateGeometry()
        {
            ConnectionType[] types = GetConnectionType(base.CurrentRow, base.CurrentColumn, base.PieceSize);
            return CreatePolygonPath(types[0], types[1], types[2], types[3], base.PieceSize);
        }

        private static PathGeometry CreatePolygonPath(ConnectionType male, ConnectionType connectionType, ConnectionType male1, ConnectionType connectionType1, int pieceSize)
        {
            throw new NotImplementedException();
        }

        protected override Rect CreateViewbox()
        {
            throw new NotImplementedException();
        }

        protected override Rect CreateViewport()
        {
            throw new NotImplementedException();
        }

        public override Point Origin
        {
            get { throw new NotImplementedException(); }
        }

        public override Point Position
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        private static ConnectionType[] GetConnectionType(int row, int column, int pieceSize)
        {
            return new ConnectionType[] { ConnectionType.Male };
        }
    }
}
