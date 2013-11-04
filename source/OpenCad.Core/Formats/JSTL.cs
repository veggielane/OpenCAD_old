using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Veg.Maths;


namespace OpenCAD.Core.Formats
{
    public class JSTL
    {
        public string Name { get; private set; }
        public IList<JSTLTriangle> Triangles;
        public IList<Color> Colors; 
        public void Load(string data)
        {
            var schema = JsonSchema.Parse(Schema);
            JObject json;
            try
            {
                json = JObject.Parse(data);
            }
            catch(JsonReaderException e)
            {
                throw new InvalidDataException();
            }
            IList<string> messages;
            if(!json.IsValid(schema, out messages)) throw new ArgumentException(String.Join(";",messages));
            Name = json["Name"].Value<string>();
            Colors = json["Colors"].Select(c => Color.FromArgb(c["A"].Value<int>(), c["R"].Value<int>(), c["G"].Value<int>(),c["B"].Value<int>())).ToList();
            Triangles = json["Data"].Select(t => {
                if(t["C"].Value<int>() >= Colors.Count()) throw new ArgumentException("Color Not Defined");
                return new JSTLTriangle
                    {
                        Normal = new Vect3(t["N"].Values<double>().ToList()),
                        ColorIndex = t["C"].Value<int>(),
                        P1 = new Vect3(t["L"][0].Values<double>().ToList()),
                        P2 = new Vect3(t["L"][1].Values<double>().ToList()),
                        P3 = new Vect3(t["L"][2].Values<double>().ToList())
                    };
            }).ToList();

        }
        public void LoadFile(string file)
        {
            if (File.Exists(file))
            {
                Load(File.ReadAllText(file));
            }
            else throw new FileNotFoundException(file);
        }



        private const string Schema = @"{
	""type"":""object"",
	""required"":true,
	""properties"":{
		""Colors"": {
			""type"":""array"",
            ""minItems"":1,
			""required"":true,
			""items"":
				{
					""type"":""object"",
					""required"":false,
					""properties"":{
						""A"": {""type"":""number"",""required"":true},
						""B"": {""type"":""number"",""required"":true},
						""G"": {""type"":""number"",""required"":true},
						""R"": {""type"":""number"",""required"":true}
					}
				}
		},
		""Data"": {
			""type"":""array"",
			""required"":true,
			""items"":
				{
					""type"":""object"",
					""required"":false,
					""properties"":{
						""C"": {
							""type"":""number"",
							""required"":false
						},
						""L"": {
								""type"":""array"",
								""required"":true,
                                ""minItems"":3,""maxItems"":3,
								""items"":{
                                     ""type"":""array"",
                                     ""required"":true,
                                     ""minItems"":3,""maxItems"":3,
								}
						},
						""N"": {""type"":""array"",""minItems"":3,""maxItems"":3,""required"":true,""items"":{""type"":""number""}
						}
					}
				}
		},
		""Name"": {""type"":""string"",""required"":true}
	}
}";
    }
    public class JSTLTriangle
    {
        public Vect3 Normal { get; set; }
        public Vect3 P1 { get; set; }
        public Vect3 P2 { get; set; }
        public Vect3 P3 { get; set; }
        public int ColorIndex { get; set; }
    }
}
