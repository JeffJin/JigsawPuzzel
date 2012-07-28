using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using MediaJigsaw.Models;

namespace MediaJigsaw.Helpers
{
    public class BezierCurveHelper
    {
        private static IList<BezierCurveModel> _bezierCurveModels =
            new List<BezierCurveModel>();
                
        static BezierCurveHelper()
        {
            Init();
        }

        private static void Init()
        {
            _bezierCurveModels.Add(new BezierCurveModel(0, 0)
                                       {
                                           ViewBoxPoint = new Point(0, 0),
                                           ViewPortPoint = new Point(0,0),
                                           Figure = Create(0,0),
                                           Position = new Point(0, 0)
                                       });
            _bezierCurveModels.Add(new BezierCurveModel(0, 1)
                                       {
                                           ViewBoxPoint = new Point(0, 200),
                                           ViewPortPoint = new Point(0, 0),
                                           Figure = Create(0, 1),
                                           Position = new Point(0, 200)
                                       });
            _bezierCurveModels.Add(new BezierCurveModel(0, 2)
                                       {
                                           ViewBoxPoint = new Point(0, 400),
                                           ViewPortPoint = new Point(0, 0),
                                           Figure = Create(0, 2),
                                           Position = new Point(0, 400)
                                       });
            _bezierCurveModels.Add(new BezierCurveModel(0, 3)
                                       {
                                           ViewBoxPoint = new Point(0, 600),
                                           ViewPortPoint = new Point(0, 0),
                                           Figure = Create(0, 3),
                                           Position = new Point(0, 600)
                                       });
            _bezierCurveModels.Add(new BezierCurveModel(1, 0)
                                       {
                                           ViewBoxPoint = new Point(100, -100),
                                           ViewPortPoint = new Point(-100, -100),
                                           Figure = Create(1, 0),
                                           Position = new Point(200, 0)
                                       });
            _bezierCurveModels.Add(new BezierCurveModel(1, 1)
                                       {
                                           ViewBoxPoint = new Point(80, 60),
                                           ViewPortPoint = new Point(-100, -100),
                                           Figure = Create(1, 1),
                                           Position = new Point(180, 160)
                                       });
            _bezierCurveModels.Add(new BezierCurveModel(1, 2)
                                       {
                                           ViewBoxPoint = new Point(140, 360),
                                           ViewPortPoint = new Point(0, -60),
                                           Figure = Create(1, 2),
                                           Position = new Point(140, 420)
                                       });
            _bezierCurveModels.Add(new BezierCurveModel(1, 3)
                                       {
                                           ViewBoxPoint = new Point(110, 540),
                                           ViewPortPoint = new Point(-100, -100),
                                           Position = new Point(210, 640),
                                           Figure = Create(1, 3)
                                       });
//            _bezierCurveModels.Add(new BezierCurveModel(2, 0)
//                                       {
//                                           ViewBoxPoint = new Point(0, 200),
//                                           ViewPortPoint = new Point(0, 0),
//                                           Figure = Create(2, 0)
//                                       });
//            _bezierCurveModels.Add(new BezierCurveModel(2, 1)
//                                       {
//                                           ViewBoxPoint = new Point(0, 200),
//                                           ViewPortPoint = new Point(0, 0),
//                                           Figure = Create(2, 1)
//                                       });
        }

        public static BezierCurveModel FindModel(int col, int row)
        {
            return _bezierCurveModels.SingleOrDefault(m => m.Col == col && m.Row == row);
        }

        private static PathFigure Create(int col, int row)
        {
            var pathFigure = new PathFigure() { Segments = new PathSegmentCollection() };
            if (row == 0 && col == 0)
            {
                pathFigure.StartPoint = ConvertPoint("0,0");
                pathFigure.Segments =
                    ConvertSegments("200,0; 200,45 0,100 180,160; 120,200 180,300 100,260; 60,240 40,200 0,200; 0,0");
            }
            else if (row == 1 && col == 0)
            {
                pathFigure.StartPoint = ConvertPoint("0,0");
                pathFigure.Segments =
                    ConvertSegments("40,0 60,40 100,60;100,120 220,160 140,220;40,220 100,300 0,200;0,0");
            }
            else if (row == 2 && col == 0)
            {
                pathFigure.StartPoint = ConvertPoint("0,0");
                pathFigure.Segments =
                    ConvertSegments("100,100 40,20 140,20;220,30 120,100 210,240;160,240 100,200 0,200;0,0");
            }
            else if (row == 3 && col == 0)
            {
                pathFigure.StartPoint = ConvertPoint("0,0");
                pathFigure.Segments =
                    ConvertSegments("100,0 160,40 210,40;50,100 140,50 200,200;0,200;0,0");
            }
            else if (row == 0 && col == 1)
            {
                pathFigure.StartPoint = ConvertPoint("0,0");
                pathFigure.Segments =
                    ConvertSegments("0,45 -200,100 -20,160;40,120 100,180 120,200;130,180 280,160 200,0;0,0");
            }
            else if (row == 1 && col == 1)
            {
                pathFigure.StartPoint = ConvertPoint("0,0");
                pathFigure.Segments =
                    ConvertSegments("60,-40 120,20 140,40;130,80 160,120 200,110;170,190 90,195 90,200;85,240 70,290 40,300;40,260 0,260 -40,260;40,200 -80,160 -80,100;0,140 -60,40 0,0");
            }
            else if (row == 2 && col == 1)
            {
                pathFigure.StartPoint = ConvertPoint("0,0");
                pathFigure.Segments =
                    ConvertSegments("40,0 80,0 80,40;110,30 125,-20 130,-60;300,30 200,100 300,180;200,330 200,100 70,220;-20,80 80,10 0,0");
            }
            else if (row == 3 && col == 1)
            {
                pathFigure.StartPoint = ConvertPoint("0,0");
                pathFigure.Segments =
                    ConvertSegments("130,-120 130,110 230,-40;210,130 55,60 210,160;-10, 160;-70,10 -160,60 0,0");
            }
            return pathFigure;
        }

        public static PathSegmentCollection ConvertSegments(string collDef)
        {
            var segDefs = collDef.Split(';');
            var segCol = new PathSegmentCollection();
            foreach (var segDef in segDefs)
            {
                segCol.Add(ConvertSegment(segDef));
            }
            return segCol;
        }

        public static PathSegment ConvertSegment(string segDef)
        {
            var countComma = segDef.Count(s => s == ',');
            if(countComma == 1)//LineSegment
            {
                return ConvertLine(segDef);
            }
            else//PolyBezierSegment
            {
                return ConvertCurve(segDef);
            }
        }

        //No space near comma
        public static PolyBezierSegment ConvertCurve(string curveDef)
        {
            var pointDefs = curveDef.Split(' ');
            var segment = new PolyBezierSegment(){ IsStroked = true };
            foreach (var pointDef in pointDefs)
            {
                if(string.IsNullOrEmpty(pointDef.Trim()))
                {
                    continue;
                }
                segment.Points.Add(ConvertPoint(pointDef));
            }
            return segment;
        }

        public static LineSegment ConvertLine(string lineDef)
        {
            var point = ConvertPoint(lineDef);
            return new LineSegment(point, true);
        }

        public static Point ConvertPoint(string pointDef)
        {
            string[] temp = pointDef.Split(',');
            if(temp.Count() != 2)
            {
                throw new FormatException(pointDef);
            }
            double x = Convert.ToDouble(temp[0].Trim());
            double y = Convert.ToDouble(temp[1].Trim());
           
            return new Point(x, y);
        }
    }
}
