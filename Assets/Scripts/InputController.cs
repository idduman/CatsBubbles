using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] public Camera _camera;

    private LayerMask _catMask;
    private void Start()
    {
        _catMask = LayerMask.GetMask("Cat");
    }
    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            var hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero,
                Mathf.Infinity, _catMask);
            if (hit && hit.collider.TryGetComponent<CatBehaviour>(out var cat))
            {
                cat.Jump();
            }
        }
    }
}
