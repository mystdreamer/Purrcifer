using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouseHover : MonoBehaviour, IPointerEnterHandler
{
    public MenuBase menu;
    public int index;

    public void OnPointerEnter(PointerEventData eventData) => 
        menu.SetIndex = index;
}
