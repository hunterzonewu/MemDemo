using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public struct ShaderIndex
{
    public string shaderName;
    public string abName;
}

public class ShaderIndexData : ScriptableObject
{
    public List<ShaderIndex> shaderIdxList = new List<ShaderIndex>();

#if UNITY_EDITOR
    const string ASSET_PATH = "Assets/resources/shaders/svc/shader_idx.asset";
    public static ShaderIndexData createInstance()
    {
        ShaderIndexData sid = AssetDatabase.LoadAssetAtPath<ShaderIndexData>(ASSET_PATH);
        if (null == sid)
        {
            sid = ScriptableObject.CreateInstance<ShaderIndexData>();
            AssetDatabase.CreateAsset(sid, ASSET_PATH);
        }
        sid.shaderIdxList.Clear();
        return sid;
    }
    public void save()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
#endif
}
