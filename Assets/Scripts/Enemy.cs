using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    private GunSystem gunSystem;
    private EnemySpawner enemySpawner;
    private GameObject Bullets;
    private AudioSource audioSource;
    public int health;
    public int gold = 5;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private float lastAttackTime;

    private void Awake()
    {
        Bullets = GameObject.FindGameObjectWithTag("BulletsHolder");
        enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").gameObject.GetComponent<EnemySpawner>();
        gunSystem = GameObject.FindGameObjectWithTag("GunSystem").gameObject.GetComponent<GunSystem>();
        player = GameObject.Find("PlayerCapsule").transform;
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        // Make sure the enemy doesn't move
        agent.SetDestination(transform.position);

        // Calculate the direction to the player, including the vertical component
        Vector3 playerDirection = (player.position - transform.position).normalized;

        // Rotate towards the player in all directions
        transform.rotation = Quaternion.LookRotation(playerDirection);

        if (!alreadyAttacked)
        {
            // Yukarýya kaydýrýlmýþ ateþleme noktasý
            Vector3 firePoint = transform.position + playerDirection + Vector3.up;

            // Attack code here
            GameObject go = Instantiate(projectile, firePoint, Quaternion.identity, Bullets.transform);
            audioSource.Play();
            go.GetComponent<Rigidbody>().AddForce(playerDirection * 50f, ForceMode.Impulse);

            alreadyAttacked = true;
            lastAttackTime = Time.time;
        }

        if (Time.time - lastAttackTime >= timeBetweenAttacks)
        {
            ResetAttack();
        }
    }



    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public int TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gunSystem.gold += gold;
            enemySpawner.enemyCount--;
            Destroy(gameObject);
            return 1;
        }
        return 0;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
