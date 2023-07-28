// 日本語対応
using UnityEditor;
using UnityEngine;

/// <summary>
/// Entryノードのインスペクタを隠すクラス
/// </summary>
[CustomEditor(typeof(EntryNode))]
public class EntryNodeCustomInspectorDrawer : Editor
{
    // Entryノードはインスペクタに何も表示しない。
    public override void OnInspectorGUI()
    {
    }
}
