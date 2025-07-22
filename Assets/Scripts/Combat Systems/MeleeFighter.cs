using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeFighter : MonoBehaviour
{
    Animator animator;
    float normalizedTime = 0f; // biến để lưu trữ thời gian đã trôi qua so với tổng thời gian của hoạt hình
    [SerializeField] GameObject sword; // đối tượng vũ khí, có thể là một GameObject chứa Collider để va chạm với đối tượng khác
    BoxCollider swordCollider; // Collider của vũ khí, có thể là BoxCollider hoặc SphereCollider tùy thuộc vào hình dạng của vũ khí

    public enum AttackState
    {
        Idle,
        Windup,
        Impact,
        Cooldown
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (sword != null)
        {
            swordCollider = sword.GetComponent<BoxCollider>(); // lấy Collider của vũ khí
            swordCollider.enabled = false; // tắt Collider ban đầu để không va chạm với đối tượng khác
        }
        
    }

    public AttackState attackState;

    public bool inAction { get; private set; } = false;

    public void TryToAttack()
    {
        if (!inAction)
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        inAction = true;
        attackState = AttackState.Windup; // đánh dấu là đang thực hiện hành động Windup

        float impactStartTime = 0.33f; // % thời gian bắt đầu của Impact trong animation
        float impactEndTime = 0.55f; // % thời gian kết thúc của Impact trong animation

        animator.CrossFade("Slash", 0.2f);
        yield return null;

        var animState = animator.GetCurrentAnimatorStateInfo(1);// lấy thông tin về trạng thái hoạt hình hiện tại của Animator ở layer 1

        float timer = 0f;

        while (timer < animState.length)
    { 
        {
            timer += Time.deltaTime;
            normalizedTime = timer / animState.length; // tính toán thời gian đã trôi qua so với tổng thời gian của hoạt hình
        }

        if (attackState == AttackState.Windup)
        {
            if (normalizedTime >= impactStartTime)
            {
                attackState = AttackState.Impact; // chuyển sang trạng thái Impact
                swordCollider.enabled = true; // bật Collider của vũ khí để va chạm với đối tượng khác
            }
        }
        else if (attackState == AttackState.Impact)
        {
            if (normalizedTime >= impactEndTime)
            {
                attackState = AttackState.Cooldown; // chuyển sang trạng thái Cooldown
                swordCollider.enabled = false; // tắt Collider của vũ khí sau khi va chạm
                }
        }
        else if (attackState == AttackState.Cooldown)
        {
            // đợi đến khi hoạt hình kết thúc
        }
        yield return null;
    }
        attackState = AttackState.Idle; // chuyển về trạng thái Idle sau khi hoạt hình kết thúc
        inAction = false; // đánh dấu là không còn đang thực hiện hành động nào nữa

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "SwordPlayer" && !inAction)
        {
            StartCoroutine(PlayHitReaction());
        }
    }

    IEnumerator PlayHitReaction()
    {
        inAction = true;

        animator.CrossFade("Impact", 0.2f); // gọi animation bằng CrossFade kiểu gọi từ tên State, không cần biết tên của Animation Clip
        yield return null;

        var animState = animator.GetCurrentAnimatorStateInfo(1);// lấy thông tin về trạng thái hoạt hình hiện tại của Animator ở layer 1

        yield return new WaitForSeconds(animState.length); // đợi đến khi hoạt hình layer 1 kết thúc

        inAction = false; // đánh dấu là không còn đang thực hiện hành động nào nữa

    }
}
