using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] Sprite[] numberSprite; //数字画像
    [SerializeField] Image[] numberImage;   //表示UI
    [Space]
    [SerializeField] ScoreEffect efePre;    //エフェクト

    //-----------------------------------------------------
    //  得点を表示
    //-----------------------------------------------------
    public void ScoreDraw(int point)
    {
        int score = ScoreManager.instance.GetScore();
        int num = score * 10;

        for (int i = 2; i >= 0; i--)
        {
            int spriteNo = (int)(num / Mathf.Pow(10, i));
            num -= (int)(spriteNo * Mathf.Pow(10, i));

            numberImage[i].sprite = numberSprite[spriteNo];
        }

        if (point == 0) return;

        //Effect
        ScoreEffect efe = Instantiate(efePre, transform);
        efe.PointSet(point);
    }
}