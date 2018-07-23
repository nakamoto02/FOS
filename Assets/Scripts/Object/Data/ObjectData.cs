using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ObjectData : ScriptableObject
{
    //移動速度
    public float moveSpeed;

    //壊れたときのEffect
    public ObjectDestroy sandEfect;

    //中に入ったときの得点
    [Space]
    public int backInPoint;

    //前に落としたときの得点
    public int frontFallPoint;

    //はさんだときの得点
    public int sandPoint;
}
