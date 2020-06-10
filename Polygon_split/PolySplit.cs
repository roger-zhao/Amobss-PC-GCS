using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GeoUtility.GeoSystem;
using GeoUtility.GeoSystem.Base;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using MissionPlanner.Utilities;
using MissionPlanner.Poly_split;

namespace MissionPlanner.Poly_split
{

    using QPolygonF = System.Collections.Generic.List<QPointF>;

    // splits convex and (!) concanve polygons along a line.
    // all sorts of evil configurations tested, except things
    // like non-manifold vertices, crossing edges and alike.
    class PolySplitter
    {
        enum LineSide
        {
            On,
            Left,
            Right,
        };

        public class QLineF
        {
            public QLineF(QPointF pnt1, QPointF pnt2)
            {
                point1 = pnt1;
                point2 = pnt2;

            }
            public QPointF p1()
            {
                return point1;
            }
            public QPointF p2()
            {
                return point2;
            }


            // find if a point is in a line
            public bool pointInLine(QPointF p, QLineF line)
            {
                double AB = Math.Abs(line.p1().point.GetDistance(line.p2().point));
                double AP = Math.Abs(p.point.GetDistance(line.p1().point));
                double PB = Math.Abs(p.point.GetDistance(line.p2().point));
                if (Math.Abs(AB - (AP + PB)) < 1.0)
                    return true;
                else
                    return false;
            }

            // public PointLatLng FindLineIntersection(PointLatLng start1, PointLatLng end1, PointLatLng start2, PointLatLng end2)
            public QPointF intersect(QLineF otherLine)
            {
                PointLatLng start1 = point1.point;
                PointLatLng end1 = point2.point;
                PointLatLng start2 = otherLine.point1.point;
                PointLatLng end2 = otherLine.point2.point;

                double denom = ((end1.Lng - start1.Lng) * (end2.Lat - start2.Lat)) -
                               ((end1.Lat - start1.Lat) * (end2.Lng - start2.Lng));
                //  AB & CD are parallel         
                if (denom == 0)
                    return null;

                double numer = ((start1.Lat - start2.Lat) * (end2.Lng - start2.Lng)) -
                               ((start1.Lng - start2.Lng) * (end2.Lat - start2.Lat));
                double r = numer / denom;
                double numer2 = ((start1.Lat - start2.Lat) * (end1.Lng - start1.Lng)) -
                                ((start1.Lng - start2.Lng) * (end1.Lat - start1.Lat));
                double s = numer2 / denom;
                if ((r < 0 || r > 1) || (s < 0 || s > 1))
                    return null;

                // Find intersection point      
                QPointF result = new QPointF(PointLatLng.Empty);

                result.point.Lng = start1.Lng + (r * (end1.Lng - start1.Lng));
                result.point.Lat = start1.Lat + (r * (end1.Lat - start1.Lat));

                // delete intersect no in split line
                if(!pointInLine(result, otherLine))
                {
                    return null;
                }
                return result;
            }

#if ACM_INTERSECT

