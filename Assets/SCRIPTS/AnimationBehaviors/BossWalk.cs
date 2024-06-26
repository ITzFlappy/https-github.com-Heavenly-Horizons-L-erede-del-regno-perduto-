using UnityEngine;

public class BossWalk : StateMachineBehaviour
{
    public float speed = 2.5f;
    public float attackRange;
    private Boss boss;
    private Transform bossTransform;
    private BossWeapon bossWeapon;
    private Transform player;
    private Rigidbody2D rb;
    private Vector2 target;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
        bossWeapon = animator.GetComponent<BossWeapon>();
        attackRange = bossWeapon.attackRange * 2.5f;
        target = new Vector2(player.position.x, -30);
        bossTransform = animator.GetComponent<Transform>();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");

        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().isPlayerDead)
        {
            animator.SetBool("PlayerDead", true);
            rb.velocity = Vector3.zero;
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }
        else
        {
            animator.SetBool("PlayerDead", false);
            rb.velocity = Vector3.one;
            rb.constraints = RigidbodyConstraints2D.None;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.LookAtPlayer();

        // Blocca la rotazione sull'asse Z impostando la rotazione iniziale
        var eulerRotation = bossTransform.rotation.eulerAngles;
        eulerRotation.z = 0;
        bossTransform.rotation = Quaternion.Euler(eulerRotation);

        target = new Vector2(player.position.x, -30);
        var newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        if (Vector2.Distance(player.position, rb.position) <= attackRange + 1 &&
            !GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>().isPlayerDead)
            animator.SetTrigger("Attack");
    }
}