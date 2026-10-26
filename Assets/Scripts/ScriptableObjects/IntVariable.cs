using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "ScriptableObjects/IntVariable", order = 2)]
public class IntVariable : Variable<int>
{

    public int previousHighestValue;
    public override void SetValue(int value)
    {
        if (value > previousHighestValue) previousHighestValue = value;

        _value = value;
    }

    // overload
    public void SetValue(IntVariable value)
    {
        SetValue(value.Value);
    }

    public void ApplyChange(int amount)
    {
        this.Value += amount;
    }

    public void ApplyChange(IntVariable amount)
    {
        ApplyChange(amount.Value);
    }

    public void ResetHighestValue()
    {
        previousHighestValue = 0;
    }


    // honestly, i've been struggling to get the game score value to reset between each run. eg when exiting
    // and entering play mode. so im trying this script to see if it works. its directly from claude. 
    #if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
    static void RegisterReset()
    {
        UnityEditor.EditorApplication.playModeStateChanged += state =>
        {
            if (state != UnityEditor.PlayModeStateChange.ExitingEditMode) return;

            foreach (string guid in UnityEditor.AssetDatabase.FindAssets("t:IntVariable"))
            {
                var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var v = UnityEditor.AssetDatabase.LoadAssetAtPath<IntVariable>(path);
                v.Value = 0;   // leave previousHighestValue alone
                UnityEditor.EditorUtility.SetDirty(v);
            }
        };
    }
    #endif  

}