            /// <summary>
            /// Calculates intersection - if any - of two lines
            /// </summary>
            /// <param name="otherLine"></param>
            /// <returns>Intersection or null</returns>
            /// <remarks>Taken from http://tog.acm.org/resources/GraphicsGems/gemsii/xlines.c </remarks>
            public QPointF intersect(QLineF otherLine)
            {
                var a1 = point2.y() - point1.y();
                // var a1 = Y2 - Y1;
                var b1 = point1.x() - point2.x();
                // var b1 = X1 - X2;
                var c1 = point2.x() * point1.y() - point1.x() * point2.y();
                // var c1 = X2 * Y1 - X1 * Y2;

                /* Compute r3 and r4.
                 */

                var r3 = a1 * otherLine.point1.x() + b1 * otherLine.point1.y() + c1;
                var r4 = a1 * otherLine.point2.x() + b1 * otherLine.point2.y() + c1;
                // var r3 = a1 * otherLine.X1 + b1 * otherLine.Y1 + c1;
                // var r4 = a1 * otherLine.X2 + b1 * otherLine.Y2 + c1;

                /* Check signs of r3 and r4.  If both point 3 and point 4 lie on
                 * same side of line 1, the line segments do not intersect.
                 */

                if (r3 != 0 && r4 != 0 && Math.Sign(r3) == Math.Sign(r4))
                {
                    return null; // DONT_INTERSECT
                }

                /* Compute a2, b2, c2 */

                var a2 = otherLine.point2.y() - otherLine.point1.y();
                var b2 = otherLine.point1.x() - otherLine.point2.x();
                var c2 = otherLine.point2.x() * otherLine.point1.y() - otherLine.point1.x() * otherLine.point2.y();

                // var a2 = otherLine.Y2 - otherLine.Y1;
                // var b2 = otherLine.X1 - otherLine.X2;
                // var c2 = otherLine.X2 * otherLine.Y1 - otherLine.X1 * otherLine.Y2;

                /* Compute r1 and r2 */

                var r1 = a2 * point1.x() + b2 * point1.y() + c2;
                var r2 = a2 * point2.x() + b2 * point2.y() + c2;

                // var r1 = a2 * X1 + b2 * Y1 + c2;
                // var r2 = a2 * X2 + b2 * Y2 + c2;
                /* Check signs of r1 and r2.  If both point 1 and point 2 lie
                 * on same side of second line segment, the line segments do
                 * not intersect.
                 */
                if (r1 != 0 && r2 != 0 && Math.Sign(r1) == Math.Sign(r2))
                {
                    return (null); // DONT_INTERSECT
                }

                /* Line segments intersect: compute intersection point. 
                 */

                var denom = a1 * b2 - a2 * b1;
                if (denom == 0)
                {
                    return null; //( COLLINEAR );
                }
                var offset = denom < 0 ? -denom / 2 : denom / 2;

                /* The denom/2 is to get rounding instead of truncating.  It
                 * is added or subtracted to the numerator, depending upon the
                 * sign of the numerator.
                 */

                var num = b1 * c2 - b2 * c1;
                var x = (num < 0 ? num - offset : num + offset) / denom;

                num = a2 * c1 - a1 * c2;
                var y = (num < 0 ? num - offset : num + offset) / denom;
                return new QPointF(new PointLatLngAlt(x, y));
            }
#endif
            QPointF point1;
            QPointF point2;
        }


        class PolyEdge
        {
            public PolyEdge(QPointF startPos, LineSide side)
            {
                StartPos = startPos;
                StartSide = side;
                Next = null;
                Prev = null;
                DistOnLine = (0.0f);
                IsSrcEdge = (false);
                IsDstEdge = (false);
                Visited = (false);
            }

            public QPointF StartPos;   // start position on edge
            public LineSide StartSide;  // start position's side of split line
            public PolyEdge Next;       // next polygon in linked list
            public PolyEdge Prev;       // previous polygon in linked list
            public double DistOnLine; // distance relative to first point on split line
            public bool IsSrcEdge;  // for visualization
            public bool IsDstEdge;  // for visualization
            public bool Visited;    // for collecting split polygons
        };

        List<PolyEdge> SplitPoly = new List<PolyEdge>();
        List<PolyEdge> EdgesOnLine = new List<PolyEdge>();

        static LineSide GetSideOfLine(QLineF line, QPointF pt)
        {
            double d = (pt.x() - line.p1().x()) * (line.p2().y() - line.p1().y()) - (pt.y() - line.p1().y()) * (line.p2().x() - line.p1().x());
            return ((Math.Sign(d) >= 0) ? LineSide.Right : ((Math.Sign(d) < 0) ? LineSide.Left : LineSide.On));
        }

        static double PointDistance(QPointF pt0, QPointF pt1)
        {
            return pt0.getDistance(pt1);
        }

        static double CalcSignedDistance(QLineF line, QPointF p)
        {
            // scalar projection on line. in case of co-linear
            // vectors this is equal to the signed distance.
            return (p.x() - line.p1().x()) * (line.p2().x() - line.p1().x()) + (p.y() - line.p1().y()) * (line.p2().y() - line.p1().y());
        }

        // -----------------------------------------------------------------------

        public List<QPolygonF> Split(QPolygonF poly, QLineF line)
        {
            SplitEdges(poly, line);
            SortEdges(line);
            SplitPolygon();
            return CollectPolys();
        }

        public List<QPolygonF> Split2(QPolygonF poly, QLineF line)
        {
            

            return SplitEdges2(poly, line); ;
        }

