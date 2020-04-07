using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quicksand : MonoBehaviour
{
    [SerializeField] float fallSpeed = 2f;
    [SerializeField] COLORS color = COLORS.NONE;
    [SerializeField] private BoxCollider2D boxCollider = null;

    private Vector2 defaultSize = Vector2.zero;
    private Vector2 defaultOffset = Vector2.zero;

    private PlayerColor player = null;

    private void Start()
    {
        defaultSize = boxCollider.size;
        defaultOffset = boxCollider.offset;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerColor>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Player different color from quicksand
        if(color != player.GetCurrentColor.GetMain)
        {
            float height = Mathf.Clamp(boxCollider.size.y - Time.fixedDeltaTime * fallSpeed, 0f, defaultSize.y);
            if(height > 0f)
            {
                boxCollider.size = new Vector2(boxCollider.size.x, boxCollider.size.y - Time.fixedDeltaTime * fallSpeed);
                boxCollider.offset = new Vector2(boxCollider.offset.x, boxCollider.offset.y - Time.deltaTime * 0.5f * fallSpeed);
            }
        }
        else
        {
            float height = Mathf.Clamp(boxCollider.size.y - Time.fixedDeltaTime * fallSpeed, 0f, defaultSize.y);
            if (height < defaultSize.y)
            {
                boxCollider.size = new Vector2(boxCollider.size.x, boxCollider.size.y + Time.fixedDeltaTime * fallSpeed);
                boxCollider.offset = new Vector2(boxCollider.offset.x, boxCollider.offset.y + Time.deltaTime * 0.5f * fallSpeed);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        boxCollider.size = defaultSize;
        boxCollider.offset = defaultOffset;
    }
}
