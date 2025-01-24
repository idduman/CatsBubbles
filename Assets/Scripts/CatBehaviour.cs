using UnityEngine;

public class CatBehaviour : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnMouseDown()
    {
        Jump();
    }

    private void Jump()
    {
        _rb.AddForce(Random.Range(5f,6f) * Vector2.up, ForceMode2D.Impulse);
    }
}