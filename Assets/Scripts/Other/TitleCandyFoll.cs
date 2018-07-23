using System.Collections;
using UnityEngine;
using FOS.Sound;

public class TitleCandyFoll : MonoBehaviour
{
    [SerializeField]
    Object[] candyPreArr;      //CandyPrefab
    Transform candyCreatePos;   //Candy生成位置
    Transform titleEnemy;       //Title用Enemy
    
    //TitleFastener
    [SerializeField, Space]
    TitleFastener titleFastener;

    //=====================================================
	void Start ()
    {
        candyPreArr = Resources.LoadAll("Object/Title") as Object[];
        candyCreatePos = transform.GetChild(0);
        titleEnemy = transform.GetChild(1);
        //生成開始
        StartCoroutine(CandyCreater());
	}
    //-----------------------------------------------------
    //  飴を落とす
    //-----------------------------------------------------
    IEnumerator CandyCreater()
    {
        const int CANDY_NUM = 160;                      //Candyの総数
        const float CREATE_POS_MIN = -0.2f;             //生成位置最低値
        const float CREATE_POS_MAX = 0.2f;              //生成位置最大値
        int candyCnt = 0, candyType = 0, timeCnt = 0;   //カウント
        float[] intervalArr = { 0.2f, 1.0f, 0.2f, 0.1f, 0.01f };
        int[] checkCnt = { 1, 2, 10, 20, -1 };
        float soundTimeMemory = 0;

        //Candyを生成
        while (candyCnt < CANDY_NUM)
        {
            yield return new WaitForSeconds(intervalArr[timeCnt]);

            //効果音
            soundTimeMemory += intervalArr[timeCnt];
            if (soundTimeMemory >= 0.1f)
            {
                soundTimeMemory = 0;
            }
            SoundManager.instance.StartSound_SE(SE.FollCandy);

            //Candyを生成
            Vector3 createPos = candyCreatePos.position;
            createPos.x += Random.Range(CREATE_POS_MIN, CREATE_POS_MAX);
            GameObject candy = (GameObject)Instantiate(candyPreArr[candyType], createPos, Quaternion.identity);
            candy.transform.parent = candyCreatePos;

            //カウント
            candyCnt++;
            candyType = (candyType + 1) % candyPreArr.Length;
            if (checkCnt[timeCnt] == candyCnt) timeCnt++;
        }

        //次の行動へ
        StartCoroutine(EnemyMove());
    }
    //-----------------------------------------------------
    //  Titleへ
    //-----------------------------------------------------
    IEnumerator EnemyMove()
    {
        const float MOVE_WAIT_TIME = 2.0f;  //動作開始までの待機時間
        const float MOVE_SPEED = 2.0f;      //移動速度
        const float DOWN_HEIGHT = 4.0f;    //下がる高さ
        
        yield return new WaitForSeconds(MOVE_WAIT_TIME);

        //移動
        Vector3 nextPos = titleEnemy.localPosition;
        Vector3 moveDir = -titleEnemy.up;
        while (nextPos.y > DOWN_HEIGHT)
        {
            nextPos += moveDir * MOVE_SPEED * Time.deltaTime;
            titleEnemy.localPosition = nextPos;
            yield return null;
        }
        
        titleFastener.gameObject.SetActive(true);
    }
}
