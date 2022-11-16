using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


[CustomEditor(typeof(AISight))]
public class ES_AISight : Editor
{
    // private Label _label;
    // private Toggle _toggle;
    // private GUIStyle style;
    // private GUIContent content;
    // private Sprite image;

    /*public override VisualElement CreateInspectorGUI()
    {
        VisualElement element = new VisualElement();
        
        //element.Add(_label = new Label("Deneme"));
        
        element.Add(_toggle = new Toggle("Deneme"));
        
        VisualTreeAsset visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Deneme.uxml");
        visualTree.CloneTree(element);
        
        return element;
    }*/

    public override void OnInspectorGUI()
    {
        //var toggle = EditorGUILayout.Toggle(false);
        
        //content.image = AssetDatabase.LoadAssetAtPath<Texture>("Assets/_Main/OTHER/159078.png");

        // SetTitleStyle();
        //
        // GUILayout.Label(content, style);
        
        base.OnInspectorGUI();
    }

    /*private void SetTitleStyle()
    {
        content.text = "SIGHT";
        content.image = AssetDatabase.LoadAssetAtPath<Texture>("Assets/_Main/OTHER/159078.png");
        
        style.font = AssetDatabase.LoadAssetAtPath<Font>("Assets/Third Party/Graphy - Ultimate Stats Monitor/Font/Roboto/Roboto-Bold.ttf");
        style.fontSize = 52;
        style.normal.textColor = new Color(255, 172, 0, 255);
        style.active.textColor = new Color(255, 172, 0, 255);
        style.imagePosition = ImagePosition.ImageLeft;
        style.fontStyle = FontStyle.Bold;
        style.alignment = TextAnchor.MiddleCenter;
    }*/
}
