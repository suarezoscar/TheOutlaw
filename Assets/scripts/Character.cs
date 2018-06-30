using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Character : MonoBehaviour {

    [SerializeField]
    protected Transform bulletPos;

    [SerializeField]
    public GameObject[] Bullets;

    public int BulletsNumber { get; set; }
    public bool NoAmmo { get; set; }

    [SerializeField]
    protected float movementSpeed;

    protected bool facingRight;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    protected int health;

    [SerializeField]
    private EdgeCollider2D meleeCollider;

    [SerializeField]
    private List<string> damageSource;

    public abstract bool IsDead { get; }

    public bool Attack { get; set; }

    public bool Reload { get; set; }

    public bool TakingDamage { get; set; }

    public Animator MyAnimator { get; private set; }

    public EdgeCollider2D MeleeCollider
    {
        get
        {
            return meleeCollider;
        }
    }

    // Use this for initialization
    public virtual void Start ()
    {
        facingRight = true;

        MyAnimator = GetComponent<Animator>();

        //BulletsNumber = 5;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
    
    public abstract IEnumerator TakeDamage();

    public abstract void Death();
    
    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    public virtual void Shoot(int value)
    {
        if (BulletsNumber < 0)
        {
            NoAmmo = true;
        }
        else
        {
            NoAmmo = false;

            if (facingRight)
            {
                GameObject tmp = (GameObject)Instantiate(bulletPrefab, bulletPos.position, Quaternion.identity);
                tmp.GetComponent<Bullet>().Initialize(Vector2.right);
                Bullets[BulletsNumber].SetActive(false);
                BulletsNumber--;
            }
            else
            {
                GameObject tmp = (GameObject)Instantiate(bulletPrefab, bulletPos.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                tmp.GetComponent<Bullet>().Initialize(Vector2.left);
                Bullets[BulletsNumber].SetActive(false);
                BulletsNumber--;
            }
        }
    }

    public void MeleeAttack()
    {
        MeleeCollider.enabled = true;
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (damageSource.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }
}
