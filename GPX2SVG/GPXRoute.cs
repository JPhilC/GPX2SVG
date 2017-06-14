using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPX2SVG
{
   public class GPXRoute
   {
      public string Name { get; set; }
      public IEnumerable<Coordinate> WayPoints { get; set; }

      public GPXRoute()
      {
      }
   }
}
