using UnityEngine;

public interface ITouch
{
    void TouchBegan(Vector2 tPos);
    void TouchMove(Vector2 tPos);
    void TouchEnd();
    bool IsTouchRange(Vector2 tPos);
}