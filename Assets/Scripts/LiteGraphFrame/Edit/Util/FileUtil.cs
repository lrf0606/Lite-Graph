using System;
using System.IO;
using UnityEditor;

namespace LiteGraphFrame
{
    static class LiteGraphFileUtil
    {
        public static bool WriteToDisk(string path, string text)
        {
            while (true)
            {
                try
                {
                    File.WriteAllText(path, text);
                }
                catch (Exception e)
                {
                    // file is read onley
                    if (e.GetBaseException() is UnauthorizedAccessException &&
                        (File.GetAttributes(path) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    {
                        if (EditorUtility.DisplayDialog("File is Read-Only", path, "Make Writable", "Cancel Save"))
                        {
                            // make writeable
                            FileInfo fileInfo = new FileInfo(path);
                            fileInfo.IsReadOnly = false;
                            continue;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    UnityEngine.Debug.LogException(e);
                    if (EditorUtility.DisplayDialog("Exception While Saving", e.ToString(), "Retry", "Cancel"))
                    {
                        continue;
                    }
                    else
                    {
                        return false;
                    }
                }
                break;
            }
            return true;
        }

        public static string SafeReadAllText(string assetPath)
        {
            string result;
            try
            {
                result = File.ReadAllText(assetPath);
            }
            catch
            {
                result = null;
            }
            return result;
        }

        public static bool IsFileExist(string assetPath)
        {
            return File.Exists(assetPath);
        }
    }

}


