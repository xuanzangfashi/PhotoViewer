using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonBase : MonoBehaviour  ,IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    protected Main mainCom;
    [SerializeField]
    Button ButtonRef;
    [SerializeField]
    Image ImgRef;
   
    // Use this for initialization
    void Start () {
        ButtonRef.onClick.AddListener(onclick);
    }

    protected virtual void onclick()
    {
        

    }
    void OnHover(object e)
    {

        this.ImgRef.color = new Color(ImgRef.color.r, ImgRef.color.g, ImgRef.color.b, 1);
    }
	
    void OnUnhover(object e)
    {
        this.ImgRef.color = new Color(ImgRef.color.r, ImgRef.color.g, ImgRef.color.b, 0);
    }
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHover(eventData);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnUnhover(eventData);
    }
}
