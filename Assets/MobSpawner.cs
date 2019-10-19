using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    public GameObject Mob;
    public float SpawnRate;
    public float SpawnDelay;
    void Start()
    {
        InvokeRepeating("SpawnMob", SpawnDelay, SpawnRate);   
    }

    private void SpawnMob()
    {
        Instantiate(Mob, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
    }
}
