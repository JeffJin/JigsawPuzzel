﻿using System;
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
            Point tempPoint;
            IJigsawPiece tempPiece;
            foreach (JigsawPiece piece in pieces)
            {
                int row = random.Next(0, rows);
                int col = random.Next(0, cols);

                while (temp.Exists(t => (t.Key == row) && (t.Value == col)))
                {
                    row = random.Next(0, rows);
                    col = random.Next(0, cols);
                }
                tempPiece = pieces.Single(p => p.OriginRow == row && p.OriginColumn == col);
                tempPoint = new Point(piece.Position.X, piece.Position.Y);

                temp.Add(new KeyValuePair<int, int>(row, col));
                piece.CurrentRow = row;
                piece.CurrentColumn = col;
                piece.Position = new Point(tempPiece.Position.X, tempPiece.Position.Y);
                tempPiece.Position = tempPoint;

            }
            return pieces;
        }
    }
}
