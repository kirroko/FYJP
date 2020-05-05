using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmationScreen : MonoBehaviour
{
    [SerializeField] private Button yesBtn = null;
    [SerializeField] private TextMeshProUGUI confirmationText = null;

    public void Init(string question, List<System.Action> yesFunctions)
    {
        confirmationText.text = question;
        yesBtn.onClick.AddListener(No);

        foreach(System.Action yesFunction in yesFunctions)
            yesBtn.onClick.AddListener(delegate () { yesFunction(); });

    }

    public void No()
    {
        Destroy(gameObject);
    }
}
