using System;
using UnityEngine;
using TMPro;

public class Level1Manager : MonoBehaviour
{
    public TextMeshProUGUI messageToPlayer;
    private const int BOSS_FIGHT = 3;
    private int currentWave = 1;
    public GameObject wave1Object;
    public GameObject wave2Object;
    public GameObject bossFightMark;
    public GameObject bossFightObject;
    public GameObject bossHUD;
    private bool wave1MinionsKilled = false;
    private bool wave2MinionsKilled = false;
    private bool wave2Spawned = false;
    private bool bossFightSpawned = false;
    private bool bossKilled = false;

    public void SetWaveCompletion(int waveNumber)
    {
        switch(waveNumber)
        {
            case 1:
                wave1MinionsKilled = true;
                break;
            case 2:
                wave2MinionsKilled = true;
                break;
            case BOSS_FIGHT:
                bossKilled = true;
                break;
        }
    }
    private void SpawnWave()
    {
        switch (currentWave)
        {
            case 1:
                wave1Object.SetActive(true);
                break;
            case 2:
                wave2Object.SetActive(true);
                break;
            case BOSS_FIGHT:
                bossFightMark.SetActive(false);
                bossFightObject.SetActive(true);
                bossHUD.SetActive(true);
                break;
            default:
                break;
        }
    }
    private void SetMessageToPlayer(String msg)
    {
        messageToPlayer.text = msg;
    }
    void Start()
    {
        SetMessageToPlayer("Something seems to be coming from inside the forest! Get ready for combat!");
        Invoke("SpawnWave",3f);
    }

    // Update is called once per frame
    void Update()
    {

        int minionKills = GlobalVariables.GetMinionsKil(1);
        
        if(minionKills==3)
        {
            wave1MinionsKilled = true;
        }
        else if(minionKills==9)
        {
            wave2MinionsKilled = true;
        }

        if(GlobalVariables.boss1Killed)
        {
            bossKilled = true;
        }

        if(wave1MinionsKilled && !wave2Spawned)
        {
            currentWave=2;
            SetMessageToPlayer("More enemies seem to be coming from the forest!");
            Invoke("SpawnWave",3f);
            wave2Spawned = true;
        }
        if(wave2MinionsKilled && !bossFightSpawned)
        {
            currentWave=BOSS_FIGHT;
            SetMessageToPlayer("An intense fire seems to be burning the area, a great evil is approaching.");
            bossFightMark.SetActive(true);
            Invoke("SpawnWave",3f);
            bossFightSpawned = true;
        }
        if(bossKilled)
        {
            bossHUD.SetActive(false);
            SetMessageToPlayer("Congratulations, Hero, you managed to destroy all the damn enemies!");
        }
    }
}
