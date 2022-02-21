using Sunny.UI;

namespace ThermalCameraNet
{
    partial class FormMain : UIForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.btnOpenCamera = new Sunny.UI.UIButton();
            this.timerProcessFrame = new System.Windows.Forms.Timer(this.components);
            this.cbxCameraList = new Sunny.UI.UIComboBox();
            this.btnRefresh = new Sunny.UI.UIButton();
            this.btnCloseCamera = new Sunny.UI.UIButton();
            this.lblFPS = new Sunny.UI.UILabel();
            this.btnCameraSettings = new Sunny.UI.UIButton();
            this.uiLabel1 = new Sunny.UI.UILabel();
            this.cbxCameraMode = new Sunny.UI.UIComboBox();
            this.uiLabel2 = new Sunny.UI.UILabel();
            this.uiLabel3 = new Sunny.UI.UILabel();
            this.uiLabel4 = new Sunny.UI.UILabel();
            this.uiLabel5 = new Sunny.UI.UILabel();
            this.cbxFaceDetect = new Sunny.UI.UICheckBox();
            this.cbxHotmap = new Sunny.UI.UICheckBox();
            this.btnOpenVideo = new Sunny.UI.UIButton();
            this.openFileDialogVideo = new System.Windows.Forms.OpenFileDialog();
            this.lblTime = new Sunny.UI.UILabel();
            this.Video_seek = new System.Windows.Forms.TrackBar();
            this.cbxRepeat = new Sunny.UI.UICheckBox();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.picHotmap = new System.Windows.Forms.PictureBox();
            this.picPreview = new System.Windows.Forms.PictureBox();
            this.lblFrame = new Sunny.UI.UILabel();
            ((System.ComponentModel.ISupportInitialize)(this.Video_seek)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHotmap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpenCamera
            // 
            this.btnOpenCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenCamera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenCamera.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.btnOpenCamera.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.btnOpenCamera.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(233)))), ((int)(((byte)(255)))));
            this.btnOpenCamera.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.btnOpenCamera.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.btnOpenCamera.Font = new System.Drawing.Font("Microsoft YaHei", 11F, System.Drawing.FontStyle.Bold);
            this.btnOpenCamera.ForeHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnOpenCamera.ForePressColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnOpenCamera.ForeSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnOpenCamera.Location = new System.Drawing.Point(680, 50);
            this.btnOpenCamera.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnOpenCamera.Name = "btnOpenCamera";
            this.btnOpenCamera.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnOpenCamera.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnOpenCamera.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnOpenCamera.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnOpenCamera.Size = new System.Drawing.Size(223, 53);
            this.btnOpenCamera.Style = Sunny.UI.UIStyle.Custom;
            this.btnOpenCamera.TabIndex = 3;
            this.btnOpenCamera.Text = "Open Camera";
            this.btnOpenCamera.Click += new System.EventHandler(this.btnOpenCamera_Click);
            // 
            // timerProcessFrame
            // 
            this.timerProcessFrame.Interval = 10;
            this.timerProcessFrame.Tick += new System.EventHandler(this.timerProcessFrame_Tick);
            // 
            // cbxCameraList
            // 
            this.cbxCameraList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxCameraList.DataSource = null;
            this.cbxCameraList.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cbxCameraList.FillColor = System.Drawing.Color.White;
            this.cbxCameraList.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.cbxCameraList.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cbxCameraList.Location = new System.Drawing.Point(101, 50);
            this.cbxCameraList.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbxCameraList.MinimumSize = new System.Drawing.Size(63, 0);
            this.cbxCameraList.Name = "cbxCameraList";
            this.cbxCameraList.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cbxCameraList.Size = new System.Drawing.Size(404, 29);
            this.cbxCameraList.Style = Sunny.UI.UIStyle.Custom;
            this.cbxCameraList.TabIndex = 4;
            this.cbxCameraList.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.btnRefresh.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.btnRefresh.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(233)))), ((int)(((byte)(255)))));
            this.btnRefresh.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.btnRefresh.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft YaHei", 11F, System.Drawing.FontStyle.Bold);
            this.btnRefresh.ForeHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnRefresh.ForePressColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnRefresh.ForeSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnRefresh.Location = new System.Drawing.Point(528, 45);
            this.btnRefresh.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnRefresh.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnRefresh.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnRefresh.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnRefresh.Size = new System.Drawing.Size(128, 40);
            this.btnRefresh.Style = Sunny.UI.UIStyle.Custom;
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnCloseCamera
            // 
            this.btnCloseCamera.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCloseCamera.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCloseCamera.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.btnCloseCamera.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.btnCloseCamera.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(233)))), ((int)(((byte)(255)))));
            this.btnCloseCamera.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.btnCloseCamera.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.btnCloseCamera.Font = new System.Drawing.Font("Microsoft YaHei", 11F, System.Drawing.FontStyle.Bold);
            this.btnCloseCamera.ForeHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnCloseCamera.ForePressColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnCloseCamera.ForeSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnCloseCamera.Location = new System.Drawing.Point(680, 120);
            this.btnCloseCamera.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnCloseCamera.Name = "btnCloseCamera";
            this.btnCloseCamera.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnCloseCamera.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnCloseCamera.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnCloseCamera.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnCloseCamera.Size = new System.Drawing.Size(223, 53);
            this.btnCloseCamera.Style = Sunny.UI.UIStyle.Custom;
            this.btnCloseCamera.TabIndex = 6;
            this.btnCloseCamera.Text = "Close Camera";
            this.btnCloseCamera.Click += new System.EventHandler(this.btnCloseCamera_Click);
            // 
            // lblFPS
            // 
            this.lblFPS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFPS.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.lblFPS.ForeColor = System.Drawing.Color.White;
            this.lblFPS.Location = new System.Drawing.Point(809, 677);
            this.lblFPS.Name = "lblFPS";
            this.lblFPS.Size = new System.Drawing.Size(94, 34);
            this.lblFPS.Style = Sunny.UI.UIStyle.Custom;
            this.lblFPS.TabIndex = 7;
            this.lblFPS.Text = "0 fps";
            this.lblFPS.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCameraSettings
            // 
            this.btnCameraSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCameraSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCameraSettings.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.btnCameraSettings.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.btnCameraSettings.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(233)))), ((int)(((byte)(255)))));
            this.btnCameraSettings.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.btnCameraSettings.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.btnCameraSettings.Font = new System.Drawing.Font("Microsoft YaHei", 11F, System.Drawing.FontStyle.Bold);
            this.btnCameraSettings.ForeHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnCameraSettings.ForePressColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnCameraSettings.ForeSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnCameraSettings.Location = new System.Drawing.Point(680, 190);
            this.btnCameraSettings.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnCameraSettings.Name = "btnCameraSettings";
            this.btnCameraSettings.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnCameraSettings.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnCameraSettings.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnCameraSettings.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnCameraSettings.Size = new System.Drawing.Size(223, 53);
            this.btnCameraSettings.Style = Sunny.UI.UIStyle.Custom;
            this.btnCameraSettings.TabIndex = 8;
            this.btnCameraSettings.Text = "Camera Settings";
            this.btnCameraSettings.Click += new System.EventHandler(this.btnCameraSettings_Click);
            // 
            // uiLabel1
            // 
            this.uiLabel1.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.uiLabel1.ForeColor = System.Drawing.Color.White;
            this.uiLabel1.Location = new System.Drawing.Point(16, 52);
            this.uiLabel1.Name = "uiLabel1";
            this.uiLabel1.Size = new System.Drawing.Size(91, 23);
            this.uiLabel1.Style = Sunny.UI.UIStyle.Custom;
            this.uiLabel1.TabIndex = 9;
            this.uiLabel1.Text = "Device:";
            this.uiLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxCameraMode
            // 
            this.cbxCameraMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxCameraMode.DataSource = null;
            this.cbxCameraMode.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cbxCameraMode.FillColor = System.Drawing.Color.White;
            this.cbxCameraMode.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.cbxCameraMode.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cbxCameraMode.Items.AddRange(new object[] {
            "MSMF",
            "DShow",
            "FFmepg",
            "V4L2"});
            this.cbxCameraMode.Location = new System.Drawing.Point(680, 384);
            this.cbxCameraMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbxCameraMode.MinimumSize = new System.Drawing.Size(63, 0);
            this.cbxCameraMode.Name = "cbxCameraMode";
            this.cbxCameraMode.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cbxCameraMode.Size = new System.Drawing.Size(223, 29);
            this.cbxCameraMode.Style = Sunny.UI.UIStyle.Custom;
            this.cbxCameraMode.TabIndex = 10;
            this.cbxCameraMode.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel2
            // 
            this.uiLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiLabel2.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.uiLabel2.ForeColor = System.Drawing.Color.White;
            this.uiLabel2.Location = new System.Drawing.Point(680, 342);
            this.uiLabel2.Name = "uiLabel2";
            this.uiLabel2.Size = new System.Drawing.Size(174, 23);
            this.uiLabel2.Style = Sunny.UI.UIStyle.Custom;
            this.uiLabel2.TabIndex = 11;
            this.uiLabel2.Text = "Camera Driver:";
            this.uiLabel2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel3
            // 
            this.uiLabel3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.uiLabel3.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.uiLabel3.ForeColor = System.Drawing.Color.White;
            this.uiLabel3.Location = new System.Drawing.Point(16, 687);
            this.uiLabel3.Name = "uiLabel3";
            this.uiLabel3.Size = new System.Drawing.Size(52, 23);
            this.uiLabel3.Style = Sunny.UI.UIStyle.Custom;
            this.uiLabel3.TabIndex = 13;
            this.uiLabel3.Text = "0°C";
            this.uiLabel3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uiLabel4
            // 
            this.uiLabel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uiLabel4.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.uiLabel4.ForeColor = System.Drawing.Color.White;
            this.uiLabel4.Location = new System.Drawing.Point(291, 687);
            this.uiLabel4.Name = "uiLabel4";
            this.uiLabel4.Size = new System.Drawing.Size(58, 23);
            this.uiLabel4.Style = Sunny.UI.UIStyle.Custom;
            this.uiLabel4.TabIndex = 14;
            this.uiLabel4.Text = "60°C";
            this.uiLabel4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // uiLabel5
            // 
            this.uiLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.uiLabel5.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.uiLabel5.ForeColor = System.Drawing.Color.White;
            this.uiLabel5.Location = new System.Drawing.Point(590, 687);
            this.uiLabel5.Name = "uiLabel5";
            this.uiLabel5.Size = new System.Drawing.Size(71, 23);
            this.uiLabel5.Style = Sunny.UI.UIStyle.Custom;
            this.uiLabel5.TabIndex = 15;
            this.uiLabel5.Text = "120°C";
            this.uiLabel5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbxFaceDetect
            // 
            this.cbxFaceDetect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxFaceDetect.Checked = true;
            this.cbxFaceDetect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbxFaceDetect.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.cbxFaceDetect.ForeColor = System.Drawing.Color.White;
            this.cbxFaceDetect.Location = new System.Drawing.Point(680, 435);
            this.cbxFaceDetect.MinimumSize = new System.Drawing.Size(1, 1);
            this.cbxFaceDetect.Name = "cbxFaceDetect";
            this.cbxFaceDetect.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.cbxFaceDetect.Size = new System.Drawing.Size(223, 29);
            this.cbxFaceDetect.Style = Sunny.UI.UIStyle.Custom;
            this.cbxFaceDetect.TabIndex = 16;
            this.cbxFaceDetect.Text = "Face Detection";
            // 
            // cbxHotmap
            // 
            this.cbxHotmap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxHotmap.Checked = true;
            this.cbxHotmap.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbxHotmap.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.cbxHotmap.ForeColor = System.Drawing.Color.White;
            this.cbxHotmap.Location = new System.Drawing.Point(680, 479);
            this.cbxHotmap.MinimumSize = new System.Drawing.Size(1, 1);
            this.cbxHotmap.Name = "cbxHotmap";
            this.cbxHotmap.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.cbxHotmap.Size = new System.Drawing.Size(223, 29);
            this.cbxHotmap.Style = Sunny.UI.UIStyle.Custom;
            this.cbxHotmap.TabIndex = 17;
            this.cbxHotmap.Text = "Hot map";
            // 
            // btnOpenVideo
            // 
            this.btnOpenVideo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenVideo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenVideo.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.btnOpenVideo.FillColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.btnOpenVideo.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(233)))), ((int)(((byte)(255)))));
            this.btnOpenVideo.FillPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.btnOpenVideo.FillSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.btnOpenVideo.Font = new System.Drawing.Font("Microsoft YaHei", 11F, System.Drawing.FontStyle.Bold);
            this.btnOpenVideo.ForeHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnOpenVideo.ForePressColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnOpenVideo.ForeSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnOpenVideo.Location = new System.Drawing.Point(680, 260);
            this.btnOpenVideo.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnOpenVideo.Name = "btnOpenVideo";
            this.btnOpenVideo.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnOpenVideo.RectHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnOpenVideo.RectPressColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnOpenVideo.RectSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.btnOpenVideo.Size = new System.Drawing.Size(223, 53);
            this.btnOpenVideo.Style = Sunny.UI.UIStyle.Custom;
            this.btnOpenVideo.TabIndex = 18;
            this.btnOpenVideo.Text = "Open Video";
            this.btnOpenVideo.Click += new System.EventHandler(this.btnOpenVideo_Click);
            // 
            // openFileDialogVideo
            // 
            this.openFileDialogVideo.Filter = "Video files (*.mp4) | *.mp4; | All files (*.*) | *.*";
            // 
            // lblTime
            // 
            this.lblTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTime.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.lblTime.ForeColor = System.Drawing.Color.White;
            this.lblTime.Location = new System.Drawing.Point(384, 602);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(204, 32);
            this.lblTime.Style = Sunny.UI.UIStyle.Custom;
            this.lblTime.TabIndex = 49;
            this.lblTime.Text = "Time: 00:00:00";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Video_seek
            // 
            this.Video_seek.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Video_seek.AutoSize = false;
            this.Video_seek.Location = new System.Drawing.Point(16, 582);
            this.Video_seek.Name = "Video_seek";
            this.Video_seek.Size = new System.Drawing.Size(579, 24);
            this.Video_seek.TabIndex = 48;
            this.Video_seek.TickStyle = System.Windows.Forms.TickStyle.None;
            this.Video_seek.Scroll += new System.EventHandler(this.Video_seek_Scroll);
            // 
            // cbxRepeat
            // 
            this.cbxRepeat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbxRepeat.Checked = true;
            this.cbxRepeat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbxRepeat.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.cbxRepeat.ForeColor = System.Drawing.Color.White;
            this.cbxRepeat.Location = new System.Drawing.Point(21, 605);
            this.cbxRepeat.MinimumSize = new System.Drawing.Size(1, 1);
            this.cbxRepeat.Name = "cbxRepeat";
            this.cbxRepeat.Padding = new System.Windows.Forms.Padding(22, 0, 0, 0);
            this.cbxRepeat.Size = new System.Drawing.Size(223, 29);
            this.cbxRepeat.Style = Sunny.UI.UIStyle.Custom;
            this.cbxRepeat.TabIndex = 52;
            this.cbxRepeat.Text = "Repeat";
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlay.BackColor = System.Drawing.Color.Transparent;
            this.btnPlay.BackgroundImage = global::ThermalCameraNet.Properties.Resources.play;
            this.btnPlay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPlay.Location = new System.Drawing.Point(632, 582);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(25, 25);
            this.btnPlay.TabIndex = 51;
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnReload
            // 
            this.btnReload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReload.BackColor = System.Drawing.Color.Transparent;
            this.btnReload.BackgroundImage = global::ThermalCameraNet.Properties.Resources.reload;
            this.btnReload.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnReload.Location = new System.Drawing.Point(601, 582);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(25, 25);
            this.btnReload.TabIndex = 50;
            this.btnReload.UseVisualStyleBackColor = false;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // picHotmap
            // 
            this.picHotmap.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picHotmap.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.picHotmap.Image = global::ThermalCameraNet.Properties.Resources.hotmap;
            this.picHotmap.Location = new System.Drawing.Point(16, 644);
            this.picHotmap.Name = "picHotmap";
            this.picHotmap.Size = new System.Drawing.Size(640, 40);
            this.picHotmap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picHotmap.TabIndex = 12;
            this.picHotmap.TabStop = false;
            // 
            // picPreview
            // 
            this.picPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picPreview.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.picPreview.Location = new System.Drawing.Point(16, 96);
            this.picPreview.Name = "picPreview";
            this.picPreview.Size = new System.Drawing.Size(640, 480);
            this.picPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picPreview.TabIndex = 2;
            this.picPreview.TabStop = false;
            // 
            // lblFrame
            // 
            this.lblFrame.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFrame.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.lblFrame.ForeColor = System.Drawing.Color.White;
            this.lblFrame.Location = new System.Drawing.Point(663, 679);
            this.lblFrame.Name = "lblFrame";
            this.lblFrame.Size = new System.Drawing.Size(128, 32);
            this.lblFrame.Style = Sunny.UI.UIStyle.Custom;
            this.lblFrame.TabIndex = 53;
            this.lblFrame.Text = "Frame: 0";
            this.lblFrame.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 27F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(917, 720);
            this.ControlBoxFillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(216)))), ((int)(((byte)(233)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.lblFrame);
            this.Controls.Add(this.cbxRepeat);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.Video_seek);
            this.Controls.Add(this.btnOpenVideo);
            this.Controls.Add(this.cbxHotmap);
            this.Controls.Add(this.cbxFaceDetect);
            this.Controls.Add(this.uiLabel5);
            this.Controls.Add(this.uiLabel4);
            this.Controls.Add(this.uiLabel3);
            this.Controls.Add(this.picHotmap);
            this.Controls.Add(this.uiLabel2);
            this.Controls.Add(this.cbxCameraMode);
            this.Controls.Add(this.btnCameraSettings);
            this.Controls.Add(this.lblFPS);
            this.Controls.Add(this.btnCloseCamera);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.cbxCameraList);
            this.Controls.Add(this.btnOpenCamera);
            this.Controls.Add(this.picPreview);
            this.Controls.Add(this.uiLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.Style = Sunny.UI.UIStyle.Custom;
            this.Text = "Thermal Camera";
            this.TitleColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(130)))), ((int)(((byte)(130)))));
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Video_seek)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHotmap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UIButton btnOpenCamera;
        private System.Windows.Forms.PictureBox picPreview;
        private System.Windows.Forms.Timer timerProcessFrame;
        private UIComboBox cbxCameraList;
        private UIButton btnRefresh;
        private UIButton btnCloseCamera;
        private UILabel lblFPS;
        private UIButton btnCameraSettings;
        private UILabel uiLabel1;
        private UIComboBox cbxCameraMode;
        private UILabel uiLabel2;
        private System.Windows.Forms.PictureBox picHotmap;
        private UILabel uiLabel3;
        private UILabel uiLabel4;
        private UILabel uiLabel5;
        private UICheckBox cbxFaceDetect;
        private UICheckBox cbxHotmap;
        private UIButton btnOpenVideo;
        private System.Windows.Forms.OpenFileDialog openFileDialogVideo;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnReload;
        private UILabel lblTime;
        private System.Windows.Forms.TrackBar Video_seek;
        private UICheckBox cbxRepeat;
        private UILabel lblFrame;
    }
}

