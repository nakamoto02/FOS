using UnityEngine;

public class FastenerSetter : MonoBehaviour
{
    const float FASTENER_RANGE = 6.0f;  //Fastener同士の距離

    //Transform
    Transform transformCache;

    [SerializeField] DezineData dezineData;                 //
    [SerializeField, Space] DezineSetter fastenerSetPre;    //FastenerSetのプレファブ
    [SerializeField] DezineSetter[] dezineSetterArr;        //

    int memory = -1;        //前回のデザイン番号
    bool isMove = false;    //動作フラグ

    //=====================================================
    void Start ()
    {
        transformCache = transform;
    }
    void Update()
    {
        if (isMove) SetMove();
    }
    //-----------------------------------------------------
    //  移動
    //-----------------------------------------------------
    void SetMove()
    {
        //移動先
        Vector2 movePos = transformCache.position;
        //移動量
        movePos.x = Mathf.Lerp(movePos.x, -FASTENER_RANGE * 1.1f, 0.1f);
        movePos.x = Mathf.Max(movePos.x, -FASTENER_RANGE);

        //確認
        if (movePos.x <= -FASTENER_RANGE)
        {
            isMove = false; //停止
            movePos.x = 0;  //位置調整
            dezineSetterArr[0].transform.localPosition = movePos;
            //削除
            Destroy(dezineSetterArr[1].gameObject);
            //オブジェクト生成開始
            GameManager.instance.StartGame_Create();
        }
        //更新
        transformCache.position = movePos;
    }
    //-----------------------------------------------------
    //  新しいファスナーを生成
    //-----------------------------------------------------
    public void SetFastener()
    {
        //生成
        DezineSetter fastener = Instantiate(fastenerSetPre);
        //デザインを変更
        int dezineNo;
        do
        {
            dezineNo = Random.Range(0, dezineData.dezine.Count - 1);
        } while (dezineNo == memory);

        fastener.DezineSet(dezineNo);
        //記録
        memory = dezineNo;

        dezineSetterArr[0] = fastener;
        dezineSetterArr[0].transform.parent = transformCache;
    }
    //-----------------------------------------------------
    //  動作開始
    //-----------------------------------------------------
    public void NextFastener()
    {
        //生成
        dezineSetterArr[1] = dezineSetterArr[0];
        SetFastener();
        //位置
        Vector2 fasPos = new Vector2(FASTENER_RANGE, 0);
        dezineSetterArr[0].transform.localPosition = fasPos;
        //動作開始
        isMove = true;
    }
}
