using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[CreateAssetMenu(fileName = "GreenColor", menuName = "Colors/Green", order = 8)]
public class GreenColor : BaseColor
{
    [SerializeField] private float moveSpeed = 5f;

    [Header("Camera")]
    [SerializeField] private Vector2 zoomBounds = new Vector2(3f, 7f);//x is min y is max
    [SerializeField] private float zoomSpeed = 5f;

    private PlayerColor playerColor = null;
    private PlayerMovement movement = null;
    private Joystick movementInput = null;
    private Camera cam = null;

    private float defaultZoom = 0f;
    private float zoom = 0f;

    private ControllablePlatform collidedPlatform = null;

    public override void InitAbility(GameObject player)
    {
        base.InitAbility(player);

        abilityInput.AxisOptions = AxisOptions.Vertical;

        playerColor = player.GetComponent<PlayerColor>();
        movement = player.GetComponent<PlayerMovement>();
        movementInput = ObjectReferences.instance.movementInput;

        cam = Camera.main;
        zoom = cam.orthographicSize;
        defaultZoom = zoom;

        collidedPlatform = null;

        EventManager.instance.setPlatform -= SetPlatform;
        EventManager.instance.setPlatform += SetPlatform;
    }

    public override void UpdateAbility(GameObject player)
    {
        if(abilityInput.Direction.y > 0f)
            zoom += Time.deltaTime * zoomSpeed;
        else if(abilityInput.Direction.y < 0f)
            zoom -= Time.deltaTime * zoomSpeed;

        zoom = Mathf.Clamp(zoom, zoomBounds.x, zoomBounds.y);

        cam.orthographicSize = zoom;

        //Check if the player is on a controllable platform
        player.GetComponent<PlayerMovement>().enabled = true;
        player.transform.SetParent(null);

        if (collidedPlatform == null || !player.GetComponent<PlayerMovement>().OnGround) return;

        player.GetComponent<PlayerMovement>().enabled = false;

        player.transform.SetParent(collidedPlatform.transform);
        collidedPlatform.transform.position += new Vector3(movementInput.Direction.x, movementInput.Direction.y, 0f) * moveSpeed * Time.deltaTime;
    }

    public override void ExitAbility(GameObject player)
    {
        base.ExitAbility(player);

        abilityInput.AxisOptions = AxisOptions.Both;
        cam.orthographicSize = defaultZoom;

        player.GetComponent<PlayerMovement>().enabled = true;
        player.transform.SetParent(null);

        collidedPlatform = null;
    }

    private void SetPlatform(GameObject platform, COLORS platformColor)
    {
        if (platformColor != mainColor) return;

        if(platform == null)
        {
            collidedPlatform = null;
            return;
        }

        if (platform.GetComponent<ControllablePlatform>() != null) collidedPlatform = platform.GetComponent<ControllablePlatform>();
    }
}
