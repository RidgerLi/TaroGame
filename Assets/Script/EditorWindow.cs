using UnityEngine;
using UnityEditor;

public class MyTool : EditorWindow
{
    [MenuItem("Tool/My Tool")]
    public static void ShowWindow()
    {
        GetWindow<MyTool>("My Tool");
    }

    [SerializeField]
    public Object CardManager;

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        CardManager = EditorGUILayout.ObjectField(CardManager, typeof(GameObject), true);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Run Card Manager Jitter Card"))
        {
            JitterCard();
        }
    }

    private void JitterCard()
    {
        Debug.Log("JitterCard");
        var jitter = (CardManager as GameObject).GetComponent<CardManager>();
        jitter.JitterCard();
    }
}