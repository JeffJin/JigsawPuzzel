using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MediaJigsaw.Helpers;
using MediaJigsaw.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaJigsaw.Tests
{
    [TestClass]
    public class JigsawHelperTests
    {
        [TestMethod]
        public void TestConvertPointWithValidStrings()
        {
            const int rows = 4;
            const int columns = 4;
            var imaeSource = new BitmapImage();
            var pieces = new List<IJigsawPiece>();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    IJigsawPiece jigsawPiece = JigsawPieceFactory.Create(imaeSource, col, row, 200, PieceType.SimpleBezier);
                    pieces.Add(jigsawPiece);
                }
            }
            var scrambledPieces = JigsawHelper.ScramblePieces(pieces, rows, columns);

            foreach (var jigsawPiece in scrambledPieces)
            {
                var tempModel = BezierCurveHelper.FindModel(jigsawPiece.CurrentColumn, jigsawPiece.CurrentRow);
                Assert.AreEqual(jigsawPiece.Position, tempModel.Position);
            }
        }
        
    }
}
