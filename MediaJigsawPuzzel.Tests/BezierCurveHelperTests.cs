using System;
using System.Windows.Media;
using MediaJigsaw.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MediaJigsaw.Tests
{
    [TestClass]
    public class BezierCurveHelperTests
    {
        [TestMethod]
        public void TestConvertPointWithValidStrings()
        {
            string s = "60,240";
            var point1 = BezierCurveHelper.ConvertPoint(s);
            Assert.AreEqual(point1.X, 60);
            Assert.AreEqual(point1.Y, 240);

            s = " 70 , 270 ";
            var point2 = BezierCurveHelper.ConvertPoint(s);
            Assert.AreEqual(point2.X, 70);
            Assert.AreEqual(point2.Y, 270);
        }
        
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestConvertPointWithInvalidNumberString()
        {
            string s = "60,24i";
            var point1 = BezierCurveHelper.ConvertPoint(s);
            Assert.AreEqual(point1.X, 60);
            Assert.AreEqual(point1.Y, 240);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestConvertPointWithInvalidString()
        {
            string s = "60,240,";
            var point1 = BezierCurveHelper.ConvertPoint(s);
            Assert.AreEqual(point1.X, 60);
            Assert.AreEqual(point1.Y, 240);
        }


        [TestMethod]
        public void TestConvertSegmentWithValidString()
        {
            string s = "200,0; 200,45  0,100 180,160; 120,200 180,300 100,260; 60,240 40,200 0,200; 0,0";
            var segmentCollection = BezierCurveHelper.ConvertSegments(s);
            Assert.AreEqual(((LineSegment)segmentCollection[0]).Point.X, 200);
            Assert.AreEqual(((LineSegment)segmentCollection[0]).Point.Y, 0);
            
            Assert.AreEqual(((PolyBezierSegment)segmentCollection[1]).Points[0].X, 200);
            Assert.AreEqual(((PolyBezierSegment)segmentCollection[1]).Points[0].Y, 45);
            Assert.AreEqual(((PolyBezierSegment)segmentCollection[1]).Points[1].X, 0);
            Assert.AreEqual(((PolyBezierSegment)segmentCollection[1]).Points[1].Y, 100);
            Assert.AreEqual(((PolyBezierSegment)segmentCollection[1]).Points[2].X, 180);
            Assert.AreEqual(((PolyBezierSegment)segmentCollection[1]).Points[2].Y, 160);

            Assert.AreEqual(((LineSegment)segmentCollection[4]).Point.X, 0);
            Assert.AreEqual(((LineSegment)segmentCollection[4]).Point.Y, 0);
        }


        [TestMethod]
        public void TestConvertCurveWithValidString()
        {
            string s = " 200,45  0,100 180,160";
            var bezierSegment = BezierCurveHelper.ConvertCurve(s);
            Assert.AreEqual(bezierSegment.Points[0].X, 200);
            Assert.AreEqual(bezierSegment.Points[0].Y, 45);
            Assert.AreEqual(bezierSegment.Points[1].X, 0);
            Assert.AreEqual(bezierSegment.Points[1].Y, 100);
            Assert.AreEqual(bezierSegment.Points[2].X, 180);
            Assert.AreEqual(bezierSegment.Points[2].Y, 160);
        }

        [TestMethod]
        public void TestConvertLineWithValidString()
        {
            string s = "200,45";
            var lineSegment = BezierCurveHelper.ConvertLine(s);
            Assert.AreEqual(lineSegment.Point.X, 200);
            Assert.AreEqual(lineSegment.Point.Y, 45);
        }
    }
}
