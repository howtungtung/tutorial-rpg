﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CharacterController : MonoBehaviour, IAttackFrameReceiver
{
    private enum State
    {
        DEFAULT,
        HIT,
        ATTACKING
    }

    private State currentState;

    private NavMeshAgent agent;
    public Animator animator;
    public GameObject hitEffectPrefab;
    private InteractableObject targetInteractable;
    private CharacterData targetCharacterData;
    private CharacterData characterData;

    private bool clearPostAttack;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        characterData = GetComponent<CharacterData>();
        characterData.Init();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (targetCharacterData != null)
        {
            if (targetCharacterData.currentStat.hp == 0)
                targetCharacterData = null;
            else
                CheckAttack();
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (currentState != State.ATTACKING)
            {
                targetCharacterData = null;
            }
            else
            {
                clearPostAttack = true;
            }
        }

        if (currentState != State.ATTACKING)
        {
            ObjectsRaycasts(screenRay);
            if (Input.GetMouseButton(0))
            {
                if (targetCharacterData == null)
                {
                    CharacterData data = targetInteractable as CharacterData;
                    if (data != null)
                    {
                        targetCharacterData = data;
                    }
                    else
                    {
                        MoveCheck(screenRay);
                    }
                }
            }
        }
    }

    private void ObjectsRaycasts(Ray screenRay)
    {
        bool somethingFound = false;
        RaycastHit[] hits = Physics.SphereCastAll(screenRay, 1f, 1000f, LayerMask.GetMask("Target"));
        if (hits.Length > 0)
        {
            CharacterData data = hits[0].collider.GetComponent<CharacterData>();
            if (data != null)
            {
                targetInteractable = data;
                somethingFound = true;
            }
        }
        if (somethingFound == false)
        {
            targetInteractable = null;
        }
    }

    private void MoveCheck(Ray screenRay)
    {
        RaycastHit hit;
        if (Physics.Raycast(screenRay, out hit, LayerMask.GetMask("Level"), 1000))
        {
            agent.SetDestination(hit.point);
        }
    }

    private void CheckAttack()
    {
        if (currentState == State.ATTACKING)
            return;
        if (characterData.CanAttackReach(targetCharacterData))
        {
            StopAgent();
            if (Input.GetMouseButton(0))
            {
                Vector3 forward = targetCharacterData.transform.position - transform.position;
                forward.y = 0;
                forward.Normalize();

                transform.forward = forward;
                if (characterData.CanAttackTarget(targetCharacterData))
                {
                    currentState = State.ATTACKING;
                    animator.SetTrigger("Attack");
                }
            }
        }
        else
        {
            agent.SetDestination(targetCharacterData.transform.position);
        }
    }

    public void AttackFrame()
    {
        if (targetCharacterData == null)
        {
            clearPostAttack = false;
            return;
        }
        if (characterData.CanAttackReach(targetCharacterData))
        {
            characterData.Attack(targetCharacterData);
            Vector3 attackPos = targetCharacterData.transform.position + transform.up * 0.5f;
            Instantiate(hitEffectPrefab, attackPos, Random.rotationUniform);
        }
        if (clearPostAttack)
        {
            clearPostAttack = false;
            targetCharacterData = null;
        }
        currentState = State.DEFAULT;
    }

    private void StopAgent()
    {
        agent.ResetPath();
        agent.velocity = Vector3.zero;
    }
}
