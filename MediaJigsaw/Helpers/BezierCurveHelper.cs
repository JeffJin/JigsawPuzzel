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

        public static PathFigure Create(int rows, int cols)
        {
            PathFigure pathFigure = new PathFigure() { Segments = new PathSegmentCollection() };
            pathFigure.StartPoint = new Point(0, 0);
            pathFigure.Segments.Add(new LineSegment(new Point(200, 0), true));
            pathFigure.Segments.Add(new PolyBezierSegment(new []{new Point(200,45), new Point(0,100), new Point(180, 160) }, true));
            pathFigure.Segments.Add(new PolyBezierSegment(new []{new Point(120,200), new Point(180,300), new Point(80, 260)}, true));
            pathFigure.Segments.Add(new PolyBezierSegment(new []{new Point(60,240), new Point(40,200), new Point(0, 200)}, true));
            pathFigure.Segments.Add(new LineSegment(new Point(0, 0), true));
            return pathFigure;
        }
    }
}
