using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float attackRange = 2f;
    public LayerMask attackLayer;

    public bool isGrounded;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Move();
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }
    void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movement = transform.right * horizontal + transform.forward * vertical;
        rb.MovePosition(transform.position + movement * moveSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump")&&isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
    void Attack()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position,transform.forward,out hit,attackRange,attackLayer))
        {
            Debug.Log(hit.collider.name);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}