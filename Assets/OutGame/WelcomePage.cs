using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class WelcomePage : MonoBehaviour
    {
        public Button kokuchi;
        public TMP_Text welcomeText;
        public AudioClip audioClip;

        private void Awake()
        {
            kokuchi.onClick.AddListener(GotoMain);
        }

        private void Start()
        {
            welcomeText.text = "Welcome to the game!";
            OutGameManager.instance.audioSourceBGM.clip = audioClip;
            OutGameManager.instance.audioSourceBGM.Play();
        }

        private void Update()
        {
            float tmp = welcomeText.color.a;
            tmp =0.5f * Mathf.Sin( 2.0f*Time.time)+0.5f;
            welcomeText.color = new Color(
                welcomeText.color.r,
                welcomeText.color.g,
                welcomeText.color.b,
                tmp);
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OutGameManager.instance.StartGame();
            }
        }

        private void GotoMain()
        {
            OutGameManager.instance.audioSourceBGM.Stop();
            OutGameManager.instance.GotoScene("loading");
        }
    }
    
}