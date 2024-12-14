using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundDistance;
    private bool isGrounded;

    [Header("Mouse Look")]
    [SerializeField] private Transform xRotation;
    [SerializeField] private Transform yRotation;
    private float xDegrees;
    private float yDegrees;

    void FixedUpdate()
    {
        isGrounded = IsGrounded();
    }

    public void Move(float x, float z)
    {
        if (!isGrounded) return;

        Vector3 forward = transform.forward * z * speed;
        Vector3 right = transform.right * x * speed;
        rb.velocity = new Vector3(forward.x + right.x, rb.velocity.y, forward.z + right.z);
    }

    public void Jump()
    {
        if (!isGrounded) return;

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundDistance);
    }

    public void Rotate(float x, float y)
    {
        xDegrees -= x;
        xDegrees = Mathf.Clamp(xDegrees, -90, 90);
        xRotation.localRotation = Quaternion.Euler(xDegrees, 0, 0);
        yDegrees += y;
        yRotation.localRotation = Quaternion.Euler(0, yDegrees, 0);
    }

    public void LookAt(Vector3 target)
    {
        Vector3 direction = target - transform.position;
        direction.y = 0;
        Quaternion rotation = Quaternion.LookRotation(direction);
        // calculate xDegrees and yDegrees and set xRotation and yRotation
        xDegrees = rotation.eulerAngles.x;
        yDegrees = rotation.eulerAngles.y;
        xRotation.localRotation = Quaternion.Euler(xDegrees, 0, 0);
        yRotation.localRotation = Quaternion.Euler(0, yDegrees, 0);
    }
}
