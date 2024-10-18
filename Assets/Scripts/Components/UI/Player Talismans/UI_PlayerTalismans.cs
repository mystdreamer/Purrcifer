using System.Collections;
using UnityEngine;
using TMPro;

public class UI_PlayerTalismans : MonoBehaviour
{
    public GameObject talismanParent;
    public TextMeshProUGUI textMeshProUGUI;

    public bool EnableDisplay
    {
        set { talismanParent.SetActive(value); }
    }

    void Update()
    {
        if (GameManager.Instance == null)
            return;

        if (GameManager.Instance.PlayerState == null)
            return;

        textMeshProUGUI.text = GameManager.Instance.PlayerState.Talismans.ToString();
    }
}