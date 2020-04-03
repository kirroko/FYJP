using UnityEngine;
using Cinemachine;

public class CameraFindPlayer : MonoBehaviour
{
    private void Start()
    {
        GetComponent<CinemachineVirtualCamera>().Follow = ObjectReferences.instance.player.transform;
    }
}
