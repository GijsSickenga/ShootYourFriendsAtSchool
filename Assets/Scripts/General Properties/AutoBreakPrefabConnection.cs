﻿#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[ExecuteInEditMode]
public class AutoBreakPrefabConnection : MonoBehaviour
{
    void Start()
    {
    #if UNITY_EDITOR
        PrefabUtility.DisconnectPrefabInstance(gameObject);
    #endif
        DestroyImmediate(this); // Remove this script
    }
}
