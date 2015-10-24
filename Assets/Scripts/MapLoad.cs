using UnityEngine;
using System.Collections;
using System;

public class MapLoad : MonoBehaviour {
    
    public Transform generatedWallFolder;
    public Transform generatedEnemyFolder;
    public Transform generatedBulletFolder;
    public Transform spawnLocation;
    public Transform powerUp;

    public Transform player1;
    public Transform player2;

    public int level;

    public Transform wall;
    public Transform iron;
    public Transform bush;
    public Transform ice;
    public Transform water;

    public AudioSource levelStarting;

    private bool multiplayer = false;
    private int currentLevel;

    void Start ()
    {
        LoadMap(level);

        Application.targetFrameRate = 60;
        QualitySettings.antiAliasing = 0;
        QualitySettings.shadowCascades = 0;
        QualitySettings.vSyncCount = 1;
        QualitySettings.SetQualityLevel(2);
    }

    void Update()
    {
        if (currentLevel != level)
        {
            LoadMap(level);
        }

        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            multiplayer = !multiplayer;
            LoadMap(1);
        }
    }
    // message receiver (getter) from "EnemySpawn"
    public void GetMultiplayer(ArgsPointer<bool> pointer)
    {
        pointer.Args = new bool[] { multiplayer };
    }

    private void LoadMap(bool won)
    {
        if (won)
        {
            LoadMap(++level);
        }
    }

    private void LoadMap(int lev)
    {
        currentLevel = lev;
        level = lev;

        // Reset data
        DeleteChilds(generatedWallFolder);
        DeleteChilds(generatedEnemyFolder);
        DeleteChilds(generatedBulletFolder);

        player1.SendMessage("ResetPosition");
        player1.GetComponent<Animator>().SetBool("hit", false);
        player1.SendMessage("SetShooting", false);
        player1.SendMessage("SetShooting", false);
        player1.SendMessage("SetShield", 6);

        if (multiplayer)
        {
            player2.SendMessage("ResetPosition");
            player2.GetComponent<Animator>().SetBool("hit", false);
            player2.SendMessage("SetShooting", false);
            player2.SendMessage("SetShooting", false);
            player2.SendMessage("SetShield", 6);
            player2.SendMessage("SetIsTemplate", false);
        }
        else
        {
            player2.GetComponent<Transform>().position = new Vector3(0, -155, 0);
            player2.SendMessage("SetIsTemplate", true);
        }


        // Enemy spawning reset
        spawnLocation.SendMessage("Reset");

        // Read map file
        string[] m = System.IO.File.ReadAllLines(@"Assets/Maps/map" + currentLevel + ".txt");
        GenerateObjects(m);

        // powerUp reset
        powerUp.SendMessage("Reset");

        // play a sound
        levelStarting.Play();
        
    }

    private void DeleteChilds(Transform folder)
    {
        Transform[] ts = folder.GetComponentsInChildren<Transform>();

        foreach (var t in ts)
        {
            if (!t.gameObject.name.Contains("Generated"))
            {
                Destroy(t.gameObject);
            }
        }
    }

    private void GenerateObjects(string[] m)
    {
        for (int i = 0; i < 26; i++)
        {
            for (int j = 0; j < 26; j++)
            {
                Transform t = null;
                if (m[i][j] == 'o')
                {
                    t = Instantiate(wall, new Vector3(j - 13, 13 - (i + 1), 0), wall.rotation) as Transform;
                }
                else if (m[i][j] == 'Q')
                {
                    t = Instantiate(iron, new Vector3(j - 13, 13 - (i + 1), 0), wall.rotation) as Transform;
                }
                else if (m[i][j] == 'b')
                {
                    t = Instantiate(bush, new Vector3(j - 13, 13 - (i + 1), 0), wall.rotation) as Transform;
                }
                else if (m[i][j] == 'i')
                {
                    t = Instantiate(ice, new Vector3(j - 13, 13 - (i + 1), 0), wall.rotation) as Transform;
                }
                else if (m[i][j] == 'w')
                {
                    t = Instantiate(water, new Vector3(j - 13, 13 - (i + 1), 0), wall.rotation) as Transform;
                }
                if (m[i][j] != '.')
                {
                    t.parent = generatedWallFolder;
                }
            }
        }
    }
}
