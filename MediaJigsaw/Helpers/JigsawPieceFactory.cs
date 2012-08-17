using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using MediaJigsaw.Models;
using MediaJigsaw.Models.Pieces;

namespace MediaJigsaw.Helpers
{
    public class JigsawPieceFactory
    {
        public static IJigsawPiece CreateImagePuzzelPiece(BitmapImage bitmapImage, int col, int row, double pieceSize, PieceType type)
        {
            if (type == PieceType.Rectangle)
                return new ImageJigsawRectPiece(bitmapImage, col, row, pieceSize);
            if (type == PieceType.PolyBezier)
                return new JigsawPolyBezierPiece(bitmapImage, col, row, pieceSize);
            if (type == PieceType.SimpleBezier)
                return new ImageJigsawSimpleBezierPiece(bitmapImage, col, row, pieceSize);

            throw new Exception("Invalid piece type");
        }

        public static IList<IJigsawPiece> CreateImagePuzzelPieces(BitmapImage bitmapImage, int totalCols, int totalRows, double pieceSize, PieceType pieceType)
        {
            var pieces = new List<IJigsawPiece>();
            for (int row = 0; row < totalRows; row++)
            {
                for (int col = 0; col < totalCols; col++)
                {
                    IJigsawPiece jigsawPiece = CreateImagePuzzelPiece(bitmapImage, col, row, pieceSize, pieceType);
                    pieces.Add(jigsawPiece);
                }
            }
            return pieces;
        }


        private static IJigsawPiece CreateVideoPuzzelPiece(MediaElement mediaElement, int col, int row, double pieceSize, PieceType type)
        {
            if (type == PieceType.Rectangle)
                return new VideoJigsawRectPiece(mediaElement, col, row, pieceSize);
            if (type == PieceType.PolyBezier)
                return new VideoJigsawPolyBezierPiece(mediaElement, col, row, pieceSize);
            if (type == PieceType.SimpleBezier)
                return new VideoJigsawSimpleBezierPiece(mediaElement, col, row, pieceSize);

            throw new Exception("Invalid piece type");
        }


        public static IList<IJigsawPiece> CreateVideoPuzzelPieces(MediaElement videoSource, int columns, int rows, double pieceSize, PieceType pieceType)
        {
            throw new NotImplementedException();
        }
    }
}
