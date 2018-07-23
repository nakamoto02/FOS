using UnityEngine;
using FOS.Sound;

public class BaseEnemy : StageObject
{
    const float MOVE_DIR_MIN = -30.0f;
    const float MOVE_DIR_MAX = 30.0f;

    enum MoveState
    {
        Move,
        Stop,
        Streat
    }

    MoveState state;            //現在の状態
    public float ChangeTime;    //行動切り替えまでの時間
    float TimeCnt;              //行動時間
    float RandomChangeAngle;    //移動角度
    float Speed;                //移動速度
    float StopTime;             //停止時間
    //=====================================================
    void Start()
    {
        //初期角度設定
        RandomChangeAngle = Random.Range(MOVE_DIR_MIN, MOVE_DIR_MAX);
        //効果音設定
        SandSE = SE.DestroyEnemy;
    }
    void Update()
    {
        AutoDestroy();

        switch (state)
        {
            case MoveState.Move:
                Move();
                break;
            case MoveState.Stop:
                Stop();
                break;
            case MoveState.Streat:
                StreatMove();
                break;
        }

        //行動変更用のCntを増やす。
        TimeCnt += Time.deltaTime;
    }
    //-----------------------------------------------------
    //  移動
    //-----------------------------------------------------
    void Move()
    {
        //今向いている角度のほうへ動く
        float angleDir = transform.eulerAngles.z * (Mathf.PI / 180.0f);
        Vector3 dir = new Vector3(Mathf.Sin(angleDir), -Mathf.Cos(angleDir), 0.0f);

        // 自身の向きに移動
        transform.position += dir * Speed * Time.deltaTime;

        //方向転換
        ChangeDirection();

        //一定時間(cnt秒)以上動くと行動を切り替える。
        if (TimeCnt >= ChangeTime)
        {
            TimeCnt = 0;
            state = MoveState.Stop;
        }

        // 位置が半分以下なら直進
        if (transform.position.y <= 0.5f)
        {
            state = MoveState.Streat;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
    //-----------------------------------------------------
    //  停止
    //-----------------------------------------------------
    void Stop()
    {
        //一定時間(StopTime)以上止まると行動を切り替える。
        if (TimeCnt >= StopTime)
        {
            TimeCnt = 0;
            //Randの実行（新たな乱数を取得する）。
            Rand();
            state = MoveState.Move;
        }
    }
    //-----------------------------------------------------
    //  乱数取得
    //-----------------------------------------------------
    void Rand()
    {
        const float STOP_TIME_MIN = 0.3f;
        const float STOP_TIME_MAX = 2.0f;

        //新たな角度を決め、その角度のほうへ向ける。
        RandomChangeAngle = Random.Range(MOVE_DIR_MIN, MOVE_DIR_MAX);
        transform.rotation = Quaternion.Euler(0f, 0f, RandomChangeAngle);
        //次に進む速度を停止していた時間から求る。
        ChangeMoveSpeed();
        //次の停止時間を決める。	
        StopTime = Random.Range(STOP_TIME_MIN, STOP_TIME_MAX);
    }
    //-----------------------------------------------------
    //  直進
    //-----------------------------------------------------
    void StreatMove()
    {
        const float STREATE_SPEED = 3.0f;
        transform.position += Vector3.down * STREATE_SPEED * Time.deltaTime;
    }
    //-----------------------------------------------------
    //  方向転換
    //-----------------------------------------------------
    void ChangeDirection()
    {
        const float OUT_AREA_MAX = 1.8f;
        const float OUT_AREA_MIN = -1.8f;
        //エリア外に出そうになったら反転させる。
        if (transform.position.x >= OUT_AREA_MAX)
        {
            ReflectionRight();
        }
        else if (transform.position.x <= OUT_AREA_MIN)
        {
            ReflectionLeft();
        }
    }
    //-----------------------------------------------------
    //  方向転換 (右)
    //-----------------------------------------------------
    void ReflectionRight()
    {
        //新たな角度を決め、その角度のほうへ向ける。
        RandomChangeAngle = Random.Range(MOVE_DIR_MIN, 0);
        transform.rotation = Quaternion.Euler(0, 0, RandomChangeAngle);
        //次に進む速度を停止していた時間から求る。
        ChangeMoveSpeed();
    }
    //-----------------------------------------------------
    //  方向転換 (左)
    //-----------------------------------------------------
    void ReflectionLeft()
    {
        //新たな角度を決め、その角度のほうへ向ける。
        RandomChangeAngle = Random.Range(0, MOVE_DIR_MAX);
        transform.rotation = Quaternion.Euler(0, 0, RandomChangeAngle);
        //次に進む速度を停止していた時間から求る。
        ChangeMoveSpeed();
    }
    //-----------------------------------------------------
    //  移動速度変更
    //-----------------------------------------------------
    void ChangeMoveSpeed()
    {
        const float MOVE_SPEED_MIN = 1.0f;
        const float MOVE_SPEED_MAX = 2.0f;
        Speed = Random.Range(MOVE_SPEED_MIN, MOVE_SPEED_MAX) * (1 + StopTime);
    }
}
