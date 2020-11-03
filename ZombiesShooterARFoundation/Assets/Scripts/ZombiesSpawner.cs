using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiesSpawner : MonoBehaviour
{
    public GameObject[] zombiePrefab;
    public GroundPlaneManager groundplaneManager;
    public Camera arCamera;
    public GameManager gameManager;

    float waitTime = 2f;
    bool isGrounded = false;

    private bool isPlaced;
    public static Vector3 hitPosition;
    // Update is called once per frame
    void Update()
    {

        if (!groundplaneManager.isGroundPlaneReady) return;

        Vector3 rayOrigin = arCamera.transform.position;
        Vector3 rayDirection = Vector3.down;
        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, rayDirection, out hit))
        {
            if (hit.transform.CompareTag("Ground"))
            {
                isGrounded = true;

                hitPosition = hit.point;

                // to update the position of plane so that even if player moves we are changing the position of plane
                hit.transform.position = hitPosition;


            }
            else
            {
                isGrounded = false;
            }
        }
        if (isGrounded && !gameManager.IsGameOver)
        {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                SpawnZombies();
                waitTime = 2;
            }
        }

    }

    private void SpawnZombies()
    {
        Vector3 camForward = arCamera.transform.forward;
        camForward.y = 0;//because we want horizontal vector irrespetive of angle of arcamera
        camForward.Normalize();
        Vector3 spawnPoint = hitPosition + (camForward * 5);
        spawnPoint = Quaternion.AngleAxis(Random.Range(-45, 45), Vector3.up) * spawnPoint;
        GameObject zombieInstance = Instantiate(zombiePrefab[Random.Range(0, zombiePrefab.Length)],
                                                spawnPoint, Quaternion.identity);

        // isPlaced = true;
    }
}
