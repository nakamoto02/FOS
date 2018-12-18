using UnityEngine;
using FOS.Scene;

public class TouchManager : MonoBehaviour
{
    // シングルトン
    static public TouchManager instance;

    void Awake()
    {
        Debug.Log("TouchManager");

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        // 読み込み
        touchEffect = Resources.Load("Effect/TouchEffect") as GameObject;

        // リセット
        tObjCount = 0;
    }
    //-----------------------------------------------------
    //  Private
    //-----------------------------------------------------
    GameObject touchEffect;     // EffectのPrefab
    bool isOn = true;           // 機能フラグ
    int tObjCount;
    //-----------------------------------------------------
    //  Delegate
    //-----------------------------------------------------
    delegate void TouchBegan(Vector2 pos);
    delegate void TouchMove(Vector2 pos);
    delegate void TouchEnd();

    TouchBegan tBegan;
    TouchMove tMove;
    TouchEnd tEnd;
    //=====================================================
    void Update ()
    {
        if (Input.GetMouseButtonDown(0)) TouchEffect();
        if (!isOn || tObjCount < 1) return;

        // タッチパネル
        if (Input.touchSupported) Touch();
        // マウス
        else Click();
	}
    //-----------------------------------------------------
    //  Effect生成
    //-----------------------------------------------------
    void TouchEffect()
    {
        Vector3 createPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Instantiate(touchEffect, createPos, Quaternion.identity);
    }
    //-----------------------------------------------------
    //  登録
    //-----------------------------------------------------
    public void AddTouchObject(ITouch touchObj)
    {
        tBegan += touchObj.TouchBegan;
        tMove  += touchObj.TouchMove;
        tEnd   += touchObj.TouchEnd;
        ++tObjCount;
    }
    //-----------------------------------------------------
    //  削除
    //-----------------------------------------------------
    public void RemoveTouchObject(ITouch touchObj)
    {
        tBegan -= touchObj.TouchBegan;
        tMove  -= touchObj.TouchMove;
        tEnd   -= touchObj.TouchEnd;
        --tObjCount;
    }
    //-----------------------------------------------------
    //  Touch
    //-----------------------------------------------------
    void Touch()
    {
        switch(Input.touches[0].phase)
        {
            // 指が触れたとき
            case TouchPhase.Began:    tBegan(Input.touches[0].position); break;
            // 触れている間
            case TouchPhase.Moved:    tMove(Input.touches[0].position); break;
            // 離れたとき
            case TouchPhase.Ended:    tEnd(); break;
            // キャンセルされたとき
            case TouchPhase.Canceled: tEnd(); break;
        }
    }
    //-----------------------------------------------------
    //  Click
    //-----------------------------------------------------
    void Click()
    {
        // クリックされたとき
        if (Input.GetMouseButtonDown(0)) tBegan(Input.mousePosition);
        // クリックされている間
        if (Input.GetMouseButton(0))     tMove(Input.mousePosition);
        // 離れたとき
        if (Input.GetMouseButtonUp(0))   tEnd();
    }
    //-----------------------------------------------------
    //  機能の ON/OFF
    //-----------------------------------------------------
    public void TouchSwitch(bool flg) { isOn = flg; }
}