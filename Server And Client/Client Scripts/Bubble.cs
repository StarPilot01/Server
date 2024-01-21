using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Bubble : MonoBehaviour
{
    // Start is called before the first frame update
    //public MoveDir _dir;
    public int _speed;
    public int damage;
    public bool isNormal = true;
    public int ownerID = -1;
    void Start()
    {
        
    }

    private float timer = 0f;
    private float interval = 0.02f;
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            // 0.02초마다 실행할 로직
            timer = 0f;

            if(isNormal)
            {
                transform.Translate(_speed * interval, 0, 0);

            }


        }
    }

    public void Init(int id, bool isRight)
    {
        ownerID = id;
        _speed = isRight ? _speed : -_speed;
    }
}
