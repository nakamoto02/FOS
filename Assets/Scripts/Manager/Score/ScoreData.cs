using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ScoreData : ScriptableObject
{
    public List<StageScore> stage;

    [System.Serializable]
    public class StageScore
    {
        //回数
        public int num;
        //上限点数
        public int maxScore;
        //最高評価
        public int maxEvaluation;
    }
}