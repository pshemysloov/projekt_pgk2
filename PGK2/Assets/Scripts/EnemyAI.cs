using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAI : Actor
{
    [SerializeField]
    protected List<SteeringBehaviour> steeringBehaviours;

    [SerializeField]
    protected List<Detector> detectors;

    [SerializeField]
    protected AIData aiData;

    [SerializeField]
    protected float detectionDelay, aiUpdateDelay, attackDelay, attackDistance;

    [SerializeField]
    protected Vector3 projectileOffset;

    [SerializeField]
    protected float projectileSpeed;

    [SerializeField]
    protected float projectileDuration;

    [SerializeField]
    public float damage, statusChance, criticalChance, criticalDamage;

    //Inputs sent from the Enemy AI to the Enemy controller
    public UnityEvent OnAttackPressed;
    public UnityEvent<Vector2> OnMovementInput, OnPointerInput;

    [SerializeField]
    protected Vector2 movementInput;

    [SerializeField]
    protected ContextSolver movementDirectionSolver;


    protected bool following = false;

    protected RuntimeAnimatorController controller;





    protected override void Start()
    {
        base.Start();
        InitializeHealth();

        //Detecting Player and Obstacles around
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }

    private void PerformDetection()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(aiData);
        }
    }

    private void Update()
    {
        //Enemy AI movement based on Target availability
        if (aiData.currentTarget != null)
        {
            //Looking at the Target
            OnPointerInput?.Invoke(aiData.currentTarget.position);
            if (following == false)
            {
                following = true;
                StartCoroutine(ChaseAndAttack());
            }
        }
        else if (aiData.GetTargetsCount() > 0)
        {
            //Target acquisition logic
            aiData.currentTarget = aiData.targets[0];
        }
        //Moving the Agent
        OnMovementInput?.Invoke(movementInput);
    }

    

    public virtual IEnumerator ChaseAndAttack()
    {
        if (aiData.currentTarget == null)
        {
            //Stopping Logic
            Debug.Log("Stopping");
            movementInput = Vector2.zero;
            following = false;
            yield break;
        }
        else
        {
            float distance = Vector2.Distance(aiData.currentTarget.position, transform.position);

            if (distance < attackDistance)
            {
                //Attack logic
                movementInput = Vector2.zero;
                OnAttackPressed?.Invoke();
                yield return new WaitForSeconds(attackDelay);
                StartCoroutine(ChaseAndAttack());
            }
            else
            {
                //Chase logic
                movementInput = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
                yield return new WaitForSeconds(aiUpdateDelay);
                StartCoroutine(ChaseAndAttack());
            }

        }

    }

    public override void Movement()
    {
        // Try to move player in input direction, followed by left right and up down input if failed
        bool success = CollisionCheck(movementInput);

        if (!success)
        {
            // Try Left / Right
            CollisionCheck(new Vector2(movementInput.x, 0));

            // Try UP / Down
            CollisionCheck(new Vector2(0, movementInput.y));
        }
    }
    public virtual void Attack()
    {

    }
}