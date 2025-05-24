using System;
using UnityEngine;

public class NpcController : MonoBehaviour
{
    [SerializeField] private StateId currentState = StateId.None;
    [SerializeField] private float movementSpeed = 0.2f;

    public Animator modelAnimator;
    public bool isFacingRight = true;

    public Transform destination;

    public event Action reachedDestination;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SwitchState(currentState, true);
    }

    public void SwitchState(StateId nextState , bool canOverride = false )
    {
        if(currentState == nextState && !canOverride)
        {
            return;
        }

        OnStateExited();
        currentState = nextState;
        OnStateEntered();
    }

    private void OnStateEntered()
    {
        switch (currentState)
        {
            case StateId.Idle:
                if (modelAnimator != null) modelAnimator.Play("Idle");
                break;
            case StateId.Walk:
                if (modelAnimator != null) modelAnimator.Play("Walk");
                break;
            default:
                break;
        }
    }

    private void OnStateExited()
    {
        switch (currentState)
        {
            case StateId.Idle:
                break;
            case StateId.Walk:
                break;
            default:
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case StateId.Idle:
                break;
            case StateId.Walk:
                if (destination != null)
                {
                    FlipFacingDirection(destination.position.x >= transform.position.x ? true : false);
                }
                float facingModifier = isFacingRight ? 1f : -1f;


                // if have destination
                if (destination != null)
                {
                    float designatedPositionAfterTranslate = transform.position.x + movementSpeed * facingModifier;
                    if ((designatedPositionAfterTranslate >= destination.position.x && isFacingRight) || (designatedPositionAfterTranslate <= destination.position.x && !isFacingRight))
                    {
                        transform.position = new Vector2(destination.position.x, transform.position.y); 
                        return;
                    }
                }

                transform.position = new Vector2(transform.position.x + movementSpeed * facingModifier, transform.position.y);
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case StateId.Idle:
                break;
            case StateId.Walk:
                if (destination != null)
                {
                    if (transform.position.x == destination.position.x)
                    {
                        destination = null;
                        SwitchState(StateId.Idle);
                        print("Invoke destination");
                        reachedDestination?.Invoke();
                    }
                }
                break;
            default:
                break;
        }
    }

    public const string ResourceNPCDefaultPath = "NPC/Animator";
    public void Initialize(int npcId)
    {
        GameObject modelPrefab = Resources.Load<GameObject>(ResourceNPCDefaultPath + "/NPC" +npcId + "/NPC" +npcId);
        modelAnimator = Instantiate(modelPrefab, this.transform).GetComponent<Animator>();
    }

    public void FlipFacingDirection(bool facingRight)
    {
        isFacingRight = facingRight;
        float xScale = Mathf.Abs(transform.localScale.x);
        transform.localScale = new Vector3(isFacingRight ? xScale : xScale * -1f, transform.localScale.y, transform.localScale.z);
    }
}
