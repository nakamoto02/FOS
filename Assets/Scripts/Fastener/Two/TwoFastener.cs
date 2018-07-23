using System.Collections.Generic;
using UnityEngine;
using FOS.Sound;

public class TwoFastener : BaseFastener
{
    const float POINT_MOVE_NUM = 0.2f;
    const float SLIDER_TWO_RANGE = 0.9f;
    const float BONE_MAX_DIR = 36.0f;

    //Transform
    [SerializeField] Transform endPoint;        //終点
    [SerializeField] Transform dirPointList;    //
    [SerializeField, Space] Transform[] sliderArr = new Transform[2];   //スライダー

    //SpriteMesh
    [SerializeField]
    Anima2D.SpriteMeshInstance[] spriteMeshs = new Anima2D.SpriteMeshInstance[2];

    //Mask用オブジェクト
    [SerializeField, Space]
    Transform maskObj;

    //スライダーの可動距離
    float sliderRange;

    //スライダーの移動値
    float[] moveValueArr = { 0.0f, 1.0f }, memoryValueArr = { 0.0f, 1.0f};

    //触れている物リスト
    List<StageObject> touchList = new List<StageObject>();

    
    int boneNum;    //Bone数 (1つのテープに対しての個数)
    //Transform
    Transform[] boneArr;

    int backLayerNo;    //背面Layer

