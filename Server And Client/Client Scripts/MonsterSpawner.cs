using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    static MonsterSpawner _instance;
    public static MonsterSpawner Instance { get { return _instance; } }

    [SerializeField]
    GameObject mushroomPrefab;

    [SerializeField]
    GameObject batooPrefab;

    [SerializeField]
    int mushroomToSpawnAtOnce;
    [SerializeField]
    int batooToSpawnAtOnce;

    public int curMushroomCount;
    public int curBatooCount;
    private void Awake()
    {
        _instance = this;
        UnityEngine.Random.InitState(2023);

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DecreaseMushRoom()
    {
        curMushroomCount--;

        CheckAndGenerateMob();
    }

    void CheckAndGenerateMob()
    {
        if (curBatooCount == 0 && curMushroomCount == 0)
        {
            mushroomToSpawnAtOnce = (int)(mushroomToSpawnAtOnce * 1.5f);
            batooToSpawnAtOnce = (int)(batooToSpawnAtOnce * 1.5f);

            StartStage();
        }
    }
    public void DecreaseBatoo()
    {
        curBatooCount--;

        CheckAndGenerateMob();

    }

    public void StartStage()
    {

        curMushroomCount = mushroomToSpawnAtOnce;
        curBatooCount = batooToSpawnAtOnce;


        for (int i = 0; i < mushroomToSpawnAtOnce; i++)
        {
            int posX = UnityEngine.Random.Range(-6, 6);
            int posY = 6;

            Instantiate(mushroomPrefab, new Vector2(posX, posY), Quaternion.identity);

        }
        
        for(int i = 0; i < batooToSpawnAtOnce; i++)
        {
            int posX = Random.Range(-6, 6);
            int posY = Random.Range(3 ,5);

            Instantiate(batooPrefab, new Vector2(posX, posY), Quaternion.identity);

        }

    }


}

