using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Grounded1 = Animator.StringToHash("Grounded");
    private static readonly int Jump1 = Animator.StringToHash("Jump");
    [SerializeField] private float speed = 25f; // Campo serializzabile per impostare la velocità da Unity
    [SerializeField] private float jumpForce = 5f; // Forza del salto

    public float KBForce = 40f;
    public float KBCounter;
    public float KBTotalTime = 0.2f;

    public bool KnockFromRight;
    public Testa_Tyr testaTyr;
    private Animator anim;
    private Rigidbody2D body;
    private bool canMove = true;

    private bool Grounded;

    private float horizontalInput;

    void Start() {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        testaTyr = GameObject.FindGameObjectWithTag("Testa_Tyr")?.GetComponent<Testa_Tyr>();

        if (body == null) {
            Debug.LogError("Rigidbody2D component is missing on " + gameObject.name);
        }
        else {
            body.constraints = RigidbodyConstraints2D.FreezeRotation; // Blocca la rotazione sull'asse Z
            body.isKinematic = false;
        }
    }

    void Update() {
        AnimatorStateInfo animatorStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (canMove) {
            // Ottieni l'input orizzontale in Update
            horizontalInput = Input.GetAxis("Horizontal"); // Correzione del nome della variabile
            if (KBCounter <= 0)
                // Imposta la velocità del rigidbody
            {
                body.velocity = new(horizontalInput * speed, body.velocity.y);
            }
            else if (testaTyr != null && !testaTyr.tyrHead) {
                if (KnockFromRight) body.velocity = new(-KBForce, KBForce / 3);
                else body.velocity = new(KBForce, KBForce / 3);

                KBCounter -= Time.deltaTime;
            }
            else if (testaTyr != null && testaTyr.tyrHead) {
                if (KnockFromRight) body.velocity = new(-KBForce * 3, KBForce / 4);
                else body.velocity = new(KBForce * 3, KBForce / 4);

                KBCounter -= Time.deltaTime;
            }
            else {
                if (animatorStateInfo.IsName("hurt") &&
                    animatorStateInfo.normalizedTime > 0) {
                    if (KnockFromRight) body.velocity = new(-KBForce / 2, body.velocity.y);
                    else body.velocity = new(KBForce / 2, body.velocity.y);
                }
                else {
                    KBCounter -= Time.deltaTime;
                }
            }


            // Serve per far girare il personaggio da una parte all'altra
            if (horizontalInput > 0.01f)
                transform.localScale = new(2f, 2f, 2f);
            else if (horizontalInput < -0.01f) transform.localScale = new(-2f, 2f, 2f);

            // Logica per il salto
            if (Input.GetKey(KeyCode.Space) && Grounded) Jump();

            anim.SetBool(Run, horizontalInput != 0);
            anim.SetBool(Grounded1, Grounded);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("terra")) Grounded = true;

        // Gestione del knockback dalla freccia
        if (collision.gameObject.CompareTag("Freccia")) {
            // Calcola la posizione della freccia rispetto al giocatore
            Vector3 relativePosition = collision.transform.position - transform.position;

            // Determina la direzione del knockback in base alla posizione della freccia
            KnockFromRight =
                relativePosition.x > 0; // Se la freccia è alla destra del giocatore, KnockFromRight sarà true

            // Applica il knockback e altri effetti desiderati
            KBCounter = KBTotalTime;
        }
    }

    private void Jump() {
        body.velocity = new(body.velocity.x, jumpForce);
        anim.SetTrigger(Jump1);
        Grounded = false;
    }

    public void CanMove() {
        canMove = true;
    }

    public void CanNotMove() {
        canMove = false;
    }

    public bool CanAttack() {
        return Grounded;
    }

     public void StopMovement() {
        body.velocity = Vector2.zero;
    }
}
