using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer activatedIcon = null;
    [SerializeField] private GameObject activatedText = null;
    [SerializeField] private float fadeSpeed = 2f;
    [SerializeField] private float raiseHeight = 2f;
    [SerializeField] private float raiseSpeed = 2f;

    private Vector3 startPos = Vector3.zero;

    private void Start()
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        EventManager.instance.checkpointEvent -= CheckpointEvent;
        EventManager.instance.checkpointEvent += CheckpointEvent;

        startPos = activatedText.transform.position;
        raiseHeight += activatedText.transform.position.y;
        activatedText.SetActive(false);


        Color temp = activatedIcon.color;
        temp.a = 0f;
        activatedIcon.color = temp;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        LevelManager.instance.SpawnPoint = transform.position;
        EventManager.instance.TriggerCheckpointEvent(gameObject);

        activatedText.SetActive(true);
        StartCoroutine(ILerpAlpha(activatedIcon, 1f, fadeSpeed));
        StartCoroutine(TextResponse());
    }

    private void CheckpointEvent(GameObject me)
    {
        if (me == gameObject) return;

        StartCoroutine(ILerpAlpha(activatedIcon, -1f, fadeSpeed));
        Color temp = activatedText.GetComponent<SpriteRenderer>().color;
        temp.a = 1f;
        activatedText.GetComponent<SpriteRenderer>().color = temp;
    }

    private void OnDestroy()
    {
        EventManager.instance.checkpointEvent -= CheckpointEvent;
    }

    private void LerpAlpha(SpriteRenderer sprite, float dir, float speed)
    {
        Color temp = sprite.color;
        temp.a = Mathf.Clamp01(temp.a + dir * Time.deltaTime * speed);
        sprite.color = temp;
    }

    private IEnumerator ILerpAlpha(SpriteRenderer sprite, float dir, float speed)
    {
        while(sprite.color.a > 0f && sprite.color.a < 1f)
        {
            LerpAlpha(sprite, dir, speed);
            yield return null;
        }
    }

    private IEnumerator TextResponse()
    {
        while(activatedText.transform.position.y <= raiseHeight)
        {
            LerpAlpha(activatedText.GetComponent<SpriteRenderer>(), -1f, 0.5f);

            Vector3 targetPos = activatedText.transform.position;
            targetPos.y += raiseSpeed * Time.deltaTime;
            activatedText.transform.position = targetPos;


            yield return null;
        }

        activatedText.gameObject.SetActive(false);
    }
}
