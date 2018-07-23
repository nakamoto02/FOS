using UnityEngine;
using UnityEngine.UI;

public class ScoreEffect : MonoBehaviour
{
    Animator animator;  //Animator

    [SerializeField] Sprite[] numberSprite; //数字画像
    [SerializeField] Sprite[] signs;        //符号画像 0.正, 1.負
    [SerializeField] Image[] numberImage;   //表示UI
    //-----------------------------------------------------
    //  点数をセット
    //-----------------------------------------------------
    public void PointSet(int point)
    {
        //正か負か
        bool isUp = point > 0;
        //符号
        numberImage[0].sprite = (isUp) ? signs[0] : signs[1];
        //値をセット
        numberImage[1].sprite = numberSprite[Mathf.Abs(point)];

        //アニメーション
        if (animator == null) animator = GetComponent<Animator>();
        if (isUp) animator.SetTrigger("Up");
        else animator.SetTrigger("Down");
    }
    //-----------------------------------------------------
    //  削除
    //-----------------------------------------------------
    public void EfeDestroy()
    {
        Destroy(this.gameObject);
    }
}