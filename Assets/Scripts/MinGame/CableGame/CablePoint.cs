using UnityEngine;

public class CablePoint : MonoBehaviour
{
    [SerializeField] private CableColor _color;

    public CableColor Color => _color;
}
