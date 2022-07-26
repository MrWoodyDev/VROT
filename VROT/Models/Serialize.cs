using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VROT.Models
{
    public static class Serialize
    {
        public static string ToJson(this Event self) => JsonConvert.SerializeObject(self, Converter.Settings);
    }

}
