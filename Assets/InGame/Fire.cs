using UnityEngine;

public class Fire : MonoBehaviour
{
    private bool inWall=false;

    public void Reset()
    {
        inWall = false;
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (!inWall && other.CompareTag("Wall")) inWall = true;
    }
}