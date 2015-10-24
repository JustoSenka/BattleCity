using UnityEngine;
using System.Collections;

public class BulletDestroy : MonoBehaviour {

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Transform other = collider.GetComponent<Transform>();

        if (other.name.Contains("Bullet"))
        {
            other.GetComponent<Animator>().SetBool("hit", true);
            other.gameObject.SendMessage("PlayIronHitSound");
        }
    }
}
