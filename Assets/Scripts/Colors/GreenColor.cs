using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[CreateAssetMenu(fileName = "GreenColor", menuName = "Colors/Green", order = 8)]
public class GreenColor : WhiteColor
{
    [SerializeField] private float moveSpeed = 5f;

    [Header("Camera")]
    [SerializeField] private Vector2 zoomBounds = new Vector2(3f, 7f);//x is min y is max
    [SerializeField] private float zoomSpeed = 5f;

    private PlayerColor playerColor = null;
    private PlayerMovement movement = null;
    private Joystick movementInput = null;
    private CinemachineVirtualCamera camera = null;

    private float defaultZoom = 0f;
    private float zoom = 0f;

    public override void InitAbility(GameObject player)
    {
        base.InitAbility(player);

        abilityInput.AxisOptions = AxisOptions.Vertical;

        playerColor = player.GetComponent<PlayerColor>();
        movement = player.GetComponent<PlayerMovement>();
        movementInput = ObjectReferences.instance.movementInput;
        camera = Camera.main.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject.GetComponent<CinemachineVirtualCamera>();

        if (camera == null) Debug.Log("Not found");
        else Debug.Log("Found Cinmachine");

        zoom = camera.m_Lens.OrthographicSize;
        defaultZoom = zoom;
    }

    public override void UpdateAbility(GameObject player)
    {
        if(abilityInput.Direction.y > 0f)
            zoom += Time.deltaTime * zoomSpeed;
        else if(abilityInput.Direction.y < 0f)
            zoom -= Time.deltaTime * zoomSpeed;

        zoom = Mathf.Clamp(zoom, zoomBounds.x, zoomBounds.y);
        
        camera.m_Lens.OrthographicSize = zoom;

        //Check if the player is on a controllable platform
        player.GetComponent<PlayerMovement>().enabled = true;
        GameObject collidedPlatform = playerColor.GetCollidedPlatform;

        if (collidedPlatform == null || collidedPlatform.GetComponent<ControllablePlatform>() == null) return;

        player.GetComponent<PlayerMovement>().enabled = false;

        player.transform.SetParent(collidedPlatform.transform);
        collidedPlatform.transform.position += new Vector3(movementInput.Direction.x, movementInput.Direction.y, 0f) * moveSpeed * Time.deltaTime;
    }

    public override void ExitAbility(GameObject player)
    {
        base.ExitAbility(player);

        abilityInput.AxisOptions = AxisOptions.Both;
        camera.m_Lens.OrthographicSize = defaultZoom;

        player.GetComponent<PlayerMovement>().enabled = true;
    }
}
