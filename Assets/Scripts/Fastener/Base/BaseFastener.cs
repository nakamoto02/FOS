using UnityEngine;

public abstract class BaseFastener : MonoBehaviour
{
    //-----------------------------------------------------
    //  Protected
    //-----------------------------------------------------
    [SerializeField]
    protected Transform refarencePoint; //基準点
    protected Transform transformCache; //Transform
    protected Animator animator;        //Animation
    //-----------------------------------------------------
    //  Sliderの位置を更新
    //-----------------------------------------------------
    public abstract void SliderTouchMove(int sliderID ,Vector2 sliderPos, Vector2 touchPos, float radius, float side);
    //-----------------------------------------------------
    //  点から線に垂線を引いた際のの交点を求める
    //-----------------------------------------------------
    protected Vector2 GetMatchPoint(Vector2 line_start, Vector2 line_end, Vector2 point)
    {
        //EdgeColliderの向き
        Vector2 edgeVec = line_end - line_start;
        //マウスのポジションまでの向き
        Vector2 startToTouchVec = point - line_start;

        //角度
        float rad = Vector2.Angle(edgeVec, startToTouchVec) * Mathf.Deg2Rad;

        float b = startToTouchVec.magnitude;

        Vector2 touchVec;

        //位置
        float side = edgeVec.y * startToTouchVec.x - edgeVec.x * startToTouchVec.y;
        if (side < 0)
        {
            touchVec = (Quaternion.Euler(0, 0, -90) * edgeVec).normalized;
        }
        else
        {
            touchVec = (Quaternion.Euler(0, 0, 90) * edgeVec).normalized;
        }

        Vector2 cross = point + touchVec * (b * Mathf.Sin(rad));
        return cross;
    }
}