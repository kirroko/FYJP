using UnityEngine;

[CreateAssetMenu(fileName = "WhiteColor", menuName = "Colors/White", order = 2)]
public class WhiteColor : BaseColor
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private Vector2 bounds = Vector2.zero;


    private Camera2D cam = null;
    private float startY = 0f;

    public override void InitAbility(GameObject player)
    {
        base.InitAbility(player);
        cam = Camera.main.gameObject.GetComponent<Camera2D>();
    }

    public override void UpdateAbility(GameObject player)
    {
        if(abilityInput.IsPressed)
        {
            if (!cam.isControlled)
                startY = cam.transform.position.y;

            cam.isControlled = true;

            dir = abilityInput.Direction;

            Vector3 targetPos = cam.transform.position;
            targetPos.x = Mathf.Clamp(targetPos.x + dir.x, player.transform.position.x -bounds.x * 0.5f, player.transform.position.x + bounds.x * 0.5f);
            targetPos.y = Mathf.Clamp(targetPos.y + dir.y, startY - bounds.y * 0.5f, startY + bounds.y * 0.5f);

            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPos, Time.deltaTime * moveSpeed);
        }
        else
        {
            cam.isControlled = false;
        }
    }

    public override void ExitAbility(GameObject player)
    {
        base.ExitAbility(player);
    }
}
