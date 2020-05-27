using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class handles the afterimage creative known as mirage. It's main objective is to disperse after
 * a given time that is determined in the inspector.
 * 
 * It'll take the current sprite from the gaemobject with a tag "Player" and reduce it's alpha value to create
 * the illusion of a mirage
 * 
 */
public class PlayerAfterImageSprite : MonoBehaviour
{
    // VALUES
    [SerializeField]
    private float activeTime = 0.1f;
    private float duration = 0;
    private float alpha;
    [SerializeField]
    private float alphaSet = 0.8f;
    private float alphaMultiplier = 0.85f;

    // REFERENCE
    private Transform player;

    private SpriteRenderer sr;
    private SpriteRenderer playerSr;

    private Color color;

    /**
     * This Function is for safety. When there is no gameobject with a tag "Player", it will not initialize
     */
    private void OnEnable()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
            Init();
    }

    private void Init()
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSr = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;
        sr.sprite = playerSr.sprite;
        sr.flipX = playerSr.flipX;
        transform.position = player.position;
        transform.rotation = player.rotation;
        duration = 0;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        color = new Color(1f, 1f, 1f, alpha);
        sr.color = color;

        duration += Time.deltaTime;
        if (duration > activeTime) // disable mirage and send back to object pool
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
    }
}
