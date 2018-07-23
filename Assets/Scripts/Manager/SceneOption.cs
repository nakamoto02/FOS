using UnityEngine;
using UnityEngine.SceneManagement;

namespace FOS.Scene
{
    public enum SceneName
    {
        TitleScene,
        SelectScene,
        GameScene
    }

    public class SceneOption : MonoBehaviour
    {
        //シングルトン
        static public SceneOption instance;

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
            }
        }
        //-------------------------------------------------
        //  Private
        //-------------------------------------------------
        [SerializeField] TransitionFastener transFastenerPre;
        //-------------------------------------------------
        //  シーン遷移
        //-------------------------------------------------
        public void LoadScene(SceneName name)
        {
            //SelectSceneをLoad
            SceneManager.LoadScene(name.ToString());
        }
        //-------------------------------------------------
        //  ゲームシーンへ遷移
        //-------------------------------------------------
        public void LoadGameScene(int stageNo)
        {
            GameManager.instance.GameSet(stageNo);

            //TransitionFastenerを生成
            TransitionFastener trans = Instantiate(transFastenerPre);
            trans.SceneSetting(SceneName.GameScene);
        }
        //-------------------------------------------------
        //  セレクトシーンへ遷移
        //-------------------------------------------------
        public void LoadSelectScene()
        {
            //TransitionFastenerを生成
            TransitionFastener trans = Instantiate(transFastenerPre);
            trans.SceneSetting(SceneName.SelectScene);
        }
        //-------------------------------------------------
        //  引数が現在のシーンかどうか
        //-------------------------------------------------
        public bool IsSceneName(string name) { return SceneManager.GetActiveScene().name == name; }
    }
}