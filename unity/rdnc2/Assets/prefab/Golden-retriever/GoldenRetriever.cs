
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
    private Camera mainCam;
    public int dragSpeed = 5;
    public LayerMask layerMask;

    private const string ANIMATION = "Animation";
    private const string WATER = "Water";
    private const string STATIC_OBJ = "Static_Object";
    private const string ANIMAL = "Animal";

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

        OnMouseEventHandler();

        if (DateTime.Now > state.endTime)
        {
            SetState(GetAnim(), GetDirection(), GetDuration());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag(WATER))
        {
            SetState((int)animEnum.Swim, Vector3.forward, GetDuration());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag(WATER))
        {
            SetState(GetMoveAnim(), Vector3.forward, GetDuration());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag(STATIC_OBJ))
        {
            SetState(GetAnim(), GetDirection(), GetDuration());
        }
        if (collision.transform.CompareTag(ANIMAL))     // TODO: distinct duck, dog/cat, panda
        {
            SetState(GetAnim(), GetDirection(), GetDuration());
            // bark audio
        }
    }


    private bool isThisSelected = false;
    private void OnMouseEventHandler()
    {
        if (Input.GetMouseButtonDown(0))    // TODO: multiple fingers?
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
            {
                if (hit.transform == this)
                {
                    Debug.Log("hit this obj");
                    isThisSelected = true;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isThisSelected = false;
        }

        if (isThisSelected) // during hold
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, layerMask))        // to limit drag on land only?
            {
                var dragDirection = hit.point - transform.position; // TODO: enhance with a color line from target to obj
                dragDirection.y = 0;

                SetState((int)animEnum.Lie, dragDirection.normalized, 1);   // TODO: enhance water trigger change anim
            }

        }

    }

    #endregion

    #region pool life cycle
    // for pool get
    public void Init(Vector3 spawnPosition)
    {
        weight = UnityEngine.Random.Range(5, 10);       // TODO: dog specific, from input
        gameObject.transform.position = spawnPosition;

        SetState((int)animEnum.Run, Vector3.forward, 3); // pass in init cool time
    }

    // for pool create
    public void Init(Vector3 spawnPosition, Action<GoldenRetriever> rdfn)
    {
        animator = GetComponent<Animator>();
        releaseFn = rdfn;
        Init(spawnPosition);
        mainCam = Camera.main;
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
        if (animator.GetInteger(ANIMATION) == (int)animEnum.Swim) return (int)animEnum.Swim;

        return UnityEngine.Random.Range(0, 1) > 0.3f ? GetIdleAnim() : GetMoveAnim();// TODO: input idle move ratio
    }

    private int GetDuration()
    {
        return UnityEngine.Random.Range(2, 10);   // pass in range
    }

    private Vector3 GetDirection()
    {
        var direction = UnityEngine.Random.insideUnitCircle;
        direction.y = 0f;
        return direction;
    }

    private int GetSpeed(int anim)
    {
        if (isThisSelected) return dragSpeed;

        var speed = SPEED_IDLE;

        if (anim == (int)animEnum.Run) speed = SPEED_RUN;
        else if (anim == (int)animEnum.Walk) speed = SPEED_WALK;
        else if (anim == (int)animEnum.Swim) speed = SPEED_SWIM;

        return speed;
    }

    private void SetState(int anim, Vector3 direction, int duration)
    {
        state = new State
        {
            anim = anim,
            direction = direction,
            endTime = DateTime.Now.AddSeconds(duration)
        };
        animator.SetInteger(ANIMATION, state.anim);
        animator.speed = GetSpeed(state.anim);
    }
    #endregion
}
