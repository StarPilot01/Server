using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Define;
public class BubbleDragon : MonoBehaviour , IDamageable
{

    public int _id;
    protected MoveDir _moveDir;
    public Role _role;

    public string nickName;
    public float speed;
    public float diffBubblePos;

    public float _maxHP;
    float _hp;
    [SerializeField]
    Slider slider;

    [SerializeField]
    ParticleSystem healEffect;

    [SerializeField]
    TextMeshProUGUI nickNameText;
    [SerializeField]
    TextMeshProUGUI roleText;

    public float HP { get { return _hp; } 
        set
        {  
            if(value > _hp)
            {
                _hp = _maxHP;
            }
            else if(value < 0)
            {
                _hp = 0;
                Die();
            }
            else
            {
                _hp = value;
            }
            
            RefreshUI();
        }
    }

    //public float jumpForce;

    [SerializeField]
    GameObject bubblePrefab;
    protected Rigidbody2D _rigid;

    // Start is called before the first frame update
    protected void Start()
    {
        _moveDir = MoveDir.Stop;
        _rigid = GetComponent<Rigidbody2D>();
        _hp = _maxHP;

        
        TextMeshProUGUI speechBubble = transform.Find("Canvas").Find("speech bubble Text").GetComponent<TextMeshProUGUI>();
        UIManager.Instance.AddSpeechBubbleDic(_id, speechBubble);

        
    }

    // Update is called once per frame
    private float timer = 0f;
    private float interval = 0.02f;

    private int test = 0;
    protected void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            // 0.02초마다 실행할 로직
            timer = 0f;

            
            
            Move();
           

        }

        nickNameText.text = nickName;


        //roleText.text = _role == Role.Magician ? "Magician" : "Healer";
    }

    public void Jump(int jumpForce)
    {
        _rigid.AddForce(new Vector2(0, jumpForce));
    }
    private void Move()
    {
        
        if (_moveDir == MoveDir.Stop)
        {
            return;
        }

        float xMove = 0;

        switch (_moveDir)
        {
            case MoveDir.Left:
                this.GetComponent<SpriteRenderer>().flipX = false;
                xMove = -speed * Time.fixedDeltaTime;
                break;
            case MoveDir.Right:
                this.GetComponent<SpriteRenderer>().flipX = true;
                xMove = speed * Time.fixedDeltaTime;

                break;


        }

        //transform.position = new Vector2(targetX, transform.position.y);

        //_rigid.MovePosition(new Vector2(targetX, 0));

        
        transform.Translate(new Vector2(xMove, 0));

        //_rigid.velocity = new Vector2(targetX, _rigid.velocity.y);
    }

    public void MakeBubble(int id, float posX, float posY, bool isRight)
    {
        //posX = isRight ? posX + diffBubblePos : posX - diffBubblePos;

        Instantiate(bubblePrefab, new Vector2(posX, posY),Quaternion.identity ).GetComponent<Bubble>().Init(id, isRight);

    }

    public void SetDir(MoveDir dir)
    {
        _moveDir = dir;
    }

    protected void FixedUpdate()
    {
        //Move();
    }


   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bubble")
        {
            OnDamage(100);
        }
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Bubble" && collider.GetComponent<Bubble>().ownerID != _id)
        {
            OnDamage(100);
            Destroy(collider.gameObject);
        }
    }

    
    public void OnDamage(float damage)
    {
        HP -= damage;

        //캐릭터 깜빡임

    }

    public void MakeBubble360(int id)
    {
        int bubbletCount = 15;
        float angleStep = 360f / bubbletCount;

        for (int i = 0; i < bubbletCount; i++)
        {
            // 회전 각도에 따라 탄막의 발사 방향을 계산합니다.
            float angle = i * angleStep;
            Vector3 direction = Quaternion.Euler(0f, 0f, angle) * transform.right;

            // 탄막을 생성하고 발사합니다.
            GameObject bullet = Instantiate(bubblePrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Bubble>().isNormal = false;
            bullet.GetComponent<Bubble>().ownerID = id;
            bullet.GetComponent<Rigidbody2D>().velocity = direction * 10f; 
        }
    }

    
   

    public void OnHeal()
    {
        HP += 50;
        healEffect.Play();
    }
    void RefreshUI()
    {
        slider.value = _hp / _maxHP;
    }

    void Die()
    {
        gameObject.SetActive(false);

        C_RequestDead deadPacket = new C_RequestDead();
        deadPacket.id = _id;

        GameManager.Instance.Send(deadPacket);
    }
}
