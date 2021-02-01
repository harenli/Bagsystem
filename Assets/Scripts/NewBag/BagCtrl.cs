using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class BagCtrl : MonoBehaviour
{
    private LayoutGroup Baglayout;
    private LayoutGroup EquipLayout;
    //当前背包里的格子数
    [HideInInspector]
    public static int bagItemAcount;
    [HideInInspector]
    public int equipItemAcount;
    public static List<GameObject> BagsList = new List<GameObject>();
    public static List<BagData> BagData = new List<BagData>();

    public GameObject item;

    public static bool onlyShowOneIntroduce;
    [HideInInspector]
    public GameObject IntroduceObject;

    // Start is called before the first frame update
    void Start()
    {
        bagItemAcount = 10;
        equipItemAcount = 4;
        onlyShowOneIntroduce = true;
        Baglayout = gameObject.transform.Find("Canvas/BagSystemBack/Bag").GetComponent<LayoutGroup>();
        EquipLayout = gameObject.transform.Find("Canvas/BagSystemBack/Equip").GetComponent<LayoutGroup>();
        BagData = LoadJson();
        CreateAllItem();

        ItemCtrl.pointenter = ShowText;
        ItemCtrl.pointout = hideText;
    }


    // Update is called once per frame
    void Update()
    {
        ///测试增加和删除道具
        if (Input.GetKeyDown(KeyCode.W))
        {
            BagCtrl.DeleteItem(BagsList[2]);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            BagCtrl.AddItem(BagsList[3].GetComponent<ItemCtrl>().BagData);
        }
    }
    /// <summary>
    /// 创造得到json数据的背包里所有的物品
    /// </summary>
    private void CreateAllItem()
    {
        int BagdataAcount = 0;
        int EquipdataAcount = 0;

      
        for (int i = 0; i < BagData.Count; i++)
        {
            //创建有数据的背包栏
            if (BagData[i].ItemPositon == TagType.BagTag)
            {
                BagdataAcount++;
                GameObject BagTemp = GameObject.Instantiate(item, Vector3.zero, Quaternion.identity);

                BagTemp.transform.SetParent(Baglayout.transform);
                ItemCtrl TempItemCtrl = BagTemp.AddComponent<ItemCtrl>();

                if (i <= BagData.Count)
                {
                    TempItemCtrl.BagData = BagData[i];
                    TempItemCtrl.LoadTextrue();
                }
                if (BagTemp != null) BagsList.Add(BagTemp);
            }
            //创建有数据的装备栏
            else if (BagData[i].ItemPositon == TagType.EquipTag)
            {
                EquipdataAcount++;
                GameObject EquipTemp = GameObject.Instantiate(item, Vector3.zero, Quaternion.identity);

                EquipTemp.transform.SetParent(EquipLayout.transform);
                ItemCtrl TempItemCtrl = EquipTemp.AddComponent<ItemCtrl>();

                 if (i <= BagData.Count)
                {
                    TempItemCtrl.BagData = BagData[i];

                    TempItemCtrl.LoadTextrue();
                }

                if (EquipTemp != null) BagsList.Add(EquipTemp);
            }
        }
        //当给定的背包的空格还需要再创建的时候创建空格
        if (bagItemAcount >= BagdataAcount)
        {
            for (int i = 0; i < bagItemAcount - BagdataAcount; i++)
            {
                GameObject BagTemp = GameObject.Instantiate(item, Vector3.zero, Quaternion.identity);

                BagTemp.transform.SetParent(Baglayout.transform);
                ItemCtrl TempItemCtrl = BagTemp.AddComponent<ItemCtrl>();
                TempItemCtrl.BagData = new BagData();
                TempItemCtrl.BagData.ItemPositon = TagType.BagTag;
                BagData.Add(TempItemCtrl.BagData);
                if (BagTemp != null) BagsList.Add(BagTemp);
            }
        }
        //当给定的装备栏的空格还需要再创建的时候创建空格
        if (equipItemAcount >= EquipdataAcount)
        {
            for (int i = 0; i < equipItemAcount - EquipdataAcount; i++)
            {
                EquipdataAcount++;
                GameObject EquipTemp = GameObject.Instantiate(item, Vector3.zero, Quaternion.identity);

                EquipTemp.transform.SetParent(EquipLayout.transform);
                ItemCtrl TempItemCtrl = EquipTemp.AddComponent<ItemCtrl>();
                TempItemCtrl.BagData = new BagData();
                TempItemCtrl.BagData.ItemPositon = TagType.EquipTag;
                BagData.Add(TempItemCtrl.BagData);
                if (EquipTemp != null) BagsList.Add(EquipTemp);
            }
        }


    }


    /// <summary>
    /// 重新加载所有物品
    /// </summary>
    public static void reLoadAllcell()
    {
        for (int i = 0; i < BagData.Count; i++)
        {

            BagsList[i].GetComponent<ItemCtrl>().BagData = BagData[i];
            BagsList[i].GetComponent<ItemCtrl>().LoadTextrue();

        }
    }


    /// <summary>
    /// 重新加载背包里的物品
    /// </summary>
    public static void reLoadBagcell()
    {

        for (int i = 0; i < BagData.Count; i++)
        {
            if (BagData[i].ItemPositon == TagType.BagTag)
            {
                BagsList[i].GetComponent<ItemCtrl>().BagData = BagData[i];
                if (BagsList[i].GetComponent<ItemCtrl>().BagData.ResourceID != "")
                {
                    BagsList[i].GetComponent<ItemCtrl>().LoadTextrue();
                }

            }



        }
    }

    /// <summary>
    /// 重新加载装备栏里的物品
    /// </summary>
    public static void reLoadEquipcell()
    {

        for (int i = 0; i < BagData.Count; i++)
        {
            if (BagData[i].ItemPositon == TagType.EquipTag)
            {
                BagsList[i].GetComponent<ItemCtrl>().BagData = BagData[i];
                if (BagsList[i].GetComponent<ItemCtrl>().BagData.ResourceID != "")
                {
                    BagsList[i].GetComponent<ItemCtrl>().LoadTextrue();
                }
            }



        }
    }

    /// <summary>
    /// 拾取道具,向背包内添加物品
    /// </summary>
    /// <param name="item">新增物品的数据</param>
    public static void AddItem(BagData item)
    {
        //如果背包已经满了
        if (BagCtrl.bagItemAcount <= BagsList.FindAll(e => e.GetComponent<ItemCtrl>().BagData.ItemPositon == TagType.BagTag && e.GetComponent<ItemCtrl>().BagData.ResourceID != null).Count)
        {
            return;
        }
        //指定物品添加的位置
        if (item.ItemPositon == null)
        {
            item.ItemPositon = TagType.BagTag;
        }
        //从背包中找到第一个空格进行赋值
        int i = 0;
        foreach (var lastitem in BagsList)
        {
         
            if (lastitem.GetComponent<ItemCtrl>().BagData.ItemPositon == TagType.BagTag && lastitem.GetComponent<ItemCtrl>().BagData.ResourceID == null)//找到数据层背包里第一个空白的物体进行赋值
            {
                BagData[i]=item;
                lastitem.GetComponent<ItemCtrl>().BagData = BagData[i];
                lastitem.GetComponent<ItemCtrl>().LoadTextrue();
                break;
            }
            i++;
        }
    }
    //获取现在的备包中有道具占格的数量
    public List<GameObject> GetNowBagAcount()
    {
        List<GameObject> NowBagAcount = new List<GameObject>();
        foreach (var item in BagsList)
        {
            if (item.GetComponent<ItemCtrl>().BagData.ItemPositon == TagType.BagTag && item.GetComponent<ItemCtrl>().BagData.ResourceID != null)
            {
                NowBagAcount.Add(item);
            }
        }
        return NowBagAcount;
    }

    /// <summary>
    /// 删除相应位置的物品,在移除背包时使用该方法
    /// </summary>
    /// <param name="Gameobj"></param>
    public static void DeleteItem(GameObject Gameobj)
    {
        for (int i = 0; i < BagData.Count; i++)
        {
            if (Gameobj.GetComponent<ItemCtrl>().BagData.Equals(BagData[i]))
            {

                BagData[i] = new BagData();
                BagData[i].ItemPositon = Gameobj.GetComponent<ItemCtrl>().BagData.ItemPositon;
                Gameobj.GetComponent<ItemCtrl>().BagData = BagData[i];
                Gameobj.GetComponent<ItemCtrl>().LoadTextrue();
            }
        }

    }

    /// <summary>
    /// 加载得到的json,本地或网络皆可.
    /// </summary>
    /// <returns></returns>
    public static List<BagData> LoadJson()
    {

        if (!File.Exists(Application.dataPath + "/Resources/Json/" + "Item" + ".json"))
        {
            return null;
        }
        else
        {
            StreamReader sr = new StreamReader(Application.dataPath + "/Resources/Json/" + "Item" + ".json");
            if (sr == null) return null;
            string json = sr.ReadToEnd();
            List<BagData> rt = JsonConvert.DeserializeObject<List<BagData>>(json);
            if (rt != null) return rt;

            return null;
        }
        // return "";
    }

    /// <summary>
    /// 判断最终停止拖拽时是哪个道具
    /// </summary>
    /// <param name="objectpositon"></param>
    /// <returns></returns>
    public static GameObject judgeRunintoObject(Vector3 objectpositon)
    {
        Vector3 Itemposition;
        for (int i = 0; i < BagsList.Count; i++)
        {
            Itemposition = BagsList[i].transform.position;
            if (Mathf.Abs(Itemposition.x - objectpositon.x) < 50 && Mathf.Abs(Itemposition.y - objectpositon.y) < 50)
            {
                return BagsList[i];
            }
        }

        return null;
    }



    /// <summary>
    /// 互换物品的真正方法
    /// </summary>
    /// <param name="item1">最终被换的物品</param>
    /// <param name="item2">开始换的物品</param>
    /// <returns></returns>
    public static bool ExchangeItemFunc(GameObject item1, GameObject item2)
    {
        try
        {
            //装备栏原则上不会更换
            if (item1.GetComponent<ItemCtrl>().BagData.ItemPositon == TagType.EquipTag && item1.GetComponent<ItemCtrl>().BagData.ItemPositon == TagType.EquipTag)
            {
                if (item1.GetComponent<ItemCtrl>().BagData.ItemType == item2.GetComponent<ItemCtrl>().BagData.ItemType)
                {
                    //不做任何操作,继续交换逻辑
                }
                else return false;

            }

            int i = BagsList.FindIndex(e => e.Equals(item1));
            int j = BagsList.FindIndex(e => e.Equals(item2));
           

            if (item1.GetComponent<ItemCtrl>().BagData == BagData[i] && item2.GetComponent<ItemCtrl>().BagData == BagData[j])//防止中间赋值导致不一致
            {
                if (item1.GetComponent<ItemCtrl>().BagData.ItemPositon != item2.GetComponent<ItemCtrl>().BagData.ItemPositon)//如果是不一样的位置互换物品
                {
                    if (item1.GetComponent<ItemCtrl>().BagData.ItemType != item2.GetComponent<ItemCtrl>().BagData.ItemType)//如果物品的类型不一致就不能互换
                    {
                        return false;
                    }
                }
                //开始互换物品
                BagData tempItem = BagData[i];

                BagData[i] = BagData[j];
                BagData[j] = tempItem;

                if (BagData[i].ItemPositon != BagData[j].ItemPositon)
                {
                    string tempItemPositon = BagData[i].ItemPositon;
                    BagData[i].ItemPositon = BagData[j].ItemPositon;
                    BagData[j].ItemPositon = tempItemPositon;

                }

                item1.GetComponent<ItemCtrl>().BagData = BagData[i];
                item2.GetComponent<ItemCtrl>().BagData = BagData[j];
                item1.GetComponent<ItemCtrl>().LoadTextrue();
                item2.GetComponent<ItemCtrl>().LoadTextrue();

            }
            return true;
        }
        catch (System.Exception)
        {

            return false;
        }


    }
    /// <summary>
    /// 寻找返回真正在数据层的物品
    /// </summary>
    /// <param name="Item"></param>
    /// <returns></returns>
    public static GameObject FindItem(GameObject Item)
    {
        foreach (var item in BagsList)
        {
            if (item.Equals(Item))
            {
                return item;

            }
        }

        return null;

    }

    /// <summary>
    /// 显示物品的文字提示信息
    /// </summary>
    /// <param name="gameObject"></param>
    public void ShowText(GameObject gameObject)
    {
      StartCoroutine(WaitAndPrint(gameObject));
    }

    IEnumerator WaitAndPrint(GameObject Item)
    {
        //空白格不展示
        if (Item.GetComponent<ItemCtrl>().BagData.ResourceID==null) yield break;
     
        if (IntroduceObject == null) IntroduceObject = Item.transform.Find("Text").gameObject;

        yield return new WaitForSeconds(1.0f);

        if (IntroduceObject == null) yield break;

        //if (IntroduceObject != Item) yield break;
        if (BagCtrl.onlyShowOneIntroduce)
        {
            try
            {

                Item.transform.Find("Image").gameObject.SetActive(true);
                Item.transform.Find("Text").gameObject.SetActive(true);

                IntroduceObject.GetComponent<Text>().text =
                     "道具类型:" + Item.GetComponent<ItemCtrl>().BagData.ItemType.ToString()
                 + "\n 力量:" + Item.GetComponent<ItemCtrl>().BagData.Power.ToString()
                + "\n 速度:" + Item.GetComponent<ItemCtrl>().BagData.Speed.ToString()
                + "\n 生命值:" + Item.GetComponent<ItemCtrl>().BagData.Health.ToString();

                BagCtrl.onlyShowOneIntroduce = false;
            }
            catch (System.Exception)
            {


            }

        }
    }
    /// <summary>
    /// 当鼠标移出物品位置时隐藏说明
    /// </summary>
    /// <param name="Item"></param>
    public void hideText(GameObject Item)
    {
        Item.transform.Find("Image").gameObject.SetActive(false);
        Item.transform.Find("Text").gameObject.SetActive(false);
        IntroduceObject = null;
        BagCtrl.onlyShowOneIntroduce = true;
    }

}
