using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameObjective : MonoBehaviour
{

    public List<FinishLine> FinishLines;
    public Text ScoreText;
    private int length;

    // Use this for initialization
    void Start()
    {
        length = FinishLines.Count();
    }

    // Update is called once per frame
    void Update()
    {
        var count = FinishLines.Count(x => x.passed);
        ScoreText.text = count.ToString();
        if (count >= length)
        {
            SceneManager.LoadScene("End");
        }
    }
}
