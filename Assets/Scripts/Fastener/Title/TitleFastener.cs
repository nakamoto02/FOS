using UnityEngine;
using FOS.Scene;
using FOS.Sound;

public class TitleFastener : BaseFastener, IFAutoBack
{
    //Transform
    [SerializeField] Transform endPoint;    //終点
    float sliderRange;                      //スライダーの可動距離
    float moveValue = 0;                    //スライダーの移動値
    float memoryValue = 0;                  //スライダーの前回値
    bool isTouch = false;                   //タッチフラグ
    //=====================================================
    private void Start()
    {
        //Transformを取得
        transformCache = transform;
        //Animationを取得
        animator = transformCache.GetComponent<Animator>();
        //スライダーの可動距離を計算
        sliderRange = (endPoint.localPosition - refarencePoint.localPosition).magnitude;
    }
    void Update()
    {
        if (!isTouch) AutoBackMove();
    }
    //-----------------------------------------------------
    //  Sliderを元の位置へ戻す
    //-----------------------------------------------------
    public void AutoBackMove()
    {
        if (moveValue == 0) return;

        //戻る
        moveValue = Mathf.Lerp(moveValue, -0.1f, 0.1f);

        moveValue = Mathf.Max(moveValue, 0);
        animator.SetFloat("Move", moveValue);
    }
    //-----------------------------------------------------
    //  Sliderの位置を更新
    //-----------------------------------------------------
    public override void SliderTouchMove(int sliderID, Vector2 sliderPos, Vector2 touchPos, float radius, float side)
    {
        //交点(WorldPosition)
        Vector2 matchPoint = GetMatchPoint(refarencePoint.position, endPoint.position, touchPos);

        //移動量
        float moveDif = (sliderPos - matchPoint).magnitude;
        moveDif = Mathf.Lerp(0, moveDif, (moveDif - radius) / moveDif);

        //スライダーの移動方向
        if (side < 0) moveDif = -moveDif;

        //animation用の値
        moveValue = moveValue + (moveDif / sliderRange);
        moveValue = Mathf.Clamp(moveValue, 0, 1);

        //animationに反映
        animator.SetFloat("Move", moveValue);

        //音再生
        float dif = Mathf.Abs(moveValue - memoryValue);
        if (dif > 0.05f)
            SoundManager.instance.StartSound_SEF(Fastener_SE.Move);

        //値を記憶
        memoryValue = moveValue;
    }
    //-----------------------------------------------------
    //  タッチフラグの ON/OFF
    //-----------------------------------------------------
    public void IsTouch(bool flg)
    {
        isTouch = flg;
        animator.SetBool("Touch", isTouch);

        if (!flg && moveValue != 0)
            SoundManager.instance.StartSound_SEF(Fastener_SE.Back);
    }
    //-----------------------------------------------------
    //  閉じた時
    //-----------------------------------------------------
    public void CloseEnd()
    {
        //消さない
        DontDestroyOnLoad(this.gameObject);

        OnDisplay();
        //SelectSceneをLoad
        SceneOption.instance.LoadScene(SceneName.SelectScene);
        //BGM_Title
        SoundManager.instance.StartSound_BGM(BGM.Title);
        //TouchManager起動
        OnTouch();
    }
    //-----------------------------------------------------
    //  開いた時
    //-----------------------------------------------------
    public void OpenEnd()
    {
        //BGM_Select
        SoundManager.instance.StartSound_BGM(BGM.Select);
        //TouchManagerを起動
        OnTouch();

        //削除
        Destroy(this.gameObject);
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
    //-----------------------------------------------------
    //  ガイドの表示
    //-----------------------------------------------------
    public void OnDisplay() { animator.SetBool("Display", true); }
}