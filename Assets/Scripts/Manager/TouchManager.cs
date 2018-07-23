using UnityEngine;
using FOS.Scene;

public class TouchManager : MonoBehaviour
{
    // シングルトン
    static public TouchManager instance;

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
        // 読み込み
        touchEffect = Resources.Load("Effect/TouchEffect") as GameObject;
    }
    //-----------------------------------------------------
    //  Private
    //-----------------------------------------------------
    GameObject touchEffect;                         // EffectのPrefab
    BaseSlider[] sliderArr = new BaseSlider[10];    // 現在タッチ中のSliderの配列
    BaseSlider sliderColl;                          // 現在タッチ中のSlider
    bool isOn = true;                               // 機能フラグ
    //-----------------------------------------------------
    //  Update
    //-----------------------------------------------------
	void Update ()
    {
        if (Input.GetMouseButtonDown(0)) TouchEffect();
        if (!isOn) return;

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
    delegate void ITouchBegan(Vector2 pos);
    delegate void ITouchMove(Vector2 pos);
    delegate void ITouchEnd();

    ITouchBegan tBegan;
    ITouchMove  tMove;
    ITouchEnd   tEnd;
    void AddTouchObject(ITouch touchObj)
    {
        tBegan += touchObj.TouchBegan;
        tMove  += touchObj.TouchMove;
        tEnd   += touchObj.TouchEnd;
    }
    void RemoveTouchObject(ITouch touchObj)
    {
        tBegan -= touchObj.TouchBegan;
        tMove  -= touchObj.TouchMove;
        tEnd   -= touchObj.TouchEnd;
    }
    //-----------------------------------------------------
    //  Touch
    //-----------------------------------------------------
    void Touch()
    {
        if (Input.touchCount < 1) return;

        foreach (Touch t in Input.touches)
        {
            if (t.fingerId == 0)
            {
                switch (t.phase)
                {
                    //指が触れたとき
                    case TouchPhase.Began:
                        TouchBegan(t.fingerId);
                        break;

                    //指が離されたとき
                    case TouchPhase.Ended:
                        TouchEnd(t.fingerId);
                        break;
                    //キャンセル
                    case TouchPhase.Canceled:
                        TouchEnd(t.fingerId);
                        break;
                }
            }
        }
    }
    //-----------------------------------------------------
    //  指が触れたとき
    //-----------------------------------------------------
    void TouchBegan(int id)
    {
        //Collider
        Collider2D coll = TouchCollider2D(id);

        //判定
        if (coll && coll.tag == "Slider")
        {
            sliderArr[id] = coll.GetComponent<BaseSlider>();
            sliderArr[id].SliderTouchDown(id);
        }
    }
    //-----------------------------------------------------
    //  指が離れたとき
    //-----------------------------------------------------
    void TouchEnd(int id)
    {
        if (sliderArr[id])
        {
            sliderArr[id].SliderTouchUp();
            sliderArr[id] = null;
        }
    }
    //-----------------------------------------------------
    //  Click
    //-----------------------------------------------------
    void Click()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Collider
            Collider2D coll = TouchCollider2D(-1);

            if (coll && coll.tag == "Slider")
            {
                sliderColl = coll.GetComponent<BaseSlider>();
                sliderColl.SliderTouchDown(-1);
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (sliderColl != null)
            {
                sliderColl.SliderTouchUp();
                sliderColl = null;
            }
        }
    }
    //-----------------------------------------------------
    //  機能の ON/OFF
    //-----------------------------------------------------
    public void TouchSwitch(bool flg) { isOn = flg; }
    //-----------------------------------------------------
    //  タッチしたCollider
    //-----------------------------------------------------
    Collider2D TouchCollider2D (int id)
    {
        //タッチ位置を取得
        Vector2 touchPos = (id != -1) ? Input.touches[id].position : (Vector2)Input.mousePosition;
        //タッチ位置をWorld座標に
        touchPos = Camera.main.ScreenToWorldPoint(touchPos);

        //GameScene以外なら
        if (!SceneOption.instance.IsSceneName("GameScene")) return Physics2D.OverlapPoint(touchPos);

        //タッチ位置にあるColloder2D
        Collider2D[] colls = Physics2D.OverlapPointAll(touchPos);
        Collider2D touchColl = null;
        //タッチ位置に一番近いCollider
        foreach(Collider2D coll in colls)
        {
            if(coll.tag == "Slider")
            {
                touchColl = ColliderCheck(coll, touchColl, touchPos);
            }
        }
        return touchColl;
    }
    //-----------------------------------------------------
    //  近いColliderを返す
    //-----------------------------------------------------
    Collider2D ColliderCheck(Collider2D value, Collider2D near, Vector2 pos)
    {
        if (near == null) return value;
        float touchToValue = ((Vector2)value.transform.position - pos).magnitude;
        float touchToNear = ((Vector2)near.transform.position - pos).magnitude;
        if (touchToValue < touchToNear) return value;
        return near;
    }
}
