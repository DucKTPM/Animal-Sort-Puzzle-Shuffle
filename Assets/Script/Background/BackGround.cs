using UnityEngine;

public class BackGround : MonoBehaviour
{
    [SerializeField] private SpriteRenderer view;

    private void OnEnable()
    {
        var defaultSize = view.localBounds.size;

        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        var scaleX = width / defaultSize.x;
        var scaleY = height / defaultSize.y;

        gameObject.transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
}