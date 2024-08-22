using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool IsComplete { get; protected set; }

    protected float startTime;

    public float time => Time.time - startTime;

    public virtual void Enter() { }

    public virtual void Do() { }

    public virtual void FixedDo() { }

    public virtual void Exit() { }
}
