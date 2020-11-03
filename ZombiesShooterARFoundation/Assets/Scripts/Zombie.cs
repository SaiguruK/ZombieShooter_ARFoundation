using System;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public float speed;
    public float attackDistance;
    public float animationSpeed;

    private Animator anim;
    private Camera arCamera;
    private bool canWalk = true;
    private float health = 3.0f;

    public static Action IsAttacking;
    public static Action NotAttacking;

    public static Action IsActive;
    public static Action IsEliminated;

    PlayerAttack playerAttack;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        arCamera = Camera.main;
        playerAttack = arCamera.GetComponent<PlayerAttack>();
    }
    // Start is called before the first frame update
    void Start()
    {
        anim.speed = animationSpeed;

        if (IsActive != null)
            IsActive.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionVector = arCamera.transform.position - transform.position;

        // as we just want horizontal direction, otherwise it will be slant vector..normalize will ensure magnitude to be 1 only
        directionVector = new Vector3(directionVector.x, 0, directionVector.z).normalized;

        transform.rotation = Quaternion.LookRotation(directionVector);

        if (canWalk)
        {
            transform.Translate(directionVector * speed * Time.deltaTime, Space.World);
        }

        //distance from zombie to base of camera
        float distance = Vector3.Distance(ZombiesSpawner.hitPosition, transform.position);

        if (health >= 0)
        {
            if (distance <= attackDistance)
            {
                canWalk = false;
                anim.SetTrigger("attack");

                playerAttack.PlayerHealth -= 0.05f * Time.deltaTime;

                if (IsAttacking != null)
                {
                    IsAttacking.Invoke();

                }
            }
            else
            {
                canWalk = true;
                anim.SetTrigger("walk");

                if (NotAttacking != null)
                {
                    NotAttacking.Invoke();

                }
            }
        }

    }

    public void Damaged(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            var colliders = GetComponents<Collider>();
            foreach (var col in colliders)
            {
                col.enabled = false;// As we are detroying zombie after 5 seconds
            }

            if (IsEliminated != null)
            {
                IsEliminated.Invoke();
            }
            canWalk = false;
            anim.SetTrigger("die");
            Destroy(gameObject, 5);
        }
    }
}
