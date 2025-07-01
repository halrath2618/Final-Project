using UnityEngine;
using UnityEditor;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

public class ManifestGenerator
{
    // The name of the manifest file we will create
    private const string ManifestFileName = "file_manifest.json";

    [MenuItem("Tools/Validation/Generate File Manifest")]
    public static void GenerateManifest()
    {
        string manifestPath = Path.Combine(Application.streamingAssetsPath, ManifestFileName);
        string streamingAssetsPath = Application.streamingAssetsPath;

        if (!Directory.Exists(streamingAssetsPath))
        {
            Directory.CreateDirectory(streamingAssetsPath);
        }

        FileManifest manifest = new FileManifest();

        // Get all files, recursively, from the StreamingAssets folder
        string[] allFiles = Directory.GetFiles(streamingAssetsPath, "*.*", SearchOption.AllDirectories);

        EditorUtility.DisplayProgressBar("Generating Manifest", "Calculating file hashes...", 0f);

        for (int i = 0; i < allFiles.Length; i++)
        {
            string filePath = allFiles[i];

            // IMPORTANT: Normalize path separators to be consistent across platforms (Windows uses \, others use /)
            string normalizedPath = filePath.Replace('\\', '/');

            // Skip the manifest file itself
            if (Path.GetFileName(normalizedPath) == ManifestFileName)
            {
                continue;
            }

            // Make the path relative to the StreamingAssets folder
            string relativePath = normalizedPath.Substring(streamingAssetsPath.Length + 1);

            EditorUtility.DisplayProgressBar("Generating Manifest", $"Processing: {relativePath}", (float)i / allFiles.Length);

            // Calculate MD5 hash
            string md5 = CalculateMD5(normalizedPath);

            manifest.files.Add(new ManifestEntry { filePath = relativePath, md5Hash = md5 });
        }

        // Serialize the manifest object to JSON and save it
        string json = JsonUtility.ToJson(manifest, true); // 'true' for pretty print
        File.WriteAllText(manifestPath, json);

        EditorUtility.ClearProgressBar();
        Debug.Log($"Manifest generated successfully at: {manifestPath} with {manifest.files.Count} files.");
        AssetDatabase.Refresh(); // Refresh the asset database to show the new file in Unity
    }

    private static string CalculateMD5(string filename)
    {
        using (var md5 = MD5.Create())
        {
            using (var stream = File.OpenRead(filename))
            {
                var hash = md5.ComputeHash(stream);
                return ByteArrayToHexString(hash);
            }
        }
    }

    private static string ByteArrayToHexString(byte[] bytes)
    {
        StringBuilder sb = new StringBuilder(bytes.Length * 2);
        foreach (byte b in bytes)
        {
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
}