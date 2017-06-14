using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPX2SVG
{

   public struct Point
   {
      public double X;
      public double Y;
   }

   public class Coordinate
   {
      private double _Lat;
      private double _Long;
      private double _Elev;
      private double _X;
      private double _Y;

      public double Latitude
      {
         get
         {
            return _Lat;
         }
         set
         {
            if (_Lat == value) {
               return;
            }
            _Lat = value;
            RefreshCartesean();
         }
      }

      public double Longitude
      {
         get
         {
            return _Long;
         }
         set
         {
            if (_Long == value) {
               return;
            }
            _Long = value;
            RefreshCartesean();
         }
      }

      public double Elevation
      {
         get
         {
            return _Elev;
         }
         set
         {
            if (_Elev == value) {
               return;
            }
            _Elev = value;
         }
      }

      public double X
      {
         get
         {
            return _X;
         }
      }

      public double Y
      {
         get
         {
            return _Y;
         }
      }

      public Coordinate(double latitude, double longitude, double elevation) {
         _Lat = latitude;
         _Long = longitude;
         _Elev = elevation;
         RefreshCartesean();
      }


      /// <summary>
      /// Refresh the X & Y values using a MercatorProjection.
      /// 
      /// </summary>
      private void RefreshCartesean()
      {
         /*
     def mercatorProjection(coord):
         """Calculate the Mercator projection of a coordinate pair"""

         
         r = 6378137.0

         # As long as meridian = 0 and can't be changed, we don't need:
         #    meridian = meridian * math.pi / 180.0
         #    x = r * ((coord[0] * math.pi / 180.0) - meridian)

         # Instead, we use this simplified version:
         x = r * coord[0] * math.pi / 180.0
         y = r * math.log(math.tan((math.pi / 4.0) + ((coord[1] * math.pi / 180.0) / 2.0)))
         return x, y
          */
         
         // Assuming we're on earth, we have (according to GRS 80):
         double r = 6378137.0;
         // As long as meridian = 0 and can't be changed, we don't need:
         // meridian = meridian * math.pi / 180.0
         // x = r * ((coord[0] * math.pi / 180.0) - meridian)

         // Instead, we use this simplified version:
         _X = r * Longitude * Math.PI / 180.0;
         _Y = r * Math.Log(Math.Tan((Math.PI / 4.0) + ((Latitude * Math.PI / 180.0) / 2.0)));
      }
   }
}
