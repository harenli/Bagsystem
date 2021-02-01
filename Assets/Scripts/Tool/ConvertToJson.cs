using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class ConvertToJson 
{
   
    public static List<BagData> LoadJson(string jsonName)
    {

        if (!File.Exists(Application.dataPath + "/Resources/Json/"+ jsonName + ".json"))
        {
            return null;
        }
        else
        {
            StreamReader sr = new StreamReader(Application.dataPath + "/Resources/Json/" + jsonName + ".json");
            if (sr == null) return null;
            string json = sr.ReadToEnd();         
            List<BagData>  rt = JsonConvert.DeserializeObject<List<BagData>>(json);
            if (rt != null) return rt;
       
            return null;
        }
    
    }

    public static string SaveJson(List<BagData> items)
    {

        return "";
    }
}
