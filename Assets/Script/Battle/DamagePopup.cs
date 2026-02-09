using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public float moveSpeed = 2f;    // 위로 올라가는 속도
    public float disappearTime = 1f; // 사라지는 시간
    private TextMeshProUGUI textMesh;
    private float timer;

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        timer = disappearTime;
    }
    public void Setup(int damage)
    {
        textMesh.text = damage.ToString();
    }
    void Update()
    {
        // 위로 이동
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // 서서히 투명해지다가 삭제
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            Color c = textMesh.color;
            c.a = timer / disappearTime;
            textMesh.color = c;
        }
    }
}
