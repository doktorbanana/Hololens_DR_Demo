using UnityEngine;

public interface ILaserPointerBehaviour
{
    void EnableHighlight();

    void DisableHighlight();
}

public class LaserPointerBehaviour : MonoBehaviour, ILaserPointerBehaviour
{
    private Material material_;
    private readonly Color normalColor_ = new Color(0f, 1f, 0.4f, 0.3f);
    private readonly Color highlightColor_ = new Color(0.7f, 1f, 0.7f, 0.9f);

    private void Awake()
    {
        material_ = GetComponent<Renderer>().material;
    }

    public void EnableHighlight()
    {
        material_.color = highlightColor_;
    }

    public void DisableHighlight()
    {
        material_.color = normalColor_;
    }
}
