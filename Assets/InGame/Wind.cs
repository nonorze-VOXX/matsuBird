using UnityEngine;

namespace InGame
{
    public class Wind : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("bird")) other.GetComponent<Bird>().EnterFire();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("bird")) other.GetComponent<Bird>().ExitFire();
        }
    }
}