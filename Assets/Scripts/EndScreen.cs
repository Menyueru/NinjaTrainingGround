using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour {

    public Text timerLabel;
	
	// Update is called once per frame
	void Update () {
        timerLabel.text = Timer.FormattedTime;
        if (Input.GetAxis("Submit") > 0)
        {
            Application.Quit();
        }
        if (Input.GetAxis("Cancel")>0)
        {
            Application.Quit();
        }
    }
}
