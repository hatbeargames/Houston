using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopScoreTracker : MonoBehaviour
{
    [SerializeField] GameManager gm;
    public static TopScoreTracker Instance;
    [SerializeField]private TMP_Text topScore;
    private int topScoreValue;
    // Start is called before the first frame update

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else 
        { 
            Destroy(gameObject);
        }
    }
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        topScoreValue = (int)gm.GetGoal();
    }

    // Update is called once per frame
    void Update()
    {
        topScore.text = "Top Score: " + topScoreValue;
    }
    public int GetTopScore()
    {
        return topScoreValue;
    }
    public void SetTopScore(int value)
    {
        if (value < topScoreValue)
        {
            topScoreValue = value;
        }
    }
}
