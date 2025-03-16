using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace weather_fccasting
{
    class weatherForecast
    {

        public class main
        {
            public double day { get; set; }

        }
        public class weather
        {
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }

        }
        public class list
        {
            public long dt { get; set; }
            public main main { get; set; }
            public List<weather> weather { get; set; }

        }
        public class ForecastInfo
        {
            public List<list> list { get; set; }
        }
    }
}
