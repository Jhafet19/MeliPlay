using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingPlatformCoroutine : MonoBehaviour
{
    [Header("Move Points")]
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [Header("Movement Settings")]
    [SerializeField] private float speed = 2f;       // unidades por segundo
    [SerializeField] private float waitAtPoint = 0.5f;

    private Rigidbody2D rb;
    private Vector2 posA;
    private Vector2 posB;

    private Coroutine moveRoutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void Start()
    {
        // Auto-busca hijos si no los asignaron en el inspector
        if (pointA == null || pointB == null)
        {
            var mp = transform.Find("MovePoints");
            if (mp != null)
            {
                pointA = mp.Find("PointA");
                pointB = mp.Find("PointB");
            }
        }

        if (pointA == null || pointB == null)
        {
            Debug.LogError("MovingPlatformCoroutine: faltan PointA / PointB.");
            enabled = false;
            return;
        }

        // Guardamos posiciones en mundo (aunque los puntos sean hijos)
        posA = pointA.position;
        posB = pointB.position;

        moveRoutine = StartCoroutine(MoveLoop());
    }

    private IEnumerator MoveLoop()
    {
        Vector2 from = posA;
        Vector2 to = posB;

        while (true)
        {
            // Mover de from -> to
            float distance = Vector2.Distance(from, to);
            float t = 0f;

            while (t < 1f)
            {
                // Progreso normalizado en función de la velocidad
                t += (speed / distance) * Time.fixedDeltaTime;

                Vector2 newPos = Vector2.Lerp(from, to, t);
                rb.MovePosition(newPos);

                // Usamos FixedUpdate en corrutina para respetar physics
                yield return new WaitForFixedUpdate();
            }

            // Aseguramos posición final exacta
            rb.MovePosition(to);

            // Espera en el extremo
            if (waitAtPoint > 0f)
                yield return new WaitForSeconds(waitAtPoint);

            // Intercambia puntos
            var temp = from;
            from = to;
            to = temp;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawSphere(pointA.position, 0.08f);
            Gizmos.DrawSphere(pointB.position, 0.08f);
        }
    }
}
