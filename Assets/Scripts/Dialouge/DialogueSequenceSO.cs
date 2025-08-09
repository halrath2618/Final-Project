// DialogueSequenceSO.cs
using System;
using System.Collections.Generic;
using UnityEngine;

public enum EntryType
{
    Line,
    Choice
}

[Serializable]
public class LineData
{
    public CharacterSO speaker;

    [TextArea(2, 5)]
    public string text;
    public string nextId; // để nhảy sang entry khác; trống = tự qua entry kế
}

[Serializable]
public class ChoiceOption
{
    [TextArea(1, 3)]
    public string text;
    public string nextId; // id entry tiếp theo khi chọn option này
}

[Serializable]
public class ChoiceData
{
    public List<ChoiceOption> options = new();
}

[Serializable]
public class DialogueEntry
{
    public string id; // unique trong sequence (vd: "n1", "n2a", ...)
    public EntryType type;
    public LineData line;
    public ChoiceData choice;
}

[CreateAssetMenu(menuName = "VN/Dialogue Sequence")]
public class DialogueSequenceSO : ScriptableObject
{
    public string startId; // id bắt đầu
    public List<DialogueEntry> entries = new();

    // index tra cứu nhanh
    private Dictionary<string, int> _index;

    public void BuildIndex()
    {
        _index = new Dictionary<string, int>(entries.Count);
        for (int i = 0; i < entries.Count; i++)
        {
            var id = string.IsNullOrEmpty(entries[i].id) ? $"auto_{i}" : entries[i].id;
            entries[i].id = id;
            _index[id] = i;
        }
        if (string.IsNullOrEmpty(startId) && entries.Count > 0)
            startId = entries[0].id;
    }

    public DialogueEntry GetById(string id)
    {
        if (_index == null)
            BuildIndex();
        if (!_index.TryGetValue(id, out var idx))
            throw new Exception($"Dialogue id not found: {id}");
        return entries[idx];
    }

    public DialogueEntry GetNextLinear(DialogueEntry current)
    {
        if (_index == null)
            BuildIndex();
        int idx = _index[current.id];
        int next = idx + 1;
        return next < entries.Count ? entries[next] : null;
    }
}
