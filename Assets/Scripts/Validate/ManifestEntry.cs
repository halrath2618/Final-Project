using System;
using System.Collections.Generic;

// These [Serializable] attributes are crucial for Unity's JsonUtility to work.
[Serializable]
public class ManifestEntry
{
    public string filePath; // Relative path within the validation directory
    public string md5Hash;  // The correct MD5 hash for this file
}

[Serializable]
public class FileManifest
{
    public List<ManifestEntry> files = new List<ManifestEntry>();
}