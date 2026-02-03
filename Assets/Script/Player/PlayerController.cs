using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 moveInput;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        // 1. 입력 받기 (WASD / 화살표)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput != Vector2.zero)
        {
            moveInput.Normalize();

            // 1번 & 3번 문제 해결: 움직임이 있을 때 방향과 속도를 업데이트
            anim.SetFloat("DirX", moveInput.x);
            anim.SetFloat("DirY", moveInput.y);
            anim.SetFloat("Speed", 1f); // 움직이는 상태
        }
        else
        {
            // 3번 문제 해결: 움직임이 없으면 Speed만 0으로 (방향은 유지)
            anim.SetFloat("Speed", 0f);
        }
    }
    void FixedUpdate()
    {
        // 3. 물리 이동
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
}
