using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MediaJigsaw.Models;

namespace MediaJigsaw.Helpers
{
    public class JigsawHelper
    {
        public static IList<IJigsawPiece> ScramblePieces(List<IJigsawPiece> pieces, int rows, int cols)
        {
            var random = new Random();
            var temp = new List<KeyValuePair<int, int>>();
            IList<IJigsawPiece> scrambledPieces = new List<IJigsawPiece>();
            foreach (JigsawPiece originPiece in pieces)
            {
                int row = random.Next(0, rows);
                int col = random.Next(0, cols);

                while (temp.Exists(t => (t.Key == row) && (t.Value == col)))
                {
                    row = random.Next(0, rows);
                    col = random.Next(0, cols);
                }
                temp.Add(new KeyValuePair<int, int>(row, col));
                IJigsawPiece targetPiece = pieces.Single(p => p.OriginRow == row && p.OriginColumn == col);

                //swap positions
                var tempPoint = new Point(originPiece.Position.X, originPiece.Position.Y);
                originPiece.Position = new Point(targetPiece.Position.X, targetPiece.Position.Y);
                targetPiece.Position = tempPoint;

                //Swap column and row numbers
                int targetColumn = targetPiece.CurrentColumn;
                int targetRow = targetPiece.CurrentRow;
                targetPiece.CurrentColumn = originPiece.CurrentColumn;
                targetPiece.CurrentRow = originPiece.CurrentRow;
                originPiece.CurrentColumn = targetColumn;
                originPiece.CurrentRow = targetRow;
            }
            return pieces;
        }
    }
}
