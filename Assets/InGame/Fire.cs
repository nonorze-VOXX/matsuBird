using UnityEngine;

namespace InGame
{
    public class Fire : MonoBehaviour
    {
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log(other.name);
            if (other.CompareTag("bird")) other.GetComponent<bird>().EnterFire();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("bird")) other.GetComponent<bird>().ExitFire();
            ;
        }
    }
}