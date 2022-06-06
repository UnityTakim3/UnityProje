using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviourPunCallbacks
{
    public short level = 1;
    public bool main = false;
    bool levelChanged = false;
    public void Start()
    {
        DontDestroyOnLoad(gameObject);
        PhotonNetwork.AutomaticallySyncScene = true;
    }
    private void Update()
    {
        if (levelChanged)
        {
            Clear();
        }
    }
    public void LoadLevel()
    {
        if (level == 1)
        {
            main = true;
            if (PhotonNetwork.IsMasterClient)
            {
                if (SceneManager.GetActiveScene().buildIndex == 2)
                {
                    SceneManager.LoadScene(3);
                    main = true;
                    levelChanged = true;
                }
                else
                {
                    SceneManager.LoadScene(2);
                    main = true;
                    levelChanged = true;
                }

            }
        }
        else
        {
            main = true;

            GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
            foreach (var item in players)
            {
                if (item.GetComponent<PhotonView>().IsMine)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        item.transform.position = GameObject.FindGameObjectWithTag("PlayerSpawnClient").transform.position;
                    }
                    else
                    {
                        item.transform.position = GameObject.FindGameObjectWithTag("PlayerSpawnMaster").transform.position;

                    }
                }
            }
        }

    }
    private void Clear()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("LevelController");
        foreach (var item in objects)
        {
            if (item.GetComponent<LevelController>().main == false)
            {
                Destroy(item);
            }
        }
    }
}
