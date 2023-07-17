using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIInventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, 
                                IDropHandler, IDragHandler
    {
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private Image borderImage;

    public event Action<UIInventoryItem>
        OnItemDroppedOn,
        OnItemBeginDrag,
        OnItemEndDrag,
        OnLeftMouseClicked;
    
    private bool empty = true;
    private IBeginDragHandler _beginDragHandlerImplementation;
    private IDragHandler _dragHandlerImplementation;
    private IDropHandler _dropHandlerImplementation;
    private IEndDragHandler _endDragHandlerImplementation;
    private IPointerClickHandler _pointerClickHandlerImplementation;

    public void Awake()
    {
        ResetData();
        Deselect();
    }

    /// <summary>
    /// 取消选择当前item：隐藏border
    /// </summary>
    public void Deselect()
    {
        borderImage.enabled = false;
    }

    /// <summary>
    /// item清 0并设置为 false
    /// </summary>
    public void ResetData()
    {
        this.itemImage.gameObject.SetActive(false);
        this.empty = true;
    }

    /// <summary>
    /// item设置为true，设置icon和数量
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="count"></param>
    public void SetData(Sprite sprite, int count)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.countText.text = count + "";
        this.empty = false;
    }

    /// <summary>
    /// 选择当前item：显示border
    /// </summary>
    public void Select()
    {
        this.borderImage.enabled = true;
    }

    /// <summary>
    /// 若当前item为空，不可drag
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (empty)
        {
            return;
        }
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    /// <summary>
    /// 判断左右点击
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (this.empty)
        {
            //TODO:取消当前选择的item
            //TODO:清空description
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftMouseClicked?.Invoke(this);
        }
    }
}

}
