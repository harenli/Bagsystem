using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemCtrl : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public BagData BagData;
    [HideInInspector]
    public Transform IntroduceText;
    public Transform IntroduceImage;

    public delegate void Pointenter(GameObject gameObject);
    public delegate void Pointout(GameObject gameObject);

    public static Pointenter pointenter;
    public static Pointout pointout;
    // Start is called before the first frame update
    void Start()
    {
        IntroduceText = gameObject.transform.Find("Text");
        IntroduceImage = gameObject.transform.Find("Image");

        IntroduceText.gameObject.SetActive(false);
        IntroduceImage.gameObject.SetActive(false);
  }

    // Update is called once per frame
    void Update()
    {
      

    }
   
  
    /// <summary>
    /// 重新加载该位置的图片
    /// </summary>
    internal void LoadTextrue()
    {
        if (BagData.ResourceID==null)
        {
            gameObject.GetComponent<Image>().sprite = null;
            return;
        }
        Texture2D ItemTexTure = Resources.Load("Sprite/" + BagData.ResourceID) as Texture2D;
        Sprite Itemsprite = Sprite.Create(ItemTexTure, new Rect(0, 0, 84, 84), new Vector2(0.5f, 0.5f));
        gameObject.GetComponent<Image>().sprite = Itemsprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        if (gameObject != null) pointenter(gameObject);
             
    }


   
    public void OnPointerExit(PointerEventData eventData)
    {
       if(gameObject!=null) pointout(gameObject);
      

    }
}
