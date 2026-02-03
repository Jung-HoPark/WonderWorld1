using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float interactDis = 1.2f;
    public LayerMask interactableLayer;
    public GameObject interactIcon;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 moveInput;
    private Vector2 lastMoveDir;
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if(interactIcon != null) interactIcon.SetActive(false);
    }
    void Update()
    {
        // 1. 입력 받기 (WASD / 화살표)
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        if (moveInput != Vector2.zero)
        {
            moveInput.Normalize();
            lastMoveDir = moveInput;

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

        CheckInteractable();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformInteraction();
        }
    }
    void CheckInteractable()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, lastMoveDir, interactDis, interactableLayer);

        if(hit.collider != null && hit.collider.GetComponent<IInteractable>()  != null)
        {
            if (interactIcon != null) interactIcon.SetActive(true);
        }
        else
        {
            if (interactIcon != null) interactIcon.SetActive(false);
        }
    }
    void PerformInteraction()
    {
        RaycastHit2D hit = Physics2D.Raycast(rb.position, lastMoveDir, interactDis, interactableLayer);

        if(hit.collider != null)
        {
            IInteractable target = hit.collider.GetComponent<IInteractable>();
            target?.Interact();
        }
    }
    void FixedUpdate()
    {
        // 3. 물리 이동
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector2 direction = lastMoveDir == Vector2.zero ? Vector2.down : lastMoveDir;
        Gizmos.DrawRay(transform.position, direction * interactDis);
    }
}
