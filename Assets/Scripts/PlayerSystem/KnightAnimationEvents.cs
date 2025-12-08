using UnityEngine;

public class KnightAnimationEvents : MonoBehaviour
{
    private KnightController _controller;

    private void Start()
    {
        // Buscamos el script en el objeto padre (Knight)
        _controller = GetComponentInParent<KnightController>();
    }

    // Esta es la función que llamará la Animación
    public void TriggerStepSound()
    {
        if (_controller != null)
        {
            _controller.PlayStepSound();
        }
    }
}
