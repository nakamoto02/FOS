using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class DezineData : ScriptableObject
{
    public List<DezineSet> dezine;

    [System.Serializable]
    public class DezineSet
    {
        //ファスナー
        public Anima2D.SpriteMesh fastenerSprite;
        //背景前
        public Sprite frontSprite;
        //背景後
        public Sprite backSprite;
    }
}
