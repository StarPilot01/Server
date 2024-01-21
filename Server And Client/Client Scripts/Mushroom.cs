using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Define;

public class Mushroom : MonoBehaviour , IDamageable
{
    // Start is called before the first frame update

    Rigidbody2D _rigid;
    [SerializeField]
    int speed;
    [SerializeField]
    int rewardScore;
    [SerializeField]
    int damage;
    MoveDir _dir;


    public float _maxHP;
    float _hp;
    public float HP
    {
        get { return _hp; }
        set
        {
            if (value > _hp)
            {
                _hp = _maxHP;
            }
            else if (value < 0)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _hp = value;
            }

            
        }
    }
    void Start()
    {
        _dir = (MoveDir)Random.Range(0, 3);

    }

    // Update is called once per frame
    private float timer = 0f;
    private float moveSwitchingTimer = 0f;
    private float interval = 0.02f;
    private float moveSwitchingInterval = 2.5f;

    bool isCollided = false;
    void Update()
    {
        timer += Time.deltaTime;
        moveSwitchingTimer += Time.deltaTime;

        if (timer >= interval)
        {
            // 0.02초마다 실행할 로직
            timer = 0f;



            Move();


        }

        if(moveSwitchingTimer >= moveSwitchingInterval)
        {
            moveSwitchingTimer = 0;

            _dir = (MoveDir)Random.Range(0, 3);
        }
    }

    
    private void Move()
    {
        if(_dir == MoveDir.Stop)
        {
            return;
        }

        float moveX = 0;
        switch (_dir)
        { 
           case MoveDir.Left:
                moveX = -speed * Time.fixedDeltaTime;
                break; 
            case MoveDir.Right:
                moveX = speed * Time.fixedDeltaTime;
                break;

        
        }


        transform.Translate(moveX, 0 , 0);


        if(transform.position.x > 7)
        {
            _dir = MoveDir.Left;
        }
        else if(transform.position.x < -7)
        {
            _dir = MoveDir.Right;
        }


    }


    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Bubble" && !isCollided)
        {

            isCollided = true;
    
            UIManager.Instance.Score += rewardScore;
    
            MonsterSpawner.Instance.DecreaseMushRoom();
    
            Destroy(collider.gameObject);
    
            Destroy(this.gameObject);
        }
    
       
    
    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<BubbleDragon>().OnDamage(damage);
        }
    }

    public void OnDamage(float damage)
    {
        throw new System.NotImplementedException();
    }
}
