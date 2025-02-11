using System.Collections.Generic;
using UnityEngine;

public class Bullet02Pool : MonoBehaviour
{
    public static Bullet02Pool SharedInstance;
    public List<GameObject> bulletsList;
    public GameObject bullet;
    public int poolSize = 10;

    public GameObject GetBullet()
    {
        for(int i = 0; i < poolSize; i++)
        {
            if(!bulletsList[i].activeInHierarchy)
            {
                return bulletsList[i];
            }
        }
        return null;
    }

    void Awake()
    {
        SharedInstance = this;
    }

    void Start()
    {
        bulletsList = new List<GameObject>();
        GameObject tmp;
        for(int i = 0; i < poolSize; i++)
        {
            tmp = Instantiate(bullet);
            tmp.SetActive(false);
            bulletsList.Add(tmp);
        }
    }
}
