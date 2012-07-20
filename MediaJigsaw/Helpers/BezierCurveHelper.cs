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


        private static PathFigure PathFigure0_0;

        static BezierCurveHelper()
        {
            //Init();
        }

        private static void Init()
        {
            PathFigure0_0.StartPoint = new Point(0, 0);
            PathFigure0_0.Segments.Add(new LineSegment(new Point(200, 0), true));
            var points1 = new List<Point>();
            points1.AddRange(new []{new Point(200,45), new Point(0,100), new Point(180, 160) });

            PathFigure0_0.Segments.Add(new PolyBezierSegment(points1, true));
            PathFigure0_0.Segments.Add(new PolyBezierSegment(new []{new Point(120,200), new Point(180,300), new Point(80, 260) }, true));
            PathFigure0_0.Segments.Add(new PolyBezierSegment(new []{new Point(60,240), new Point(40,200), new Point(0, 200) }, true));
            PathFigure0_0.Segments.Add(new LineSegment(new Point(0, 0), true));
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
                    new PolyBezierSegment(new[] {new Point(120, 200), new Point(180, 300), new Point(80, 260)}, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] {new Point(60, 240), new Point(40, 200), new Point(0, 200)}, true));
                pathFigure.Segments.Add(new LineSegment(new Point(0, 0), true));
            }
            else if (row == 1 && col == 0)
            {
                pathFigure.StartPoint = new Point(0, 200);
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(40, 200), new Point(60, 240), new Point(80, 260) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(80, 320), new Point(220, 360), new Point(140, 420) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(40, 420), new Point(100, 500), new Point(0, 400) }, true));
                pathFigure.Segments.Add(new LineSegment(new Point(0, 200), true));
            }
            else if (row == 2 && col == 0)
            {
                pathFigure.StartPoint = new Point(0, 400);
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(100, 500), new Point(40, 420), new Point(140, 420) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(220, 430), new Point(120, 500), new Point(210, 550) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(160, 550), new Point(100, 600), new Point(0, 600) }, true));
                pathFigure.Segments.Add(new LineSegment(new Point(0, 400), true));
            }
            else if (row == 3 && col == 0)
            {
                pathFigure.StartPoint = new Point(0, 600);
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(100, 600), new Point(160, 550), new Point(210, 550) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(50, 700), new Point(140, 650), new Point(200, 800) }, true));
                pathFigure.Segments.Add(new LineSegment(new Point(0, 800), true));
                pathFigure.Segments.Add(new LineSegment(new Point(0, 600), true));
            }
            else if (row == 0 && col == 1)
            {
                pathFigure.StartPoint = new Point(200, 0);
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(200, 45), new Point(0, 100), new Point(180, 160) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(240, 120), new Point(300, 180), new Point(320, 200) }, true));
                pathFigure.Segments.Add(
                    new PolyBezierSegment(new[] { new Point(330, 180), new Point(480, 160), new Point(400, 0) }, true));
                pathFigure.Segments.Add(new LineSegment(new Point(200, 0), true));
            }
            return pathFigure;
        }
    }
}
