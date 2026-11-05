using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json;

namespace UNodes.Project
{
    public static class SaveEncoder
    {


        public static Dictionary<string, string> Decode(string saveDataString)
            => JsonConvert.DeserializeObject<Dictionary<string, string>>(saveDataString);


        public static string Encode(Dictionary<string, string> saveData)
            => JsonConvert.SerializeObject(saveData);


    }
}
