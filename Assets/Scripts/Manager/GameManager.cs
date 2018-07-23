using UnityEngine;
using FOS.Scene;

public class GameManager : MonoBehaviour
{
    //シングルトン
    static public GameManager instance;

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
    //Stage情報
    [SerializeField] StageData[] stageDataArr = new StageData[3];

    int stageNo;                    //ステージ番号
    FastenerSetter fastenerSetter;  //ファスナー生成
    Form creater;                   //オブジェクト生成

    //-----------------------------------------------------
    //  ゲーム開始の準備
    //-----------------------------------------------------
    public void GameSet(int no)
    {
        stageNo = no;
    }
    //-----------------------------------------------------
    //  スコア準備
    //-----------------------------------------------------
    public void ScoreSet()
    {
        ScoreManager.instance.StartGame(stageNo);
    }
    //-----------------------------------------------------
    //  見た目変更
    //-----------------------------------------------------
    public void DezineChange()
    {
        creater = GameObject.Find("ObjectCreater").GetComponent<Form>();
        creater.SetStageData(stageDataArr[stageNo]);
        fastenerSetter = GameObject.Find("FastenerArr").GetComponent<FastenerSetter>();
        fastenerSetter.SetFastener();
    }
    //-----------------------------------------------------
    //  オブジェクト生成開始
    //-----------------------------------------------------
    public void StartGame_Create() 
    {
        creater.StartCreate();
    }
    //-----------------------------------------------------
    //  次のフェーズへ
    //-----------------------------------------------------
    public void NextGame()
    {
        fastenerSetter.NextFastener();
    }
    //-----------------------------------------------------
    //  ゲーム終了
    //-----------------------------------------------------
    public void GameFinish()
    {
        SceneOption.instance.LoadSelectScene();
    }
}