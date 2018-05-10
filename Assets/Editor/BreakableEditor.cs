using UnityEditor;
using UnityEngine;
using System.Collections;

// Custom Editor using SerializedProperties
// Automatic handling of multi-object editing, undo, and prefab overrides
[CustomEditor(typeof(Breakable))]
[CanEditMultipleObjects]
public class BreakableEditor : Editor
{
    // Boolean
    SerializedProperty randomHealthProp;
    SerializedProperty randomRegenProp;
    SerializedProperty showHealthbarProp;
    // Color
    SerializedProperty healthBarColorProp;
    // Vector2
    SerializedProperty healthRangeProp;
    SerializedProperty regenTimerRangeProp;
    SerializedProperty healthBarSizeProp;
    SerializedProperty healthBarOffsetProp;
    // Int
    SerializedProperty healthBarBorderSizeProp;

    void OnEnable()
    {
        // Setup the SerializedProperties

        // Boolean
        randomHealthProp = serializedObject.FindProperty("randomHealth");
        randomRegenProp = serializedObject.FindProperty("randomRegen");
        showHealthbarProp = serializedObject.FindProperty("showHealthbar");
        // Color
        healthBarColorProp = serializedObject.FindProperty("healthBarColor");
        // Vector2
        healthRangeProp = serializedObject.FindProperty("healthRange");
        regenTimerRangeProp = serializedObject.FindProperty("regenTimerRange");
        healthBarSizeProp = serializedObject.FindProperty("healthBarSize");
        healthBarOffsetProp = serializedObject.FindProperty("healthBarOffset");
        // Int
        healthBarBorderSizeProp = serializedObject.FindProperty("healthBarBorderSize");
    }

    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this at the beginning of OnInspectorGUI
        serializedObject.Update();

        randomHealthProp.boolValue = EditorGUILayout.Toggle("Randomize Health", randomHealthProp.boolValue);
        randomRegenProp.boolValue = EditorGUILayout.Toggle("Randomize Regeneration", randomRegenProp.boolValue);
        showHealthbarProp.boolValue = EditorGUILayout.Toggle("Show Healthbar", showHealthbarProp.boolValue);

        EditorGUILayout.Space();

        // Show a min and max slider if random health is enabled
        if (randomHealthProp.boolValue)
        {
            EditorGUILayout.LabelField("Health");
            healthRangeProp.vector2Value = new Vector2(EditorGUILayout.IntField("  Min", (int)healthRangeProp.vector2Value.x),
                                                       EditorGUILayout.IntField("  Max", (int)healthRangeProp.vector2Value.y));
        }
        else
        {
            int healthRange = EditorGUILayout.IntField("Health", (int)healthRangeProp.vector2Value.x);
            healthRangeProp.vector2Value = new Vector2(healthRange, healthRange);
        }

        // Show a min and max slider if random regen is enabled
        if (randomRegenProp.boolValue)
        {
            EditorGUILayout.LabelField("Regen Timer");
            regenTimerRangeProp.vector2Value = new Vector2(EditorGUILayout.FloatField("  Min", regenTimerRangeProp.vector2Value.x),
                                                           EditorGUILayout.FloatField("  Max", regenTimerRangeProp.vector2Value.y));
        }
        else
        {
            float timerRange = EditorGUILayout.FloatField("Regen Timer", regenTimerRangeProp.vector2Value.x);
            regenTimerRangeProp.vector2Value = new Vector2(timerRange, timerRange);
        }

        // Show a message indicating if and how fast the object is regenerating
        if (regenTimerRangeProp.vector2Value.y > 0)
        {
            if (randomRegenProp.boolValue)
            {
                EditorGUILayout.HelpBox("This object will regenerate after " + regenTimerRangeProp.vector2Value.x + " to " + regenTimerRangeProp.vector2Value.y + " seconds.", MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("This object will regenerate after " + regenTimerRangeProp.vector2Value.x + " seconds.", MessageType.Info);
            }
        }
        else
        {
            EditorGUILayout.HelpBox("This object will not regenerate.", MessageType.Info);
        }

        // Show a color field if the health bar is enabled
        if (showHealthbarProp.boolValue)
        {
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Health Bar");

            healthBarColorProp.colorValue = EditorGUILayout.ColorField("  Color", healthBarColorProp.colorValue);

            EditorGUILayout.LabelField("  Size");
            healthBarSizeProp.vector2Value = new Vector2(EditorGUILayout.IntField("    Width", (int)healthBarSizeProp.vector2Value.x),
                                                         EditorGUILayout.IntField("    Height", (int)healthBarSizeProp.vector2Value.y));

            healthBarOffsetProp.vector2Value = EditorGUILayout.Vector2Field("  Offset", healthBarOffsetProp.vector2Value);
            healthBarBorderSizeProp.intValue = EditorGUILayout.IntField("  Border Size", healthBarBorderSizeProp.intValue);
        }

        // Clamp health
        healthRangeProp.vector2Value = new Vector2(Mathf.Clamp(healthRangeProp.vector2Value.x, 1, Mathf.Infinity),
                                                   Mathf.Clamp(healthRangeProp.vector2Value.y, healthRangeProp.vector2Value.x, Mathf.Infinity));

        // Clamp regen timer
        regenTimerRangeProp.vector2Value = new Vector2(Mathf.Clamp(regenTimerRangeProp.vector2Value.x, 0, Mathf.Infinity),
                                                       Mathf.Clamp(regenTimerRangeProp.vector2Value.y, regenTimerRangeProp.vector2Value.x, Mathf.Infinity));

        // Apply changes to the serializedProperty - always do this at the end of OnInspectorGUI
        serializedObject.ApplyModifiedProperties();
    }
}
