using UnityEngine;
using UnityEngine.UI;
using FOS.Scene;
using FOS.Sound;

public class Result : MonoBehaviour
{
    //画像
    [SerializeField] Sprite[] numbers;          //数字
    [SerializeField] Sprite[] evaluationSprite; //評価

    [Space]
    //Image
    [SerializeField] Image[] evaluationArr;     //
    [SerializeField] Image[] evaluationNumArr;  //

    Animator animator;  //Animator
    int totaleEva = 0;  //総合評価
    int disCnt = 0;     //表示回数
    //=====================================================
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    //-----------------------------------------------------
    //  結果を表示
    //-----------------------------------------------------
    public int ResultDisplay(int[] eva)
    {
        //合計変数
        int totale = 0;

        //ステージ数分繰り返す
        for(int i = 0; i < eva.Length; i++)
        {
            //番号を表示
            evaluationNumArr[i].enabled = true;
            //それぞれの評価を表示
            evaluationArr[i].sprite = evaluationSprite[eva[i]];
            //合計
            totale += eva[i];
        }

        //総合評価
        float per = (float)totale / (3 * eva.Length);
        if (per == 1) totaleEva = 3;
        else if (per >= 0.6f) totaleEva = 2;
        else if (per >= 0.3f) totaleEva = 1;
        else totaleEva = 0;

        //表示
        if (totaleEva == 0) return totaleEva;

        return totaleEva;
    }
    //-----------------------------------------------------
    //  効果音再生
    //-----------------------------------------------------
    public void StarEfe()
    {
        SoundManager.instance.StartSound_SE(SE.ScoreUp);
    }
    //-----------------------------------------------------
    //  まだ表示するか確認
    //-----------------------------------------------------
    public void TotalCheck()
    {
        if (disCnt == totaleEva) animator.SetTrigger("Finish");
        disCnt++;
    }
    //-----------------------------------------------------
    //  SelectSceneへ
    //-----------------------------------------------------
    public void ResultFinish()
    {
        SceneOption.instance.LoadSelectScene();
    }
}