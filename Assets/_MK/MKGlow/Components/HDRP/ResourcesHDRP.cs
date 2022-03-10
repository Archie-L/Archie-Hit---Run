//////////////////////////////////////////////////////
// MK Glow Resources HDRP	    	    	        //
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
#if UNITY_EDITOR && !UNITY_CLOUD_BUILD
using UnityEditor;
#endif
*/

#pragma warning disable
namespace MK.Glow.HDRP
{
    [System.Serializable]
    /// <summary>
    /// Stores runtime required resources
    /// </summary>
    public sealed class ResourcesHDRP : ScriptableObject
    {        
        internal static void ResourcesNotAvailableWarning()
        {
            Debug.LogWarning("MK Glow resources hdrp asset couldn't be found. Effect will be skipped.");
        }

        internal static MK.Glow.HDRP.ResourcesHDRP LoadResourcesAsset()
        {
            return UnityEngine.Resources.Load<MK.Glow.HDRP.ResourcesHDRP>("MKGlowResourcesHDRP");
        }

        /*
        #if UNITY_EDITOR && !UNITY_CLOUD_BUILD
        [MenuItem("Window/MK/Glow/Create Resources HDRP Asset")]
        static void CreateAsset()
        {
            ResourcesHDRP asset = ScriptableObject.CreateInstance<ResourcesHDRP>();

            AssetDatabase.CreateAsset(asset, "Assets/_MK/MKGlow/ResourcesHDRP.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
        #endif
        */

        [SerializeField]
        private Shader _sm40Shader;
        internal Shader sm40Shader { get { return _sm40Shader; } }
        [SerializeField]
        private Shader _sm40GeometryShader;
        internal Shader sm40GeometryShader { get { return _sm40GeometryShader; } }
    }
}