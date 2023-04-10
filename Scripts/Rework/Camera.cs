using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(Camera))]
public class ViewportController : MonoBehaviour
{
    private Camera _camera;

    [Tooltip("Set the target aspect ratio.")]
    [SerializeField] private float _minAspectRatio;
    [SerializeField] private float _maxAspectRatio;

    private void Awake()
    {
        _camera = GetComponent<Camera>();

        if (Application.isPlaying)
            ScaleViewport();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (_camera)
            ScaleViewport();
#endif
    }

    private void ScaleViewport()
    {
        // determine the game window's current aspect ratio
        var windowaspect = Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        var letterboxRatio = windowaspect / _minAspectRatio;
        var pillarboxRatio = _maxAspectRatio / windowaspect;

        // if max height is less than current height, add letterbox
        if (letterboxRatio < 1)
        {
            SetRect(1, letterboxRatio, 0, (1 - letterboxRatio) / 2);
        }
        // if min height is more than current height, add pillarbox
        else if (pillarboxRatio < 1)
        {
            SetRect(pillarboxRatio, 1, (1 - pillarboxRatio) / 2, 0);
        }
        // full screen
        else
        {
            SetRect(1, 1, 0, 0);
        }
    }

    private void SetRect(float w, float h, float x, float y)
    {
        var rect = _camera.rect;

        rect.width = w;
        rect.height = h;
        rect.x = x;
        rect.y = y;

        _camera.rect = rect;
    }
}