        void SplitEdges(QPolygonF poly, QLineF line)
        {
            if(SplitPoly != null)
            {
                SplitPoly.Clear();
            }

            if (EdgesOnLine != null)
            {
                EdgesOnLine.Clear();
            }

            for (int i = 0; i < poly.Count; i++)
            {
                QLineF edge = new QLineF(poly.ElementAtOrDefault(i), poly.ElementAtOrDefault((i + 1) % poly.Count));

                LineSide edgeStartSide = GetSideOfLine(line, edge.p1());
                LineSide edgeEndSide = GetSideOfLine(line, edge.p2());
                SplitPoly.Add(new PolyEdge(poly[i], edgeStartSide));
                // SplitPoly.push_back(PolyEdge{ poly[i], edgeStartSide});

                if (edgeStartSide == LineSide.On)
                {
                    EdgesOnLine.Add(SplitPoly.LastOrDefault());
                    // EdgesOnLine.push_back(&SplitPoly.back());
                }
                else if (!edgeStartSide.Equals(edgeEndSide) && edgeEndSide != LineSide.On)
                {
                    QPointF ip;
                    ip = edge.intersect(line);
                    // assert(res != QLineF::NoIntersection);
                    if (ip != null)
                    {
                        SplitPoly.Add(new PolyEdge(ip, LineSide.On));
                        // SplitPoly.push_back(PolyEdge{ ip, LineSide::On});
                        EdgesOnLine.Add(SplitPoly.LastOrDefault());
                        // EdgesOnLine.Add(&SplitPoly.back());
                    }

                }
            }

            // connect doubly linked list, except
            // first->prev and last->next
            for (int ii = 0; ii < (SplitPoly.Count - 1); ii++) // .begin(); iter!=std::prev(SplitPoly.end()); iter++)
            // for (auto iter = SplitPoly.begin(); iter != std::prev(SplitPoly.end()); iter++)
            {
                SplitPoly.ElementAtOrDefault(ii).Next = SplitPoly.ElementAtOrDefault(ii + 1);
                SplitPoly.ElementAtOrDefault(ii + 1).Prev = SplitPoly.ElementAtOrDefault(ii);

                // auto nextIter = std::next(iter);
                // iter.Next = nextIter;
                // nextIter->Prev = &(* iter);
            }

            // connect first->prev and last->next
            SplitPoly.LastOrDefault().Next = SplitPoly.FirstOrDefault();
            SplitPoly.FirstOrDefault().Prev = SplitPoly.LastOrDefault();
            // SplitPoly.back().Next = &SplitPoly.front();
            // SplitPoly.front().Prev = &SplitPoly.back();
        }


