using UnityEngine;
using System.Collections;

public class BulletIronDestroy : MonoBehaviour {

    public Transform generatedWallsFolder;

    private Transform bullet;
    private Transform iron;
    private Transform[] ts;

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Animator bulletAnim = gameObject.GetComponent<Animator>();
        bullet = gameObject.GetComponent<Transform>();
        iron = collider.GetComponent<Transform>();

        if ((iron.name.Contains("Iron") || iron.name.Contains("Wall")) && !bulletAnim.GetBool("hit"))
        {
            bulletAnim.SetBool("hit", true);
            destroyWallsAccordingToCoordinates(Mathf.Round(bullet.position.x), Mathf.Round(bullet.position.y));
        }

        PlaySound(bulletAnim);
    }

    private void PlaySound(Animator bulletAnim)
    {
        if (iron.name.Contains("Iron"))
        {
            gameObject.SendMessage("PlayIronHitSound");
        }
        if (iron.name.Contains("Wall"))
        {
            gameObject.SendMessage("PlayBrickHitSound");
        }
    }

    private void destroyWallsAccordingToCoordinates(float x, float y)
    {
        ts = generatedWallsFolder.GetComponentsInChildren<Transform>();

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

            // If Iron destroys instantly
            ts.GetByNameAndCoords("Iron", x, y).NotNull((t) => {
                Destroy(t.gameObject); });

            ts.GetByNameAndCoords("Iron", x, y - 1).NotNull((t) => {
                Destroy(t.gameObject); });

            // Walls destroys doubled
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

            // If Iron destroys instantly
            ts.GetByNameAndCoords("Iron", x, y).NotNull((t) => {
                Destroy(t.gameObject); });

            ts.GetByNameAndCoords("Iron", x - 1, y).NotNull((t) => {
                Destroy(t.gameObject); });

            // Walls destroys doubled
            PartiallyDestroy(ts.GetByNameAndCoords("Wall", x, y), bulletAnim);
            PartiallyDestroy(ts.GetByNameAndCoords("Wall", x - 1, y), bulletAnim);
        }
    }

    private void PartiallyDestroy(Transform obj, Animator bulletAnim)
    {
        float input_x = bulletAnim.GetFloat("input_x");
        float input_y = bulletAnim.GetFloat("input_y");

        obj.NotNull((t) =>
        {
            Animator wallAnim = t.GetComponent<Animator>();

            float curr = wallAnim.GetFloat("left_numpad");

            // Strong shot will allways destroy a piece (and maybe two)
            Destroy(t.gameObject);

            // Horizontal shot
            if (input_x == 1 && curr.IsIn(3, 6, 9))
            {
                obj.position += new Vector3(1, 0, 0);
                BulletWallDestroy.PartiallyDestroy(ts.GetByNameAndCoords
                    ("Wall", obj.position.x, obj.position.y), bulletAnim);
            }
            if (input_x == -1 && curr.IsIn(1, 4, 7))
            {
                obj.position += new Vector3(-1, 0, 0);
                BulletWallDestroy.PartiallyDestroy(ts.GetByNameAndCoords
                    ("Wall", obj.position.x, obj.position.y), bulletAnim);
            }
            // Vertical shot
            if (input_y == 1 && curr.IsIn(7, 8, 9))
            {
                obj.position += new Vector3(0, 1, 0);
                BulletWallDestroy.PartiallyDestroy(ts.GetByNameAndCoords
                    ("Wall", obj.position.x, obj.position.y), bulletAnim);
            }
            if (input_y == -1 && curr.IsIn(1, 2, 3))
            {
                obj.position += new Vector3(0, -1, 0);
                BulletWallDestroy.PartiallyDestroy(ts.GetByNameAndCoords
                    ("Wall", obj.position.x, obj.position.y), bulletAnim);
            }
        });
    }
}
