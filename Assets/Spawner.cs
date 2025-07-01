using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject obj;
    private float RandomY, RandomX;
    Vector2 WhereToSpawn;
    [SerializeField]
    public float spawnDelay = 10f;
    float nextSpawn = 0.0f, delta = 0.5f;
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextSpawn)
        {
            spawnDelay = Mathf.Max(2f, spawnDelay - delta);
            nextSpawn = Time.time + spawnDelay;
            
            RandomY = Random.Range(-10, 10); // генерируем координаты
            RandomX = Random.Range(-8, 8);
            
            Vector2 pos = (Vector2)transform.position;
            
            if (Mathf.Abs(RandomY) < 3) // для удобства определения начального вектора движения
            {
                if (RandomX > 0)
                {
                    RandomX = 12;
                }
                else
                {
                    RandomX = -12;
                }
            }
            else
            {
                if (RandomY > 0)
                {
                    RandomY = 6;
                }
                else
                {
                    RandomY = -6;
                }
            }
            
            WhereToSpawn = new Vector2(RandomX, RandomY); // точка генерации
            GameObject Plan = Instantiate(obj, WhereToSpawn, Quaternion.identity); // генерация объекта

        }
    }
}