        // only support pairs split points
        List<QPolygonF> SplitEdges2(QPolygonF poly, QLineF line)
        {
            if (SplitPoly != null)
            {
                SplitPoly.Clear();
            }

            if (EdgesOnLine != null)
            {
                EdgesOnLine.Clear();
            }

            // line = new QLineF(new QPointF(new PointLatLngAlt(31.840743072180324, 121.46284818649292)), new QPointF(new PointLatLngAlt(31.840615471377312 , 121.46491885185242)));

            for (int i = 0; i < (poly.Count); i++)
            {
                QLineF edge = new QLineF(poly.ElementAtOrDefault(i), poly.ElementAtOrDefault((i + 1) % poly.Count));

                LineSide edgeStartSide = GetSideOfLine(line, edge.p1());
                LineSide edgeEndSide = GetSideOfLine(line, edge.p2());
                SplitPoly.Add(new PolyEdge(poly[i], edgeStartSide));
                // SplitPoly.push_back(PolyEdge{ poly[i], edgeStartSide});

                // if (edgeStartSide == LineSide.On)
                // {
                //     EdgesOnLine.Add(SplitPoly.LastOrDefault());
                //     // EdgesOnLine.push_back(&SplitPoly.back());
                // }
                // else if (!edgeStartSide.Equals(edgeEndSide) && edgeEndSide != LineSide.On)
                if (!edgeStartSide.Equals(edgeEndSide) && edgeEndSide != LineSide.On)
                {
                    QPointF ip;
                    ip = edge.intersect(line);

                    // assert(res != QLineF::NoIntersection);
                    if (ip != null)
                    {
                        SplitPoly.Add(new PolyEdge(ip, LineSide.On));
                        // SplitPoly.push_back(PolyEdge{ ip, LineSide::On});
                        EdgesOnLine.Add(SplitPoly.LastOrDefault());
                        // EdgesOnLine.Add(&SplitPoly.back());
                    }

                }
            }


            int poly_in_split_pair = 0;
            List<QPointF> split_pair = new List<QPointF>();
            List<QPolygonF> resPolys = new List<QPolygonF>();
            // connect doubly linked list and split it into pieces poly
            // first->prev and last->next
            QPolygonF split_out = new QPolygonF();
            QPointF start_point = new QPointF(new PointLatLng());
            QPointF end_point = new QPointF(new PointLatLng());

            int ii = 0;
            for (ii = 0; ii < (SplitPoly.Count); ii++) // .begin(); iter!=std::prev(SplitPoly.end()); iter++)
            // for (auto iter = SplitPoly.begin(); iter != std::prev(SplitPoly.end()); iter++)
            {

                if (SplitPoly.ElementAtOrDefault(ii).StartSide != LineSide.On) // normal points
                {
                    QPointF qp_added = new QPointF(new PointLatLng(SplitPoly.ElementAtOrDefault(ii).StartPos.x(), SplitPoly.ElementAtOrDefault(ii).StartPos.y()));
                    split_out.Add(qp_added);
                }
                else // edge points
                {

                    if (0 == (poly_in_split_pair%=2)) // new start point, then find out the end point
                    {
                        poly_in_split_pair = 0;


                        QPolygonF split_out2 = new QPolygonF(); // 2nd poly

                        // add start point
                        start_point = new QPointF(new PointLatLng(SplitPoly.ElementAtOrDefault(ii).StartPos.x(), SplitPoly.ElementAtOrDefault(ii).StartPos.y()));
                        split_out.Add(start_point);
                        split_out2.Add(start_point);

                        // find next split point to 1st poly
                        // SplitPoly.ElementAtOrDefault(ii - 1).Next = SplitPoly.ElementAtOrDefault(ii);
                        bool collect_pnt = false;
                        for (int jj = ii + 1; jj < (SplitPoly.Count); jj++) // .begin(); iter!=std::prev(SplitPoly.end()); iter++)
                        {
                            if(collect_pnt)
                            {
                                QPointF qp_added = new QPointF(new PointLatLng(SplitPoly.ElementAtOrDefault(jj).StartPos.x(), SplitPoly.ElementAtOrDefault(jj).StartPos.y()));
                                split_out.Add(qp_added);
                            }
                            else if (SplitPoly.ElementAtOrDefault(jj).StartSide == LineSide.On) // end points
                            {
                                end_point = new QPointF(new PointLatLng(SplitPoly.ElementAtOrDefault(jj).StartPos.x(), SplitPoly.ElementAtOrDefault(jj).StartPos.y()));
                                split_out.Add(end_point);
                                // split_out2.Add(qp_added);
                                collect_pnt = true;
                            }
                            else // 2nd poly
                            {
                                QPointF qp_added = new QPointF(new PointLatLng(SplitPoly.ElementAtOrDefault(jj).StartPos.x(), SplitPoly.ElementAtOrDefault(jj).StartPos.y()));
                                split_out2.Add(qp_added);
                            }
                        }


                        resPolys.Add(split_out);

                        split_out2.Add(end_point);
                        resPolys.Add(split_out2);

                        start_point = new QPointF(new PointLatLng());
                        end_point = new QPointF(new PointLatLng());
                        split_out = new QPolygonF();
                        split_out2 = new QPolygonF();

                    }
                    poly_in_split_pair++;
                }


                // auto nextIter = std::next(iter);
                // iter.Next = nextIter;
                // nextIter->Prev = &(* iter);
            }

            // add last poly
            // resPolys.Add(split_out);

            return resPolys;
        }

        void SortEdges(QLineF line)
        {
            // sort edges by start position relative to
            // the start position of the split line
            /*std::sort(EdgesOnLine.begin(), EdgesOnLine.end(), [&](PolyEdge * e0, PolyEdge * e1)
            {
                // it's important to take the signed distance here,
                // because it can happen that the split line starts/ends
                // inside the polygon. in that case intersection points
                // can fall on both sides of the split line and taking
                // an unsigned distance metric will result in wrongly
                // ordered points in EdgesOnLine.
                return CalcSignedDistance(line, e0->StartPos) < CalcSignedDistance(line, e1->StartPos);
            });*/
            // TODO: ?? to confirm
            EdgesOnLine.Sort((x, y) => (CalcSignedDistance(line, x.StartPos).CompareTo(CalcSignedDistance(line, y.StartPos))));


            // compute distance between each edge's start
            // position and the first edge's start position
            for (int i = 1; i < EdgesOnLine.Count; i++)
                EdgesOnLine.ElementAtOrDefault(i).DistOnLine = PointDistance(EdgesOnLine.ElementAtOrDefault(i).StartPos, EdgesOnLine.ElementAtOrDefault(0).StartPos);

            // EdgesOnLine[i]->DistOnLine = PointDistance(EdgesOnLine[i]->StartPos, EdgesOnLine[0]->StartPos);

        }

