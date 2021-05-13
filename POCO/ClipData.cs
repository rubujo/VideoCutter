namespace VideoCutter.POCO;

/// <summary>
/// 短片資料
/// </summary>
public class ClipData
{
    /// <summary>
    /// 短片編號
    /// </summary>
    public int ClipNo { get; set; }

    /// <summary>
    /// 短片名稱
    /// </summary>
    public string? ClipName { get; set; }

    /// <summary>
    /// 開始時間
    /// </summary>
    public string? StartTime { get; set; }

    /// <summary>
    /// 結束時間
    /// </summary>
    public string? EndTime { get; set; }

    /// <summary>
    /// 僅輸出音訊
    /// </summary>
    public bool AudioOnly { get; set; }

    /// <summary>
    /// 使用參數 -crf 28
    /// </summary>
    public bool UseCrf28 { get; set; }
}