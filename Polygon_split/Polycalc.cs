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
    public class Polygoncalc
    {
        #region CalculateClockDirection  
        /// <summary>  
        /// 判断多边形是顺时针还是逆时针.  
        /// </summary>  
        /// <param name="points">所有的点</param>  
        /// <param name="isYAxixToDown">true:Y轴向下为正(屏幕坐标系),false:Y轴向上为正(一般的坐标系)</param>  
        /// <returns></returns>  
        public ClockDirection CalculateClockDirection(List<PointLatLng> points, bool isYAxixToDown = false)
        {

            int i, j, k;
            int count = 0;
            double z;
            int yTrans = isYAxixToDown ? (-1) : (1);
            if (points == null || points.Count < 3)
            {
                return (0);
            }
            int n = points.Count;
            for (i = 0; i < n; i++)
            {
                j = (i + 1) % n;
                k = (i + 2) % n;
                z = (points[j].Lat - points[i].Lat) * (points[k].Lng * yTrans - points[j].Lng * yTrans);
                z -= (points[j].Lng * yTrans - points[i].Lng * yTrans) * (points[k].Lat - points[j].Lat);
                if (z < 0)
                {
                    count--;
                }
                else if (z > 0)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                return (ClockDirection.Counterclockwise);
            }
            else if (count < 0)
            {
                return (ClockDirection.Clockwise);
            }
            else
            {
                return (ClockDirection.None);
            }
        }
        #endregion


        #region CalculatePolygonType  
        /// <summary>        
        ///判断多边形是凸多边形还是凹多边形.  
        ///假定该多边形是简单的多边形，即没有横穿也没有洞的多边形。  
        /// </summary>  
        /// <param name="points"></param>  
        /// <param name="isYAxixToDown">true:Y轴向下为正(屏幕坐标系),false:Y轴向上为正(一般的坐标系)</param>  
        /// <returns></returns>  
        public PolygonType CalculatePolygonType(List<PointLatLng> points, bool isYAxixToDown)
        {
            int i, j, k;
            int flag = 0;
            double z;

            if (points == null || points.Count < 3)
            {
                return (0);
            }
            int n = points.Count;
            int yTrans = isYAxixToDown ? (-1) : (1);
            for (i = 0; i < n; i++)
            {
                j = (i + 1) % n;
                k = (i + 2) % n;
                z = (points[j].Lat - points[i].Lat) * (points[k].Lng * yTrans - points[j].Lng * yTrans);
                z -= (points[j].Lng * yTrans - points[i].Lng * yTrans) * (points[k].Lat - points[j].Lat);
                if (z < 0)
                {
                    flag |= 1;
                }
                else if (z > 0)
                {
                    flag |= 2;
                }
                if (flag == 3)
                {
                    return (PolygonType.Concave);
                }
            }
            if (flag != 0)
            {
                return (PolygonType.Convex);
            }
            else
            {
                return (PolygonType.None);
            }
        }
        #endregion
    }

    /// <summary>  
    /// 时钟方向  
    /// </summary>  
    public enum ClockDirection
    {
        /// <summary>  
        /// 无.可能是不可计算的图形，比如多点共线  
        /// </summary>  
        None,

        /// <summary>  
        /// 顺时针方向  
        /// </summary>  
        Clockwise,

        /// <summary>  
        /// 逆时针方向  
        /// </summary>  
        Counterclockwise
    }

    public enum PolygonType
    {
        /// <summary>  
        /// 无.不可计算的多边形(比如多点共线)  
        /// </summary>  
        None,

        /// <summary>  
        /// 凸多边形  
        /// </summary>  
        Convex,

        /// <summary>  
        /// 凹多边形  
        /// </summary>  
        Concave

    }

}