using UnityEngine;

public class TouchFollow : MonoBehaviour
{
    //Component
    Transform transformCache;
    ParticleSystem trail;
    
    Vector2 beforePos;      //前回位置
    bool deadFlg = false;   //削除フラグ
    bool trailFlg = false;  //エフェクトフラグ
    //===========================================
	void Start ()
    {
        DontDestroyOnLoad(this.gameObject);
        transformCache = transform;
        trail = transformCache.GetChild(0).GetComponent<ParticleSystem>();
        beforePos = transformCache.position;
	}
    void Update()
    {
        if (deadFlg) return;

        if (Input.GetMouseButton(0)) Follow();
        else TouchUp();
    }
    //-------------------------------------------
    //  追尾
    //-------------------------------------------
    public void Follow()
    {
        Vector2 touchPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Effectの ON / OFF
        if (trailFlg && beforePos == touchPoint) { trail.Stop(); trailFlg = false; }
        else if (!trailFlg && beforePos != touchPoint) { trail.Play(); trailFlg = true; }
        //位置を更新
        transformCache.position = touchPoint;
        beforePos = touchPoint;
    }
    //-----------------------------------------------------
    //  指が離れた時
    //-----------------------------------------------------
    public void TouchUp()
    {
        const float DESTROY_TIME = 1.0f;
        deadFlg = true;
        trail.Stop();
        Destroy(this.gameObject, DESTROY_TIME);
    }
}