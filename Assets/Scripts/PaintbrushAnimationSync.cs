using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintbrushAnimationSync : MonoBehaviour
{
    public Animator playerAnimator;
    private Animator paintbrushAnimator;

    void Awake() {
        paintbrushAnimator = GetComponent<Animator>();
    }
    void Update() {

        bool isWalking = playerAnimator.GetBool("IsWalking");
        bool isDoubleJumping = playerAnimator.GetBool("IsDoubleJumping");
        bool isAttacking1 = playerAnimator.GetBool("isAttacking1");
        bool isAttacking2 = playerAnimator.GetBool("isAttacking2");

        paintbrushAnimator.SetBool("isWalking", isWalking);
        paintbrushAnimator.SetBool("IsDoubleJumping", isDoubleJumping);
        paintbrushAnimator.SetBool("isAttacking1", isAttacking1);
        paintbrushAnimator.SetBool("isAttacking2", isAttacking2);
    }
}
