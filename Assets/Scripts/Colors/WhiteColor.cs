using UnityEngine;

public enum COLORS
{
    WHITE,
    YELLOW,
    RED,
    BLUE,
    ORANGE,
    PURPLE,
    GREEN,

    NONE,
}

[CreateAssetMenu(fileName = "WhiteColor", menuName = "Colors/White", order = 2)]
public class WhiteColor : ScriptableObject
{
    public bool IsLocked { get { return locked; } set { locked = value; } }
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

    [Header("Ability Related")]
    [SerializeField] protected float abilityInterval = 1f;


    protected Joystick abilityInput = null;
    protected Vector2 dir = Vector2.zero;
    protected float abilityCD = 0f;
    protected bool abilityActivated = false;

    private float offsetPos = 1.5f;

    public virtual void InitAbility(GameObject player)
    {
        abilityInput = ObjectReferences.instance.abilityInput;
    }

    public virtual void UpdateAbility(GameObject player)
    {
        abilityCD -= Time.deltaTime;

        if(abilityInput.IsPressed && abilityCD <= 0f)
        {
            dir = abilityInput.Direction;

            Vector2 direction = Vector2.zero;

            if (dir.x > 0.5f)
                direction.x = 1f;
            else if (dir.x < -0.5f)
                direction.x = -1f;

            if (dir.y > 0.5f)
                direction.y = 1f;
            else if (dir.y < -0.5f)
                direction.y = -1f;

            dir = direction;
            abilityActivated = true;
        }

    }

    public virtual void ExitAbility(GameObject player)
    {
        abilityCD = 0f;
        abilityActivated = false;
        dir = Vector2.zero;
    }

    public virtual void OnPlayerDestroyed()
    {

    }

    protected void Shoot(Projectile projectile, float projectileSpeed, GameObject player)
    {
        Bounds playerColliderBounds = player.GetComponent<Collider2D>().bounds;
        Vector3 firePoint = playerColliderBounds.center + new Vector3(playerColliderBounds.extents.x * dir.x, playerColliderBounds.extents.y * dir.y, 0f) * offsetPos;

        Projectile temp = Instantiate(projectile, firePoint, Quaternion.identity);
        temp.Init(dir, projectileSpeed);

        dir = Vector2.zero;
        abilityCD = abilityInterval;

        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetTrigger("Shoot");
    }
}
