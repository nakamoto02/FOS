using System.Collections;
using UnityEngine;
using FOS.Sound;

public class ObjectDestroy : MonoBehaviour
{
    //待機時間
    [SerializeField]
    float waitTime = 0;
    //-----------------------------------------------------
    //  挟まれた時
    //-----------------------------------------------------
	public void Sand (int point, SE se)
    {
        //音
        SoundManager.instance.StartSound_SE(se);
        //削除
        StartCoroutine(WaitDestroy(point));
	}
    //-----------------------------------------------------
    //  一定時間後、削除
    //-----------------------------------------------------
    IEnumerator WaitDestroy(int point)
    {
        yield return new WaitForSeconds(waitTime);

        //得点加算
        ScoreManager.instance.AddScore(point);
        //削除
        Destroy(this.gameObject, waitTime);
    }
}