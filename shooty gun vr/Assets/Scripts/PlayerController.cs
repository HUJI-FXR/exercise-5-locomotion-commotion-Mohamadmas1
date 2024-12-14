using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundDistance;
    [SerializeField] private ActionBasedController rightController;



    [Header("Health")]
    [SerializeField] private float health;
    [SerializeField] private Image healthBar;
    [SerializeField] TMP_Text healthText;
    private float initialHealth;

    private bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        initialHealth = health;
        healthBar.fillAmount = 1;
        healthText.text = "100%";
    }

    //write code that the player jump when the player press the A button in the right controller
    void update()
    {
        if (rightController.selectAction.action.triggered)
        {
            Debug.Log("A button pressed");
           Jump();
        }
    }

        void FixedUpdate()
    {
        isGrounded = IsGrounded();
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
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / initialHealth;
        healthText.text = Mathf.RoundToInt(health / initialHealth * 100) + "%";
        if (health <= 0)
        {
            GameManager.instance.KillPlayer();
        }
    }
}
