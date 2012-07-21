using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using MediaJigsaw.Models;

namespace MediaJigsaw.Helpers
{
    public class JigsawPieceFactory
    {
        public static IJigsawPiece Create(BitmapImage bitmapImage, int col, int row, double pieceSize, PieceType type)
        {
            if (type == PieceType.Rectangle)
                return new JigsawRectPiece(bitmapImage, col, row, pieceSize);
            if (type == PieceType.Polygon)
                return new JigsawPolygonPiece(bitmapImage, col, row, pieceSize);
            if (type == PieceType.PolyBezier)
                return new JigsawBezyPiece(bitmapImage, col, row, pieceSize);

            throw new Exception("Invalid piece type");
        }
    }
}
