using System.Collections;
using UnityEngine;
using FOS.Sound;

public class ScoreManager : MonoBehaviour
{
    //シングルトン
    static public ScoreManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //-----------------------------------------------------
    //  Private  
    //-----------------------------------------------------
    [SerializeField] ScoreData data;        //スコアデータ
    [SerializeField] Result resultBoadPre;  //リザルトボード
    [SerializeField] int[] score;           //スコア
    [SerializeField] int[] evaluation;      //評価
    int stageNo;                            //現在のステージ番号
    int gameNum;                            //ゲーム回数
    int objNum;                             //Object数
    ScoreDisplay scoreDis;                  //スコア表記

    //-----------------------------------------------------
    //  ゲーム開始
    //-----------------------------------------------------
    public void StartGame(int no)
    {
        stageNo = no;   //ステージ番号取得
        gameNum = 0;    //回数をリセット

        //スコアをリセット
        score = new int[data.stage[stageNo].num];
        evaluation = new int[data.stage[stageNo].num];
        for(int i = 0; i < score.Length; i++) score[i] = evaluation[i] = 0;

        //ScoreDisplay取得
        scoreDis = GameObject.Find("ScoreBoard").GetComponent<ScoreDisplay>();
    }
    //-----------------------------------------------------
    //  Object数を設定
    //-----------------------------------------------------
    public void SetObjectNum(int num) { objNum = num; }
    //-----------------------------------------------------
    //  得点追加
    //-----------------------------------------------------
    public void AddScore(int point)
    {
        //得点を加算
        score[gameNum] = Mathf.Max(score[gameNum] + point, 0);
        //SE
        if (point > 0) SoundManager.instance.StartSound_SE(SE.ScoreUp);
        else if (point < 0) SoundManager.instance.StartSound_SE(SE.ScoreDown);
        //表記
        scoreDis.ScoreDraw(point);

        //Object数をカウント
        objNum--;
        if (objNum <= 0) StartCoroutine(NextGame());
    }
    //-----------------------------------------------------
    //  得点を渡す
    //-----------------------------------------------------
    public int GetScore()
    {
        
        if (gameNum >= score.Length) return 0;
        return score[gameNum];
    }
    //-----------------------------------------------------
    //  次のフェーズへ
    //-----------------------------------------------------
    IEnumerator NextGame()
    {
        yield return new WaitForSeconds(1.0f);

        //評価
        float per = (float)score[gameNum] / data.stage[stageNo].maxScore;

        if (per == 1) evaluation[gameNum] = 3;
        else if (per >= 0.7f) evaluation[gameNum] = 2;
        else if (per >= 0.3f) evaluation[gameNum] = 1;
        else evaluation[gameNum] = 0;

        //ゲーム数加算
        gameNum++;

        //確認
        if (gameNum < score.Length)
        {
            //スコア表記リセット
            scoreDis.ScoreDraw(0);

            GameManager.instance.NextGame();
        }
        else
        {
            GameResult();
        }
    }
    //-----------------------------------------------------
    //  リザルト表示
    //-----------------------------------------------------
    void GameResult()
    {
        //Resultを生成
        GameObject canvas = GameObject.Find("ResultCanvas");
        Result result = Instantiate(resultBoadPre, canvas.transform);
        //表示
        int totaleEva = result.ResultDisplay(evaluation);

        //最高評価と比較
        data.stage[stageNo].maxEvaluation = Mathf.Max(totaleEva, data.stage[stageNo].maxEvaluation);
    }
}