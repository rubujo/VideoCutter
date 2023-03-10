# 影片分割器

## 使用說明

1. 選擇要進行分割的檔案，目前支援 `.mp4`、`.mkv` 等副檔名的視訊檔案。 
2. 在選擇完成後，`影片分割器` 會透過 `ffprobe.exe` 取得視訊檔案的相關資訊，這時請先確認`檔案時間長度`是否不為 `0:00:00.000`，當`檔案時間長度`不為 `0:00:00.000` 時才可以進行作業。
3. 當確認 `檔案時間長度`不為 `0:00:00.000` 後，請在新增短片區塊輸入`短片的名稱`、`開始時間`及`結束時間`，若想要只輸出音訊時，請再額外勾選`僅輸出音訊`的選項，當上述操作成完成後，按下`新增`按鈕，即可在短片列表新增短片。
4. 請依據自身的需求，重複步驟 `3.` 數次。
5. 當將所有的短片新增完後，請點選`開始作業`按鈕。
6. 請等待應用程式執行相關作業，當操作記錄欄位內有顯示作業完成後，即可查看成果。

## 進階使用說明

### 刪除短片列表的項目

按`清除短片列表`按鈕，即可將短片列表整個清空。

### 允許覆蓋檔案

`ffmpeg.exe` 預設不會覆蓋已經存在的檔案，但若想要強制覆寫已經存在的檔案，請在按下`開始作業`按鈕前，先勾選`允許覆蓋檔案`此選項後即可套用此設定。

### 中斷 FFmpeg

因為程式會以迴圈的方式執行每一個短片項目的處理作業，所以當作業中途突然需要終止時，按下 `中斷 FFmpeg` 按鈕，即會中斷該短片項目的 FFmpeg 處理作業，並會開始執行下一個短片項目的作業。

### 重設作業

按下`重設作業`按鈕，即可清除已選擇的檔案的內容。

### 重設新增短片

按下`新增短片`區塊內的`重設`按鈕，即可將`新增短片`區塊內的各欄位清空。

## 注意事項

- 本應用程式需要配合 `ffmpeg.exe` 及 `ffprobe.exe` 這兩個應用程式才可以正常使用。
  - ※`ffmpeg.exe` 及 `ffprobe.exe` 這兩個應用程式需要跟 `VideoCutter.exe` 位於同一個資料夾內。