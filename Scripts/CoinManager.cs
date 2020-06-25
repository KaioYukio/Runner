using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;

    public int coin;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        coin = CheckPointManager.instance.coinsSaved;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCoin()
    {
        coin++;
    }
}
