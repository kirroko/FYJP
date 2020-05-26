using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * This Class is used on buttons that has a cooldown
 */
public class CooldownIndicator : MonoBehaviour
{
    private Image image = null;

    private float cooldownDuration = 0f;
    private bool onCooldown = false;
    private float divideBy = 0f;

    private void Awake()
    {
        image = GetComponent<Image>();
        image.fillAmount = 0f;
    }

    private void Update()
    {
        if (!onCooldown) return;

        cooldownDuration -= Time.deltaTime;

        image.fillAmount = cooldownDuration / divideBy;

        if(cooldownDuration <= 0f)
        {
            onCooldown = false;
            image.fillAmount = 0f;
        }
    }

    public void StartCooldown(float duration)
    {
        divideBy = duration;
        cooldownDuration = duration;
        onCooldown = true;

        image.fillAmount = cooldownDuration / divideBy;
    }

    public void UpdateSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
