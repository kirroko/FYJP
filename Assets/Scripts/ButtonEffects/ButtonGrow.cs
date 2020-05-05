using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonGrow : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Vector2 growSize = Vector2.zero;

    private Vector2 defaultSize = Vector2.zero;
    private RectTransform rectTransform = null;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        defaultSize = rectTransform.sizeDelta;        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        rectTransform.sizeDelta = growSize;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rectTransform.sizeDelta = defaultSize;
    }
}
