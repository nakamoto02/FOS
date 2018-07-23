using UnityEngine;
using FOS.Sound;

public class BaseItem : StageObject
{
    //=====================================================
    void Start()
    {
        SandSE = SE.BreakCandy;
    }
    void Update()
    {
        FallObject();   //落下
        AutoDestroy();  //削除
    }
    //-----------------------------------------------------
    //  落下
    //-----------------------------------------------------
    void FallObject()
    {
        //位置
        Vector2 movePos = transformCache.position;
        //移動
        movePos.y -= moveSpeed * Time.deltaTime;
        //更新
        transformCache.position = movePos;
    }
}