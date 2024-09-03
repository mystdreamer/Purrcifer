using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouseHover : MonoBehaviour, IPointerEnterHandler
{
    public MainMenuController mainMenuController;
    public int index; 

    public void OnPointerEnter(PointerEventData eventData)
    {
        mainMenuController.menuIndexer.CurrentIndex = index;
    }
}
