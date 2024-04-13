using UnityEngine;

public class MiniMapActive : MonoBehaviour
{
    [SerializeField] bool isActive;
    private void Awake()
    {
        GameManager.Instance.miniMap.SetActive(isActive);
    }
}
