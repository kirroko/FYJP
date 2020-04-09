using UnityEngine;
using Cinemachine;

public class CameraFindPlayer : MonoBehaviour
{
    private CinemachineVirtualCamera machine = null;

    private void Start()
    {
        machine = GetComponent<CinemachineVirtualCamera>();
    }

    private void Update()
    {
        if (machine.Follow == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if(player != null)
                machine.Follow = player.transform;
        }
    }
}
