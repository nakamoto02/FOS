using UnityEngine;

public abstract class BaseSlider : MonoBehaviour
{
    Transform transformCache;
    //-----------------------------------------------------
    //  Protected
    //-----------------------------------------------------
    [SerializeField] protected BaseFastener parentFastener;     //親ファスナー
    [SerializeField] protected Transform handleTransformCache;  //ハンドル
    [SerializeField] protected Transform dirPoint;              //縦の向き用

    protected float radius;         //当たり判定の半径
    protected int touchID = -1;     //タッチ情報
    protected bool isTouch = false; //タッチフラグ
    //=====================================================
    void Awake ()
    {
        //Transform
        transformCache = transform;
        //CircleColliderの半径
        radius = (dirPoint.position - handleTransformCache.position).magnitude;
	}
    void Update()
    {
        if (isTouch) HandleRotate();
        else AutoAngle();
    }
    //-----------------------------------------------------
    //  持ち手をタッチ方向へ回転
    //-----------------------------------------------------
    protected abstract void HandleRotate();
    //-----------------------------------------------------
    //  自動回転
    //-----------------------------------------------------
    void AutoAngle()
    {
        const float HANDLE_BASE_DIR = 180;
        //持ち手の角度
        float handleDir = handleTransformCache.rotation.eulerAngles.z;

        if (handleDir == HANDLE_BASE_DIR) return;

        //回転
        handleDir = Mathf.Lerp(handleDir, HANDLE_BASE_DIR, 0.1f);

        //更新
        handleTransformCache.rotation = Quaternion.Euler(Vector3.forward * handleDir);
    }
    //-----------------------------------------------------
    //  タッチ位置
    //-----------------------------------------------------
    protected Vector2 TouchOrMousePos()
    {
        //Touch
        if(touchID != -1) return Input.touches[touchID].position;
        //Mouse
        else return Input.mousePosition;
    }
    //-----------------------------------------------------
    //  押された時
    //-----------------------------------------------------
    public abstract void SliderTouchDown(int id);
    //-----------------------------------------------------
    //  離した時
    //-----------------------------------------------------
    public abstract void SliderTouchUp();
}