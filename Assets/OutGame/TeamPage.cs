using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeamPage : Scene_manerger
{
    // Start is called before the first frame update
    private TeamView _teamView;
    private int currentSelectTeamMember = 1;
    private List<Sprite> _teamBird = new List<Sprite>();

    public override void Start()
    {
        _teamView = GameObject.FindObjectOfType<TeamView>();
        
        _teamView.backButton.onClick.AddListener(BackMain);
        _teamView.team_1.GetComponent<Button>().onClick.AddListener(delegate { ChangeTeam(_teamView.team_1, 1); });
        _teamView.team_2.GetComponent<Button>().onClick.AddListener(delegate { ChangeTeam(_teamView.team_2, 2); });
        _teamView.team_3.GetComponent<Button>().onClick.AddListener(delegate { ChangeTeam(_teamView.team_3, 3); });

        
        _teamBird = OutGameManager.instance.GetTeamBird();

        _teamView._leftB.onClick.AddListener(PagoSubOne);
        _teamView._rightB.onClick.AddListener(PagoAddOne);
        
        _teamView.CreateOnePage();
    }

    // Update is called once per frame
    public override void Update()
    {
        _teamView.team_1.transform.Find("Image").gameObject.transform.Find("bird").gameObject.GetComponent<Image>().sprite = _teamBird[0];
        _teamView.team_2.transform.Find("Image").gameObject.transform.Find("bird").gameObject.GetComponent<Image>().sprite = _teamBird[1];
        _teamView.team_3.transform.Find("Image").gameObject.transform.Find("bird").gameObject.GetComponent<Image>().sprite = _teamBird[2];
        
        foreach (var birdButton in _teamView.GetBirdList())
        {
            if (birdButton != null)
            {
                birdButton.transform.Find("Button").GetComponent<ButtonClickLongShort>().SetCurrentTeamSelect(currentSelectTeamMember);
            }
        }

        if (_teamView.GetCurrentIndex() == 0)
        {
            _teamView.SetLeft(false);
        }
        else
        {
            _teamView.SetLeft(true);
        }
        
        if (_teamView.GetBirdPageNumber()-1 == _teamView.GetCurrentIndex())
        {
            _teamView.SetRight(false);
        }
        else
        {
            _teamView.SetRight(true);
        }
    }

    private void BackMain()
    {
        OutGameManager.instance.GotoScene("Main");
    }

    private void ChangeTeam(GameObject target, int index)
    {
        _teamView.team_1.GetComponent<Outline>().enabled = false;
        _teamView.team_2.GetComponent<Outline>().enabled = false;
        _teamView.team_3.GetComponent<Outline>().enabled = false;
        
        target.GetComponent<Outline>().enabled = true;

        currentSelectTeamMember = index;
    }

    void PagoAddOne()
    {
        _teamView.gotoPage(_teamView.GetCurrentIndex()+1);
    }
    void PagoSubOne()
    {
        _teamView.gotoPage(_teamView.GetCurrentIndex()-1);
    }
    
    
}
