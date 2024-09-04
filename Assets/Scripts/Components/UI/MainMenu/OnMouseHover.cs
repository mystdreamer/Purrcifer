using UnityEngine;
using UnityEngine.EventSystems;

public class OnMouseHover : MonoBehaviour, IPointerEnterHandler
{
    public MainMenuController menuIndexer;
    public UI_GameOverController gameOverIndexer;
    public int index;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if ((IMenu)menuIndexer != null)
        {
            ((IMenu)menuIndexer).SetIndex(index);
        }

        if ((IMenu)gameOverIndexer != null)
        {
            ((IMenu)gameOverIndexer).SetIndex(index);
        }
    }
}
