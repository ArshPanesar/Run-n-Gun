using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bulletPrefab;
    [HideInInspector] public Vector2 spawnPosition;
    [HideInInspector] public Vector2 lineOfShot;

    private bool canShoot = true;
    private float cooldownTimer = 0f;
    private float cooldownTime = 0f;

    public void Spawn(BulletPreset preset, int layerMask)
    {
        if (!canShoot)
        {
            return;
        }

        List<GameObject> gameObjects = new List<GameObject>(preset.numOfBulletsPerSpawn);

        // Spawn with Position Offsets
        Vector2 globalPosition = new Vector2(transform.position.x, transform.position.y);
        for (int i = 0; i < preset.numOfBulletsPerSpawn; i++)
        {
            Vector2 position = new Vector2(spawnPosition.x, spawnPosition.y) + preset.spawnPointOffsets[i];
            gameObjects.Add(Instantiate(bulletPrefab, globalPosition + position, Quaternion.identity));
        }

        // Set Bullet
        for (int i = 0; i < gameObjects.Count; i++)
        {
            gameObjects[i].layer = layerMask;

            BulletBehaviour bullet = gameObjects[i].GetComponent<BulletBehaviour>();

            Quaternion angle = Quaternion.Euler(0f, 0f, preset.angleOffsets[i]);
            bullet.transform.rotation = angle;

            Vector2 direction = angle * lineOfShot;

            bullet.SetBullet(preset.speed, direction, preset.damage, preset.animationController);
        }

        cooldownTime = preset.cooldownTime;
        canShoot = false;
    }

    private void Start()
    {
        EventManager.GetInstance().AddListener(GameEvents.DestroyBullet, Despawn);
    }

    private void OnDestroy()
    {
        EventManager.GetInstance().RemoveListener(GameEvents.DestroyBullet, Despawn);
    }

    private void Update()
    {
        if (!canShoot)
        {
            cooldownTimer += Time.deltaTime;
            if (cooldownTimer > cooldownTime)
            {
                canShoot = true;
                cooldownTimer = 0f;
            }
        }
    }

    // Events
    //
    void Despawn(Dictionary<string, object> args)
    {
        GameObject bullet = (GameObject)args["bullet"];
        
        Destroy(bullet);
    }
}
