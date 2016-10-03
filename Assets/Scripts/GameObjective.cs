using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameObjective : MonoBehaviour {

    public List<FinishLine> FinishLines;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        var i = 0;
        FinishLines.ForEach(x => Debug.Log(i++ + " " + x.passed));
        if (FinishLines.All(x => x.passed))
        {
            Debug.Log(1);
            SceneManager.LoadScene("End");
        }
	}
}
