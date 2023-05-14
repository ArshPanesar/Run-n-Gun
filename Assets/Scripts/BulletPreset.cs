using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bullet Preset", menuName = "Bullet Preset")]
public class BulletPreset : ScriptableObject
{
    public RuntimeAnimatorController animationController;

    public int damage;
    public int speed;

    public int numOfBulletsPerSpawn;
    public float cooldownTime;

    public List<Vector2> spawnPointOffsets;
    public List<float> angleOffsets;
}
