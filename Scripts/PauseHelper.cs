using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHelper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pause()
    {
        CheckPointManager.instance.isPaused = true;
        Debug.LogError("Pause");
    }

    public void Unpause()
    {
        CheckPointManager.instance.isPaused = false;
        Time.timeScale = 1;
    }
}
