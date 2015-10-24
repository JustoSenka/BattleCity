using UnityEngine;
using System.Collections;
using System;

public class Player : MonoBehaviour
{
    public int level = 1;
    public int lives = 3;
    public Transform bulletWeak;
    public Transform bulletFast;
    public Transform bulletStrong;
    
    public Animator shieldAnim;

    public int shieldTime = 0;

    void Start()
    {
        level = 1;
    }

    void Update()
    {
        if (level == 1)
        {
            gameObject.SendMessage("SetBullet", bulletWeak);
            gameObject.SendMessage("SetMaxBullets", 1);
        }
        if (level == 2)
        {
            gameObject.SendMessage("SetBullet", bulletFast);
            gameObject.SendMessage("SetMaxBullets", 1);
        }
        if (level == 3)
        {
            gameObject.SendMessage("SetBullet", bulletFast);
            gameObject.SendMessage("SetMaxBullets", 2);
        }
        if (level == 4)
        {
            gameObject.SendMessage("SetBullet", bulletStrong);
            gameObject.SendMessage("SetMaxBullets", 2);
        }

        gameObject.GetComponent<Animator>().SetInteger("level", level);
    }

    // Bonus taken
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Transform other = collision.GetComponent<Transform>();
        
        if (other.name.Contains("PowerUp"))
        {
            int bonus = Mathf.RoundToInt(other.GetComponent<Animator>().GetFloat("bonus"));

            if (bonus == 1)
            {
                level++;
            }
            if (bonus == 2)
            {
                other.gameObject.SendMessage("DestroyAllTanks");
            }
            if (bonus == 3)
            {
                other.gameObject.SendMessage("FreezeTime");
            }
            if (bonus == 4)
            {
                SetShield(15);
            }
            if (bonus == 5)
            {
                lives++;
            }

            other.gameObject.SendMessage("HidePowerUp");
        }
    }

    // Message receiver from "Map load" and/ "this"
    private void SetShield(int time)
    {
        if (shieldTime <= 0)
        {
            shieldTime = time;
            StartCoroutine(ShieldEnumerator());
        }

        shieldTime = time;
        shieldAnim.SetBool("isOn", true);
        gameObject.GetComponent<Animator>().SetBool("shield", true);
    }

    IEnumerator ShieldEnumerator()
    {
        while (shieldTime > 0)
        {
            yield return new WaitForSeconds(1);
            shieldTime--;
        }
        if (shieldTime <= 0)
        {
            shieldAnim.SetBool("isOn", false);
            gameObject.GetComponent<Animator>().SetBool("shield", false);
        }
    }

    // message receiver from "load map"
    public void SetLevel(int level)
    {
        this.level = level;
    }

    // message receiver from "BulletTankDestroy"
    public void Hit()
    {
        lives--;
    }

    // message receiver from "BulletTankDestroy"
    public void GetLives(ArgsPointer<int> pointer)
    {
        pointer.Args = new int[] { lives };
    }

    // message receiver from "load map"
    public void SetLives(int lives)
    {
        this.lives = lives;
    }

}