    //=====================================================
    void Start()
    {
        //Transformを取得
        transformCache = transform;
        //DirPointを生成
        GetBoneDirPoint();
        //Sliderの可動域
        sliderRange = (endPoint.position - refarencePoint.position).magnitude - SLIDER_TWO_RANGE;
        //BackGroundレイヤーを取得
        backLayerNo = LayerMask.NameToLayer("BackGround");
    }
    void Update()
    {
        if (touchList.Count != 0) SandCheck();
    }
    //-----------------------------------------------------
    //  Boneの取得
    //-----------------------------------------------------
    void GetBoneDirPoint()
    {
        //Boneを取得
        boneNum = spriteMeshs[0].bones.Count;
        boneArr = new Transform[boneNum * 2];

        //boneのtransformを取得し、
        for (int i = 0; i < boneNum; i++)
        {
            //Boneを取得
            Anima2D.Bone2D bone = spriteMeshs[0].bones[i];

            //boneのtransformを取得
            boneArr[i] = bone.transform;
            boneArr[i + boneNum] = spriteMeshs[1].bones[i].transform;
        }
    }
    //-----------------------------------------------------
    //  Objectを挟んだか確認
    //-----------------------------------------------------
    void SandCheck()
    {
        foreach (StageObject obj in touchList)
        {
            //挟んだとき
            if (obj != null)
                if (!ObjectValue(obj)) obj.SandObject();
        }
    }
    //-----------------------------------------------------
    //  ObjectがFastenerの開いている場所かどうか
    //-----------------------------------------------------
    bool ObjectValue(StageObject obj)
    {
        const float SLIDER_HALF_SIZE = 0.45f;

        //位置
        Vector2 objPos = obj.transform.position;
        Vector2 matchPoint = GetMatchPoint(refarencePoint.position, endPoint.position, objPos);

        Vector2 startPos = new Vector2(refarencePoint.position.x + SLIDER_HALF_SIZE, refarencePoint.position.y);
        Vector2 endPos = new Vector2(endPoint.position.x - SLIDER_HALF_SIZE, endPoint.position.y);

        //敵と点の距離
        float enemyToStartDif = (matchPoint - startPos).magnitude - obj.GetRadius();
        float enemyToEndDif = (matchPoint - endPos).magnitude - obj.GetRadius();

        //割合
        enemyToStartDif = enemyToStartDif / sliderRange;
        enemyToEndDif = enemyToEndDif / sliderRange;

        //開いているところでない
        if (moveValueArr[0] >= enemyToStartDif) return false;
        if (moveValueArr[1] >= enemyToEndDif) return false;

        return true;
    }
    //-----------------------------------------------------
    //  Sliderの位置を更新
    //-----------------------------------------------------
    public override void SliderTouchMove(int sliderID, Vector2 sliderPos, Vector2 touchPos, float radius, float side)
    {
        //Sliderの可動域を更新
        sliderRange = (endPoint.position - refarencePoint.position).magnitude - SLIDER_TWO_RANGE;

        //交点(WorldPosition)
        Vector2 matchPoint = GetMatchPoint(refarencePoint.position, endPoint.position, touchPos);

        //移動量
        float moveDif = (sliderPos - matchPoint).magnitude;
        moveDif = Mathf.Lerp(0, moveDif, (moveDif - radius) / moveDif);

        //スライダーの移動方向
        if (side < 0) moveDif = -moveDif;

        //スライダー移動用の値
        moveValueArr[sliderID] = moveValueArr[sliderID] + (moveDif / sliderRange);
        moveValueArr[sliderID] = Mathf.Clamp(moveValueArr[sliderID], 0, 1);

        //合計値
        float totalValue = TotaleMoveValue();
        if (totalValue > 1)
        {
            int otherID = (sliderID == 0) ? 1 : 0;
            moveValueArr[otherID] -= totalValue - 1;
            memoryValueArr[otherID] = moveValueArr[otherID];
        }

        //音再生
        float dif = Mathf.Abs(moveValueArr[sliderID] - memoryValueArr[sliderID]);
        if (dif > 0.02f)
            SoundManager.instance.StartSound_SEF(Fastener_SE.Move);
        memoryValueArr[sliderID] = moveValueArr[sliderID];

        //適用
        SliderLoad();
        BoneRotation();
        FastenerMove();
    }
    //-----------------------------------------------------
    //  Sliderの位置適用
    //-----------------------------------------------------
    void SliderLoad()
    {
        for (int i = 0; i < sliderArr.Length; i++)
        {
            //移動位置
            Vector2 movePos = sliderArr[i].localPosition;

            movePos.x = (i == 0) ? refarencePoint.localPosition.x : endPoint.localPosition.x;
            movePos.x += (i == 0) ? sliderRange * moveValueArr[i] : sliderRange * -moveValueArr[i];

            //更新
            sliderArr[i].localPosition = movePos;
        }
    }
    //-----------------------------------------------------
    //  Boneの回転
    //-----------------------------------------------------
    void BoneRotation()
    {
        //基準値
        float refa = 1.0f / (boneNum - 2);
        float refaDir = BONE_MAX_DIR * (1 - TotaleMoveValue());


        //閉じていたとき
        if (TotaleMoveValue() == 1 || (1 - TotaleMoveValue()) < refa)
        {
            for (int i = 1; i < boneNum; i++)
                boneArr[i].eulerAngles = boneArr[i + boneNum].eulerAngles = new Vector3(0, 0, 180);

            return;
        }

        //準備
        int boneNo = (int)((moveValueArr[1] / refa));
        int moveBoneNum = (int)((moveValueArr[1] + (1 - TotaleMoveValue())) * 10);
        int count = 0;

        bool isFlg = false;

        //角度
        for (int i = 0; i < boneNum - 1; i++)
        {
            Vector3 nextDir = new Vector3(0, 0, 0);

            if (i == boneNo) //曲がりはじめ
            {
                nextDir.z = -refaDir;
                count++;
            }
            else if (1 == moveBoneNum - boneNo && count == 1)    //曲がりが一つだった場合
            {
                nextDir.z = refaDir * 2;
                isFlg = true;
                count++;
            }
            else if (count >= 1 && count < moveBoneNum - boneNo)    //曲がっている途中
            {
                nextDir.z = (refaDir * 2) / (moveBoneNum - boneNo - 1);
                count++;
            }
            else if (count == moveBoneNum - boneNo || isFlg)  //曲がり切った後
            {
                nextDir.z = -refaDir;
                isFlg = false;
                count++;
            }

            //Boneの向き更新
            boneArr[i + 1].localEulerAngles = nextDir;
            boneArr[i + 1 + boneNum].localEulerAngles = -nextDir;

        }

        //一番後ろのBone
        boneArr[boneNum - 1].eulerAngles = new Vector3(0, 0, 180);
        boneArr[boneNum * 2 - 1].eulerAngles = new Vector3(0, 0, 180);
    }
    //-----------------------------------------------------
    //  Fastenerの位置を調整
    //-----------------------------------------------------
    void FastenerMove()
    {
        //スライダーに合わせてFastenerの位置をずらす
        Vector2 fastenerPos = transformCache.localPosition;
        fastenerPos.x = -POINT_MOVE_NUM * (1 -TotaleMoveValue());

        //スライダーに合わせてrefarencePointの位置をずらす
        Vector2 startPos = refarencePoint.localPosition;
        startPos.x = -2.8f + (POINT_MOVE_NUM * 2) * (1 - TotaleMoveValue());

        //更新
        transformCache.localPosition = fastenerPos;
        refarencePoint.localPosition = startPos;

        //MaskObjの大きさを更新
        Vector3 maskSize = new Vector3(2.8f, 1.8f, 1.0f);
        maskSize.x = maskSize.x * (1 - TotaleMoveValue());
        maskSize.y = maskSize.y * (1 - TotaleMoveValue());
        maskObj.localScale = maskSize;

        //MaskObjの位置を更新
        Vector2 maskPos = new Vector3(0, 0, 0);
        float moveRange = (sliderArr[1].position - sliderArr[0].position).magnitude / 2;
        maskPos.x = sliderArr[0].position.x + moveRange;
        maskObj.position = maskPos;
    }
    //-----------------------------------------------------
    //  SliderMoveの合計値
    //-----------------------------------------------------
    float TotaleMoveValue()
    {
        return moveValueArr[0] + moveValueArr[1];
    }
    //---------------------------------------------------------------------------------------------
    //  当たり判定
    //---------------------------------------------------------------------------------------------
    void OnTriggerEnter2D(Collider2D coll)
    {
        //敵かアイテム
        if(coll.tag == "StageObject")
        {
            StageObject obj = coll.GetComponent<StageObject>();

            //触れているものリストに追加
            if(ObjectValue(obj))
            {
                touchList.Add(obj);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D coll)
    {
        //敵かアイテム
        if(coll.tag == "StageObject")
        {
            //触れているものリストから削除
            StageObject obj = coll.GetComponent<StageObject>();
            touchList.Remove(obj);

            //Layerを変更
            if (obj != null)
                if(ObjectValue(obj)) obj.gameObject.layer = backLayerNo;
        }
    }
}