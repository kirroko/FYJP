using UnityEngine;

public enum COLORS
{
    WHITE,
    RED,
    YELLOW,
    BLUE,
    ORANGE,
    PURPLE,
    GREEN,

    NONE,
}

[CreateAssetMenu(fileName = "WhiteColor", menuName = "Colors/White", order = 2)]
[System.Serializable]
public class WhiteColor : ScriptableObject
{
    public bool IsLocked { get { return locked; } }
    public COLORS GetMain { get { return mainColor; } }
    public COLORS GetParent1 { get { return parentColor1; } }
    public COLORS GetParent2 { get { return parentColor2; } }

    [Header("Color Related")]
    [SerializeField] protected COLORS mainColor = COLORS.NONE;
    [SerializeField] protected COLORS parentColor1 = COLORS.NONE;
    [SerializeField] protected COLORS parentColor2 = COLORS.NONE;

    [SerializeField] protected bool locked = false;

    public virtual void InitAbility(GameObject player)
    {
    }

    public virtual void UpdateAbility(GameObject player)
    {
    }

    public virtual void ExitAbility(GameObject player)
    {
    }

}
