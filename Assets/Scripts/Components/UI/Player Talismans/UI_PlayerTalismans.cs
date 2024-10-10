using System.Collections;
using UnityEngine;
using TMPro;

public class UI_PlayerTalismans : MonoBehaviour
{
    public GameObject talismanParent;
    public TextMeshProUGUI textMeshProUGUI;

    void Update()
    {
        if (GameManager.Instance == null)
            return;

        if (GameManager.Instance.PlayerState == null)
            return;

        textMeshProUGUI.text = GameManager.Instance.PlayerState.Talismans.ToString();
    }

    public void Enable()
    {
        talismanParent.SetActive(true);
    }

    public void Disable()
    {
        talismanParent.SetActive(false);
    }
}