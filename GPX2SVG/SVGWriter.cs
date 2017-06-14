using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GPX2SVG
{
   public class SVGWriter
   {
      /*
<path
       style="opacity:0.64899998;fill:none;fill-opacity:1;stroke:#3ae13b;stroke-width:10;stroke-linecap:round;stroke-linejoin:round;stroke-miterlimit:4;stroke-dasharray:40, 20;stroke-dashoffset:0;stroke-opacity:1"
       d="m 1593.8962,1179.2833 -19.3396,-0.4717 -11.3208,5.1887 -13.6792,-0.9434 -20.283,-13.6792 -20.283,-19.8114 -4.717,-4.2452 -14.6227,1.8867 -8.9622,6.1321 -21.2264,-10.3773 -17.4529,-9.9057 -5.1886,2.8302 -8.0189,-12.7359 -9.434,-9.4339 -3.3019,-9.9057 -6.6037,-7.0755 -26.4151,-20.283 -11.3208,9.434 -13.6792,3.7736 -20.283,-5.1887 -15.5661,-13.2076 -6.1321,-15.0943 -1.8868,-10.3774 -11.3207,-9.4339 -21.2264,1.8868 -28.7736,1.4151 -4.717,0.4717 -9.434,-3.3019 -12.7358,-7.0755 -9.9057,-1.4151 -6.132,2.8302 -8.0189,-8.9623 -8.0189,-6.132 -0.4717,-37.7359 -103.7736,-7.0755 -9.4339,0 -4.2453,1.8868 -13.2075,3.3019 -41.0378,-0.4717 -51.88678,-11.3207 -12.73585,1.4151 -13.67924,-6.1321 -16.50944,-7.0755 -9.90566,4.2453 -20.75471,-2.8302 -29.71698,-7.5472 -13.67925,-6.6037 -8.96226,2.8302 -35.37736,-5.6604 -26.88679,0 -28.77359,0 -16.03774,6.6038 -35.84905,17.9245 -7.54717,-23.5849 -9.43396,-22.16983 -19.33963,-24.0566 -5.18868,-17.45284 -1.88679,-17.92452 -4.71698,-16.50944 -35.37736,4.71698 -16.03773,6.60378 3.30188,-16.98113 -10.84905,-21.22642 -16.98114,-13.67925 -19.81132,-6.13207 -25,-5.66038 -14.62264,-6.13207 -25.4717,-12.73585 -19.33962,-2.83019 -15.09434,-10.84906 -13.20755,-11.32075 -19.81132,0.4717 0,-15.56604 -7.54717,-11.79245 -19.81132,9.90566 -8.96226,0 -19.33962,-26.4151 -13.20755,13.20755 -8.01887,13.20755 -21.22641,9.90566 -7.54717,-8.49057 -2.83019,-2.35849 -3.77359,-14.62264 -8.01887,9.90566 -5.18867,-12.26415 -1.8868,-12.73585 -8.96226,-6.13208 L 275.5,684.472 l -6.13208,-23.11321 -2.35849,-23.58491 0,-15.09434 7.54717,-25.94339 0.4717,-21.69812 2.35849,-7.54717 -3.77358,-13.67924 13.20754,-11.32076 16.98114,-14.62264 14.62264,-13.67924 -1.88679,-15.09434 15.56603,-11.79246"
       id="path4143"
       inkscape:connector-curvature="0" />       */
      public void WriteRoute(GPXRoute route, double scaleFactor, string filename)
      {
         IEnumerable<Point> pathPoints = ScaleRoute(route.WayPoints, scaleFactor);
         StringBuilder sb = new StringBuilder();
         sb.Append("m");
         foreach (Point p in pathPoints) {
            sb.AppendFormat(" {0},{1}", p.X, p.Y);
         }

         string style = "opacity:1;fill:none;stroke:#000000;stroke-width:10;stroke-miterlimit:4;stroke-dasharray:none;stroke-opacity:1";

         XNamespace ns = "http://www.w3.org/2000/svg";
         XNamespace inkscape = "http://www.inkscape.org/namespaces/inkscape";
         XDocumentType docType = new XDocumentType("svg", "-//W3C//DTD SVG 1.0//EN", "http://www.w3.org/TR/2001/REC-SVG-20010904/DTD/svg10.dtd", null);
         var root = new XElement(ns + "svg",
            new XAttribute(XNamespace.Xmlns + "inkscape", inkscape),
            new XElement(ns + "g",
               new XAttribute("id", "track " + route.Name),
               new XAttribute("opacity", "1"),
               new XElement(ns + "path",
                  new XAttribute("stroke-width", "5"),
                  new XAttribute("stroke", "#f1e914"),
                  new XAttribute("fill", "none"),
                  new XAttribute("opacity", "1"),
                  new XAttribute("stroke-linecap", "round"),
                  new XAttribute("stroke-linejoin","round"),
                  new XAttribute("stroke-miterlimit","4"),
                  new XAttribute("stroke-dasharray","none"),
                  new XAttribute("stroke-dashoffset","0"),
                  new XAttribute("stroke-opacity","0.55"),
                  new XAttribute("d", sb.ToString()),
                  new XAttribute("id", "path" + route.Name.Replace(" ", "")),
                  new XAttribute(inkscape + "connector-curvature", "0")
            )
            )
            );
         XDocument doc = new XDocument(
            new XDeclaration("1.0", "UTF-8", "yes"),
            docType,
            root);
         doc.Save(filename);
      }

      /// <summary>
      /// Convert a list of coordinates into a list of points suitable for creating an SVG Path.
      /// Note: Can return an empty list if there are problems.
      /// </summary>
      /// <param name="waypoints"></param>
      /// <param name="maxWidth"></param>
      /// <param name="maxHeight"></param>
      /// <returns></returns>
      private IEnumerable<Point> ScaleRoute(IEnumerable<Coordinate> waypoints, double scaleFactor)
      {
         List<Point> results = new List<Point>();

         // Now we can build our list of points
         double lastX = 0.0;
         double lastY = 0.0;
         double currentX, currentY;
         bool firstPoint = true;
         foreach (Coordinate coord in waypoints) {
            if (firstPoint) {
               lastX = coord.X * scaleFactor;
               lastY = coord.Y * scaleFactor;
               results.Add(new Point() { X = lastX, Y = lastY });
               firstPoint = false;
            }
            else {
               currentX = coord.X * scaleFactor;
               currentY = coord.Y * scaleFactor;
               results.Add(new Point() {
                  X = currentX - lastX,
                  Y = (currentY - lastY) * -1
               });
               lastX = currentX;
               lastY = currentY;
            }
         }

         return results;
      }
   }
}
