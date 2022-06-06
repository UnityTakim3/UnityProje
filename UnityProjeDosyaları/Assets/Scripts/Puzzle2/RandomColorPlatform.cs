using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RandomColorPlatform : MonoBehaviour
{
    [SerializeField] GameObject[] platformsP;
    List<GameObject> platforms = new List<GameObject>();
    public List<GameObject> platformsToRed;
    public List<GameObject> platformsToYellow;
    public List<GameObject> platformsToGreen;

    [SerializeField] Color[] colors;

    [SerializeField] float timeBetweenChangeColor = 6f;
    [SerializeField] float timeToShowColor = 2f;

    [SerializeField] Door doorToOpen;

    List<GameObject> choosenPlatforms = new List<GameObject>();
    List<Color> choosenPlatformsColors = new List<Color>();
    Color platformsDefaultColor;

    PhotonView photonView;
    Coroutine timer = null;
    [SerializeField] int pointForSuccess = 5;
    int points = 0;

    [SerializeField] AudioSource startCountSound;
    [SerializeField] AudioSource startSound;
    [SerializeField] AudioSource loseSound;

    bool isTriggered = false;
    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        foreach (var item in platformsP)
        {
            platforms.Add(item.transform.GetChild(0).gameObject);
        }

        platformsDefaultColor = platforms[0].GetComponent<Renderer>().material.color;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                isTriggered = true;
                StartCoroutine("StartTimer");
            }
        }
    }

    IEnumerator StartTimer()
    {
        photonView.RPC("PaintChoosenPlatforms", RpcTarget.All, "red");
        yield return new WaitForSeconds(1f);
        photonView.RPC("ClearAllPlatformsColors", RpcTarget.All);
        photonView.RPC("PaintChoosenPlatforms", RpcTarget.All, "yellow");
        yield return new WaitForSeconds(1f);
        photonView.RPC("ClearAllPlatformsColors", RpcTarget.All);
        photonView.RPC("PaintChoosenPlatforms", RpcTarget.All, "green");
        yield return new WaitForSeconds(1f);
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            print("startTimer");
            photonView.RPC("ClearAllPlatformsColors", RpcTarget.All);
            //ClearAllPlatformsColors();
            ChooseRandomPlatform();
            yield return new WaitForSeconds(timeToShowColor);

            photonView.RPC("ClearChoosenPlatformsColors", RpcTarget.All);
            // ClearChoosenPlatformsColors();

            yield return new WaitForSeconds(timeBetweenChangeColor - timeToShowColor);

            ControllColors();

            photonView.RPC("ClearLists", RpcTarget.All);
            //ClearLists();

        }
    }
    [PunRPC]
    private void ClearLists()
    {
        choosenPlatforms.Clear();
        choosenPlatformsColors.Clear();
    }

    [PunRPC]
    private void ClearChoosenPlatformsColors()
    {
        foreach (GameObject item in choosenPlatforms)
        {
            item.GetComponent<Renderer>().material.color = platformsDefaultColor;
        }
    }
    [PunRPC]
    private void ClearAllPlatformsColors()
    {
        foreach (GameObject item in platforms)
        {
            item.GetComponent<Renderer>().material.color = platformsDefaultColor;
        }
    }
    private void ChooseRandomPlatform()
    {
        int random1 = UnityEngine.Random.Range(0, platforms.Count);
        int random2 = UnityEngine.Random.Range(0, platforms.Count);
        while (random2 == random1)
        {
            random2 = UnityEngine.Random.Range(0, platforms.Count);
        }
        int random3 = UnityEngine.Random.Range(0, colors.Length);
        int random4 = UnityEngine.Random.Range(0, colors.Length);


        photonView.RPC("ChangeColors", RpcTarget.All, random1, random2, random3, random4);
        //ChangeColors(random1, random2);



    }
    [PunRPC]
    private void ChangeColors(int random1, int random2, int random3, int random4)
    {
        platforms[random1].GetComponent<Renderer>().material.color = colors[random3];
        platforms[random2].GetComponent<Renderer>().material.color = colors[random4];
        startSound.Play();
        choosenPlatforms.Add(platforms[random1]);
        choosenPlatforms.Add(platforms[random2]);
        for (int i = 0; i < choosenPlatforms.Count; i++)
        {
            choosenPlatformsColors.Add(choosenPlatforms[i].GetComponent<Renderer>().material.color);
        }

    }
    private void ControllColors()
    {
        Color color1 = choosenPlatforms[0].GetComponent<Renderer>().material.color;
        Color color2 = choosenPlatforms[1].GetComponent<Renderer>().material.color;

        if (color1.r == choosenPlatformsColors[0].r && color1.g == choosenPlatformsColors[0].g && color1.b == choosenPlatformsColors[0].b && color2.r == choosenPlatformsColors[1].r && color2.g == choosenPlatformsColors[1].g && color2.b == choosenPlatformsColors[1].b)
        {
            if (choosenPlatforms[0].GetComponent<CatWalk>().isPressed && choosenPlatforms[1].GetComponent<CatWalk>().isPressed)
            {
                points++;
                if (points >= pointForSuccess)
                {
                    print("success");

                    doorToOpen.Open(true, 1, 1);
                    StopCoroutine("StartTimer");
                    photonView.RPC("ClearAllPlatformsColors", RpcTarget.All);
                    photonView.RPC("ClearLists", RpcTarget.All);
                }
            }
            else
            {

                photonView.RPC("Lose", RpcTarget.All);
            }
        }
        else
        {
            Lose();

        }
    }

    private void Lose()
    {
        StopCoroutine("StartTimer");
        photonView.RPC("ClearLists", RpcTarget.All);
        points = 0;
        StartCoroutine(LoseTimer());
    }
    IEnumerator LoseTimer()
    {
        print("LoseTimer");
        photonView.RPC("PaintAllPlatformsRed", RpcTarget.All);
        yield return new WaitForSeconds(2f);
        photonView.RPC("ClearAllPlatformsColors", RpcTarget.All);
        yield return new WaitForSeconds(2f);
        StartCoroutine("StartTimer");
    }
    [PunRPC]
    private void PaintAllPlatformsRed()
    {
        loseSound.Play();
        foreach (GameObject item in platforms)
        {
            item.GetComponent<Renderer>().material.color = Color.red;
        }
    }

    [PunRPC]

    private void PaintChoosenPlatforms(string color)
    {
        switch (color)
        {
            case "red":
                foreach (GameObject item in platformsToRed)
                {

                    item.GetComponent<Renderer>().material.color = Color.red;
                }
                break;
            case "yellow":
                foreach (GameObject item in platformsToYellow)
                {

                    item.GetComponent<Renderer>().material.color = Color.yellow;
                }
                break;
            case "green":
                foreach (GameObject item in platformsToGreen)
                {

                    item.GetComponent<Renderer>().material.color = Color.green;
                }
                break;
            default:
                break;
        }
        startCountSound.Play();

    }



}
