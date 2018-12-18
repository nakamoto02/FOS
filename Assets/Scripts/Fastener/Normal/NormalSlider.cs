using UnityEngine;

public class NormalSlider : MonoBehaviour, ITouch
{
    const float HIT_RADIUS = 2.0f;

    Transform transformCache;
    Transform handleTransformCache;

    BaseFastener parentFastener;

    bool isTouch = false;

    
    //=====================================================
    void OnEnable()
    {
        Debug.Log("NormalSlider");
        //TouchManager.instance.AddTouchObject(this);
    }
    void OnDisable()
    {
        TouchManager.instance.RemoveTouchObject(this);
    }
    void Update()
    {

    }
    //-----------------------------------------------------
    //  Touch
    //-----------------------------------------------------
    public void TouchBegan(Vector2 tPos)
    {
        if (IsTouchRange(tPos)) isTouch = true;
    }
    public void TouchMove(Vector2 tPos)
    {
        if (!isTouch) return;
        HandleRotate(tPos);
    }
    public void TouchEnd()
    {
        if (isTouch) isTouch = false;
    }
    public bool IsTouchRange(Vector2 tPos)
    {

        return false;
    }
    //-----------------------------------------------------
    //  持ち手をタッチ方向へ回転
    //-----------------------------------------------------
    void HandleRotate(Vector2 tPos)
    {
        //位置
        Vector2 sliderCenterPos = handleTransformCache.position;
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(tPos);

        //角度
        float handleDir = Mathf.Atan2(touchPos.x - sliderCenterPos.x, touchPos.y - sliderCenterPos.y) * Mathf.Rad2Deg;

        //反映
        handleTransformCache.rotation = Quaternion.Euler(new Vector3(0, 0, -handleDir));

        //二点間の距離
        float dif = (touchPos - sliderCenterPos).magnitude;

        if (HIT_RADIUS < dif)
        {
            //スライダーからの交点の位置
            Vector2 sliderVec = Vector2.up - sliderCenterPos;
            Vector2 sliderToTouchVec = touchPos - sliderCenterPos;
            float side = sliderVec.y * sliderToTouchVec.x - sliderVec.x * sliderToTouchVec.y;

            parentFastener.SliderTouchMove(0, sliderCenterPos, touchPos, HIT_RADIUS, side);
        }
    }
}