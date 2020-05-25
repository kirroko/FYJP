using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ConfirmationScreen confirmationRef = null;
    [SerializeField] private GameObject main = null;
    [SerializeField] private Button toggleMusicBtn = null;
    [SerializeField] private Button toggleSFXBtn = null;
    [SerializeField] private Button godModeBtn = null;
    [SerializeField] private Image slash = null;

    private void Start()
    {
        SwapImages swapSpritesBGM = toggleMusicBtn.GetComponent<SwapImages>();
        swapSpritesBGM.Swap(AudioManager.instance.HasBGM);
        toggleMusicBtn.onClick.AddListener(AudioManager.instance.ToggleMuteBGM);
        toggleMusicBtn.onClick.AddListener(delegate () { swapSpritesBGM.Swap(AudioManager.instance.HasBGM); });

        SwapImages swapSpritesSFX = toggleSFXBtn.GetComponent<SwapImages>();
        swapSpritesSFX.Swap(AudioManager.instance.HasSFX);
        toggleSFXBtn.onClick.AddListener(AudioManager.instance.ToggleMuteSFX);
        toggleSFXBtn.onClick.AddListener(delegate () { swapSpritesSFX.Swap(AudioManager.instance.HasSFX); });

        godModeBtn.onClick.AddListener(ToggleGodMode);
        if (LevelManager.instance.GodModeOn)
            slash.gameObject.SetActive(false);
        else
            slash.gameObject.SetActive(true);
    }

    public void ToMain()
    {
        main.SetActive(true);
        gameObject.SetActive(false);
    }

    public void ClearData()
    {
        List<System.Action> yesFunctions = new List<System.Action>();
        yesFunctions.Add(LevelManager.instance.ClearSavedData);
        PlayerPrefs.DeleteAll();

        ConfirmationScreen temp = Instantiate(confirmationRef);
        temp.Init("Are you sure you want to clear all Data?", yesFunctions);
    }

    private void ToggleGodMode()
    {
        Debug.Log("Called");
        LevelManager.instance.ToggleGodMode();
        if (LevelManager.instance.GodModeOn)
            slash.gameObject.SetActive(false);
        else
            slash.gameObject.SetActive(true);
    }
}
