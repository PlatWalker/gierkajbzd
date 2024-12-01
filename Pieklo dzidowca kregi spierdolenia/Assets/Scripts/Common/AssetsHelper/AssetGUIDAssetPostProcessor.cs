using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace jbzd.Common.AssetsHelper
{
#if UNITY_EDITOR
    internal sealed class AssetGUIDAssetPostProcessor : AssetPostprocessor
    {
#pragma warning disable UNT0033 // Incorrect message case
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
#pragma warning restore UNT0033 // Incorrect message case
        {
            if (didDomainReload)
            {
                return;
            }

            int importedCount = importedAssets.Length;
            for (int i = 0; i < importedCount; i++)
            {
                string assetPath = importedAssets[i];
                UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);

                if (asset != null && asset is DuplicatedScriptableObjects assetGUID)
                {
                    bool hasGuid = assetGUID.HasGeneratedGuid();
                    if (!hasGuid)
                    {
                        Debug.LogWarning($"Asset GUID: {asset.name} does not have a generated GUID.", asset);
                    }

                    var duplicates = FindAssets(asset.GetType(), assetGUID.Id);
                    if (duplicates != null && duplicates.Count > 1)
                    {
                        Debug.LogWarning("Asset GUID: Created Asset has same GUID as other assets. Recreating GUID", asset);
                        assetGUID.ForceGenerateGuid();
                    }
                }
            }
        }

        private static List<string> FindAssets(Type type, string guid)
        {
            var assetsToCheck = AssetDatabase.FindAssets($"t: {type}").ToList();
            return assetsToCheck.Where(x =>
            {
                var path = AssetDatabase.GUIDToAssetPath(x);
                DuplicatedScriptableObjects asset = AssetDatabase.LoadAssetAtPath<DuplicatedScriptableObjects>(path);
                return asset.Id == guid;
            }).ToList();
        }
    }
    #endif
}
