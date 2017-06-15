using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using System;

public class CharacterController : MonoBehaviour {
    //actually is static but it needs not to be static so yeah
    public List<Sprite> sprites;


    //Constants
    public float shootDelay = 1.5f; //in seconds
    public float jumpIntensifier = 7f; //This can't be 5 or less else it just bugs and dies.
	public int lives = 3;


    public float MaxSpeed = 5f; //This is the maximum speed that the object will achieve
    public float acceleration = 7f; //How fast will object reach a maximum speed

	public GameObject feet;

    private KeyCode left, right, jump, shoot;
    //object information
    private Direction dir = Direction.EAST;
    private float speed = 0f; //object current speed
    private bool jumping;
    private bool alive;
    private Animator anim;


    //HOOKS
    private Text livesText;


    //Game information
    private int score;
    private int kills;

    private string item;


    private Rigidbody2D rb;
    private GameManager gm;

    // Use this for initialization
    void Start () {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        gm = FindObjectOfType<GameManager>();
       // livesText = FindObjectOfType<Text>();
        //livesText.text = "Lives: " + lives;
        alive = true;
        if (jumpIntensifier < 7) {
            jumpIntensifier = 7;
        }
	}

    internal void setKeys(PlayerManager pm) {
        this.right = pm.Right;
        this.left = pm.Left;
        this.jump = pm.Jump;
        this.shoot = pm.Shoot;
    }

    // Update is called once per frame
    void Update () {
        Move();
        if (Input.GetKeyUp(shoot)) {
            BoomTriggerScript bt = GetComponent<BoomTriggerScript>();
            switch (direction)
            {
                case Direction.EAST:
                    bt.dir = Vector3.right;
                    break;
                case Direction.NORTH:
                    bt.dir = Vector3.up;
                    break;
                case Direction.WEST:
                    bt.dir = Vector3.left;
                    break;
                default:
                    bt.dir = Vector3.right;
                    break;
            }

            Debug.Log("3"+bt.dir);
            bt.OnTrigger();
        }
    }

    void Move() {
        float x = 0;
        if (Input.GetKey(right)) {
            x += 1;
        }
        if (Input.GetKey(left)) {
            x -= 1;
        }
        //movement acceleration until max speed.
        if (x < 0 && speed > -MaxSpeed) {
			if(direction!=Direction.WEST){
                gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x,gameObject.transform.localScale.y,gameObject.transform.localScale.z);
                transform.position -= new Vector3(0.5f, 0, 0);
			}
            speed -= (acceleration * Time.deltaTime);
            anim.SetFloat("speedx", Mathf.Abs(x));
            direction = Direction.WEST;
			
        } else if (x > 0 && speed < MaxSpeed) {
			if(direction!=Direction.EAST){
				gameObject.transform.localScale = new Vector3(-gameObject.transform.localScale.x,gameObject.transform.localScale.y,gameObject.transform.localScale.z);
                transform.position += new Vector3(0.5f, 0, 0);
            }
            speed += (acceleration * Time.deltaTime);
            direction = Direction.EAST;
            anim.SetFloat("speedx", Mathf.Abs(x));
        } /*else if (Input.GetKey(KeyCode.W))
        {
            direction = Direction.NORTH;
        }*/ else {
            if (speed > acceleration*Time.deltaTime) {
                speed -= acceleration * Time.deltaTime;
            } else if (speed < -acceleration * Time.deltaTime) {
                speed += acceleration * Time.deltaTime;
            } else {
                speed = 0;
                anim.SetFloat("speedx", 0);
            }
        }
        gameObject.transform.position +=new Vector3(speed * Time.deltaTime,0,0);

		if (jumping && Physics2D.Linecast (feet.transform.position, feet.transform.position) &&
            GetComponent<Rigidbody2D>().velocity.y == 0) {
			jumping = false;
            anim.SetBool("InGrounded", !jumping);
        }

