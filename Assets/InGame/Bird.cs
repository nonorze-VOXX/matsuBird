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
        Die,
        Win,
        Drop
    }

    public class Bird : MonoBehaviour
    {
        public BirdPictureObject birdPictureObject;
        private readonly UnityEvent<GameObject> getFood = new();
        private int aboveFire;
        private Vector2 aboveFireSpeed;
        private Collider2D collider2D;
        private UnityAction gameover;
        private UnityAction gameWin;

        private bool higher;
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
                {
                    var pos = transform.position;
                    timer += Time.deltaTime;
                    pos.y = +Mathf.Lerp(gameConfig.groundHeight + collider2D.bounds.extents.y,
                        (gameConfig.birdEnterPosition.y - gameConfig.groundHeight) / 2 + gameConfig.groundHeight,
                        timer / gameConfig.flyToSkyTime);
                    transform.position = pos;
                    if (timer >= gameConfig.flyToSkyTime) SetState(BirdState.Fly);
                }
                    break;
                case BirdState.Win:
                    break;
                case BirdState.Die:
                    break;
                case BirdState.Drop:
                {
                    timer += Time.deltaTime;
                    if (timer >= gameConfig.DropTime) Destroy(gameObject);
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (state == BirdState.Die)
            {
                SetState(BirdState.Drop);
            }
            else if (other.transform.CompareTag("ground") && state != BirdState.Die)
            {
                higher = false;
                SetState(BirdState.Land);
            }
            else if (other.transform.CompareTag("food") && state != BirdState.Die)
            {
                hp = Mathf.Min(hp + gameConfig.foodHp, gameConfig.initHp);
                getFood.Invoke(other.gameObject);
                higher = true;
                SetState(BirdState.Land);
            }
            else if (other.transform.CompareTag("mapBird") && state != BirdState.Die)
            {
                SetState(BirdState.Win);
            }
        }

        public void AddGameWinListener(UnityAction call)
        {
            gameWin += call;
        }

        public void GameWin()
        {
            gameWin.Invoke();
        }

        public void AddGameOverListener(UnityAction call)
        {
            gameover += call;
        }

        public void GameOver()
        {
            Debug.Log(transform.name + "bird invole gameover");
            gameover.Invoke();
        }

        private void UpdateBirdSprite()
        {
            var birdPicture = (int)gameConfig.birdType * 2 + (int)gameConfig.birdPictureState;
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
                    if (!higher) hp -= gameConfig.flyToSkyHp;
                    timer = 0;
                    gameConfig.birdPictureState = BirdPictureState.stand;
                    UpdateBirdSprite();
                    break;
                case BirdState.Stop:
                    rigidbody2D.velocity = Vector2.zero;
                    break;
                case BirdState.Die:
                    rigidbody2D.velocity = Vector2.down;
                    GameOver();
                    break;
                case BirdState.Win:
                    rigidbody2D.velocity = Vector2.down;
                    GameWin();
                    break;
                case BirdState.Drop:
                    timer = 0;
                    collider2D.isTrigger = true;
                    rigidbody2D.velocity = new Vector2(-10, 10);
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

        public Sprite GetSprite()
        {
            return spriteRenderer.sprite;
        }

        public void Drop()
        {
            SetState(BirdState.Drop);
        }
    }
}