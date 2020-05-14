using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/**
 * This Object is to be created whenever there is something that needs to be confirmed
 */
public class ConfirmationScreen : MonoBehaviour
{
    [SerializeField] private Button yesBtn = null;
    [SerializeField] private TextMeshProUGUI confirmationText = null;

    public void Init(string question, ///< The Questions that you want to ask the player
        List<System.Action> yesFunctions) ///< The list of function to perform if player chooses yes
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
