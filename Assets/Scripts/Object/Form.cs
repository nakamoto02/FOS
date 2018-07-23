using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Form : MonoBehaviour
{
	float HailItemTime;     //Object生成のインターバル
    int ChangeCreateNumber; //生成する番号 
    int CSVLength;          //CSVファイルのサイズ
    int RandomStageNumber;  //読み込む番号
    int RandomStageNumberD; //ひとつ前に読み込んだ番号
	bool randomFlg = false; //ランダムフラグ

	//CSV
    [SerializeField] TextAsset csvFile;             // CSVファイル
	List<string[]> csvDatas = new List<string[]>(); // CSVの中身を入れるリスト

	StageData stageData;                    //ステージデータ
	[SerializeField] ObjectListData objData;//Objectデータ
	//--------------------------------------------------------
	//  生成開始
	//--------------------------------------------------------
	public void StartCreate()
	{
        ChangeCreateNumber = 0;

		RandomStageNumberD = RandomStageNumber;
		while (randomFlg == false)
		{
			RandomStageNumber = Random.Range(0, stageData.stage.Length);

			if (RandomStageNumber != RandomStageNumberD)
			{
				randomFlg = true;
			}
		}

		randomFlg = false;

		csvFile = stageData.stage[5];
		StringReader reader = new StringReader(csvFile.text);

        csvDatas.Clear();

		while (reader.Peek() > -1)
		{
			string line = reader.ReadLine();
			csvDatas.Add(line.Split(',')); // リストに入れる
		}

		foreach (string[] str in csvDatas)
		{
			CSVLength = str.Length;
		}

        //ScoreManager用意
        ScoreManager.instance.SetObjectNum(CSVLength);

        StartCoroutine(ObjectCreate());
    }
	//-----------------------------------------------------
	//  生成コルーチン
	//-----------------------------------------------------
	IEnumerator ObjectCreate()
	{
		yield return new WaitForSeconds(HailItemTime);

        RandomForm();

        if (ChangeCreateNumber < CSVLength)
		{
			StartCoroutine(ObjectCreate());
		}
	}
    //-----------------------------------------------------
    //  Object生成
    //-----------------------------------------------------
    void RandomForm()
    {
        //値を用意
        int HailItem = int.Parse(csvDatas[0][ChangeCreateNumber]);
        float HailItemArea = float.Parse(csvDatas[1][ChangeCreateNumber]);
        HailItemTime = float.Parse(csvDatas[2][ChangeCreateNumber]);

        //生成
        GameObject prefab = objData.ObjectList[HailItem];
        Instantiate(prefab, new Vector3(HailItemArea, 6.0f, 0.0f), Quaternion.identity);

        //次の番号
        ChangeCreateNumber++;
    }
    public void SetStageData(StageData data)
    {
        stageData = data;
    }
}
