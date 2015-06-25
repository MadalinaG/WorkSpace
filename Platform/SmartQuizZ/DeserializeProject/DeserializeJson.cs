using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeserializeProject
{
    public class DeserializeJson
    {

        public static Rootobject Deserialize(string response)
        {
            return JsonConvert.DeserializeObject<Rootobject>(response);
        }
    }
}
