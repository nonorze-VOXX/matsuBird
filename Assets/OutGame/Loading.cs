using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;    

using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{    

    private int range = 770;
    public Text run_number;
    public GameObject bird;
    public AudioClip announce;

   // Start is called before the first frame update
   private void Awake()
   {
       if (run_number == null || bird == null)
       {
           Debug.LogError("public null");
           this.enabled = false;
       }
       OutGameManager.instance.audioSourceSE.PlayOneShot(announce);
   }

   // Update is called once per frame
    void Update(){ 
        run_number.text = (int)((bird.transform.GetComponent<RectTransform>().localPosition.x + 440) / range * 100) + " %";
        if (run_number.text == "99 %" || run_number.text == "100 %")
        {
            OutGameManager.instance.GotoScene("Main");
        }
    }
}
