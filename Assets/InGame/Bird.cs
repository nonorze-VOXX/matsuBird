using System;
using UnityEngine;
using UnityEngine.Events;

namespace InGame
{
    internal enum BirdState
    {
        Fly,
        Land,
        Stop,
        Die
    }

    public class Bird : MonoBehaviour
    {
        public BirdPictureObject birdPictureObject;
        private readonly UnityEvent<GameObject> getFood = new();
        private int aboveFire;
        private Vector2 aboveFireSpeed;
        private Collider2D collider2D;
        private float hp;
        private Vector2 initSpeed;

        private Rigidbody2D rigidbody2D;
        private SpriteRenderer spriteRenderer;
        private BirdState state = BirdState.Fly;
        private float timer;
        public GameConfig gameConfig { get; set; }

        private void Awake()
        {
            collider2D = GetComponent<Collider2D>();
            rigidbody2D = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            SetState(BirdState.Stop);
        }

        private void Start()
        {
            UpdateBirdSprite();
        }

        private void Update()
        {
            switch (state)
            {
                case BirdState.Stop:
                    break;

                case BirdState.Fly:
                    hp -= Time.deltaTime;

                    if (aboveFire == 0)
                        rigidbody2D.velocity = initSpeed;
                    else
                        rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, aboveFireSpeed.y);
                    if (hp <= 0) SetState(BirdState.Die);
                    break;
                case BirdState.Land:
                    var pos = transform.position;
                    timer += Time.deltaTime;
                    pos.y = +Mathf.Lerp(gameConfig.groundHeight + collider2D.bounds.extents.y,
                        (gameConfig.birdEnterPosition.y - gameConfig.groundHeight) / 2 + gameConfig.groundHeight,
                        timer / gameConfig.flyToSkyTime);
                    transform.position = pos;
                    if (timer >= gameConfig.flyToSkyTime) SetState(BirdState.Fly);
                    break;
                case BirdState.Die:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.transform.CompareTag("ground") && state != BirdState.Die)
                SetState(BirdState.Land);
            if (other.transform.CompareTag("food") && state != BirdState.Die)
            {
                hp += gameConfig.foodHp;
                getFood.Invoke(other.gameObject);
            }
        }

        private void UpdateBirdSprite()
        {
            var birdPicture = (int)gameConfig.birdType * 2 + (int)gameConfig.birdPictureState;
            Debug.Log(birdPicture);
            spriteRenderer.sprite = birdPictureObject.birdPicture[birdPicture];
        }

        public void AddFoodListener(UnityAction<GameObject> call)
        {
            getFood.AddListener(call);
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
                    gameConfig.birdPictureState = BirdPictureState.fly;
                    UpdateBirdSprite();
                    break;
                case BirdState.Land:
                    rigidbody2D.velocity = Vector2.zero;
                    hp -= gameConfig.flyToSkyHp;
                    timer = 0;
                    gameConfig.birdPictureState = BirdPictureState.stand;
                    UpdateBirdSprite();
                    break;
                case BirdState.Stop:
                    rigidbody2D.velocity = Vector2.zero;
                    break;
                case BirdState.Die:
                    rigidbody2D.velocity = Vector2.down;
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