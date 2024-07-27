using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ImageUtilities
{
    /// <summary>
    /// Returns the DCIM folder path for Android
    /// </summary>
    /// <returns>DCIM path for Android</returns>
    public static string GetAndroidDCIMPath()
    {
        if (Application.platform != RuntimePlatform.Android)
            return Application.persistentDataPath;

        var jc = new AndroidJavaClass("android.os.Environment");
        var path = jc.CallStatic<AndroidJavaObject>("getExternalStoragePublicDirectory",
            jc.GetStatic<string>("DIRECTORY_DCIM"))
            .Call<string>("getAbsolutePath");
        return path;
    }

    /// <summary>
    /// Returns the texture at the specified path
    /// </summary>
    /// <param name="path">The texture path on the device</param>
    /// <returns>Texture at the specified path if it exists, null otherwise</returns>
    public static Texture2D GetTextureAtPath(string path)
    {
        if (!System.IO.File.Exists(path))
            return null;

        byte[] byteArray = System.IO.File.ReadAllBytes(path);

        Texture2D texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        texture.LoadImage(byteArray);

        return texture;
    }

    /// <summary>
    /// Saves a Texture at the specified path
    /// </summary>
    /// <param name="texture">The texture to be saved</param>
    /// <param name="path">The texture path on the device</param>
    public static void SaveTextureToPath(Texture2D texture, string path)
    {
        // Encode the image as PNG data
        byte[] imageBytes = texture.EncodeToPNG();

        try
        {
            // Write the image data to the file
            System.IO.File.WriteAllBytes(path, imageBytes);
            Debug.Log("Image saved successfully!");
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error saving image: " + e.Message);
        }
    }

    /// <summary>
    /// Creates a Sprite from a defined Texture
    /// </summary>
    /// <param name="tex">The texture to use for the Sprite</param>
    /// <returns>The Created Sprite if any, null otherwise</returns>
    public static Sprite SpriteFromTexture(Texture2D tex)
    {
        if (tex == null)
            return null;

        Sprite screenshotSprite = Sprite.Create(tex,
            new Rect(0, 0, tex.width, tex.height),
            Vector2.one / 2,
            100);

        return screenshotSprite;
    }
}
