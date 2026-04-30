using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class MaterialConverter : EditorWindow
{
    // 머티리얼 폴더 경로
    private static readonly string[] MaterialFolders = new string[]
    {
        "Assets/Little Heroes Mega Pack/Materials",
        "Assets/Little Heroes Mega Pack/Materials/Modern Materials"
    };

    // 텍스처 폴더 경로
    private static readonly string[] TextureFolders = new string[]
    {
        "Assets/Little Heroes Mega Pack/Texture",
        "Assets/Little Heroes Mega Pack/Texture/Modern"
    };

    [MenuItem("Tools/Convert Materials to URP")]
    static void Convert()
    {
        Shader urpLit = Shader.Find("Universal Render Pipeline/Lit");
        if (urpLit == null)
        {
            Debug.LogError("URP Lit 셰이더를 찾을 수 없습니다. URP 패키지가 설치되어 있는지 확인하세요.");
            return;
        }

        // 1) 텍스처 폴더에서 모든 텍스처를 이름 기준으로 딕셔너리에 저장
        Dictionary<string, Texture> textureMap = new Dictionary<string, Texture>();

        foreach (string folder in TextureFolders)
        {
            if (!AssetDatabase.IsValidFolder(folder)) continue;

            string[] texGuids = AssetDatabase.FindAssets("t:Texture", new[] { folder });
            foreach (string guid in texGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string fileName = Path.GetFileNameWithoutExtension(path).ToLower();
                Texture tex = AssetDatabase.LoadAssetAtPath<Texture>(path);

                if (tex != null && !textureMap.ContainsKey(fileName))
                {
                    textureMap[fileName] = tex;
                }
            }
        }

        Debug.Log($"텍스처 {textureMap.Count}개 발견");

        // 2) 머티리얼 변환
        int convertedCount = 0;
        int textureMatchedCount = 0;
        int textureMissingCount = 0;
        List<string> missingList = new List<string>();

        foreach (string folder in MaterialFolders)
        {
            if (!AssetDatabase.IsValidFolder(folder)) continue;

            // 하위 폴더 제외하고 해당 폴더만 검색하려면 SearchOption 조절
            string[] matGuids = AssetDatabase.FindAssets("t:Material", new[] { folder });

            foreach (string guid in matGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
                if (mat == null) continue;

                // 기존 텍스처와 컬러 백업
                Texture mainTex = mat.mainTexture;
                Color color = mat.HasProperty("_Color") ? mat.color : Color.white;

                // 셰이더 변환
                mat.shader = urpLit;

                // 텍스처 세팅
                if (mainTex != null)
                {
                    // 기존 텍스처가 있으면 그대로 사용
                    mat.SetTexture("_BaseMap", mainTex);
                    textureMatchedCount++;
                }
                else
                {
                    // 기존 텍스처가 없으면 머티리얼 이름으로 텍스처 검색
                    string matName = Path.GetFileNameWithoutExtension(path).ToLower();
                    Texture matchedTex = FindMatchingTexture(matName, textureMap);

                    if (matchedTex != null)
                    {
                        mat.SetTexture("_BaseMap", matchedTex);
                        textureMatchedCount++;
                        Debug.Log($"매칭 성공: {mat.name} → {matchedTex.name}");
                    }
                    else
                    {
                        textureMissingCount++;
                        missingList.Add(mat.name);
                    }
                }

                mat.SetColor("_BaseColor", color);
                EditorUtility.SetDirty(mat);
                convertedCount++;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        // 결과 로그
        Debug.Log($"===== 변환 완료 =====");
        Debug.Log($"총 {convertedCount}개 머티리얼 변환");
        Debug.Log($"텍스처 매칭 성공: {textureMatchedCount}개");
        Debug.Log($"텍스처 못 찾음: {textureMissingCount}개");

        if (missingList.Count > 0)
        {
            Debug.LogWarning($"텍스처 매칭 실패 목록 (수동 확인 필요):\n" +
                string.Join("\n", missingList));
        }
    }

    /// <summary>
    /// 머티리얼 이름과 비슷한 텍스처를 찾는 함수
    /// 정확히 일치 → 포함 관계 → 못 찾음 순서로 시도
    /// </summary>
    static Texture FindMatchingTexture(string matName, Dictionary<string, Texture> textureMap)
    {
        // 1) 정확히 일치
        if (textureMap.TryGetValue(matName, out Texture exact))
            return exact;

        // 2) 머티리얼 이름이 텍스처 이름에 포함되거나, 텍스처 이름이 머티리얼 이름에 포함
        foreach (var kvp in textureMap)
        {
            if (kvp.Key.Contains(matName) || matName.Contains(kvp.Key))
                return kvp.Value;
        }

        return null;
    }
}
