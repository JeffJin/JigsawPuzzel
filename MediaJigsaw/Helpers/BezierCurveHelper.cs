using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using MediaJigsaw.Models;

namespace MediaJigsaw.Helpers
{
    public class BezierCurveHelper
    {
        private static IEnumerable<BezierCurveModel> _bezierCurveModels =
            new List<BezierCurveModel>(16)
                {
                    new BezierCurveModel(0, 0)
                        {
                            
                        }
                };


        static BezierCurveHelper()
        {
            //Init();
        }

        private static void Init()
        {

        }

        public static PathFigure Create(int row, int col)
        {
            var pathFigure = new PathFigure() { Segments = new PathSegmentCollection() };
            if (row == 0 && col == 0)
            {
                pathFigure.StartPoint = new Point(0, 0);
                pathFigure.Segments.Add(new LineSegment(new Point(200, 0), true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] {new Point(200, 45), new Point(0, 100), new Point(180, 160)}, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] {new Point(120, 200), new Point(180, 300), new Point(100, 260)}, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] {new Point(60, 240), new Point(40, 200), new Point(0, 200)}, true));
                pathFigure.Segments.Add(new LineSegment(new Point(0, 0), true));
            }
            else if (row == 1 && col == 0)
            {
                pathFigure.StartPoint = new Point(0, 100);
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(40, 100), new Point(60, 140), new Point(100, 160) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(100, 220), new Point(220, 260), new Point(140, 320) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(40, 320), new Point(100, 400), new Point(0, 300) }, true));
                pathFigure.Segments.Add(new LineSegment(new Point(0, 100), true));
            }
            else if (row == 2 && col == 0)
            {
                pathFigure.StartPoint = new Point(0, 100);
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(100, 200), new Point(40, 120), new Point(140, 120) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(220, 130), new Point(120, 200), new Point(210, 340) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(160, 340), new Point(100, 300), new Point(0, 300) }, true));
                pathFigure.Segments.Add(new LineSegment(new Point(0, 100), true));
            }
            else if (row == 3 && col == 0)
            {
                pathFigure.StartPoint = new Point(0, 100);
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(100, 100), new Point(160, 140), new Point(210, 140) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(50, 200), new Point(140, 150), new Point(200, 300) }, true));
                pathFigure.Segments.Add(new LineSegment(new Point(0, 300), true));
                pathFigure.Segments.Add(new LineSegment(new Point(0, 100), true));
            }
            else if (row == 0 && col == 1)
            {
                pathFigure.StartPoint = new Point(100, 0);
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(100, 45), new Point(-100, 100), new Point(80, 160) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(140, 120), new Point(200, 180), new Point(220, 200) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(230, 180), new Point(380, 160), new Point(300, 0) }, true));
                pathFigure.Segments.Add(new LineSegment(new Point(100, 0), true));
            }
            else if (row == 1 && col == 1)
            {
                pathFigure.StartPoint = new Point(100, 0);
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(140, 20), new Point(200, 80), new Point(220, 100) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(210, 140), new Point(240, 180), new Point(280, 170) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(250, 250), new Point(170, 255), new Point(170, 260) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(165, 300), new Point(150, 350), new Point(120, 360) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(120, 320), new Point(80, 320), new Point(40, 320) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(120, 260), new Point(0, 220), new Point(0, 160) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(80, 200), new Point(20, 100), new Point(80, 160) }, true));
            }
            return pathFigure;
        }
    }
}
