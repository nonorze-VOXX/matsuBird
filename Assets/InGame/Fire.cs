using UnityEngine;

public class Fire : MonoBehaviour
{
    private bool inWall;
    private bool used;

    public void Reset()
    {
        inWall = false;
        used = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (other.CompareTag("Wall")) inWall = true;
    }

    public bool GetUsed()
    {
        return used;
    }

    public void SetUesd(bool used)

    {
        this.used = used;
    }

    public bool GetInWall()
    {
        return inWall;
    }
}