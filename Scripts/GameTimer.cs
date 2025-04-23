using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public float totalTime = 5f;
    public Text timerText;
    public GameEnding gameEnding;

    private float currentTime;
    
    // Start is called before the first frame update
    void Start()
    {
        if (timerText == null)
        {
            Debug.LogError("Timer text is null.");
        }
        
        currentTime = totalTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        currentTime = Mathf.Max(0, currentTime);

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime % 60f);
        timerText.text = $"{minutes:00}:{seconds:00}";

        if (currentTime <= 0f)
        {
            gameEnding.CaughtPlayer();
            enabled = false;
        }
    }
}