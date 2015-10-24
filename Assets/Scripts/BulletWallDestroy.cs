using UnityEngine;
using System.Collections;
using System;

public class BulletWallDestroy : MonoBehaviour {

    public Transform generatedWallsFolder;

    private Transform bullet;
    private Transform wall;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Animator bulletAnim = gameObject.GetComponent<Animator>();
        bullet = gameObject.GetComponent<Transform>();
        wall = collider.GetComponent<Transform>();

        if ((wall.name.Contains("Wall") || wall.name.Contains("Iron")) && !bulletAnim.GetBool("hit"))
        {
            bulletAnim.SetBool("hit", true);
            destroyWallsAccordingToCoordinates(Mathf.Round(bullet.position.x), Mathf.Round(bullet.position.y));
            PlaySound(bulletAnim);
        }
    }

    private void PlaySound(Animator bulletAnim)
    {
        if (wall.name.Contains("Iron"))
        {
            gameObject.SendMessage("PlayIronHitSound");
        }
        if (wall.name.Contains("Wall"))
        {
            gameObject.SendMessage("PlayBrickHitSound");
        }
    }

    private void destroyWallsAccordingToCoordinates(float x, float y)
    {
        Transform[] ts = generatedWallsFolder.GetComponentsInChildren<Transform>();

        Animator bulletAnim = gameObject.GetComponent<Animator>();
        float input_x = bulletAnim.GetFloat("input_x");
        float input_y = bulletAnim.GetFloat("input_y");

        // Horizontal shot
        if (input_y == 0)
        {
            if (input_x == -1)
            {
                x -= 1;
            }

            PartiallyDestroy(ts.GetByNameAndCoords("Wall", x, y), bulletAnim);
            PartiallyDestroy(ts.GetByNameAndCoords("Wall", x, y - 1), bulletAnim);
        }

        // Vertical shot
        if (input_x == 0)
        {
            if (input_y == -1)
            {
                y -= 1;
            }

            PartiallyDestroy(ts.GetByNameAndCoords("Wall", x, y), bulletAnim);
            PartiallyDestroy(ts.GetByNameAndCoords("Wall", x - 1, y), bulletAnim);
        }
    }

    public static void PartiallyDestroy(Transform obj, Animator bulletAnim)
    {
        float input_x = bulletAnim.GetFloat("input_x");
        float input_y = bulletAnim.GetFloat("input_y");
        
        obj.NotNull((t) =>
        {
            Animator wallAnim = t.GetComponent<Animator>();

            float curr = wallAnim.GetFloat("left_numpad");

            // The tyniest piece of wall left
            if (curr.IsIn(1, 3, 7, 9))
            {
                Destroy(t.gameObject);
            }
            // Vertical shot
            else if (input_x == 0)
            {
                if (curr.IsIn(2, 8))
                {
                    Destroy(t.gameObject);
                }
                else if (curr.IsIn(4, 5, 6))
                {
                    wallAnim.SetFloat("left_numpad", curr + input_y * 3);
                }
            }
            // Horizontal shot
            else if (input_y == 0)
            {
                if (curr.IsIn(4, 6))
                {
                    Destroy(t.gameObject);
                }
                else if (curr.IsIn(2, 5, 8))
                {
                    wallAnim.SetFloat("left_numpad", curr + input_x);
                }
            }
        });
    }
}
