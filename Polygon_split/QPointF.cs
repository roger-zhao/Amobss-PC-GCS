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

namespace MissionPlanner.Poly_split
{
    public class QPointF
    {
        public QPointF(PointLatLngAlt pnt_in)
        {
            point = new PointLatLngAlt(pnt_in);
        }
        public double x()
        {
            return point.Lat;
        }
        public double y()
        {
            return point.Lng;
        }
        public double getDistance(QPointF pnt)
        {
            return point.GetDistance(pnt.point);
        }
        public PointLatLngAlt point;
    }

}