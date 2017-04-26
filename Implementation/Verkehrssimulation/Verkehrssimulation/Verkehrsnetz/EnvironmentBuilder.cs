using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Verkehrssimulation.Verkehrsnetz
{
    class EnvironmentBuilder
    {
        JObject obj; // für json -> Projekt-> nu-getpakete verwalten -> json linq irgendwas

        public EnvironmentBuilder()
        {
            Console.WriteLine("Buidler loaded");
            LoadJson();
            LoadEnvironment();
        }

        public void LoadJson()
        {
            using (StreamReader r = new StreamReader("env_config.json"))
            {
                string json = r.ReadToEnd();
                obj = JObject.Parse(json);
            }
        }

        public void LoadEnvironment()
        {
            Console.WriteLine("Geregelte Kreuzungen:");
            Console.WriteLine(obj.GetValue("geregelte_kreuzungen")[0]);

            Console.WriteLine("Ungeregelte Kreuzungen:");
            Console.WriteLine(obj.GetValue("ungeregelte_kreuzungen"));

            Console.WriteLine("Schilder Kreuzungen:");
            Console.WriteLine(obj.GetValue("schilder"));

        }



    }
}
