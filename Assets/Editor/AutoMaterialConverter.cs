// using UnityEditor;
// using UnityEngine;
// using System.Collections.Generic;

// public class AutoMaterialConverter : AssetPostprocessor
// {
//     private static void OnPostprocessAllAssets(
//         string[] importedAssets, string[] deletedAssets,
//         string[] movedAssets, string[] movedFromAssetPaths)
//     {
//         Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
//         if (urpLit == null) return;

//         int count = 0;

//         foreach (string path in importedAssets)
//         {
//             if (!path.EndsWith(".mat")) continue;

//             Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
//             if (mat == null) continue;
//             if (mat.shader.name != "Standard" &&
//                 mat.shader.name != "Standard (Specular setup)")
//                 continue;

//             // 텍스쳐가 이미 다 임포트된 상태
//             Color color = GetColor(mat, "_Color");
//             Texture mainTex = GetTex(mat, "_MainTex");
//             Texture normalMap = GetTex(mat, "_BumpMap");
//             Texture metallicMap = GetTex(mat, "_MetallicGlossMap");
//             Texture occlusion = GetTex(mat, "_OcclusionMap");
//             Texture emission = GetTex(mat, "_EmissionMap");
//             Color emissionColor = GetColor(mat, "_EmissionColor");
//             float metallicVal = GetFloat(mat, "_Metallic");
//             float smoothness = GetFloat(mat, "_Glossiness", 0.5f);
//             float bumpScale = GetFloat(mat, "_BumpScale", 1f);

//             // 셰이더 변경
//             mat.shader = urpLit;

//             // 복원
//             mat.SetColor("_BaseColor", color);
//             if (mainTex != null) mat.SetTexture("_BaseMap", mainTex);
//             if (normalMap != null) mat.SetTexture("_BumpMap", normalMap);
//             if (metallicMap != null) mat.SetTexture("_MetallicGlossMap", metallicMap);
//             if (occlusion != null) mat.SetTexture("_OcclusionMap", occlusion);
//             mat.SetFloat("_Metallic", metallicVal);
//             mat.SetFloat("_Smoothness", smoothness);
//             mat.SetFloat("_BumpScale", bumpScale);

//             if (emission != null)
//             {
//                 mat.SetTexture("_EmissionMap", emission);
//                 mat.SetColor("_EmissionColor", emissionColor);
//                 mat.EnableKeyword("_EMISSION");
//             }

//             EditorUtility.SetDirty(mat);
//             count++;
//         }

//         if (count > 0)
//         {
//             AssetDatabase.SaveAssets();
//             Debug.Log($"[AutoConvert] {count}개 머테리얼 → URP Lit 변환 완료");
//         }
//     }

//     private static Color GetColor(Material m, string prop)
//     {
//         return m.HasProperty(prop) ? m.GetColor(prop) : Color.white;
//     }

//     private static Texture GetTex(Material m, string prop)
//     {
//         return m.HasProperty(prop) ? m.GetTexture(prop) : null;
//     }

//     private static float GetFloat(Material m, string prop, float fallback = 0f)
//     {
//         return m.HasProperty(prop) ? m.GetFloat(prop) : fallback;
//     }
// }