using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TutorialSign : MonoBehaviour
{
    [SerializeField] private TutorialPopUp popUpRef = null;
    [SerializeField] private Sprite[] pages = null;
    [SerializeField] private float holdDuration = 0.5f;

    private Joystick abilityInput = null;
    private bool triggered = false;
    private float holding = 0f;
    private GameObject HUD = null;

    private float prevTimeScale = 0f;
    private float prevFixedTimeScale = 0f;

    private void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;

        abilityInput = ObjectReferences.instance.abilityInput;
        HUD = ObjectReferences.instance.gameObject;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerInfo player = collision.GetComponent<PlayerInfo>();

        if (player == null) return;

        if(!triggered && abilityInput.IsPressed)
        {
            holding += Time.deltaTime;

            if(holding >= holdDuration)
            {
                triggered = true;

                TutorialPopUp popUp = Instantiate(popUpRef);
                popUp.Init(pages, this);

                prevFixedTimeScale = Time.fixedDeltaTime;
                prevTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                Time.fixedDeltaTime = 0f;
                HUD.SetActive(false);
            }
        }
    }

    public void PopUpClosed()
    {
        EventManager.instance.TriggerResetJoystickEvent();

        triggered = false;
        holding = 0f;
        HUD.SetActive(true);
        Time.timeScale = prevTimeScale;
        Time.fixedDeltaTime = prevFixedTimeScale;
    }
}
