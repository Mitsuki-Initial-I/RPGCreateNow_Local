using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;
    public Transform player;
    public bool isChasing = false;

    private void Start()
    {
        
    }
    private void Update()
    {
        float distance = Vector3.Distance(player.position, transform.position);
        if (distance<attackRange)
        {
            Attack();
        }
        else if (distance<10f)
        {
            Chase();
        }
        else
        {
            Idle();
        }
    }
    void Chase()
    {
        isChasing = true;
        Vector3 direction = (player.position - transform.position).normalized;
        transform.LookAt(player);
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
    void Attack()
    {
        Debug.Log("Monster‚ÌUŒ‚");
    }
    void Idle()
    {
        isChasing = false;
    }
}
