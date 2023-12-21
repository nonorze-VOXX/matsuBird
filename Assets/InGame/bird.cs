using UnityEngine;

namespace InGame
{
    public class bird : MonoBehaviour
    {
        private int aboveFire;
        private Vector2 aboveFireSpeed;
        private Collider2D collider2D;
        private Vector2 initSpeed;
        private Rigidbody2D rigidbody2D;

        private void Awake()
        {
            collider2D = GetComponent<Collider2D>();
            rigidbody2D = GetComponent<Rigidbody2D>();
            Debug.Log(rigidbody2D);
        }

        private void Update()
        {
            Debug.Log(aboveFire);
            if (aboveFire == 0)
                rigidbody2D.velocity = initSpeed;
            else
                rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x,aboveFireSpeed.y);
        }

        public void EnterFire()
        {
            aboveFire++;
        }

        public void ExitFire()
        {
            aboveFire--;
        }

        public void SetInitSpeed(Vector2 gameConfigBirdSpeed)
        {
            initSpeed = gameConfigBirdSpeed;
        }

        public void SetAboveFireSpeed(Vector2 gameConfigAboveFireSpeed)
        {
            aboveFireSpeed = gameConfigAboveFireSpeed;
        }
    }
}