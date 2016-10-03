using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class TimeText : MonoBehaviour
    {
        public Text timerLabel;

        // Update is called once per frame
        void Update()
        {
            Timer.Time += Time.deltaTime;

            //update the label value
            timerLabel.text = Timer.FormattedTime;

            if (Input.GetAxis("Cancel") > 0)
            {
                Application.Quit();
            }
        }
    }

}
