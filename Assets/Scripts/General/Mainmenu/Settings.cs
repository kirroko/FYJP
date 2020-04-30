using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject main = null;
    [SerializeField] private Button toggleMusicBtn = null;
    [SerializeField] private Button toggleSFXBtn = null;

    private void Start()
    {
        SwapSprites swapSpritesBGM = toggleMusicBtn.GetComponent<SwapSprites>();
        swapSpritesBGM.Swap(AudioManager.instance.HasBGM);
        toggleMusicBtn.onClick.AddListener(delegate () { swapSpritesBGM.Swap(AudioManager.instance.HasBGM); });

        SwapSprites swapSpritesSFX = toggleSFXBtn.GetComponent<SwapSprites>();
        swapSpritesSFX.Swap(AudioManager.instance.HasSFX);
        toggleSFXBtn.onClick.AddListener(delegate () { swapSpritesSFX.Swap(AudioManager.instance.HasSFX); });
    }

    public void ToMain()
    {
        main.SetActive(true);
        gameObject.SetActive(false);
    }
}
