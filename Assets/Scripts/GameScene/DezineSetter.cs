using UnityEngine;

public class DezineSetter : MonoBehaviour
{
    [SerializeField] DezineData data;   //データ

    //SpriteRenderer
    [SerializeField, Space]
    Anima2D.SpriteMeshInstance[] fastenerTape;      //Fastenerのテープ
    [SerializeField] SpriteRenderer[] frontGround;  //背景 (前面)
    [SerializeField] SpriteRenderer backGround;     //背景 (後面)
    //-----------------------------------------------------
    //  見た目を変更
    //-----------------------------------------------------
    public void DezineSet(int no)
    {
        //ファスナー
        if(data.dezine[no].fastenerSprite != null)
        {
            fastenerTape[0].spriteMesh = data.dezine[no].fastenerSprite;
            fastenerTape[1].spriteMesh = data.dezine[no].fastenerSprite;
        }

        //背景 (前面)
        if(data.dezine[no].frontSprite != null)
        {
            frontGround[0].sprite = data.dezine[no].frontSprite;
            frontGround[1].sprite = data.dezine[no].frontSprite;
        }

        //背景 (後面)
        if(data.dezine[no].backSprite != null)
        {
            backGround.sprite = data.dezine[no].backSprite;
        }   
    }
}
