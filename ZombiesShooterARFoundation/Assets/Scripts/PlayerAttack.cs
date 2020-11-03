using UnityEngine;
using UnityEngine.UI;

public class PlayerAttack : MonoBehaviour
{
    public float headshotDamage = 3f;
    public float bodyshotDamage = 1.5f;

    public float coolDownTime = 0.2f;
    float nextShootTime;
    bool canShoot;

    public AudioSource fireSound;
    public GameObject muzzleFlash;
    public float scaleTime;
    public GameObject zombieBlood;
    public Image playerHealthUI;

    private float playerHealth;
    public GameManager gameManager;

    private void Awake()
    {
        playerHealth = playerHealthUI.fillAmount;
    }
    public float PlayerHealth
    {
        get
        {
            return playerHealth;
        }
        set
        {
            playerHealth = value;
            UpdateHealthUI();
        }
    }

    void UpdateHealthUI()
    {
        playerHealthUI.fillAmount = playerHealth;
    }


    private void Update()
    {
        if (Time.time > nextShootTime)
            canShoot = true;
        else
            canShoot = false;

        if (playerHealth <= 0)
            gameManager.IsGameOver = true;
    }

    public void Shoot()
    {
        if (!canShoot)
            return;

        if (fireSound)
            fireSound.Play();

        muzzleFlash.transform.localScale = Vector3.zero;
        iTween.PunchScale(muzzleFlash, Vector3.one, scaleTime);

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            if (hit.transform.tag != "Zombie")
                return;

            GameObject bloodInstance = Instantiate(zombieBlood,
                                                   hit.point,
                                                   Quaternion.LookRotation(-hit.normal));

            iTween.PunchScale(bloodInstance, Vector3.one, scaleTime * 5f);
            Destroy(bloodInstance, 1f); //scaleTime * 5 = 1;

            if (hit.collider.GetType() == typeof(CapsuleCollider))
            {
                hit.transform.GetComponent<Zombie>().Damaged(bodyshotDamage);
            }
            else if (hit.collider.GetType() == typeof(SphereCollider))
            {
                hit.transform.GetComponent<Zombie>().Damaged(headshotDamage);
            }
        }
        nextShootTime = Time.time + coolDownTime;
    }
}
