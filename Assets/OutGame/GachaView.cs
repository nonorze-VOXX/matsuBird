using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class GachaView : MonoBehaviour
{
    public Image pan, bird;
    public ParticleSystem panKasu;
    public Button backButton;
    public Button kami;
    public Animation panClick;
    public Animation birdCome;
    public Sprite[] tostoList;
    
    // Start is called before the first frame update
    void Start()
    {
        panKasu.Stop();
        panClick.Stop();
        birdCome.Stop();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnDestroy()
    {
        kami.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();
    }
}
