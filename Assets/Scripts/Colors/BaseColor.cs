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

/**
 * Any class that is a color the player can become has to be
 * dereived from this class
 */
public class BaseColor : ScriptableObject
{
    public bool IsLocked { get { return locked; } set { locked = value; } }
    public COLORS GetMain { get { return mainColor; } }
    public COLORS GetParent1 { get { return parentColor1; } }
    public COLORS GetParent2 { get { return parentColor2; } }
    public COLORS GetParentOf1 { get { return parentOf1; } }
    public COLORS GetParentOf2 { get { return parentOf2; } }
    public Color Color { get { return color;} }

    [Header("Color Related")]
    /// Color value
    [SerializeField] protected Color color = Color.white;

    /// E.g Color is red, so this shld be set to red
    [SerializeField] protected COLORS mainColor = COLORS.NONE;

    /// Color that is used to make mainColor.
    /// If it is a primary color leave it as NONE.
    [SerializeField] protected COLORS parentColor1 = COLORS.NONE; 
    [SerializeField] protected COLORS parentColor2 = COLORS.NONE; ///< Same as parentColor1

    /// Color that is made by mixing mainColor and another color.
    /// If it is not part of any other color leave as NONE.
    [SerializeField] protected COLORS parentOf1 = COLORS.NONE;
    [SerializeField] protected COLORS parentOf2 = COLORS.NONE;///< Same as parentOf2

    /// If locked player cannot use it.
    [SerializeField] protected bool locked = false;

    [Header("Ability Related")]
    [SerializeField] protected Sprite abilitySprite = null;
    [SerializeField] protected float abilityInterval = 1f;


    protected Joystick abilityInput = null;
    protected Vector2 dir = Vector2.zero;
    protected float abilityCD = 0f;
    protected bool abilityActivated = false;

    private float offsetPos = 1.5f;

    private bool flipped = false;

    /// To be called once when player first changes into this color.
    public virtual void InitAbility(GameObject player)
    {
        abilityInput = ObjectReferences.instance.abilityInput;
        abilityInput.UpdateHandleImage(abilitySprite);
        abilityInput.GetComponentInChildren<CooldownIndicator>().UpdateSprite(abilitySprite);
    }

    /// To be called every frame while player is in this color.
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

    /// To be called once when player is changing to another color.
    public virtual void ExitAbility(GameObject player)
    {
        abilityCD = 0f;
        abilityActivated = false;
        dir = Vector2.zero;
    }

    /// Dereived class should override it if any function is added to the event system.
    public virtual void OnPlayerDestroyed()
    {

    }

    protected void Shoot(Projectile projectile, float projectileSpeed, GameObject player)
    {
        player.GetComponent<Animator>().SetTrigger("Attack");
        if(player.GetComponent<PlayerMovement>().FacingDirection != dir.x)
        {
            player.GetComponent<SpriteRenderer>().flipX = !player.GetComponent<SpriteRenderer>().flipX;
            flipped = true;
        }
        player.GetComponent<PlayerMovement>().InforceFlip = true;
        EventManager.instance.TriggerShootProjectileEvent(this, projectile, projectileSpeed, player);
        abilityInput.GetComponentInChildren<CooldownIndicator>().StartCooldown(abilityInterval);
    }

    private void ShootProjectileEvent(BaseColor me, Projectile projectile, float projectileSpeed, GameObject player)
    {
        if (me != this) return;

        Bounds playerColliderBounds = player.GetComponent<Collider2D>().bounds;
        Vector3 firePoint = playerColliderBounds.center + new Vector3(playerColliderBounds.extents.x * dir.x, playerColliderBounds.extents.y * dir.y, 0f) * offsetPos;

        Projectile temp = Instantiate(projectile, firePoint, Quaternion.identity);
        temp.Init(dir, projectileSpeed);

        dir = Vector2.zero;
        abilityCD = abilityInterval;

        if (flipped)
        {
            player.GetComponent<SpriteRenderer>().flipX = !player.GetComponent<SpriteRenderer>().flipX;
            flipped = false;
        }
        player.GetComponent<PlayerMovement>().InforceFlip = false;
    }

    protected virtual void AddEvent()
    {
        EventManager.instance.shootProjectileEvent -= ShootProjectileEvent;
        EventManager.instance.shootProjectileEvent += ShootProjectileEvent;
    }
}
