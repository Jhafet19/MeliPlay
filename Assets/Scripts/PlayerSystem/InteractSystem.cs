using System;
using UnityEngine;

public class InteractSystem : MonoBehaviour
{
    [Header("Configuración")]
    public float detectionDistance = 1f; // Distancia para detectar la caja
    public LayerMask boxLayer;

    [Header("Referencias")]
    public FixedJoint2D joint;

    private GameObject _currentBox;
    
    void Start()
    {
        joint.enabled = false;
    }

    void Update()
    {
        
        Vector2 origin = transform.position;
        
        float direction = GetComponentInChildren<SpriteRenderer>().flipX ? -1 : 1;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right * direction, detectionDistance, boxLayer);

        if (Input.GetKey(KeyCode.E))
        {
            if (hit.collider != null && hit.collider.gameObject != _currentBox)
            {
                if (joint.connectedBody == null)
                {
                    ConnectToBox(hit.collider.gameObject);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.E))
        {
            DisconnectBox();
        }
    }

    void ConnectToBox(GameObject box)
    {
        _currentBox = box;
        Rigidbody2D boxRb = box.GetComponent<Rigidbody2D>();
        boxRb.constraints = RigidbodyConstraints2D.FreezeRotation;
        // Configurar el Joint
        joint.connectedBody = boxRb;
        joint.enabled = true; 
        KnightController.Instance.isGrabbingBox = true;
    }

    void DisconnectBox()
    {
        if (_currentBox != null)
        {
            Rigidbody2D boxRb = _currentBox.GetComponent<Rigidbody2D>();
            
            boxRb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        }
        joint.enabled = false;
        joint.connectedBody = null;
        _currentBox = null;
        KnightController.Instance.isGrabbingBox = false;
    }

    private void OnDrawGizmos()
    {
        float direction = 1;
        if(GetComponentInChildren<SpriteRenderer>() != null)
             direction = GetComponentInChildren<SpriteRenderer>().flipX ? -1 : 1;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * direction * detectionDistance);
    }
}
