using UnityEngine;
using FOS.Sound;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class StageObject : MonoBehaviour
{
    const int BACK = 0;     //背面
    const int FRONT = 1;    //前面
    //-----------------------------------------------------
    //  Private
    //-----------------------------------------------------

    //ポイントデータ
    [SerializeField] ObjectData data;

    //SpriteRenderer
    SpriteRenderer spriteRenderer;

    //Layer番号
    int[] layerNo = new int[2]; //0.BackGround, 1.Object

    //Colliderの半径
    float radius;

    //死亡フラグ
    bool isDead = false;
    //-----------------------------------------------------
    //  Protected
    //-----------------------------------------------------

    protected Transform transformCache;

    //移動速度
    protected float moveSpeed;
    //効果音
    protected SE SandSE { private get; set; }

    //=====================================================
    private void Awake()
    {
        transformCache = transform;

        //SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        //CircleColliderの半径
        radius = GetComponent<CircleCollider2D>().radius * transformCache.localScale.x;

        //移動速度を取得
        moveSpeed = data.moveSpeed;

        //Layer
        layerNo[BACK] = LayerMask.NameToLayer("BackGround");
        layerNo[FRONT] = LayerMask.NameToLayer("Object");
    }
    //-----------------------------------------------------
    //  挟んだとき
    //-----------------------------------------------------
    public void SandObject()
    {
        if (isDead) return;

        //フラグをtrueに
        isDead = true;
        //1フレーム待って、削除
        Invoke("DestroyObject", Time.deltaTime);
    }
    //-----------------------------------------------------
    //  自動削除
    //-----------------------------------------------------
    protected void AutoDestroy()
    {
        const float DESTROY_HEIGHT_FRONT = -5.2f;   //消去する高さ (前面)
        const float DESTROY_HEIGHT_BACK = -2.0f;    //消去する高さ (背面)

        if (gameObject.layer == layerNo[BACK] && transformCache.position.y > DESTROY_HEIGHT_BACK) return;
        else if (gameObject.layer == layerNo[FRONT] && transformCache.position.y > DESTROY_HEIGHT_FRONT) return;

        //前面
        if(gameObject.layer == LayerMask.NameToLayer("Object")) FrontFallObject();
        //後面
        else if(gameObject.layer == LayerMask.NameToLayer("BackGround")) BackInObject();

        //削除
        Destroy(gameObject);
    }
    //-----------------------------------------------------
    //  得点加算 (前面)
    //-----------------------------------------------------
    void FrontFallObject()
    {
        ScoreManager.instance.AddScore(data.frontFallPoint);
    }
    //-----------------------------------------------------
    //  得点加算 (背面)
    //-----------------------------------------------------
    void BackInObject()
    {
        ScoreManager.instance.AddScore(data.backInPoint);
    }
    //-----------------------------------------------------
    //  削除
    //-----------------------------------------------------
    void DestroyObject()
    {
        //Effect生成
        ObjectDestroy efe = Instantiate(data.sandEfect, transformCache.position, Quaternion.identity);
        efe.Sand(data.sandPoint, SandSE);
        //削除
        Destroy(gameObject);
    }
    //-----------------------------------------------------
    //  Colliderの半径
    //-----------------------------------------------------
    public float GetRadius()
    {
        return radius;
    }
}
