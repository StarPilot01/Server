using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BlueFire : MonoBehaviour
{
    public int speed;
    public int damage;

    private float timer = 0f;
    private float interval = 0.02f;
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            // 0.02초마다 실행할 로직
            timer = 0f;

            transform.Translate(0, -1 * speed * interval, 0);


        }


        if(transform.position.y  < - 10)
        {
            Destroy(gameObject);
        }
    }

   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<BubbleDragon>().OnDamage(damage);

            Destroy(this.gameObject);

        }

    }
}
