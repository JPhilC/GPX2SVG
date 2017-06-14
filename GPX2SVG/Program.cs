using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPX2SVG
{
   class Program
   {
      static void Main(string[] args)
      {
         string fileName = args[0];
         string outputFile = Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + ".svg");
         GPXLoader loader = new GPXLoader();
         Console.WriteLine("Processing file: {0}", fileName);
         IEnumerable<GPXRoute> routes = loader.LoadGPXRoutes(fileName);
         StringBuilder sb = new StringBuilder();
         foreach (GPXRoute route in routes) {
            // Populate track data objects. 
            foreach (Coordinate wp in route.WayPoints) {
               // Populate detailed track segments 
               // in the object model here. 
               sb.Append(
                 string.Format("Route:{0} - Latitude:{1} Longitude:{2} " +
                              "Elevation:{3} or ({4},{5})\n",
                 route.Name, wp.Latitude,
                 wp.Longitude, wp.Elevation,
                 wp.X, wp.Y));
            }
         }
         SVGWriter writer = new SVGWriter();
         System.Console.WriteLine("Writing file: {0}", outputFile);
         writer.WriteRoute(routes.First(), 0.08, outputFile);
         System.Console.WriteLine("{0} written.", outputFile);
      }
   }
}
