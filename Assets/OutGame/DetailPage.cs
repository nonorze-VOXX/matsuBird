using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetailPage : Scene_manerger
{
    private DetailView _detailView;
    private int currentType = 3;
    // Start is called before the first frame update
    public override void Start()
    {
        _detailView = GameObject.FindObjectOfType<DetailView>();
        
        _detailView.backButton.onClick.AddListener(() =>
        {
            OutGameManager.instance.GotoScene("Team");
        });
        
        currentType = OutGameManager.instance.GetCurrentDetail();
        _detailView.SetInfo(currentType);
    }

    // Update is called once per frame
    public override void Update()
    {
        
    }
}
