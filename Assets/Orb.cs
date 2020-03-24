using UnityEngine;

public class Orb : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private GameObject power = null; // Prefeb goes here
    [SerializeField] private Transform powerStartPosition = null;
    [SerializeField] private PlayerControl player = null;

    [Header("Attributes")]
    public float maxEnergy = 100f;

    [Header("Rates")]
    public float chargingRate = 1.5f;
    public float recoveryRate = 1.5f;
    public float cooldownRate = 1.5f;

    // private value
    private float energy = 0f;
    private float charge = 0f;
    private float cooldown = 0f;
    private float defaultScale = 1f;
    private bool isCharging = false;
    private GameObject GO = null;

    private void Awake()
    {
        energy = maxEnergy;
    }

    private void Start()
    {
        GO = StartCharge();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    private void Update()
    {
        if(isCharging)
        {
            charge += Time.deltaTime * chargingRate;
            energy -= Time.deltaTime * chargingRate;

            Vector3 scale = new Vector3(defaultScale * charge, defaultScale * charge, defaultScale * charge);
            GO.transform.localScale = scale;

            if(energy < 0)
            {
                isCharging = false;
                DestroyPower();
            }
        }
        else
        {
            if(cooldown > recoveryRate)
            {
                // Can now charge up the next shoot
                // for this prototype it'll auto
                GO = StartCharge();
                cooldown = 0f;
            }
            else
            {
                cooldown += Time.deltaTime;
            }
        }
    }

    private void DestroyPower()
    {
        if (GO != null)
            Destroy(GO);
    }

    private GameObject StartCharge()
    {
        GameObject tempGO = Instantiate(power, powerStartPosition.position, powerStartPosition.rotation);

        isCharging = true;

        return tempGO;
    }

    public void ReleaseCharge()
    {
        GO.GetComponent<Projectile>().TargetLocation = FindTargetNearPlayer();
        GO.GetComponent<Projectile>().Released = true;
        isCharging = false;
        charge = 0;
    }

    public Transform FindTargetNearPlayer()
    {
        return player.NearbyEnemy();
    }
}