using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BagItemTypeEnum
{
    Null,
    Arm,
    Ring,
    Necklace
}
public class BagData 
{
    private static BagData Instance;

    public static BagData instance
    {
        get
        {
            if (Instance == null)
            {
                Instance = new BagData();
            }
            return Instance;
        }
    }

    public BagData()
    {

    } 

    [JsonProperty("Name")]
    public string Name;
    [JsonProperty("ItemPositon")]
    public string ItemPositon;

    public BagItemTypeEnum ItemTypeName
    {
        get
        {
            return SetItemType(ItemType);
        }

    }
    [JsonProperty("ItemType")]
    public string ItemType;
    [JsonProperty("Count")]
    public int Count;
   

    [JsonProperty("ResourceID")]
    public string ResourceID;
    [JsonProperty("Power")]
    public float Power;
    [JsonProperty("Speed")]
    public float Speed;
    [JsonProperty("Health")]
    public float Health;

    private BagItemTypeEnum SetItemType(string ItemName)
    {

        if (ItemName == "Arm") return BagItemTypeEnum.Arm;

        if (ItemName == "Ring") return BagItemTypeEnum.Ring;

        if (ItemName == "Necklace") return BagItemTypeEnum.Necklace;

        return BagItemTypeEnum.Null;
    }

}
