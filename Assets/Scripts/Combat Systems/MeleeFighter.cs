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

}
