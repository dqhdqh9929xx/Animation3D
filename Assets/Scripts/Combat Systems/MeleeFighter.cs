using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeFighter : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

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

        animator.CrossFade("Slash", 0.2f);
        yield return null;

        var animState = animator.GetCurrentAnimatorStateInfo(1);// lấy thông tin về trạng thái hoạt hình hiện tại của Animator ở layer 1

        yield return new WaitForSeconds(animState.length); // đợi đến khi hoạt hình layer 1 kết thúc

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
