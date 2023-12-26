using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class WelcomePage : MonoBehaviour
    {
        public TMP_Text welcomeText;

        private void Start()
        {
            welcomeText.text = "Welcome to the game!";
        }

        private void Update()
        {
            var tmp = welcomeText.color.a;
            tmp = 0.5f * Mathf.Sin(2.0f * Time.time) + 0.5f;
            welcomeText.color = new Color(
                welcomeText.color.r,
                welcomeText.color.g,
                welcomeText.color.b,
                tmp);

            if (Input.GetKeyDown(KeyCode.Space)) GameManager.instance.UpdateFSM();
        }
    }
}