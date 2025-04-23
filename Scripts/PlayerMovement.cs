using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public UnityEngine.UI.Image boostActiveUI;
    
    public float turnSpeed = 20f;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    AudioSource m_AudioSource;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    public float speedMultiplier = 2f;
    public float boostDuration = 2f;
    public float boostCooldown = 5f;

    private float boostTimer;
    private float cooldownTimer;
    private bool isBoosting;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool("IsWalking", isWalking);

        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation(desiredForward);

        if (Input.GetKey(KeyCode.LeftShift) && cooldownTimer <= 0f && !isBoosting)
        {
            isBoosting = true;
            boostTimer = boostDuration;
            cooldownTimer = boostCooldown;
        }
    }

    private void Update()
    {
        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;
            if(boostTimer <= 0f)
            {
                isBoosting = false;
            }
        }

        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (boostActiveUI != null)
        {
            boostActiveUI.enabled = isBoosting;
        }
    }

    void OnAnimatorMove()
    {
        float currentSpeedMultiplier = isBoosting ? speedMultiplier : 1f;

        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude * currentSpeedMultiplier);
        m_Rigidbody.MoveRotation(m_Rotation);

    }
}