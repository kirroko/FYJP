using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorAbility : MonoBehaviour
{
    public Fireball fireBall = null;
    public IceShard iceShard = null;


    private PlayerInfo info = null;

    private void Start()
    {
        info = GetComponent<PlayerInfo>();
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;

        switch (info.colour)
        {
            case PAINT_COLOURS.RED:
                Fireball fireball = Instantiate(fireBall, transform.position, Quaternion.identity);
                fireball.Init(info.dir);
                break;

            case PAINT_COLOURS.BLUE:
                IceShard iceshard = Instantiate(iceShard, transform.position, Quaternion.identity);
                iceshard.Init(info.dir);
                break;

            default:
                break;
        }

    }
}
