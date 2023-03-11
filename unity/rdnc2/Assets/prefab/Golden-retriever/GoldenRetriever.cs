
using System;
using UnityEngine;

public class State
{
    public int anim { get; set; }
    public DateTime endTime { get; set; }
    public Vector3 direction { get; set; }
}


public class GoldenRetriever : MonoBehaviour
{
    private State state;
    private Animator animator;
    public int weight;
    private Action<GoldenRetriever> releaseFn;

    private const string ANIM = "anim";
    private const string WATER = "Water";
    private const int SPEED_RUN = 3;
    private const int SPEED_WALK = 2;
    private const int SPEED_SWIM = 2;
    private const int SPEED_IDLE = 0;

    private enum animEnum
    {
        Lie, Sit, Walk, Swim, Run   // order by movement speed; TODO: pass in?
    }

    #region unity events
    private void Update()
    {
        transform.Translate(state.direction * GetSpeed(state.anim) * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(state.direction), 0.15f);

        if (DateTime.Now > state.endTime)
        {
            SetState(GetAnim(), GetDirection(), GetEndTime());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(WATER))
        {
            SetState((int)animEnum.Swim, Vector3.forward, GetEndTime());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag(WATER))
        {
            SetState(GetMoveAnim(), Vector3.forward, GetEndTime());
        }
    }
    #endregion

    #region pool life cycle
    // for pool get
    public void Init(Vector3 spawnPosition)
    {
        weight = UnityEngine.Random.Range(5, 10);       // TODO: dog specific, from input
        gameObject.transform.position = spawnPosition;

        SetState((int)animEnum.Run, Vector3.forward, DateTime.Now.AddSeconds(3)); // pass in init cool time
    }

    // for pool create
    public void Init(Vector3 spawnPosition, Action<GoldenRetriever> rdfn)
    {
        animator = GetComponent<Animator>();
        releaseFn = rdfn;
        Init(spawnPosition);
    }

    private void Release()  // TODO: when get, release one out of sight, if no inst to release, then create
    {
        releaseFn?.Invoke(this);
    }
    #endregion

    #region util
    private int GetIdleAnim()
    {
        return (int)(UnityEngine.Random.Range(0, 1) > 0.5 ? animEnum.Lie : animEnum.Sit);
    }

    private int GetMoveAnim()
    {
        return (int)(UnityEngine.Random.Range(0, 1) > 0.5 ? animEnum.Walk : animEnum.Run);
    }

    private int GetAnim()
    {
        if (animator.GetInteger(ANIM) == (int)animEnum.Swim) return (int)animEnum.Swim;

        return UnityEngine.Random.Range(0, 1) > 0.3f ? GetIdleAnim() : GetMoveAnim();// TODO: input idle move ratio
    }

    private DateTime GetEndTime()
    {
        return DateTime.Now.AddSeconds(UnityEngine.Random.Range(2, 10));   // pass in
    }

    private Vector3 GetDirection()
    {
        var direction = UnityEngine.Random.insideUnitCircle;
        direction.y = 0f;
        return direction;
    }

    private int GetSpeed(int anim)
    {
        var speed = SPEED_IDLE;

        if (anim == (int)animEnum.Run) speed = SPEED_RUN;
        else if (anim == (int)animEnum.Walk) speed = SPEED_WALK;
        else if (anim == (int)animEnum.Swim) speed = SPEED_SWIM;

        return speed;
    }






    private void SetState(int anim, Vector3 direction, DateTime endTime)
    {
        state = new State
        {
            anim = anim,
            direction = direction,
            endTime = endTime
        };
        animator.SetInteger(ANIM, state.anim);
    }
    #endregion



}
