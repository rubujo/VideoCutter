using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using VideoCutter.Common;
using VideoCutter.Extension;
using VideoCutter.POCO;
using VideoCutter.Properties;
using Xabe.FFmpeg;
using Xabe.FFmpeg.Downloader;
using Xabe.FFmpeg.Events;
using Xabe.FFmpeg.Exceptions;

namespace VideoCutter;

public partial class MainForm : Form
{
    private string AppName = string.Empty;
    private TimeSpan? SharedTimeSpan;
    private IEnumerable<IVideoStream>? SharedVideoStreams;
    private IEnumerable<IAudioStream>? SharedAudioStreams;
    private CancellationTokenSource? SharedCancellationTokenSource;

    // 格式：00:00:00.000 / HH:mm:ss.fff
    private readonly Regex _Regex = new(@"^(\d{1}|\d{2}):\d{2}\:\d{2}.\d{3}$");
    private readonly BindingList<ClipData> DataSource = new();

    public MainForm()
    {
        InitializeComponent();

        CustomInit();
    }

    private void DGVClipList_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
        // 避免新列觸發驗證事件。
        if (DGVClipList.Rows[e.RowIndex].IsNewRow) { return; }

        DataGridViewColumn column = DGVClipList.Columns[e.ColumnIndex];

        if (column.Name == "ClipNo")
        {
            if (!int.TryParse(e.FormattedValue.ToString(), out int videoNo) || videoNo < 0)
            {
                e.Cancel = true;

                MessageBox.Show("請輸入有效的短片編號。（正整數）",
                    AppName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        else if (column.Name == "ClipName")
        {
            if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
            {
                e.Cancel = true;

                MessageBox.Show("請輸入有效的短片名稱。",
                    AppName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        else if (column.Name == "StartTime")
        {
            if (string.IsNullOrEmpty(e.FormattedValue.ToString()) ||
                !_Regex.IsMatch(e.FormattedValue.ToString()!))
            {
                e.Cancel = true;

                MessageBox.Show($"請輸入有效的開始時間。{Environment.NewLine}格式：HH:mm:ss.fff",
                    AppName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        else if (column.Name == "EndTime")
        {
            if (string.IsNullOrEmpty(e.FormattedValue.ToString()) ||
                !_Regex.IsMatch(e.FormattedValue.ToString()!))
            {
                e.Cancel = true;

                MessageBox.Show($"請輸入有效的結束時間。{Environment.NewLine}格式：HH:mm:ss.fff",
                    AppName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }

    private async void BtnSelectFile_Click(object sender, EventArgs e)
    {
        try
        {
            OpenFileDialog openFileDialog = new()
            {
                Title = "請選擇檔案",
                Filter = "MPEG-4 Part 14|*.mp4|Matroska|*.mkv",
                FilterIndex = 0
            };

            DialogResult dialogResult = openFileDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                string fileName = openFileDialog.FileName;

                if (!string.IsNullOrEmpty(fileName))
                {
                    await Task.Run(async () =>
                    {
                        IMediaInfo mediaInfo = await FFmpeg.GetMediaInfo(fileName);

                        string filePath = mediaInfo.Path;

                        SharedTimeSpan = mediaInfo.Duration;
                        SharedVideoStreams = mediaInfo.VideoStreams;
                        SharedAudioStreams = mediaInfo.AudioStreams;

                        LTotalLengthValue.InvokeIfRequired(() =>
                        {
                            string duration = SharedTimeSpan?.ToFFmpeg() ?? "0:00:00.000";

                            LTotalLengthValue.Text = duration;

                            if (duration == "0:00:00.000")
                            {
                                TBLog.InvokeIfRequired(() =>
                                {
                                    TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                                        $"[{AppName}] 無法取得此檔案的時間資訊，請選擇另外一個檔案。" +
                                        $"{Environment.NewLine}");

                                    MessageBox.Show("無法取得此檔案的時間資訊，請選擇另外一個檔案。",
                                        AppName,
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Warning);
                                });
                            }
                        });

                        TBSourceFilePath.InvokeIfRequired(() =>
                        {
                            TBSourceFilePath.Text = filePath;
                        });

                        BtnStartOperation.InvokeIfRequired(() =>
                        {
                            BtnStartOperation.Enabled = true;
                        });

                        TBLog.InvokeIfRequired(() =>
                        {
                            TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                                $"[FFmpeg] 已載入檔案的資訊。" +
                                $"{Environment.NewLine}");

                            MessageBox.Show("已載入檔案的資訊。",
                                AppName,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        });
                    });
                }
            }
        }
        catch (Exception ex)
        {
            TBLog.InvokeIfRequired(() =>
            {
                TBLog.AppendText(
                    $"{CustomFunction.GetTimeStamp()}[{AppName}] " +
                    $"發生錯誤：{ex.Message}{Environment.NewLine}");

                MessageBox.Show($"發生錯誤：{ex.Message}",
                    AppName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            });
        }
    }

    private void BtnImport_Click(object sender, EventArgs e)
    {
        try
        {
            if (SharedTimeSpan != null)
            {
                OpenFileDialog openFileDialog = new()
                {
                    Title = "請選擇檔案",
                    Filter = "文字檔|*.txt",
                    FilterIndex = 0
                };

                DialogResult dialogResult = openFileDialog.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    string fileName = openFileDialog.FileName;

                    if (!string.IsNullOrEmpty(fileName))
                    {
                        string[] lineSet = File.ReadAllLines(fileName);

                        bool canProcess = false;

                        List<ClipData> outputDataSet = new();

                        ClipData? clipData = null;

                        int timestampCount = 0, songIdx = 1;

                        foreach (string line in lineSet)
                        {
                            if (line == "時間標記：")
                            {
                                canProcess = true;

                                continue;
                            }

                            if (canProcess && !string.IsNullOrEmpty(line))
                            {
                                // 判斷是否為備註列。
                                if (line.Contains('#'))
                                {
                                    string clipName = string.Empty;

                                    // 判斷是否為開始的點。
                                    if (line.Contains("（開始）"))
                                    {
                                        clipData = new ClipData();

                                        // 去除不必要的內容。
                                        clipName = line.Replace("#", string.Empty)
                                            .Replace("（開始）", string.Empty)
                                            .TrimStart();
                                    }

                                    if (!string.IsNullOrEmpty(clipName))
                                    {
                                        if (clipData != null)
                                        {
                                            clipData.ClipNo = songIdx;
                                            clipData.ClipName = clipName;
                                            clipData.AudioOnly = false;
                                            clipData.UseCrf28 = false;
                                        }

                                        songIdx++;
                                    }
                                }
                                else
                                {
                                    string[] timestampSet = line.Split(
                                        new char[] { '｜' },
                                        StringSplitOptions.RemoveEmptyEntries);

                                    if (timestampSet.Length > 0 &&
                                        !string.IsNullOrEmpty(timestampSet[0]))
                                    {
                                        if (clipData != null)
                                        {
                                            // timestampCount 為 0 時，設定 clipData.StartTime。
                                            if (timestampCount == 0)
                                            {
                                                clipData.StartTime = timestampSet[0];

                                                timestampCount++;
                                            }
                                            else if (timestampCount == 1)
                                            {
                                                // timestampCount 為 1 時，設定 clipData.EndTime。
                                                clipData.EndTime = timestampSet[0];

                                                // 重置 timestampCount 為 0，以供下一個流程使用。
                                                timestampCount = 0;

                                                // 將 clipData 加入 outputDataSet;
                                                outputDataSet.Add(clipData);

                                                // 在加入 outputDataSet 後清空。
                                                clipData = null;
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        int errorCount = 0;

                        if (outputDataSet.Count > 0)
                        {
                            // 先刪除掉舊資料。
                            DGVClipList.InvokeIfRequired(() =>
                            {
                                DGVClipList.Rows.Clear();
                            });

                            foreach (ClipData data in outputDataSet)
                            {
                                TimeSpan? startTime = data.StartTime.ParseFFmpegTime();
                                TimeSpan? endTime = data.EndTime.ParseFFmpegTime();

                                if (SharedTimeSpan != null && startTime != null && endTime != null)
                                {
                                    bool isValid = true;

                                    string outputMsg = string.Empty;

                                    if (startTime >= SharedTimeSpan)
                                    {
                                        isValid = false;

                                        outputMsg += $"短片的開始時間不可晚於檔案的時間長度。{Environment.NewLine}";
                                    }

                                    if (endTime > SharedTimeSpan)
                                    {
                                        isValid = false;

                                        outputMsg += $"短片的結束時間不可晚於檔案的時間長度。{Environment.NewLine}";
                                    }

                                    if (startTime >= endTime)
                                    {
                                        isValid = false;

                                        outputMsg += $"短片的開始時間不可等於或晚於結束時間。{Environment.NewLine}";
                                    }

                                    if (isValid)
                                    {
                                        DGVClipList.InvokeIfRequired(() =>
                                        {
                                            DataSource.Add(data);
                                        });
                                    }

                                    if (!string.IsNullOrEmpty(outputMsg))
                                    {
                                        errorCount++;

                                        outputMsg = $"時間標記：「{data.ClipName}」匯入失敗，錯誤訊息：{Environment.NewLine}{outputMsg}";
                                        outputMsg = outputMsg.TrimEnd(Environment.NewLine.ToCharArray());

                                        TBLog.InvokeIfRequired(() =>
                                        {
                                            TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                                                $"[{AppName}] {outputMsg}" +
                                                $"{Environment.NewLine}");
                                        });
                                    }
                                }
                            }

                            DGVClipList.InvokeIfRequired(() =>
                            {
                                if (DGVClipList.RowCount > 0)
                                {
                                    DGVClipList.FirstDisplayedScrollingRowIndex = DGVClipList.RowCount - 1;
                                }
                            });

                            TBLog.InvokeIfRequired(() =>
                            {
                                string prefixMsg = errorCount > 0 ? "作業完成" : "時間標記已匯入完成";

                                string successCount = errorCount > 0 ?
                                    $"{outputDataSet.Count - errorCount}/{outputDataSet.Count}" :
                                    $"{outputDataSet.Count}";

                                TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                                    $"[{AppName}] {prefixMsg}，共匯入 {successCount} 筆時間標記資訊。" +
                                    $"{Environment.NewLine}");

                                MessageBox.Show($"{prefixMsg}，共匯入 {successCount} 筆時間標記資訊。",
                                    AppName,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                            });
                        }
                        else
                        {
                            TBLog.InvokeIfRequired(() =>
                            {
                                TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                                    $"[{AppName}] 所選之檔案，無法解析出有效的時間標記資訊，請確認該檔案的內容格式是否正確。" +
                                    $"{Environment.NewLine}");

                                MessageBox.Show("所選之檔案，無法解析出有效的時間標記資訊，請確認該檔案的內容格式是否正確。",
                                    AppName,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            });
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("請確認是否已選擇檔案。",
                    AppName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            TBLog.InvokeIfRequired(() =>
            {
                TBLog.AppendText(
                    $"{CustomFunction.GetTimeStamp()}[{AppName}] " +
                    $"發生錯誤：{ex.Message}{Environment.NewLine}");

                MessageBox.Show($"發生錯誤：{ex.Message}",
                    AppName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            });
        }
    }

    private void BtnClearClipList_Click(object sender, EventArgs e)
    {
        DGVClipList.InvokeIfRequired(() =>
        {
            DGVClipList.Rows.Clear();
        });

        TBLog.InvokeIfRequired(() =>
        {
            TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                $"[{AppName}] 已清除短片列表。" +
                $"{Environment.NewLine}");

            MessageBox.Show("已清除短片列表。",
                AppName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        });
    }

    private void BtnCheckFFmpeg_Click(object sender, EventArgs e)
    {
        CheckFFmpeg();
    }

    private void BtnAddNewClip_Click(object sender, EventArgs e)
    {
        if (CheckClipAdd())
        {
            DGVClipList.InvokeIfRequired(() =>
            {
                int lastRowIdx = DGVClipList.RowCount - 1;
                int insertNo = 1;

                if (DGVClipList.Rows.Count > 0)
                {
                    DataGridViewCell cell = DGVClipList.Rows[lastRowIdx].Cells[0];

                    if (cell.Value != null)
                    {
                        insertNo = int.TryParse(cell.Value.ToString(), out int parsedNo) ? parsedNo : 0;
                        insertNo++;
                    }
                }

                DataSource.Add(new ClipData()
                {
                    ClipNo = insertNo,
                    ClipName = TBClipName.Text,
                    StartTime = MTBClipStartTime.Text,
                    EndTime = MTBClipEndTime.Text,
                    AudioOnly = CBClipAudioOnly.Checked,
                    UseCrf28 = CBUseCrf28.Checked
                });

                if (lastRowIdx >= 0)
                {
                    DGVClipList.FirstDisplayedScrollingRowIndex = lastRowIdx;
                }
            });
        }
    }

    private void BtnResetNewClip_Click(object sender, EventArgs e)
    {
        TBClipName.InvokeIfRequired(() =>
        {
            TBClipName.Text = string.Empty;
        });

        MTBClipStartTime.InvokeIfRequired(() =>
        {
            MTBClipStartTime.Text = "00:00:00.000";
        });

        MTBClipEndTime.InvokeIfRequired(() =>
        {
            MTBClipEndTime.Text = "00:00:00.000";
        });

        CBClipAudioOnly.InvokeIfRequired(() =>
        {
            CBClipAudioOnly.Checked = false;
        });

        CBUseCrf28.InvokeIfRequired(() =>
        {
            CBUseCrf28.Checked = false;
        });

        TBLog.InvokeIfRequired(() =>
        {
            TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                $"[{AppName}] 已重設新增短片。" +
                $"{Environment.NewLine}");

            MessageBox.Show("已重設新增短片。",
                AppName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        });
    }

    private void BtnStartOperation_Click(object sender, EventArgs e)
    {
        try
        {
            SetControlState(false);

            if (DGVClipList.Rows.Count > 0)
            {
                CBCodecUseCopy.InvokeIfRequired(() =>
                {
                    if (CBCodecUseCopy.Checked)
                    {
                        DialogResult dialogResult = MessageBox.Show(
                            "注意！當勾選「編碼器使用 Copy」選項時，容易在分割短片的時後，" +
                            "遇到指定的時間點不在「I-影格」上的情況，這會造成分割出來的短片檔案，" +
                            "在播放時，畫面會有停滯幾秒不動的現象。\r\n\r\n如果您希望繼續使用" +
                            "「編碼器使用 Copy」選項，請按「確定」按鈕以繼續。",
                            AppName,
                            MessageBoxButtons.OKCancel,
                            MessageBoxIcon.Question);

                        if (dialogResult == DialogResult.OK)
                        {
                            DoOperation();
                        }
                        else
                        {
                            SetControlState(true);
                        }
                    }
                    else
                    {
                        DoOperation();
                    }
                });
            }
            else
            {
                MessageBox.Show("請先新增短片。",
                    AppName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                SetControlState(true);
            }
        }
        catch (Exception ex)
        {
            TBLog.InvokeIfRequired(() =>
            {
                TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                    $"[{AppName}] 發生錯誤：{ex}" +
                    $"{Environment.NewLine}");

                MessageBox.Show($"發生錯誤：{ex.Message}",
                    AppName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            });

            SetControlState(true);
        }
    }

    private void BtnResetOperation_Click(object sender, EventArgs e)
    {
        SharedTimeSpan = null;
        SharedVideoStreams = null;
        SharedAudioStreams = null;
        SharedCancellationTokenSource = null;

        TBSourceFilePath.InvokeIfRequired(() =>
        {
            TBSourceFilePath.Text = string.Empty;
        });

        DGVClipList.InvokeIfRequired(() =>
        {
            DGVClipList.Rows.Clear();
        });

        BtnStartOperation.InvokeIfRequired(() =>
        {
            BtnStartOperation.Enabled = false;
        });

        LTotalLengthValue.InvokeIfRequired(() =>
        {
            LTotalLengthValue.Text = "0:00:00.000";
        });

        TBLog.InvokeIfRequired(() =>
        {
            TBLog.Clear();
            TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                $"[{AppName}] 已重設作業。" +
                $"{Environment.NewLine}");

            MessageBox.Show($"已重設作業。",
                AppName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        });
    }

    private void BtnCancelFFmpeg_Click(object sender, EventArgs e)
    {
        try
        {
            if (!SharedCancellationTokenSource!.IsCancellationRequested)
            {
                SharedCancellationTokenSource?.Cancel();

                TBLog.InvokeIfRequired(() =>
                {
                    TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                        $"[{AppName}] 已取消此項的 FFmpeg 作業。" +
                        $"{Environment.NewLine}");
                });
            }
        }
        catch (Exception ex)
        {
            TBLog.InvokeIfRequired(() =>
            {
                TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                    $"[{AppName}] 發生錯誤：{ex}" +
                    $"{Environment.NewLine}");
            });
        }
    }

    /// <summary>
    /// 自定義初始化
    /// </summary>
    private void CustomInit()
    {
        try
        {
            // 將應用程式名稱從 MainForm.Text 反向指派至變數 AppName。
            AppName = Text;
            Icon = Resources.app_icon_small;

            #region 初始化控制項

            ActiveControl = BtnSelectFile;

            MTBClipStartTime.InvokeIfRequired(() =>
            {
                MTBClipStartTime.Text = "00:00:00.000";
            });

            MTBClipEndTime.InvokeIfRequired(() =>
            {
                MTBClipEndTime.Text = "00:00:00.000";
            });

            BtnStartOperation.InvokeIfRequired(() =>
            {
                BtnStartOperation.Enabled = false;
            });

            BtnCancelFFmpeg.InvokeIfRequired(() =>
            {
                BtnCancelFFmpeg.Enabled = false;
            });

            DGVClipList.InvokeIfRequired(() =>
            {
                DGVClipList.DataSource = DataSource;
            });

            // 設定顯示應用程式的版本號。
            LVersion.InvokeIfRequired(() =>
            {
                Version? version = Assembly.GetEntryAssembly()?.GetName().Version;

                if (version != null)
                {
                    LVersion.Text = $"版本：{version}";
                }
                else
                {
                    LVersion.Text = $"版本：無";
                }
            });

            #endregion

            CheckFFmpeg();
        }
        catch (Exception ex)
        {
            TBLog.InvokeIfRequired(() =>
            {
                TBLog.AppendText(
                    $"{CustomFunction.GetTimeStamp()}[{AppName}] " +
                    $"發生錯誤：{ex.Message}{Environment.NewLine}");

                MessageBox.Show($"發生錯誤：{ex.Message}",
                    AppName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            });
        }
    }

    /// <summary>
    /// 開始分割檔案
    /// <para>時間格式：hh:mm:ss.fff</para>
    /// </summary>
    /// <param name="OutputPath">目標檔案路徑</param>
    /// <param name="StartTime">開始時間</param>
    /// <param name="EndTime">結束時間</param>
    /// <param name="AudioOnly">僅輸出音訊，預設值為 false</param>
    /// <param name="AllowOverwrite">允許覆蓋已存在的檔案，預設值為 false</param>
    /// <param name="CodecUseCopy">允許編碼器使用 Copy，預設值為 true</param>
    /// <param name="UseMatroska">使用 Matroska 格式封裝視訊、音訊，預設值為 false</param>
    /// <param name="UserCrf28">使用餐數 -crf 28，預設值為 false</param>
    /// <returns>Task&lt;IConversionResult&gt;</returns>
    private async Task<IConversionResult> StartSplit(
        string OutputPath,
        TimeSpan StartTime,
        TimeSpan EndTime,
        bool AudioOnly = false,
        bool AllowOverwrite = false,
        bool CodecUseCopy = true,
        bool UseMatroska = false,
        bool UserCrf28 = false)
    {
        string basePath = Path.GetFullPath(OutputPath);
        string originFileName = Path.GetFileName(OutputPath);
        string originFileExtName = Path.GetExtension(OutputPath);
        string newExtName = string.Empty;

        IStream? videoStream = CodecUseCopy ?
            SharedVideoStreams?.FirstOrDefault()?.SetCodec(VideoCodec.copy) :
            SharedVideoStreams?.FirstOrDefault();

        IStream? audioStream = CodecUseCopy ?
            SharedAudioStreams?.FirstOrDefault()?.SetCodec(AudioCodec.copy) :
            SharedAudioStreams?.FirstOrDefault();

        if (AudioOnly)
        {
            videoStream = null;
        }

        IConversion conversion = FFmpeg.Conversions.New()
            .AddStream(videoStream, audioStream)
            .SetSeek(StartTime)
            .SetInputTime(EndTime)
            .SetOverwriteOutput(AllowOverwrite);

        // 判斷是否使用參數 -crf 28。
        if (UserCrf28)
        {
            conversion.AddParameter("-crf 28");
        }

        // 判斷是否設定使用 Matroska 格式封裝。
        if (UseMatroska)
        {
            conversion.SetOutputFormat(Format.matroska);

            // 若僅音訊輸出，則會強制使用 *.mka 副檔名。
            // 讓 FFmpeg 使用 *.mka 格式封裝檔案。
            newExtName = AudioOnly ? ".mka" : ".mkv";
        }
        else
        {
            // 若僅音訊輸出，則會強制使用 *.m4a 副檔名。
            // 讓 FFmpeg 使用 *.m4a 格式封裝檔案。
            // ※可能會有音訊編碼器與多媒體封裝格式不相容的問題。
            newExtName = AudioOnly ? ".m4a" : originFileExtName;
        }

        string newFileName = Path.GetFileNameWithoutExtension(OutputPath) + newExtName;
        string newOutputPath = basePath.Replace(originFileName, newFileName);

        conversion.SetOutput(newOutputPath);

        conversion.OnDataReceived += (object sender, DataReceivedEventArgs e) =>
        {
            TBLog.InvokeIfRequired(() =>
            {
                TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                    $"[FFmpeg] {e.Data}" +
                    $"{Environment.NewLine}");
            });
        };

        conversion.OnProgress += (object sender, ConversionProgressEventArgs args) =>
        {
            int percent = (int)(Math.Round(args.Duration.TotalSeconds / args.TotalLength.TotalSeconds, 2) * 100);

            TBLog.InvokeIfRequired(() =>
            {
                TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                    $"[FFmpeg] 處理識別：{args.ProcessId} " +
                    $"[{args.Duration}/{args.TotalLength}] {percent}%" +
                    $"{Environment.NewLine}");
            });
        };

        if (SharedCancellationTokenSource == null)
        {
            SharedCancellationTokenSource = new();
        }
        else
        {
            SharedCancellationTokenSource.Dispose();
            SharedCancellationTokenSource = null;

            SharedCancellationTokenSource = new();
        }

        IConversionResult conversionResult = await conversion
            .Start(SharedCancellationTokenSource.Token);

        return conversionResult;
    }

    /// <summary>
    /// 檢查新增片段
    /// </summary>
    /// <returns>布林值</returns>
    private bool CheckClipAdd()
    {
        string message = string.Empty;

        bool isValid = true;

        TimeSpan? startTime = null;
        TimeSpan? endTime = null;

        if (string.IsNullOrEmpty(TBClipName.Text))
        {
            isValid = false;

            message += $"請輸入短片的名稱。{Environment.NewLine}";
        }

        foreach (DataGridViewRow row in DGVClipList.Rows)
        {
            DataGridViewCell cell = row.Cells[1];

            if (cell.Value != null &&
                cell.Value.ToString() == TBClipName.Text)
            {
                isValid = false;

                message += $"此名稱已存在短片列表內，請使用另外一個新名稱。{Environment.NewLine}";
            }
        }

        if (string.IsNullOrEmpty(MTBClipStartTime.Text))
        {
            isValid = false;

            message += $"請輸入短片的開始時間。{Environment.NewLine}";
        }
        else
        {
            startTime = MTBClipStartTime.Text.ParseFFmpegTime();
        }

        if (string.IsNullOrEmpty(MTBClipEndTime.Text))
        {
            isValid = false;

            message += $"請輸入短片的結束時間。{Environment.NewLine}";
        }
        else
        {
            endTime = MTBClipEndTime.Text.ParseFFmpegTime();
        }

        if (SharedTimeSpan != null && startTime != null && endTime != null)
        {
            if (startTime >= SharedTimeSpan)
            {
                isValid = false;

                message += $"短片的開始時間不可晚於檔案的時間長度。{Environment.NewLine}";
            }

            if (endTime > SharedTimeSpan)
            {
                isValid = false;

                message += $"短片的結束時間不可晚於檔案的時間長度。{Environment.NewLine}";
            }

            if (startTime >= endTime)
            {
                isValid = false;

                message += $"短片的開始時間不可等於或晚於結束時間。{Environment.NewLine}";
            }
        }
        else
        {
            isValid = false;

            message += $"請確認是否已選擇檔案。{Environment.NewLine}";
        }

        if (!string.IsNullOrEmpty(message))
        {
            MessageBox.Show(message,
                AppName,
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }

        return isValid;
    }

    /// <summary>
    /// 設定控制項狀態
    /// </summary>
    /// <param name="Enable">啟用</param>
    private void SetControlState(bool Enable = true)
    {
        BtnSelectFile.InvokeIfRequired(() =>
        {
            BtnSelectFile.Enabled = Enable;
        });

        BtnImport.InvokeIfRequired(() =>
        {
            BtnImport.Enabled = Enable;
        });

        BtnClearClipList.InvokeIfRequired(() =>
        {
            BtnClearClipList.Enabled = Enable;
        });

        GBNewClip.InvokeIfRequired(() =>
        {
            GBNewClip.Enabled = Enable;
        });

        DGVClipList.InvokeIfRequired(() =>
        {
            DGVClipList.Enabled = Enable;
        });

        BtnAddNewClip.InvokeIfRequired(() =>
        {
            BtnAddNewClip.Enabled = Enable;
        });

        BtnStartOperation.InvokeIfRequired(() =>
        {
            BtnStartOperation.Enabled = Enable;
        });

        BtnResetOperation.InvokeIfRequired(() =>
        {
            BtnResetOperation.Enabled = Enable;
        });

        BtnCancelFFmpeg.InvokeIfRequired(() =>
        {
            BtnCancelFFmpeg.Enabled = !Enable;
        });

        CBAllowOverwrite.InvokeIfRequired(() =>
        {
            CBAllowOverwrite.Enabled = Enable;
        });

        CBCodecUseCopy.InvokeIfRequired(() =>
        {
            CBCodecUseCopy.Enabled = Enable;
        });

        CBUseMatroska.InvokeIfRequired(() =>
        {
            CBUseMatroska.Enabled = Enable;
        });
    }

    /// <summary>
    /// 檢查 FFmpeg
    /// </summary>
    private async void CheckFFmpeg()
    {
        try
        {
            string rootPath = Path.GetDirectoryName(AppContext.BaseDirectory)!,
                    ffmpegExecPath = Path.Combine(rootPath, "ffmpeg.exe"),
                    ffprobeExecPath = Path.Combine(rootPath, "ffprobe.exe");

            FFmpeg.SetExecutablesPath(rootPath);

            if (!File.Exists(ffmpegExecPath) || !File.Exists(ffprobeExecPath))
            {
                BtnSelectFile.InvokeIfRequired(() =>
                {
                    BtnSelectFile.Enabled = false;
                });

                TBLog.InvokeIfRequired(() =>
                {
                    TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                        $"[{AppName}] 沒有找到 FFmpeg 相關執行檔，無法開始進行作業。" +
                        $"{Environment.NewLine}");
                });

                await AskDownloadFFmpeg();
            }
            else
            {
                TBLog.InvokeIfRequired(() =>
                {
                    TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                        $"[{AppName}] 已找到 FFmpeg 相關執行檔，可以開始進行作業。" +
                        $"{Environment.NewLine}");
                });

                BtnCheckFFmpeg.InvokeIfRequired(() =>
                {
                    BtnCheckFFmpeg.Enabled = false;
                });

                BtnSelectFile.InvokeIfRequired(() =>
                {
                    BtnSelectFile.Enabled = true;
                });
            }
        }
        catch (Exception ex)
        {
            TBLog.InvokeIfRequired(() =>
            {
                TBLog.AppendText(
                    $"{CustomFunction.GetTimeStamp()}[{AppName}] " +
                    $"發生錯誤：{ex.Message}{Environment.NewLine}");

                MessageBox.Show($"發生錯誤：{ex.Message}",
                    AppName,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            });
        }
    }

    /// <summary>
    /// 詢問是否下載 FFmpeg
    /// </summary>
    private async Task AskDownloadFFmpeg()
    {
        string message = $"沒有找到 FFmpeg 相關執行檔。" +
            $"{Environment.NewLine}" +
            $"{Environment.NewLine}" +
            $"您是否要下載 FFmpeg檔？";

        DialogResult dialogResult = MessageBox.Show(
            message,
            AppName,
            MessageBoxButtons.OKCancel,
            MessageBoxIcon.Question);

        Progress<ProgressInfo> progress = new();

        progress.ProgressChanged += (object? sender, ProgressInfo e) =>
        {
            if (e.DownloadedBytes == 1 && e.TotalBytes == 1)
            {
                TBLog.InvokeIfRequired(() =>
                {
                    TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                        $"[{AppName}] FFmpeg 相關執行檔下載完成，可以開始進行作業。" +
                        $"{Environment.NewLine}");

                    BtnCheckFFmpeg.InvokeIfRequired(() =>
                    {
                        BtnCheckFFmpeg.Enabled = false;
                    });

                    BtnSelectFile.InvokeIfRequired(() =>
                    {
                        BtnSelectFile.Enabled = true;
                    });
                });
            }
            else
            {
                TBLog.InvokeIfRequired(() =>
                {
                    TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                        $"[{AppName}] FFmpeg 相關執行檔下載進度：{e.DownloadedBytes}/{e.TotalBytes}。" +
                        $"{Environment.NewLine}");
                });
            }
        };

        if (dialogResult == DialogResult.OK)
        {
            await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official, progress);
        }
    }

    /// <summary>
    /// 執行作業
    /// </summary>
    private void DoOperation()
    {
        string inputPath = string.Empty;

        TBSourceFilePath.InvokeIfRequired(() =>
        {
            inputPath = TBSourceFilePath.Text;
        });

        DGVClipList.InvokeIfRequired(async () =>
        {
            int errorCount = 0;

            for (int i = 0; i < DGVClipList.Rows.Count; i++)
            {
                DataGridViewRow row = DGVClipList.Rows[i];

                // 設定已選擇。
                row.Selected = true;

                // 設定指示器的位置。
                DGVClipList.CurrentCell = row.Cells[0];

                string directoryPath = Path.GetDirectoryName(inputPath)!;
                string sourceFileName = Path.GetFileNameWithoutExtension(inputPath);
                string sourceExtName = Path.GetExtension(inputPath);

                await Task.Run(async () =>
                {
                    try
                    {
                        bool isAllowOverwrite = false;
                        bool isCodecUseCopy = false;
                        bool isUseMatroska = false;

                        CBAllowOverwrite.InvokeIfRequired(() =>
                        {
                            isAllowOverwrite = CBAllowOverwrite.Checked;
                        });

                        CBCodecUseCopy.InvokeIfRequired(() =>
                        {
                            isCodecUseCopy = CBCodecUseCopy.Checked;
                        });

                        CBUseMatroska.InvokeIfRequired(() =>
                        {
                            isUseMatroska = CBUseMatroska.Checked;
                        });

                        int outputNo = int.TryParse(
                            row.Cells[0].Value.ToString(),
                            out int parsedNo) ?
                            parsedNo : 0;

                        string outputPath = string.Empty;
                        string outputFileName = row.Cells[1].Value.ToString()!;

                        TimeSpan startTime = row.Cells[2].Value.ToString().ParseFFmpegTime();
                        TimeSpan endTime = row.Cells[3].Value.ToString().ParseFFmpegTime();

                        bool isAudioOnly = bool.TryParse(
                            row.Cells[4].Value.ToString(),
                            out bool result1) &&
                            result1;

                        bool isUseCrf28 = bool.TryParse(
                            row.Cells[5].Value.ToString(),
                            out bool result2) &&
                            result2;

                        string outputFolderName = CustomFunction
                            .RemoveInvalidFilePathCharacters(sourceFileName, "_") +
                            (isAudioOnly ? "_Audio_Clips" : "_Video_Clips");
                        string outputDirectoryPath = Path.Combine(directoryPath, outputFolderName);

                        if (!Directory.Exists(outputDirectoryPath))
                        {
                            Directory.CreateDirectory(outputDirectoryPath);
                        }

                        outputPath = Path.Combine(outputDirectoryPath,
                            $"{sourceFileName}_{outputNo:#000}." +
                            $"{CustomFunction.RemoveInvalidFilePathCharacters(outputFileName, "_")}" +
                            $"{sourceExtName}");

                        IConversionResult convertResult = await StartSplit(
                            outputPath,
                            startTime,
                            endTime,
                            isAudioOnly,
                            isAllowOverwrite,
                            isCodecUseCopy,
                            isUseMatroska,
                            isUseCrf28);

                        TBLog.InvokeIfRequired(() =>
                        {
                            TBLog.AppendText(
                                $"{CustomFunction.GetTimeStamp()}[FFmpeg] " +
                                $"參數：{convertResult.Arguments}{Environment.NewLine}" +
                                $"{CustomFunction.GetTimeStamp()}[FFmpeg] " +
                                $"開始時間：{convertResult.StartTime}{Environment.NewLine}" +
                                $"{CustomFunction.GetTimeStamp()}[FFmpeg] " +
                                $"結束時間：{convertResult.EndTime}{Environment.NewLine}" +
                                $"{CustomFunction.GetTimeStamp()}[FFmpeg] " +
                                $"耗費時長：{convertResult.Duration}{Environment.NewLine}");
                        });
                    }
                    catch (ConversionException ex)
                    {
                        errorCount++;

                        TBLog.InvokeIfRequired(() =>
                        {
                            TBLog.AppendText(
                                $"{CustomFunction.GetTimeStamp()}[Xabe.FFmpeg] " +
                                $"發生錯誤：{ex.Message}{Environment.NewLine}" +
                                $"{CustomFunction.GetTimeStamp()}[Xabe.FFmpeg] " +
                                $"輸入的參數：{ex.InputParameters}{Environment.NewLine}" +
                                (!string.IsNullOrEmpty(ex.InnerException?.ToString()) ?
                                $"{CustomFunction.GetTimeStamp()}[Xabe.FFmpeg] " +
                                $"內部例外：{ex.InnerException}{Environment.NewLine}" :
                                string.Empty));
                        });
                    }
                    catch (Exception ex)
                    {
                        errorCount++;

                        TBLog.InvokeIfRequired(() =>
                        {
                            TBLog.AppendText(
                                $"{CustomFunction.GetTimeStamp()}[{AppName}] " +
                                $"發生錯誤：{ex.Message}{Environment.NewLine}");
                        });
                    }
                });

                if (i == DGVClipList.Rows.Count - 1)
                {
                    TBLog.InvokeIfRequired(() =>
                    {
                        TBLog.AppendText($"{CustomFunction.GetTimeStamp()}" +
                            $"[{AppName}] 作業完成。{Environment.NewLine}");

                        MessageBox.Show("作業完成。",
                            AppName,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    });

                    if (errorCount > 0)
                    {
                        MessageBox.Show($"此次作業已發生 {errorCount} 次錯誤，請查看操作記錄的內容。",
                            AppName,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                    }

                    SetControlState(true);
                }

                // 取消選擇狀態。
                row.Selected = false;
            }
        });
    }
}