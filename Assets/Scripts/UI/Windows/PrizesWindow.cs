using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrizesWindow : MonoBehaviour
{
    [SerializeField] Prizes[] prizes;
    [SerializeField] TextMeshProUGUI[] textFields;
    
    void Start()
    {
        for (int i = 0; i < textFields.Length; i++)
        {
            TextMeshProUGUI[] textInChild = textFields[i].GetComponentsInChildren<TextMeshProUGUI>();
            textInChild[1].text = Managers.uiManager.AddSeparator(prizes[i].Moneys.ToString());
            textInChild[2].text = Managers.uiManager.AddSeparator(prizes[i].Health.ToString());
        }
    }
}
