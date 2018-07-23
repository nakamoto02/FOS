using UnityEngine;

namespace FOS.Sound
{
    public enum BGM
    {
        Title,
        Select,
        Game
    }
    public enum SE
    {
        FollCandy = 0,
        BreakCandy,
        DestroyEnemy,
        ScoreUp,
        ScoreDown,
    }
    public enum Fastener_SE
    {
        Move = 0,
        Back
    }

    public class SoundManager : MonoBehaviour
    {
        //シングルトン
        static public SoundManager instance;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            //BGMSourceを用意
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.volume = 0.3f;
            bgmSource.loop = true;

            //SESourceを用意
            for (int i = 0; i < seSource.Length; i++)
            {
                seSource[i] = gameObject.AddComponent<AudioSource>();
                seSource[i].volume = 0.6f;
                seSource[i].loop = false;
            }

            //SEFSourceを用意
            seFSource = gameObject.AddComponent<AudioSource>();
            seFSource.loop = false;

            //読み込み
            bgmSound = Resources.LoadAll<AudioClip>("Sound/BGM");
            seSound  = Resources.LoadAll<AudioClip>("Sound/SE");
            seFSound = Resources.LoadAll<AudioClip>("Sound/SEF");
        }
        //-------------------------------------------------
        //  Private
        //-------------------------------------------------
        //AudioClip
        AudioClip[] bgmSound;
        AudioClip[] seSound;
        [SerializeField] AudioClip[] seFSound;

        //AudioSource
        AudioSource bgmSource;                          //BGM用
        AudioSource[] seSource = new AudioSource[3];    //SE用
        AudioSource seFSource;
        //-------------------------------------------------
        //  BGM開始
        //-------------------------------------------------
        public void StartSound_BGM(BGM sound)
        {
            bgmSource.clip = bgmSound[(int)sound];
            bgmSource.Play();
        }
        //-------------------------------------------------
        //  SE開始
        //-------------------------------------------------
        public void StartSound_SE(SE sound)
        {
            foreach (AudioSource audio in seSource)
            {
                if (!audio.isPlaying)
                {
                    audio.clip = seSound[(int)sound];
                    if (sound == SE.ScoreUp) audio.volume = 0.3f;
                    else audio.volume = 6.0f;
                    audio.Play();

                    break;
                }
            }
        }
        //-------------------------------------------------
        //  FastenerSE開始
        //-------------------------------------------------
        public void StartSound_SEF(Fastener_SE sound)
        {
            if (IsFMovePlaying(sound)) return;

            //音再生
            seFSource.clip = seFSound[(int)sound];
            seFSource.Play();
        }
        //-------------------------------------------------
        //  FastenerSE作動中か確認
        //-------------------------------------------------
        bool IsFMovePlaying(Fastener_SE sound)
        {
            if (sound != Fastener_SE.Move) return false;
            if (seFSource.clip != seFSound[(int)Fastener_SE.Move]) return false;
            if (!seFSource.isPlaying) return false;
            return true;
        }
        //-------------------------------------------------
        //  FastenerSE停止
        //-------------------------------------------------
        public void StopSound_SEF()
        {
            //音停止
            seFSource.Stop();
        }
    }
}