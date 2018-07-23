using UnityEngine;

interface ITouch
{
    void TouchBegan(Vector2 pos);
    void TouchMove(Vector2 pos);
    void TouchEnd();
}