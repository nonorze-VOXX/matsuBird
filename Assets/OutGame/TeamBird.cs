using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamBird : MonoBehaviour
{
    private string _birdPath = "none";
    private int _birdType = 0;
    // Start is called before the first frame update
    public TeamBird(string path, int type)
    {
        _birdPath = path;
        _birdType = type;
    }

    public int GetBirdType()
    {
        return _birdType;
    }

    public string GetPath()
    {
        return _birdPath;
    }

    public void SetPath(string path)
    {
        _birdPath = path;
    }

    public void SetType(int type)
    {
        _birdType = type;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
