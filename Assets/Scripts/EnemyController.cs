using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour, IAttackFrameReceiver
{
    public enum State
    {
        IDLE,
        PURSUING,
        ATTACKING
    }
    public float Speed = 6.0f;
    public float detectionRadius = 10.0f;
    Vector3 m_StartingAnchor;
    Animator m_Animator;
    NavMeshAgent m_Agent;
    CharacterData m_CharacterData;
    bool m_Pursuing;
    float m_PursuitTimer = 0.0f;

    State m_State;
    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_Agent = GetComponent<NavMeshAgent>();
        m_CharacterData = GetComponent<CharacterData>();
        m_CharacterData.Init();
        m_CharacterData.OnDamage += () =>
        {
            m_Animator.SetTrigger("Hit");
        };
        m_Agent.speed = Speed;
        m_StartingAnchor = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CharacterData.currentStat.hp == 0)
        {
            m_Animator.SetTrigger("Dead");
            Destroy(m_Agent);
            Destroy(GetComponent<Collider>());
            Destroy(this);
            return;
        }
        Vector3 playerPosition = PlayerController.instance.transform.position;
        CharacterData playerData = PlayerController.instance.characterData;

        switch (m_State)
        {
            case State.IDLE:
                {
                    if (Vector3.Distance(playerPosition, transform.position) < detectionRadius)
                    {
                        m_PursuitTimer = 4.0f;
                        m_State = State.PURSUING;
                        m_Agent.isStopped = false;
                    }
                }
                break;
            case State.PURSUING:
                {
                    float distToPlayer = Vector3.Distance(playerPosition,transform.position);
                    if (distToPlayer < detectionRadius)
                    {
                        m_PursuitTimer = 4.0f;
                       
                        if (m_CharacterData.CanAttackTarget(playerData))
                        {
                            m_State = State.ATTACKING;
                            m_Agent.velocity = Vector3.zero;
                            m_Agent.isStopped = true;
                        }
                    }
                    else
                    {
                        if (m_PursuitTimer > 0.0f)
                        {
                            m_PursuitTimer -= Time.deltaTime;

                            if (m_PursuitTimer <= 0.0f)
                            {
                                m_Agent.SetDestination(m_StartingAnchor);
                                m_State = State.IDLE;
                            }
                        }
                    }

                    if (m_PursuitTimer > 0)
                    {
                        m_Agent.SetDestination(playerPosition);
                    }
                }
                break;
            case State.ATTACKING:
                {
                    if (!m_CharacterData.CanAttackReach(playerData))
                    {
                        m_State = State.PURSUING;
                        m_Agent.isStopped = false;
                    }
                    else
                    {
                        if (m_CharacterData.CanAttackTarget(playerData))
                        {
                            m_CharacterData.AttackTriggered();
                            m_Animator.SetTrigger("Attack");
                        }
                    }
                }
                break;
        }

    }

    public void AttackFrame()
    {
        CharacterData playerData = PlayerController.instance.characterData;

        //if we can't reach the player anymore when it's time to damage, then that attack miss.
        if (!m_CharacterData.CanAttackReach(playerData))
            return;

        m_CharacterData.Attack(playerData);
    }
}