        //Jumping 
        float y = 0;
        if (Input.GetKeyDown(jump)) {
            y = 1;
        }
        if (y > 0 && !jumping) {
            Jump(y);
        }
    }

    void Jump(float yAxis) {
        if (!jumping) { //you can only jump if you're not jumping.
                jumping = true;
				AudioSource audio = GameObject.FindGameObjectWithTag("jumpfx").GetComponent<AudioSource>();
				audio.Play();
                anim.SetBool("InGrounded", !jumping);
				//audio.Play(44100);
                Vector2 force = new Vector2(0, yAxis) * jumpIntensifier;
                rb.AddForce(force, ForceMode2D.Impulse); //we jump here.
        }
    }
    
    void OnCollisionEnter2D(Collision2D coll) {
        switch (coll.gameObject.tag) {
            case "Trap":
                Die();
                break;
            case "Crusher":
                Crusher c = coll.gameObject.GetComponentInParent<Crusher>();
                if (c.Moving) {
					AudioSource audio = GameObject.FindGameObjectWithTag("crushfx").GetComponent<AudioSource>();
					audio.Play();
					//audio.Play(44100);
                    Die();
                }
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
		switch (coll.gameObject.tag) {
			case "Teleport":
				Teleporter tp = coll.gameObject.GetComponent<Teleporter> ();
				tp.Teleport (gameObject);
				break;
			case "Item":
                //This is handled in ItemScript
				break;
			case "SawBlade":
				AudioSource audio = GameObject.FindGameObjectWithTag("elecfx").GetComponent<AudioSource>();
				audio.Play();
				//audio.Play(44100);
				Die ();
				break;
			case "Bullet":
				ProjectileScript projectile = coll.gameObject.GetComponentInParent<ProjectileScript> ();
				if (projectile.parent != gameObject) {
                    Debug.Log("parent");
					if (projectile.canKill) {
						Die ();
					} else {
						gameObject.GetComponent<Rigidbody2D>().velocity += coll.gameObject.GetComponent<Rigidbody2D> ().velocity;
					}
					Destroy (coll.gameObject);
				}
				break;
		}
    }

    //Lose a life, if lives = 0, you're out!
    void Die() {
		AudioSource audio = GameObject.FindGameObjectWithTag("deathfx").GetComponent<AudioSource>();
		audio.Play();
		//audio.Play(44100);
        if (lives > 0) {
            lives -= 1;
			item = "";
			speed = 0f;
			transform.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            //livesText.text = "Lives: " + lives;
            //TODO UPDATE LIVES
            Respawn();
        } else {
            //out of luck bro, out of the round!
            gameObject.transform.position = new Vector3(100, 100, 100);
            //livesText.text = "Game Over";
            alive = false;
            //SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
            //sr.enabled = false;
            gm.CheckWin();
        }
    }

    void Respawn() {
        GameObject[] respawnObjects = GameObject.FindGameObjectsWithTag("Respawn");
        int x = UnityEngine.Random.Range(0, respawnObjects.Length);
        GameObject go = respawnObjects[x];
        gameObject.transform.position = go.transform.position;
    }

    public Direction direction {
        get {
            return dir;
        }

        set {
            dir = value;
        }
    }

    internal string Item {
        get {
            return item;
        }

        set {
            item = value;
        }
    }

    public bool Alive {
        get {
            return alive;
        }

        set {
            alive = value;
        }
    }

    public KeyCode Left {
        get {
            return left;
        }

        set {
            left = value;
        }
    }

    public KeyCode Right {
        get {
            return right;
        }

        set {
            right = value;
        }
    }

    public KeyCode Jump1 {
        get {
            return jump;
        }

        set {
            jump = value;
        }
    }

    public KeyCode Shoot {
        get {
            return shoot;
        }

        set {
            shoot = value;
        }
    }
}

public enum Direction {
    NORTH,
    EAST,
    WEST,
    SOUTH
}