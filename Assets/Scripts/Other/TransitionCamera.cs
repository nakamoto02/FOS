using UnityEngine;

public class TransitionCamera : MonoBehaviour
{
    void Awake()
    {
        //消さない
        DontDestroyOnLoad(this.gameObject);
    }
}
