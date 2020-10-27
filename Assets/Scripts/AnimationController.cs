using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public AnimationClip[] animations;
    Animator anim;
    private int currentExpression;

    void Start()
    {
        anim = GetComponent<Animator>();

        // ensure if the face mask/layer weight is 1 
        anim.SetLayerWeight(1, 1);
    }

    public void NextMotion()
    {
        anim.SetTrigger("NextTrigger");
        RandomExpression();
        // NextExpression();
    }

    public void NextExpression()
    {
        // If reached last expression, back to first expression
        if (currentExpression < animations.Length - 1)
        {
            currentExpression++;
        }
        else
        {
            currentExpression = 0;
        }

        anim.SetLayerWeight(1, 1);
        anim.CrossFade(animations[currentExpression].name, 0);
    }

    public void RandomExpression()
    {
        currentExpression = Random.Range(0, animations.Length - 1);
        anim.CrossFade(animations[currentExpression].name, 0);
    }
}