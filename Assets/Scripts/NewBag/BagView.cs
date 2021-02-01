using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BagView : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{

    /// <summary>
    /// 展示拖拽中的物品
    /// </summary>
    private GameObject OffsetThing;

    public GameObject BeginDragItem;

    public void OnBeginDrag(PointerEventData eventData)
    {
        BagCtrl.onlyShowOneIntroduce = false;
        if (!(eventData.pointerEnter.gameObject.transform.parent.name== TagType.BagTag || eventData.pointerEnter.gameObject.transform.parent.name== TagType.EquipTag)) return;

        if (eventData.pointerEnter.gameObject.GetComponent<ItemCtrl>().BagData.ResourceID == null || eventData.pointerEnter.gameObject.GetComponent<ItemCtrl>().BagData.ResourceID == "") return;
    
             BeginDragItem = eventData.pointerEnter;
            OffsetThing = GameObject.Instantiate(eventData.pointerEnter, eventData.position, Quaternion.identity);
            OffsetThing.transform.SetParent(gameObject.transform.Find("Canvas").transform);
            OffsetThing.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (OffsetThing == null) return;

        OffsetThing.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {


        if (OffsetThing == null) return;
        BagCtrl.onlyShowOneIntroduce = true;
       
        GameObject  TargetItem = BagCtrl.judgeRunintoObject(eventData.pointerEnter.transform.position);
       
        
        if (TargetItem == null)
        {
            Destroy(OffsetThing);
            return;
        }

          ItemCtrl ItemBagData = TargetItem.GetComponent<ItemCtrl>();
        if (ItemBagData.BagData.Name == null && ItemBagData.BagData.ItemPositon ==null)
        {
            //属于空白格,如果是在备包内部,拖拽的物品需要排序到最后或者不对拖拽的物品做处理,此处不做处理
            Destroy(OffsetThing);
            return;
        }
      


        if (TargetItem.Equals(BeginDragItem))
        {
            //如果是物品本身就不用替换
            Destroy(OffsetThing);
            return;
        }

        if (!(TargetItem.GetComponent<ItemCtrl>().BagData.ItemPositon == TagType.BagTag || TargetItem.GetComponent<ItemCtrl>().BagData.ItemPositon == TagType.EquipTag)) return;

            BagCtrl.ExchangeItemFunc(TargetItem, BeginDragItem);        

        if (OffsetThing != null) Destroy(OffsetThing);


    }
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
