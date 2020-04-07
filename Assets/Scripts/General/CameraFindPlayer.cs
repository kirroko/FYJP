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
            machine.Follow = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
