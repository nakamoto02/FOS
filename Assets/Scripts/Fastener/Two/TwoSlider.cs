using System.Collections;
using UnityEngine;

public class TwoSlider : BaseSlider
{
    //SliderID
    [SerializeField, Range(0, 1), Space]
    int sliderID;

    //SpriteRenderer
    [SerializeField, Space]
    SpriteRenderer[] spriteRendererArr;
    //-----------------------------------------------------
    //  持ち手をタッチ方向へ回転
    //-----------------------------------------------------
    protected override void HandleRotate()
    {
        //位置
        Vector2 sliderCenterPos = handleTransformCache.position;
        Vector2 touchPos = Camera.main.ScreenToWorldPoint(TouchOrMousePos());

        //角度
        float handleDir = Mathf.Atan2(touchPos.x - sliderCenterPos.x, touchPos.y - sliderCenterPos.y) * Mathf.Rad2Deg;

        //反映
        handleTransformCache.rotation = Quaternion.Euler(new Vector3(0, 0, -handleDir));

        //二点間の距離
        float dif = (touchPos - sliderCenterPos).magnitude;

        if (radius / 3 < dif)
        {
            //スライダーからの交点の位置
            Vector2 sliderVec = (Vector2)dirPoint.position - sliderCenterPos;
            Vector2 sliderToTouchVec = touchPos - sliderCenterPos;
            float side = sliderVec.y * sliderToTouchVec.x - sliderVec.x * sliderToTouchVec.y;

            parentFastener.SliderTouchMove(sliderID, sliderCenterPos, touchPos, radius, side);
        }
    }
    //-----------------------------------------------------
    //  押された時
    //-----------------------------------------------------
    public override void SliderTouchDown(int id)
    {
        const int ORDER_COUNT = 3;
        isTouch = true; touchID = id;
        StartCoroutine(SetOrderInLayer(ORDER_COUNT, 0));
    }
    //-----------------------------------------------------
    //  離した時
    //-----------------------------------------------------
    public override void SliderTouchUp()
    {
        isTouch = false; touchID = -1;
        StartCoroutine(SetOrderInLayer(0, 1));
    }
    //-----------------------------------------------------
    //  Sliderの描画順を変更
    //-----------------------------------------------------
    IEnumerator SetOrderInLayer(int no, float waitTime)
    {
        //待機
        yield return new WaitForSeconds(waitTime);

        int order = no;

        foreach (SpriteRenderer renderer in spriteRendererArr)
        {
            //描画順を変更
            renderer.sortingOrder = order;

            order++;
        }
    }
}