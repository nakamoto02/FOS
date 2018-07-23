using UnityEngine;
using FOS.Sound;
using FOS.Scene;

public class TransitionFastener : MonoBehaviour
{
    SceneName nextScene;        //次のシーン
    //=====================================================
    void Start()
    {
        CutTouch();
    }
    //-----------------------------------------------------
    //  次のシーンを設定
    //-----------------------------------------------------
    public void SceneSetting(SceneName name)
    {
        nextScene= name;
    }
    //-----------------------------------------------------
    //  次のシーンのBGMを再生
    //-----------------------------------------------------
    void NextSceneBgm()
    {
        BGM bgm = 0;
        
        switch(nextScene)
        {
            case SceneName.TitleScene:
                bgm = BGM.Title;
                break;
            case SceneName.SelectScene:
                bgm = BGM.Select;
                break;
            case SceneName.GameScene:
                bgm = BGM.Game;
                break;
        }
        SoundManager.instance.StartSound_BGM(bgm);
    }

    //-----------------------------------------------------
    //  閉じた時
    //-----------------------------------------------------
    public void CloseEnd()
    {
        //消さない
        DontDestroyOnLoad(this.gameObject);

        //次のシーンLoad
        SceneOption.instance.LoadScene(nextScene);
    }
    //-----------------------------------------------------
    //  見た目の変更 (ゲームシーン)
    //-----------------------------------------------------
    public void ChangeDezine()
    {
        if (nextScene == SceneName.GameScene)
        {
            GameManager.instance.DezineChange();
            GameManager.instance.ScoreSet();
        }
    }
    //-----------------------------------------------------
    //  開いた時
    //-----------------------------------------------------
    public void OpenEnd()
    {
        //ゲーム開始
        if(nextScene == SceneName.GameScene) GameManager.instance.StartGame_Create();

        OnTouch();                  //TouchManagerを有効
        NextSceneBgm();             //BGM
        Destroy(this.gameObject);   //削除
    }
    //-----------------------------------------------------
    //  タッチ OFF
    //-----------------------------------------------------
    public void CutTouch() { TouchManager.instance.TouchSwitch(false); }
    //-----------------------------------------------------
    //  タッチ ON
    //-----------------------------------------------------
    public void OnTouch() { TouchManager.instance.TouchSwitch(true); }
    //-----------------------------------------------------
    //  Fastener効果音
    //-----------------------------------------------------
    public void StartSound() { SoundManager.instance.StartSound_SEF(Fastener_SE.Move); }
}