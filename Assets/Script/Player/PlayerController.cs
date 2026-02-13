using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float interactRad = 1f;
    public LayerMask interactableLayer;
    public GameObject interactIcon;
    public bool canMove = true;

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
    void Start()
    {
        canMove = true;
        if (GameManager.Instance.lastSceneName == SceneManager.GetActiveScene().name)
        {
            transform.position = GameManager.Instance.lastOverworldPosition;
            // 위치를 이동시킨 후 정보를 초기화 (계속 텔레포트되는 것 방지)
            GameManager.Instance.lastSceneName = "";
        }
    }
    void Update()
    {
        if (!canMove)
        {
            moveInput = Vector2.zero;
            anim.SetFloat("Speed", 0); // 멈춘 애니메이션 강제 적용
            if (rb != null) rb.velocity = Vector2.zero;
            return;
        }
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
    public void SetCanMove(bool value)
    {
        canMove = value;
    }
    void CheckInteractable()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRad, interactableLayer);

        if(hit != null && hit.GetComponent<IInteractable>() != null)
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
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRad, interactableLayer);

        if (hit != null)
        {
            IInteractable target = hit.GetComponent<IInteractable>();
            target?.Interact();
        }
    }
    void FixedUpdate()
    {
        if (!canMove)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRad);
    }
}
