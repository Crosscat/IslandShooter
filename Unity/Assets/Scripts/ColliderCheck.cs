using UnityEngine;
using System.Collections;

public class ColliderCheck : MonoBehaviour
{
    public LayerMask LayerCheck;

    public bool IsColliding
    {
        get;
        private set;
    }

    public Collider OtherCollider
    {
        get;
        private set;
    }

    void FixedUpdate()
    {
        IsColliding = false;
        OtherCollider = null;
    }

    void Update()
    {
        if (OtherCollider == null)
        {
            IsColliding = false;
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        HandleTrigger(collider);
    }

    void OnTriggerStay(Collider collider)
    {
        HandleTrigger(collider);
    }

    void HandleTrigger(Collider collider)
    {
        if (((1 << collider.gameObject.layer) & LayerCheck) != 0)
        {
            IsColliding = true;
            OtherCollider = collider;
        }
    }

    void OnDrawGizmos()
    {
        if (IsColliding && OtherCollider != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, OtherCollider.transform.position);
        }
    }
}
