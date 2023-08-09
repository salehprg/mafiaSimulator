using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator animator;
    public void OnDead()
    {
        animator.SetBool("Dead" , true);
    }
    public void OnHeal()
    {
        animator.SetBool("Dead" , false);

    }
    public void UpdateSpeed(float speed)
    {
        animator.SetFloat("Speed" , speed);
    }


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {

    }
}
