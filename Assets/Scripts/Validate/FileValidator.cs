using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking; // Required for reading StreamingAssets on Android/WebGL
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks; // For asynchronous hashing

public class FileValidator : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Slider progressBar;

    [Header("Validation Settings")]
    [SerializeField] private string manifestFileName = "file_manifest.json";
    [SerializeField] private string nextSceneName = "MainMenu"; // Scene to load on success

    private List<string> corruptedFiles = new List<string>();

    void Start()
    {
        // Ensure UI is in a clean state
        if (progressBar != null) progressBar.value = 0;

        StartCoroutine(ValidateFilesCoroutine());
    }

    private IEnumerator ValidateFilesCoroutine()
    {
        // 1. Load the Manifest
        yield return null; // Wait a frame

        string manifestJson = null;
        string manifestPath = Path.Combine(Application.streamingAssetsPath, manifestFileName);

        // StreamingAssets path needs special handling for different platforms
#if UNITY_ANDROID && !UNITY_EDITOR
        using (UnityWebRequest www = UnityWebRequest.Get(manifestPath))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                HandleError($"Failed to load manifest: {www.error}");
                yield break;
            }
            manifestJson = www.downloadHandler.text;
        }
#else
        if (!File.Exists(manifestPath))
        {
            HandleError($"Manifest file not found at: {manifestPath}");
            yield break;
        }
        manifestJson = File.ReadAllText(manifestPath);
#endif

        FileManifest manifest = JsonUtility.FromJson<FileManifest>(manifestJson);
        if (manifest == null || manifest.files.Count == 0)
        {
            HandleError("Manifest is empty or invalid. Cannot validate files.");
            yield break;
        }

        // 2. Validate each file in the manifest
        corruptedFiles.Clear();
        int totalFiles = manifest.files.Count;

        for (int i = 0; i < totalFiles; i++)
        {
            ManifestEntry entry = manifest.files[i];
            string relativePath = entry.filePath;
            string fullPath = Path.Combine(Application.streamingAssetsPath, relativePath);

            if (progressBar != null) progressBar.value = (float)i / totalFiles;

            yield return null; // Update UI

            if (!File.Exists(fullPath))
            {
                Debug.LogError($"File missing: {relativePath}");
                corruptedFiles.Add(relativePath + " (Missing)");
                continue; // Move to the next file
            }

            // Offload hash calculation to a background thread to prevent stutters
            Task<string> hashTask = CalculateMD5Async(fullPath);
            yield return new WaitUntil(() => hashTask.IsCompleted);

            string calculatedHash = hashTask.Result;

            if (calculatedHash != entry.md5Hash)
            {
                Debug.LogError($"File corrupted: {relativePath}. Expected: {entry.md5Hash}, Got: {calculatedHash}");
                corruptedFiles.Add(relativePath + " (Corrupted)");
            }
        }

        // 3. Handle the result
        if (progressBar != null) progressBar.value = 1f;

        if (corruptedFiles.Count == 0)
        {
            yield return new WaitForSeconds(1.5f); // Give user time to read message
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            string errorDetails = "The following files are missing or corrupt:\n- " + string.Join("\n- ", corruptedFiles);
            HandleError(errorDetails);
        }
    }

    private void HandleError(string message)
    {
        Debug.LogError(message);
    }

    public void OnRetry()
    {
        // Reload the current scene to restart the process
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    #region MD5 Calculation (Async)

    private Task<string> CalculateMD5Async(string filename)
    {
        // Task.Run offloads the work to a background thread from the thread pool
        return Task.Run(() =>
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(filename))
                {
                    var hash = md5.ComputeHash(stream);
                    return ByteArrayToHexString(hash);
                }
            }
        });
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

    #endregion
}