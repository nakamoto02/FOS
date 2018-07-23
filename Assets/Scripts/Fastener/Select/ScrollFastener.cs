using UnityEngine;
using FOS.Sound;

public class ScrollFastener : BaseFastener
{
    //Transform
    [SerializeField] Transform endPoint;    //終点
    [SerializeField] Transform scrollView;  //スクロール
    [SerializeField] Transform backComp;    //補正用背景
    
    //スライダーの可動距離
    float sliderRange;
    //スライダーの移動値
    float moveValue = 0, memoryValue = 0;
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

        //画面のスクロール
        DisplayScroll();

        //音再生
        float dif = Mathf.Abs(moveValue - memoryValue);
        if (dif > 0.02f)
            SoundManager.instance.StartSound_SEF(Fastener_SE.Move);
        memoryValue = moveValue;
    }
    //-----------------------------------------------------
    //  画面のスクロール
    //-----------------------------------------------------
    void DisplayScroll()
    {
        const float SCROLL_MAX_POS = 3.0f;  //スクロールする最大値

        //移動先
        Vector2 movePos = scrollView.position;

        //スクロール量
        movePos.y = SCROLL_MAX_POS * moveValue;

        //位置を更新
        scrollView.position = movePos;

        Vector3 backCompScale = backComp.localScale;
        backCompScale.x = 0.18f + 0.1f * moveValue;
        backComp.localScale = backCompScale;
    }
}