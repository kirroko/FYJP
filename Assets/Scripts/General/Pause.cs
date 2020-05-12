using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private ConfirmationScreen confirmationRef = null;
    [SerializeField] private GameObject pauseScreen = null;
    [SerializeField] private Button restartBtn = null;
    [SerializeField] private Button toggleMusicBtn = null;
    [SerializeField] private Button toggleSFXBtn = null;
    [SerializeField] private Button exitBtn = null;

    private float prevTimeScale = 0f;
    private float prevFixedDelta = 0f;

    [HideInInspector] public bool isPaused = false;

    private void Start()
    {
        if (pauseScreen != null) pauseScreen.SetActive(false);

        SwapImages swapSpritesBGM = toggleMusicBtn.GetComponent<SwapImages>();
        swapSpritesBGM.Swap(AudioManager.instance.HasBGM);
        toggleMusicBtn.onClick.AddListener(AudioManager.instance.ToggleMuteBGM);
        toggleMusicBtn.onClick.AddListener(delegate() { swapSpritesBGM.Swap(AudioManager.instance.HasBGM); });

        SwapImages swapSpritesSFX = toggleSFXBtn.GetComponent<SwapImages>();
        swapSpritesSFX.Swap(AudioManager.instance.HasSFX);
        toggleSFXBtn.onClick.AddListener(AudioManager.instance.ToggleMuteSFX);
        toggleSFXBtn.onClick.AddListener(delegate () { swapSpritesSFX.Swap(AudioManager.instance.HasSFX); });
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && isPaused)
            Pausing();
    }

    public void Pausing()
    {
        isPaused = !isPaused;

        if(isPaused)
        {
            prevTimeScale = Time.timeScale;
            prevFixedDelta = Time.fixedDeltaTime;

            Time.timeScale = 0f;
            Time.fixedDeltaTime = 0f;
            pauseScreen.SetActive(true);
        }
        else
        {
            Time.timeScale = prevTimeScale;
            Time.fixedDeltaTime = prevFixedDelta;
            pauseScreen.SetActive(false);
        }
    }

    public void Restart()
    {
        List<System.Action> yesFunctions = new List<System.Action>();
        yesFunctions.Add(LevelManager.instance.RestartFromBeginning);
        yesFunctions.Add(Pausing);

        ConfirmationScreen temp = Instantiate(confirmationRef);
        temp.Init("Are you sure you want to Restart?", yesFunctions);
    }

    public void Home()
    {
        List<System.Action> yesFunctions = new List<System.Action>();
        yesFunctions.Add(delegate () { SceneTransition.instance.LoadSceneInBG("LevelSelection"); });
        yesFunctions.Add(delegate () { LevelManager.instance.ResetLevelVariables(); });
        yesFunctions.Add(delegate () { EventManager.instance.TriggerUpdateHUDEvent(false); });
        yesFunctions.Add(Pausing);

        ConfirmationScreen temp = Instantiate(confirmationRef);
        temp.Init("Are you sure you want to go Home?", yesFunctions);
    }
}
