// CharacterSO.cs
using UnityEngine;

[CreateAssetMenu(menuName = "VN/Character")]
public class CharacterSO : ScriptableObject
{
    [SerializeField]
    private string id; // ví dụ "char.alice"

    [SerializeField]
    private string displayName; // tên hiển thị

    [SerializeField]
    private Sprite defaultIcon; // ảnh đại diện

    [SerializeField]
    private Color nameColor = Color.white;

    public string Id => id;
    public string DisplayName => displayName;
    public Sprite DefaultIcon => defaultIcon;
    public Color NameColor => nameColor;
}