        void SplitPolygon()
        {
            PolyEdge useSrc = null;

            for (int i = 0; i < EdgesOnLine.Count; i++)
            {
                // find source
                PolyEdge srcEdge = useSrc;
                useSrc = null;

                // for (; !srcEdge && i < EdgesOnLine.size(); i++)
                for (; (srcEdge == null) && (i < EdgesOnLine.Count); i++)
                // for (int jj = 0; (srcEdge == null) && (jj < EdgesOnLine.Count); jj++)
                {
                    PolyEdge curEdge = EdgesOnLine.ElementAtOrDefault(i);
                    var curSide = curEdge.StartSide;
                    var prevSide = curEdge.Prev.StartSide;
                    var nextSide = curEdge.Next.StartSide;
                    // PolyEdge* curEdge = EdgesOnLine[i];
                    // const auto curSide = curEdge->StartSide;
                    // const auto prevSide = curEdge->Prev->StartSide;
                    // const auto nextSide = curEdge->Next->StartSide;
                    // assert(curSide == LineSide::On);

                    // if ((prevSide == LineSide::Left && nextSide == LineSide::Right) ||
                    // (prevSide == LineSide::Left && nextSide == LineSide::On && curEdge->Next->DistOnLine < curEdge->DistOnLine) ||
                    // (prevSide == LineSide::On && nextSide == LineSide::Right && curEdge->Prev->DistOnLine < curEdge->DistOnLine))

                    if ((prevSide == LineSide.Left && nextSide == LineSide.Right) ||
                    (prevSide == LineSide.Left && nextSide == LineSide.On && curEdge.Next.DistOnLine < curEdge.DistOnLine) ||
                    (prevSide == LineSide.On && nextSide == LineSide.Right && curEdge.Prev.DistOnLine < curEdge.DistOnLine))
                    {
                        srcEdge = curEdge;
                        srcEdge.IsSrcEdge = true;
                        // srcEdge->IsSrcEdge = true;
                    }
                }

                // find destination
                // PolyEdge* dstEdge = nullptr;
                PolyEdge dstEdge = null;

                // for (; !dstEdge && i < EdgesOnLine.size();)
                // for (; (dstEdge == null) && i < EdgesOnLine.Count;)
                for (; (dstEdge == null) && i < EdgesOnLine.Count;)
                // for (int jj = 0; (dstEdge == null) && (jj < EdgesOnLine.Count); jj++)
                {
                    PolyEdge curEdge = EdgesOnLine.ElementAtOrDefault(i);
                    // PolyEdge* curEdge = EdgesOnLine[i];
                    var curSide = curEdge.StartSide;
                    var prevSide = curEdge.Prev.StartSide;
                    var nextSide = curEdge.Next.StartSide;
                    // const auto curSide = curEdge->StartSide;
                    // const auto prevSide = curEdge->Prev->StartSide;
                    // const auto nextSide = curEdge->Next->StartSide;
                    // assert(curSide == LineSide.On);

                    if ((prevSide == LineSide.Right && nextSide == LineSide.Left) ||
                        (prevSide == LineSide.On && nextSide == LineSide.Left) ||
                        (prevSide == LineSide.Right && nextSide == LineSide.On) ||
                        (prevSide == LineSide.Right && nextSide == LineSide.Right) ||
                        (prevSide == LineSide.Left && nextSide == LineSide.Left))
                    {
                        dstEdge = curEdge;
                        // dstEdge->IsDstEdge = true;
                        dstEdge.IsDstEdge = true;
                    }
                    else
                        i++;
                }

                // bridge source and destination
                // if (srcEdge && dstEdge)
                // bool test_judge = ReferenceEquals(srcEdge, EdgesOnLine.ElementAtOrDefault(0));
                // test_judge = ReferenceEquals(srcEdge, EdgesOnLine.ElementAtOrDefault(1));
                if ((srcEdge != null) && (dstEdge != null))
                {
                    CreateBridge(ref srcEdge, ref dstEdge);
                    VerifyCycles();

                    // is it a configuration in which a vertex
                    // needs to be reused as source vertex?
                    // if (srcEdge->Prev->Prev->StartSide == LineSide::Left)
                    if (srcEdge.Prev.Prev.StartSide == LineSide.Left)
                    {
                        useSrc = srcEdge.Prev;
                        // useSrc = srcEdge->Prev;
                        useSrc.IsSrcEdge = true;
                        // useSrc->IsSrcEdge = true;
                    }
                    // else if (dstEdge->Next->StartSide == LineSide::Right)
                    else if (dstEdge.Next.StartSide == LineSide.Right)
                    {
                        useSrc = dstEdge;
                        // useSrc->IsSrcEdge = true;
                        useSrc.IsSrcEdge = true;
                    }
                }
            }
        }

