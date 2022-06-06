using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gun : MonoBehaviour
{
   protected float fps = 30;
    protected float fpsCounter = 0f;
    protected int animationStep = 0;
    public abstract void RightClick();
    public virtual void RightClickOff() { }
    public abstract void LeftClick();
    public virtual void Reload() { }
    public virtual void MiddleMouseUp() { }
    public virtual void MiddleMouseDown() { }
    public virtual void KeyboardE() { }
    public virtual void KeyboardQ() { }
   
}
