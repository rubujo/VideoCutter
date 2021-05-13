
namespace VideoCutter
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TBSourceFilePath = new System.Windows.Forms.TextBox();
            this.BtnStartOperation = new System.Windows.Forms.Button();
            this.TBLog = new System.Windows.Forms.TextBox();
            this.BtnAddNewClip = new System.Windows.Forms.Button();
            this.TBClipName = new System.Windows.Forms.TextBox();
            this.MTBClipStartTime = new System.Windows.Forms.MaskedTextBox();
            this.MTBClipEndTime = new System.Windows.Forms.MaskedTextBox();
            this.CBClipAudioOnly = new System.Windows.Forms.CheckBox();
            this.LSourceFilePath = new System.Windows.Forms.Label();
            this.BtnSelectFile = new System.Windows.Forms.Button();
            this.LFileDuration = new System.Windows.Forms.Label();
            this.LTotalLengthValue = new System.Windows.Forms.Label();
            this.LClipName = new System.Windows.Forms.Label();
            this.LClipStartTime = new System.Windows.Forms.Label();
            this.LClipEndTime = new System.Windows.Forms.Label();
            this.GBNewClip = new System.Windows.Forms.GroupBox();
            this.CBUseCrf28 = new System.Windows.Forms.CheckBox();
            this.BtnResetNewClip = new System.Windows.Forms.Button();
            this.CBAllowOverwrite = new System.Windows.Forms.CheckBox();
            this.LClipList = new System.Windows.Forms.Label();
            this.LLog = new System.Windows.Forms.Label();
            this.BtnClearClipList = new System.Windows.Forms.Button();
            this.BtnResetOperation = new System.Windows.Forms.Button();
            this.BtnCancelFFmpeg = new System.Windows.Forms.Button();
            this.CBCodecUseCopy = new System.Windows.Forms.CheckBox();
            this.CBUseMatroska = new System.Windows.Forms.CheckBox();
            this.DGVClipList = new System.Windows.Forms.DataGridView();
            this.ClipNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ClipName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AudioOnly = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.UseCrf28 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.BtnImport = new System.Windows.Forms.Button();
            this.BtnCheckFFmpeg = new System.Windows.Forms.Button();
            this.LVersion = new System.Windows.Forms.Label();
            this.GBNewClip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVClipList)).BeginInit();
            this.SuspendLayout();
            // 
            // TBSourceFilePath
            // 
            this.TBSourceFilePath.Location = new System.Drawing.Point(10, 24);
            this.TBSourceFilePath.Margin = new System.Windows.Forms.Padding(2);
            this.TBSourceFilePath.Name = "TBSourceFilePath";
            this.TBSourceFilePath.ReadOnly = true;
            this.TBSourceFilePath.Size = new System.Drawing.Size(876, 23);
            this.TBSourceFilePath.TabIndex = 2;
            // 
            // BtnStartOperation
            // 
            this.BtnStartOperation.ForeColor = System.Drawing.SystemColors.Highlight;
            this.BtnStartOperation.Location = new System.Drawing.Point(679, 241);
            this.BtnStartOperation.Margin = new System.Windows.Forms.Padding(2);
            this.BtnStartOperation.Name = "BtnStartOperation";
            this.BtnStartOperation.Size = new System.Drawing.Size(82, 23);
            this.BtnStartOperation.TabIndex = 22;
            this.BtnStartOperation.Text = "開始作業";
            this.BtnStartOperation.UseVisualStyleBackColor = true;
            this.BtnStartOperation.Click += new System.EventHandler(this.BtnStartOperation_Click);
            // 
            // TBLog
            // 
            this.TBLog.Location = new System.Drawing.Point(10, 455);
            this.TBLog.Margin = new System.Windows.Forms.Padding(2);
            this.TBLog.Multiline = true;
            this.TBLog.Name = "TBLog";
            this.TBLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TBLog.Size = new System.Drawing.Size(951, 68);
            this.TBLog.TabIndex = 29;
            // 
            // BtnAddNewClip
            // 
            this.BtnAddNewClip.Location = new System.Drawing.Point(192, 91);
            this.BtnAddNewClip.Margin = new System.Windows.Forms.Padding(2);
            this.BtnAddNewClip.Name = "BtnAddNewClip";
            this.BtnAddNewClip.Size = new System.Drawing.Size(89, 23);
            this.BtnAddNewClip.TabIndex = 20;
            this.BtnAddNewClip.Text = "新增";
            this.BtnAddNewClip.UseVisualStyleBackColor = true;
            this.BtnAddNewClip.Click += new System.EventHandler(this.BtnAddNewClip_Click);
            // 
            // TBClipName
            // 
            this.TBClipName.Location = new System.Drawing.Point(5, 36);
            this.TBClipName.Margin = new System.Windows.Forms.Padding(2);
            this.TBClipName.Name = "TBClipName";
            this.TBClipName.Size = new System.Drawing.Size(178, 23);
            this.TBClipName.TabIndex = 13;
            // 
            // MTBClipStartTime
            // 
            this.MTBClipStartTime.Location = new System.Drawing.Point(5, 79);
            this.MTBClipStartTime.Margin = new System.Windows.Forms.Padding(2);
            this.MTBClipStartTime.Mask = "00:00:00.000";
            this.MTBClipStartTime.Name = "MTBClipStartTime";
            this.MTBClipStartTime.Size = new System.Drawing.Size(178, 23);
            this.MTBClipStartTime.TabIndex = 15;
            // 
            // MTBClipEndTime
            // 
            this.MTBClipEndTime.Location = new System.Drawing.Point(5, 120);
            this.MTBClipEndTime.Margin = new System.Windows.Forms.Padding(2);
            this.MTBClipEndTime.Mask = "00:00:00.000";
            this.MTBClipEndTime.Name = "MTBClipEndTime";
            this.MTBClipEndTime.Size = new System.Drawing.Size(178, 23);
            this.MTBClipEndTime.TabIndex = 17;
            // 
            // CBClipAudioOnly
            // 
            this.CBClipAudioOnly.AutoSize = true;
            this.CBClipAudioOnly.Location = new System.Drawing.Point(198, 13);
            this.CBClipAudioOnly.Margin = new System.Windows.Forms.Padding(2);
            this.CBClipAudioOnly.Name = "CBClipAudioOnly";
            this.CBClipAudioOnly.Size = new System.Drawing.Size(86, 19);
            this.CBClipAudioOnly.TabIndex = 19;
            this.CBClipAudioOnly.Text = "僅輸出音訊";
            this.CBClipAudioOnly.UseVisualStyleBackColor = true;
            // 
            // LSourceFilePath
            // 
            this.LSourceFilePath.AutoSize = true;
            this.LSourceFilePath.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LSourceFilePath.Location = new System.Drawing.Point(10, 7);
            this.LSourceFilePath.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LSourceFilePath.Name = "LSourceFilePath";
            this.LSourceFilePath.Size = new System.Drawing.Size(55, 15);
            this.LSourceFilePath.TabIndex = 1;
            this.LSourceFilePath.Text = "檔案路徑";
            // 
            // BtnSelectFile
            // 
            this.BtnSelectFile.ForeColor = System.Drawing.SystemColors.Highlight;
            this.BtnSelectFile.Location = new System.Drawing.Point(890, 22);
            this.BtnSelectFile.Margin = new System.Windows.Forms.Padding(2);
            this.BtnSelectFile.Name = "BtnSelectFile";
            this.BtnSelectFile.Size = new System.Drawing.Size(73, 23);
            this.BtnSelectFile.TabIndex = 3;
            this.BtnSelectFile.Text = "選擇檔案";
            this.BtnSelectFile.UseVisualStyleBackColor = true;
            this.BtnSelectFile.Click += new System.EventHandler(this.BtnSelectFile_Click);
            // 
            // LFileDuration
            // 
            this.LFileDuration.AutoSize = true;
            this.LFileDuration.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LFileDuration.Location = new System.Drawing.Point(10, 51);
            this.LFileDuration.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LFileDuration.Name = "LFileDuration";
            this.LFileDuration.Size = new System.Drawing.Size(91, 15);
            this.LFileDuration.TabIndex = 5;
            this.LFileDuration.Text = "檔案時間長度：";
            // 
            // LTotalLengthValue
            // 
            this.LTotalLengthValue.AutoSize = true;
            this.LTotalLengthValue.ForeColor = System.Drawing.SystemColors.Highlight;
            this.LTotalLengthValue.Location = new System.Drawing.Point(103, 51);
            this.LTotalLengthValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LTotalLengthValue.Name = "LTotalLengthValue";
            this.LTotalLengthValue.Size = new System.Drawing.Size(72, 15);
            this.LTotalLengthValue.TabIndex = 6;
            this.LTotalLengthValue.Text = "0:00:00.000";
            // 
            // LClipName
            // 
            this.LClipName.AutoSize = true;
            this.LClipName.Location = new System.Drawing.Point(5, 18);
            this.LClipName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LClipName.Name = "LClipName";
            this.LClipName.Size = new System.Drawing.Size(55, 15);
            this.LClipName.TabIndex = 12;
            this.LClipName.Text = "短片名稱";
            // 
            // LClipStartTime
            // 
            this.LClipStartTime.AutoSize = true;
            this.LClipStartTime.Location = new System.Drawing.Point(5, 62);
            this.LClipStartTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LClipStartTime.Name = "LClipStartTime";
            this.LClipStartTime.Size = new System.Drawing.Size(55, 15);
            this.LClipStartTime.TabIndex = 14;
            this.LClipStartTime.Text = "開始時間";
            // 
            // LClipEndTime
            // 
            this.LClipEndTime.AutoSize = true;
            this.LClipEndTime.Location = new System.Drawing.Point(5, 103);
            this.LClipEndTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LClipEndTime.Name = "LClipEndTime";
            this.LClipEndTime.Size = new System.Drawing.Size(55, 15);
            this.LClipEndTime.TabIndex = 16;
            this.LClipEndTime.Text = "結束時間";
            // 
            // GBNewClip
            // 
            this.GBNewClip.Controls.Add(this.CBUseCrf28);
            this.GBNewClip.Controls.Add(this.TBClipName);
            this.GBNewClip.Controls.Add(this.BtnResetNewClip);
            this.GBNewClip.Controls.Add(this.LClipEndTime);
            this.GBNewClip.Controls.Add(this.LClipName);
            this.GBNewClip.Controls.Add(this.LClipStartTime);
            this.GBNewClip.Controls.Add(this.MTBClipStartTime);
            this.GBNewClip.Controls.Add(this.BtnAddNewClip);
            this.GBNewClip.Controls.Add(this.CBClipAudioOnly);
            this.GBNewClip.Controls.Add(this.MTBClipEndTime);
            this.GBNewClip.Location = new System.Drawing.Point(679, 87);
            this.GBNewClip.Margin = new System.Windows.Forms.Padding(2);
            this.GBNewClip.Name = "GBNewClip";
            this.GBNewClip.Padding = new System.Windows.Forms.Padding(2);
            this.GBNewClip.Size = new System.Drawing.Size(285, 149);
            this.GBNewClip.TabIndex = 11;
            this.GBNewClip.TabStop = false;
            this.GBNewClip.Text = "新增短片";
            // 
            // CBUseCrf28
            // 
            this.CBUseCrf28.AutoSize = true;
            this.CBUseCrf28.Location = new System.Drawing.Point(84, 13);
            this.CBUseCrf28.Margin = new System.Windows.Forms.Padding(2);
            this.CBUseCrf28.Name = "CBUseCrf28";
            this.CBUseCrf28.Size = new System.Drawing.Size(113, 19);
            this.CBUseCrf28.TabIndex = 18;
            this.CBUseCrf28.Text = "使用參數 -crf 28";
            this.CBUseCrf28.UseVisualStyleBackColor = true;
            // 
            // BtnResetNewClip
            // 
            this.BtnResetNewClip.Location = new System.Drawing.Point(192, 118);
            this.BtnResetNewClip.Margin = new System.Windows.Forms.Padding(2);
            this.BtnResetNewClip.Name = "BtnResetNewClip";
            this.BtnResetNewClip.Size = new System.Drawing.Size(89, 23);
            this.BtnResetNewClip.TabIndex = 21;
            this.BtnResetNewClip.Text = "重設";
            this.BtnResetNewClip.UseVisualStyleBackColor = true;
            this.BtnResetNewClip.Click += new System.EventHandler(this.BtnResetNewClip_Click);
            // 
            // CBAllowOverwrite
            // 
            this.CBAllowOverwrite.AutoSize = true;
            this.CBAllowOverwrite.Location = new System.Drawing.Point(679, 269);
            this.CBAllowOverwrite.Margin = new System.Windows.Forms.Padding(2);
            this.CBAllowOverwrite.Name = "CBAllowOverwrite";
            this.CBAllowOverwrite.Size = new System.Drawing.Size(98, 19);
            this.CBAllowOverwrite.TabIndex = 25;
            this.CBAllowOverwrite.Text = "允許覆蓋檔案";
            this.CBAllowOverwrite.UseVisualStyleBackColor = true;
            // 
            // LClipList
            // 
            this.LClipList.AutoSize = true;
            this.LClipList.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LClipList.Location = new System.Drawing.Point(10, 69);
            this.LClipList.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LClipList.Name = "LClipList";
            this.LClipList.Size = new System.Drawing.Size(55, 15);
            this.LClipList.TabIndex = 9;
            this.LClipList.Text = "短片列表";
            // 
            // LLog
            // 
            this.LLog.AutoSize = true;
            this.LLog.Font = new System.Drawing.Font("Microsoft JhengHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.LLog.Location = new System.Drawing.Point(10, 437);
            this.LLog.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.LLog.Name = "LLog";
            this.LLog.Size = new System.Drawing.Size(55, 15);
            this.LLog.TabIndex = 28;
            this.LLog.Text = "操作記錄";
            // 
            // BtnClearClipList
            // 
            this.BtnClearClipList.Location = new System.Drawing.Point(587, 58);
            this.BtnClearClipList.Margin = new System.Windows.Forms.Padding(2);
            this.BtnClearClipList.Name = "BtnClearClipList";
            this.BtnClearClipList.Size = new System.Drawing.Size(87, 23);
            this.BtnClearClipList.TabIndex = 8;
            this.BtnClearClipList.Text = "清除短片列表";
            this.BtnClearClipList.UseVisualStyleBackColor = true;
            this.BtnClearClipList.Click += new System.EventHandler(this.BtnClearClipList_Click);
            // 
            // BtnResetOperation
            // 
            this.BtnResetOperation.Location = new System.Drawing.Point(766, 241);
            this.BtnResetOperation.Margin = new System.Windows.Forms.Padding(2);
            this.BtnResetOperation.Name = "BtnResetOperation";
            this.BtnResetOperation.Size = new System.Drawing.Size(82, 23);
            this.BtnResetOperation.TabIndex = 23;
            this.BtnResetOperation.Text = "重設作業";
            this.BtnResetOperation.UseVisualStyleBackColor = true;
            this.BtnResetOperation.Click += new System.EventHandler(this.BtnResetOperation_Click);
            // 
            // BtnCancelFFmpeg
            // 
            this.BtnCancelFFmpeg.Location = new System.Drawing.Point(853, 241);
            this.BtnCancelFFmpeg.Margin = new System.Windows.Forms.Padding(2);
            this.BtnCancelFFmpeg.Name = "BtnCancelFFmpeg";
            this.BtnCancelFFmpeg.Size = new System.Drawing.Size(110, 23);
            this.BtnCancelFFmpeg.TabIndex = 24;
            this.BtnCancelFFmpeg.Text = "中斷 FFmpeg";
            this.BtnCancelFFmpeg.UseVisualStyleBackColor = true;
            this.BtnCancelFFmpeg.Click += new System.EventHandler(this.BtnCancelFFmpeg_Click);
            // 
            // CBCodecUseCopy
            // 
            this.CBCodecUseCopy.AutoSize = true;
            this.CBCodecUseCopy.Checked = true;
            this.CBCodecUseCopy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBCodecUseCopy.Location = new System.Drawing.Point(679, 293);
            this.CBCodecUseCopy.Margin = new System.Windows.Forms.Padding(2);
            this.CBCodecUseCopy.Name = "CBCodecUseCopy";
            this.CBCodecUseCopy.Size = new System.Drawing.Size(119, 19);
            this.CBCodecUseCopy.TabIndex = 26;
            this.CBCodecUseCopy.Text = "編碼器使用 Copy";
            this.CBCodecUseCopy.UseVisualStyleBackColor = true;
            // 
            // CBUseMatroska
            // 
            this.CBUseMatroska.AutoSize = true;
            this.CBUseMatroska.Location = new System.Drawing.Point(679, 316);
            this.CBUseMatroska.Margin = new System.Windows.Forms.Padding(2);
            this.CBUseMatroska.Name = "CBUseMatroska";
            this.CBUseMatroska.Size = new System.Drawing.Size(157, 19);
            this.CBUseMatroska.TabIndex = 27;
            this.CBUseMatroska.Text = "使用 Matroska 格式封裝";
            this.CBUseMatroska.UseVisualStyleBackColor = true;
            // 
            // DGVClipList
            // 
            this.DGVClipList.AllowUserToAddRows = false;
            this.DGVClipList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGVClipList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.DGVClipList.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.DGVClipList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGVClipList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ClipNo,
            this.ClipName,
            this.StartTime,
            this.EndTime,
            this.AudioOnly,
            this.UseCrf28});
            this.DGVClipList.Location = new System.Drawing.Point(10, 87);
            this.DGVClipList.Name = "DGVClipList";
            this.DGVClipList.RowHeadersWidth = 51;
            this.DGVClipList.RowTemplate.Height = 25;
            this.DGVClipList.Size = new System.Drawing.Size(664, 347);
            this.DGVClipList.TabIndex = 10;
            this.DGVClipList.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DGVClipList_CellValidating);
            // 
            // ClipNo
            // 
            this.ClipNo.DataPropertyName = "ClipNo";
            this.ClipNo.HeaderText = "短片編號";
            this.ClipNo.MinimumWidth = 6;
            this.ClipNo.Name = "ClipNo";
            this.ClipNo.ToolTipText = "短片的編號。";
            // 
            // ClipName
            // 
            this.ClipName.DataPropertyName = "ClipName";
            this.ClipName.HeaderText = "短片名稱";
            this.ClipName.MinimumWidth = 6;
            this.ClipName.Name = "ClipName";
            this.ClipName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ClipName.ToolTipText = "短片的名稱。";
            // 
            // StartTime
            // 
            this.StartTime.DataPropertyName = "StartTime";
            this.StartTime.HeaderText = "開始時間";
            this.StartTime.MinimumWidth = 6;
            this.StartTime.Name = "StartTime";
            this.StartTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.StartTime.ToolTipText = "短片的開始時間。";
            // 
            // EndTime
            // 
            this.EndTime.DataPropertyName = "EndTime";
            this.EndTime.HeaderText = "結束時間";
            this.EndTime.MinimumWidth = 6;
            this.EndTime.Name = "EndTime";
            this.EndTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.EndTime.ToolTipText = "短片的結束時間。";
            // 
            // AudioOnly
            // 
            this.AudioOnly.DataPropertyName = "AudioOnly";
            this.AudioOnly.HeaderText = "僅輸出音訊";
            this.AudioOnly.MinimumWidth = 6;
            this.AudioOnly.Name = "AudioOnly";
            this.AudioOnly.ToolTipText = "設定短片是否僅輸出音訊。";
            // 
            // UseCrf28
            // 
            this.UseCrf28.DataPropertyName = "UseCrf28";
            this.UseCrf28.HeaderText = "使用參數 -crf 28";
            this.UseCrf28.MinimumWidth = 6;
            this.UseCrf28.Name = "UseCrf28";
            this.UseCrf28.ToolTipText = "設定編碼時是否使用參數 -crf 28。";
            // 
            // BtnImport
            // 
            this.BtnImport.Location = new System.Drawing.Point(487, 58);
            this.BtnImport.Name = "BtnImport";
            this.BtnImport.Size = new System.Drawing.Size(95, 23);
            this.BtnImport.TabIndex = 7;
            this.BtnImport.Text = "匯入時間標記";
            this.BtnImport.UseVisualStyleBackColor = true;
            this.BtnImport.Click += new System.EventHandler(this.BtnImport_Click);
            // 
            // BtnCheckFFmpeg
            // 
            this.BtnCheckFFmpeg.Location = new System.Drawing.Point(853, 58);
            this.BtnCheckFFmpeg.Name = "BtnCheckFFmpeg";
            this.BtnCheckFFmpeg.Size = new System.Drawing.Size(110, 23);
            this.BtnCheckFFmpeg.TabIndex = 4;
            this.BtnCheckFFmpeg.Text = "檢查 FFmpeg";
            this.BtnCheckFFmpeg.UseVisualStyleBackColor = true;
            this.BtnCheckFFmpeg.Click += new System.EventHandler(this.BtnCheckFFmpeg_Click);
            // 
            // LVersion
            // 
            this.LVersion.AutoSize = true;
            this.LVersion.Location = new System.Drawing.Point(10, 525);
            this.LVersion.Name = "LVersion";
            this.LVersion.Size = new System.Drawing.Size(31, 15);
            this.LVersion.TabIndex = 30;
            this.LVersion.Text = "版本";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 552);
            this.Controls.Add(this.LVersion);
            this.Controls.Add(this.BtnCheckFFmpeg);
            this.Controls.Add(this.BtnImport);
            this.Controls.Add(this.DGVClipList);
            this.Controls.Add(this.CBUseMatroska);
            this.Controls.Add(this.CBCodecUseCopy);
            this.Controls.Add(this.CBAllowOverwrite);
            this.Controls.Add(this.BtnCancelFFmpeg);
            this.Controls.Add(this.BtnResetOperation);
            this.Controls.Add(this.BtnClearClipList);
            this.Controls.Add(this.LLog);
            this.Controls.Add(this.LClipList);
            this.Controls.Add(this.GBNewClip);
            this.Controls.Add(this.LTotalLengthValue);
            this.Controls.Add(this.LFileDuration);
            this.Controls.Add(this.BtnSelectFile);
            this.Controls.Add(this.LSourceFilePath);
            this.Controls.Add(this.TBLog);
            this.Controls.Add(this.BtnStartOperation);
            this.Controls.Add(this.TBSourceFilePath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "影片分割器";
            this.GBNewClip.ResumeLayout(false);
            this.GBNewClip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGVClipList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TBSourceFilePath;
        private System.Windows.Forms.Button BtnStartOperation;
        private System.Windows.Forms.TextBox TBLog;
        private System.Windows.Forms.Button BtnAddNewClip;
        private System.Windows.Forms.TextBox TBClipName;
        private System.Windows.Forms.MaskedTextBox MTBClipStartTime;
        private System.Windows.Forms.MaskedTextBox MTBClipEndTime;
        private System.Windows.Forms.CheckBox CBClipAudioOnly;
        private System.Windows.Forms.Label LSourceFilePath;
        private System.Windows.Forms.Button BtnSelectFile;
        private System.Windows.Forms.Label LFileDuration;
        private System.Windows.Forms.Label LTotalLengthValue;
        private System.Windows.Forms.Label LClipName;
        private System.Windows.Forms.Label LClipStartTime;
        private System.Windows.Forms.Label LClipEndTime;
        private System.Windows.Forms.GroupBox GBNewClip;
        private System.Windows.Forms.Label LClipList;
        private System.Windows.Forms.Label LLog;
        private System.Windows.Forms.Button BtnClearClipList;
        private System.Windows.Forms.Button BtnResetNewClip;
        private System.Windows.Forms.Button BtnResetOperation;
        private System.Windows.Forms.Button BtnCancelFFmpeg;
        private System.Windows.Forms.CheckBox CBAllowOverwrite;
        private System.Windows.Forms.CheckBox CBCodecUseCopy;
        private System.Windows.Forms.CheckBox CBUseMatroska;
        private System.Windows.Forms.DataGridView DGVClipList;
        private System.Windows.Forms.Button BtnImport;
        private System.Windows.Forms.Button BtnCheckFFmpeg;
        private DataGridViewTextBoxColumn ClipNo;
        private DataGridViewTextBoxColumn ClipName;
        private DataGridViewTextBoxColumn StartTime;
        private DataGridViewTextBoxColumn EndTime;
        private DataGridViewCheckBoxColumn AudioOnly;
        private DataGridViewCheckBoxColumn UseCrf28;
        private CheckBox CBUseCrf28;
        private Label LVersion;
    }
}