        List<QPolygonF> CollectPolys()
        {
            List<QPolygonF> resPolys = new List<QPolygonF>();

            // for (auto & e : SplitPoly)
            foreach (var e in SplitPoly)
            {
                if (!e.Visited)
                {
                    QPolygonF splitPoly = new QPolygonF();
                    var curSide = e;

                    do
                    {
                        curSide.Visited = true;
                        // curSide->Visited = true;
                        splitPoly.Add(curSide.StartPos);
                        // splitPoly.append(curSide->StartPos);

                        curSide = curSide.Next;
                        // curSide = curSide->Next;
                    }
                    while (!ReferenceEquals(curSide, e));

                    resPolys.Add(splitPoly);
                }
            }

            return resPolys;
        }

        void VerifyCycles()
        {

            // for (auto &edge : SplitPoly)
            foreach(var edge in SplitPoly)
            {
                var curSide = edge;
                int count = 0;

                do
                {
                    // ASSERT(count < SplitPoly.Count);
                    curSide = curSide.Next;

                    // no 
                    if (count++ == SplitPoly.Count)
                    {
                        // CustomMessageBox.Show("调试：切割航线内部错误");
                        continue;
                    }
                }
                while (!ReferenceEquals(curSide, edge));
            }

#if ARRAY_METHOD
            // for (auto &edge : SplitPoly)
            for (int i = 0; i < EdgesOnLine.Count; i++)
            {
                int count = 0;
                var curSide = EdgesOnLine.ElementAtOrDefault(i);

                for (int kk = 0; kk < i; kk++)
                {
                    curSide = curSide.Next;

                    if(!ReferenceEquals(curSide, EdgesOnLine.ElementAtOrDefault(i)))
                    {
                        count++;
                    }
                    
                    if (count++ == SplitPoly.Count)
                    {
                        CustomMessageBox.Show("调试：切割航线内部错误");
                    }
                }

                // do
                // {
                //     // assert(count<SplitPoly.size());
                //     curSide = curSide.Next;
                //     count++;
                // }
                // while (!curSide.Equals(edge));
                // while (curSide != &edge) ;

            }
#endif

        }

        void CreateBridge(ref PolyEdge srcEdge, ref PolyEdge dstEdge)
        {
            SplitPoly.Add(new PolyEdge(srcEdge.StartPos, srcEdge.StartSide));
            PolyEdge a = SplitPoly.LastOrDefault();
            SplitPoly.Add(new PolyEdge(dstEdge.StartPos, dstEdge.StartSide));
            PolyEdge b = SplitPoly.LastOrDefault();
            a.Next = dstEdge;
            a.Prev = srcEdge.Prev;
            b.Next = srcEdge;
            b.Prev = dstEdge.Prev;
            srcEdge.Prev.Next = a;
            srcEdge.Prev = b;
            dstEdge.Prev.Next = b;
            dstEdge.Prev = a;
            // dstEdge.DistOnLine = 999;

            //SplitPoly.push_back(*srcEdge);
            // PolyEdge *a = &SplitPoly.back();
            // SplitPoly.push_back(*dstEdge);
            // PolyEdge *b = &SplitPoly.back();
            // a->Next = dstEdge;
            // a->Prev = srcEdge->Prev;
            // b->Next = srcEdge;
            // b->Prev = dstEdge->Prev;
            // srcEdge->Prev->Next = a;
            // srcEdge->Prev = b;
            // dstEdge->Prev->Next = b;
            // dstEdge->Prev = a;

        }
    }
}