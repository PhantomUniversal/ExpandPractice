using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIData", menuName = "SO/UIData", order = int.MaxValue)]
public class UIData : ScriptableObject
{
    public List<UITable> table;
}