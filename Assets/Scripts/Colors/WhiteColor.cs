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
public class WhiteColor : ScriptableObject
{
    public bool IsLocked { get { return locked; } }
    public COLORS GetMain { get { return mainColor; } }
    public COLORS GetParent1 { get { return parentColor1; } }
    public COLORS GetParent2 { get { return parentColor2; } }
    public COLORS GetParentOf1 { get { return parentOf1; } }
    public COLORS GetParentOf2 { get { return parentOf2; } }
    public Color Color { get { return color;} }

    [Header("Color Related")]
    [SerializeField] protected Color color = Color.white;
    [SerializeField] protected COLORS mainColor = COLORS.NONE;
    [SerializeField] protected COLORS parentColor1 = COLORS.NONE;
    [SerializeField] protected COLORS parentColor2 = COLORS.NONE;
    [SerializeField] protected COLORS parentOf1 = COLORS.NONE;
    [SerializeField] protected COLORS parentOf2 = COLORS.NONE;

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
