


using UnityEditor;
using UnityEngine;

public class WirelessDebugHelpWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private string helpContent;
    private string windowTitle;

    public static void ShowHelp(string title, string content)
    {
        var window = GetWindow<WirelessDebugHelpWindow>(title);
        window.helpContent = content;
        window.windowTitle = title;
        window.minSize = new Vector2(500, 600);
        window.maxSize = new Vector2(800, 800);
        window.Show();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
         
        EditorGUILayout.LabelField(windowTitle, EditorStyles.boldLabel);
        EditorGUILayout.Space();

        float headerHeight = 50f; 
        float buttonHeight = 40f;
        float availableHeight = position.height - headerHeight - buttonHeight;

        scrollPosition = EditorGUILayout.BeginScrollView(
            scrollPosition, 
            GUILayout.Height(availableHeight)  // 🆕 명시적 높이 설정
        );
        
        // 🆕 텍스트 영역 스타일 개선
        GUIStyle textStyle = new GUIStyle(EditorStyles.label)
        {
            wordWrap = true,
            richText = false,
            fontStyle = FontStyle.Normal,
            fontSize = 12,
            padding = new RectOffset(10, 10, 10, 10)
        };
        
        // 🆕 텍스트 높이 계산
        float textHeight = textStyle.CalcHeight(new GUIContent(helpContent), position.width - 40);
        
        EditorGUILayout.SelectableLabel(
            helpContent, 
            textStyle, 
            GUILayout.Height(Mathf.Max(textHeight, availableHeight)) // 🆕 충분한 높이 보장
        );
        
        EditorGUILayout.EndScrollView();
        
        EditorGUILayout.Space();
        
        // 🆕 닫기 버튼
        if (GUILayout.Button("Close", GUILayout.Height(30)))
        {
            Close();
        }
        
        EditorGUILayout.EndVertical();
    }
}