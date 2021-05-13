using System.Text.RegularExpressions;

namespace VideoCutter.Common;

public static class CustomFunction
{
    /// <summary>
    /// 取得時間標記
    /// </summary>
    /// <returns>字串</returns>
    public static string GetTimeStamp()
    {
        return $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] ";
    }

    /// <summary>
    /// 移除檔案路徑中的無效字元
    /// <para>來源：https://stackoverflow.com/a/8626562 </para>
    /// </summary>
    /// <param name="filename">字串，檔案名稱</param>
    /// <param name="replaceChar">字串，替換無效字元的字元</param>
    /// <returns>字串</returns>
    public static string RemoveInvalidFilePathCharacters(string filename, string replaceChar)
    {
        string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());

        Regex r = new(string.Format("[{0}]", Regex.Escape(regexSearch)));

        return r.Replace(filename, replaceChar);
    }
}