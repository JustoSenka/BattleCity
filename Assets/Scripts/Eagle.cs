using UnityEngine;
using System.Collections;
using System;

public class Eagle : MonoBehaviour {

    public AudioSource eagleDestroy;
    public AudioSource gameOver;
    public Transform player1;
    public Transform player2;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Transform other = collider.GetComponent<Transform>();

        if (other.name.Contains("Bullet") && !other.GetComponent<Animator>().GetBool("hit") &&
            !gameObject.GetComponent<Animator>().GetBool("isDestroyed"))
        {
            other.GetComponent<Animator>().SetBool("hit", true);
            gameObject.GetComponent<Animator>().SetBool("isDestroyed", true);
            eagleDestroy.Play();
            
            this.DoAfter(1, () => gameOver.Play());
        }
    }


    private void FinishGame()
    {
        player1.SendMessage("SetLevel", 1);
        player1.SendMessage("SetLives", 3);
        player1.SendMessage("SetIsTemplate", false);
        player2.SendMessage("SetLevel", 1);
        player2.SendMessage("SetLives", 3);
        player2.SendMessage("SetIsTemplate", false);
        gameObject.GetComponent<Animator>().SetBool("isDestroyed", false);
        gameObject.SendMessage("LoadMap", 1);
    }
}
