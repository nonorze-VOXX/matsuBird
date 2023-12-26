using System;
using UnityEngine;

namespace InGame
{
    internal enum BirdState
    {
        Fly,
        Land,
        Stop
    }

    public class Bird : MonoBehaviour
    {
        private int aboveFire;
        private Vector2 aboveFireSpeed;
        private Collider2D collider2D;
        private float hp;
        private Vector2 initSpeed;

        private Rigidbody2D rigidbody2D;
        private BirdState state = BirdState.Fly;
        private float timer;
        public GameConfig gameConfig { get; set; }

        private void Awake()
        {
            collider2D = GetComponent<Collider2D>();
            rigidbody2D = GetComponent<Rigidbody2D>();
            SetState(BirdState.Stop);
        }

        private void Start()
        {
        }

        private void Update()
        {
            switch (state)
            {
                case BirdState.Stop:
                    break;

                case BirdState.Fly:
                    hp -= Time.deltaTime;
                    if (hp <= 0)
                    {
                        //TODO die
                    }

                    if (aboveFire == 0)
                        rigidbody2D.velocity = initSpeed;
                    else
                        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, aboveFireSpeed.y);
                    break;
                case BirdState.Land:
                    var pos = transform.position;
                    timer += Time.deltaTime;
                    pos.y = +Mathf.Lerp(gameConfig.groundHeight + collider2D.bounds.extents.y,
                        gameConfig.birdEnterPosition.y, timer / gameConfig.flyToSkyTime);
                    transform.position = pos;
                    if (timer >= gameConfig.flyToSkyTime) SetState(BirdState.Fly);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.transform.CompareTag("ground"))
                SetState(BirdState.Land);
        }

        public void SetHp(float ghp)
        {
            hp = ghp;
        }

        private void SetState(BirdState state)
        {
            switch (state)
            {
                case BirdState.Fly:
                    rigidbody2D.velocity = initSpeed;
                    break;
                case BirdState.Land:
                    rigidbody2D.velocity = Vector2.zero;
                    hp -= gameConfig.flyToSkyHp;
                    timer = 0;
                    break;
                case BirdState.Stop:
                    rigidbody2D.velocity = Vector2.zero;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }

            this.state = state;
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

        public void Stop()
        {
            SetState(BirdState.Stop);
        }

        public void Fly()
        {
            SetState(BirdState.Fly);
        }

        public float GetHp()
        {
            return hp;
        }
    }
}