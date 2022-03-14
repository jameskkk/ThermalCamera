using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny.UI;
using Emgu.CV;
using Emgu.CV.CvEnum;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ThermalCameraNet
{
    public partial class FormMain : UIForm
    {
        #region Properties

        private const int VIDEO_WIDTH = 640;
        private const int VIDEO_HEIGHT = 480;
        private const int GTC_VIDEO_WIDTH = 160;
        private const int GTC_VIDEO_HEIGHT = 120;

        private VideoCapture m_CaptureVideo = null;
        private Stopwatch m_Stopwatch = null;
        private Stopwatch m_LineStopwatch = null;
        private double m_TotalFrames = 0; // Total frame of video
        private double m_FrameRate = 0; // Capture frame Rate
        private List<CameraDevice> m_LstCamera = null;
        private Brush m_BrushTemperature = new SolidBrush(Color.Blue);
        private Font m_FontTemperature = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);
        private Font m_FontGTCTemperature = new Font("Arial", 8, FontStyle.Regular, GraphicsUnit.Pixel);
        private bool m_IsVideo = false;
        private bool m_IsPlay = false;
        private TTSUnit m_TTSUnit = new TTSUnit();
        private bool m_GTCCamera = false;
        private Random m_Random = new Random();
        private FaceUnit m_FaceUnit = null;

        #endregion

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            btnRefresh_Click(sender, e);
            cbxCameraMode.SelectedIndex = Properties.Settings.Default.CameraMode; // DShow
            m_TTSUnit.InitSpeech();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnCloseCamera_Click(sender, e); 
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Application.Exit();
            System.Environment.Exit(0);
        }

        private void btnOpenCamera_Click(object sender, EventArgs e)
        {
            if (cbxCameraList.SelectedIndex != -1)
            {
                m_IsVideo = false;
                int camID = CameraUnit.GetCameraIndexForPartName(cbxCameraList.SelectedText);
                m_CaptureVideo = new VideoCapture(camID, GetCameraDriver());

                if (!m_CaptureVideo.IsOpened)
                {
                    MessageBox.Show("Cannot open camera!");
                    return;
                }
                Properties.Settings.Default.CameraMode = cbxCameraMode.SelectedIndex;
                Properties.Settings.Default.Save();
                InitVideoCapture(m_LstCamera[cbxCameraList.SelectedIndex].Caption);
            }
        }

        private void btnCloseCamera_Click(object sender, EventArgs e)
        {
            if (m_CaptureVideo != null && m_CaptureVideo.IsOpened)
            {
#if USB_TIME
                timerProcessFrame.Stop();
#else
                Application.Idle -= ProcessCameraFrame;
#endif
                m_CaptureVideo.Stop();
                m_CaptureVideo.Dispose();
                m_CaptureVideo = null; 
                LogUnit.Log.Info("btnCloseCamera_Click(): End...");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cbxCameraList.Items.Clear();
            m_LstCamera = CameraUnit.GetAllConnectedCameras();
            foreach (CameraDevice cameraDevice in m_LstCamera)
            {
                cbxCameraList.Items.Add(cameraDevice.Caption);
            }

            if (cbxCameraList.Items.Count > 0)
            {
                cbxCameraList.SelectedIndex = 0;
            }
        }

        private void btnCameraSettings_Click(object sender, EventArgs e)
        {
            if (m_CaptureVideo != null && m_CaptureVideo.IsOpened)
            {
                m_CaptureVideo.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Settings, 1);
            }
        }

        private void btnOpenVideo_Click(object sender, EventArgs e)
        {
            openFileDialogVideo.InitialDirectory = Properties.Settings.Default.Video_Path;
            if (openFileDialogVideo.ShowDialog(this) == DialogResult.OK)
            {
                m_IsVideo = true;
                Properties.Settings.Default.Video_Path = openFileDialogVideo.FileName;
                Properties.Settings.Default.Save();

                LogUnit.Log.Info("Open Video File: " + openFileDialogVideo.FileName);
                m_CaptureVideo = new VideoCapture(openFileDialogVideo.FileName);

                if (!m_CaptureVideo.IsOpened)
                {
                    MessageBox.Show("Cannot open camera or video!");
                    return;
                }
                InitVideoCapture("Video");
            }
        }

        private void btnLoadBin_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] byteBuffer = File.ReadAllBytes("GTCImage.bin");
                //Mat gray = new Mat(new Size(180, 160), DepthType.Cv8U, 1);
                //LogUnit.Log.Info("Gray Size = " + gray.GetRawData().Length);
                //gray.SetTo(byteBuffer);
                ParseThermalData(byteBuffer, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            if (m_IsVideo && m_CaptureVideo != null)
            {
                m_Stopwatch.Restart();
                Video_seek.Value = 0;
                Video_seek_Scroll(sender, e);
            }
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (m_IsVideo && m_CaptureVideo != null)
            {
                if (m_IsPlay)
                {
                    m_IsPlay = false;
                    btnPlay.Image = Properties.Resources.play;
                    m_Stopwatch.Stop();
#if USB_TIME
                    timerProcessFrame.Stop();
#else
                    Application.Idle -= ProcessCameraFrame;
#endif
                }
                else
                {
                    m_IsPlay = true;
                    btnPlay.Image = Properties.Resources.pause;
                    m_Stopwatch.Start();
#if USB_TIME
                    timerProcessFrame.Start();
#else
                    Application.Idle += ProcessCameraFrame;
#endif
                }
            }
        }

        private void Video_seek_Scroll(object sender, EventArgs e)
        {
            if (m_IsVideo && m_CaptureVideo != null)
            {
                //LogUnit.Log.Info("Video_seek_Scroll() Video_seek.Value = " + Video_seek.Value);
                m_CaptureVideo.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames, Video_seek.Value);
                //ProcessCameraFrame(sender, e);
            }
        }

        [HandleProcessCorruptedStateExceptions, SecurityCritical]
        private void timerProcessFrame_Tick(object sender, EventArgs e)
        {
            try
            {
                ProcessCameraFrame(sender, e);
            }
            catch (Exception ex)
            {
                int line = LogUnit.GetExceptionLineNumber(ex);
                LogUnit.Log.Error("ProcessCameraFrame() Exception: line: " + line.ToString() + ", " + ex.Message);
            }
        }

        private void InitVideoCapture(string CameraCaption)
        {
            Directory.SetCurrentDirectory(FileUnit.AssemblyDirectory);
            if (m_FaceUnit == null)
            {
                m_FaceUnit = new FaceUnit();
                m_FaceUnit.InitFaceDetect();
            }

            if (CameraCaption.Contains("GTC"))
            {
                m_GTCCamera = true;
                m_CaptureVideo.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.ConvertRgb, cbxGTCFormat.Checked ? 0 : 1);
                m_CaptureVideo.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, GTC_VIDEO_HEIGHT);
                m_CaptureVideo.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, GTC_VIDEO_WIDTH);
                timerProcessFrame.Interval = 100;
            }
            else
            {
                m_GTCCamera = false;
                m_CaptureVideo.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, VIDEO_HEIGHT);
                m_CaptureVideo.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, VIDEO_WIDTH);
                timerProcessFrame.Interval = 10;

                m_CaptureVideo.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps, 30);
                m_FrameRate = m_CaptureVideo.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
                m_TotalFrames = m_CaptureVideo.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameCount);
                var codec_double = m_CaptureVideo.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FourCC);
                string codec = new string(Encoding.UTF8.GetString(BitConverter.GetBytes(Convert.ToUInt32(codec_double))).ToCharArray());
                LogUnit.Log.Info("InitVideoCapture() Codec: " + codec + ", FrameRate: " + m_FrameRate.ToString());
            }


            Video_seek.Value = 0;
            lblFPS.Text = m_FrameRate.ToString("0.0") + " fps";
            lblTime.Text = "Time: 00:00:00";

            if (m_LineStopwatch != null)
            {
                m_LineStopwatch.Stop();
                m_LineStopwatch = null;
            }

            if (m_IsVideo)
            {
                m_IsPlay = true;

                if (m_Stopwatch != null)
                {
                    m_Stopwatch.Restart();
                }
                else
                {
                    m_Stopwatch = new Stopwatch();
                    m_Stopwatch.Start();
                }
                Video_seek.Minimum = 0;
                Video_seek.Maximum = (int)m_TotalFrames - 1;
                btnPlay.Image = Properties.Resources.pause;
            }

#if USB_TIME
            timerProcessFrame.Start();
#else
            Application.Idle += ProcessCameraFrame;
#endif
        }

        public double GetDoubleValue(Mat mat, int row, int col)
        {
            var value = new double[1];
            Marshal.Copy(mat.DataPointer + (row * mat.Cols + col) * mat.ElementSize, value, 0, 1);
            return value[0];
        }

        public void SetDoubleValue(Mat mat, int row, int col, double value)
        {
            var target = new[] { value };
            Marshal.Copy(target, 0, mat.DataPointer + (row * mat.Cols + col) * mat.ElementSize, 1);
        }

        /// <summary>
        /// Parse GTC Camera Info
        /// </summary>
        /// <param name="byteBuffer"></param>
        /// <returns></returns>
        public float ParseThermalData(byte[] byteBuffer, bool isSaveTemperature)
        {
            // The UVC imcoming data is (1*28800)
            // Should be reshape to (180*160)
            int index = 0;
            byte[,] byteBuffer2D = new byte[180, 160];
            for (int x = 0; x < 180; x++)
            {
                for (int y = 0; y < 160; y++)
                {
                    byteBuffer2D[x, y] = byteBuffer[index];
                    index++;
                }
            }
            int tempValue = byteBuffer2D[120, 0] * 256 + byteBuffer2D[120, 1];
            int sensorOffset = byteBuffer2D[120, 2] * 256 + byteBuffer2D[120, 3];
            int tempRatio = byteBuffer2D[120, 4];
            LogUnit.Log.Info("tempValue = " + tempValue.ToString());
            LogUnit.Log.Info("sensorOffset = " + sensorOffset.ToString());
            LogUnit.Log.Info("tempRatio = " + tempRatio.ToString());

            if (sensorOffset > 32767)
                sensorOffset -= 65536;

            index = 0;
            byte[] rawImg = new byte[120 * 160];
            for (int x = 0; x < 120; x++)
            {
                for (int y = 0; y < 160; y++)
                {
                    rawImg[index] = byteBuffer2D[x, y];
                    index++;
                }
            }

            if (isSaveTemperature)
            {
                // Get tempImg value
                int[,] tempImg = new int[160, 120];
                for (int x = 0; x < 160; x++)
                {
                    for (int y = 0; y < 120; y++)
                    {
                        tempImg[x, y] = ((byteBuffer2D[y, x] - sensorOffset) / tempRatio) + (tempValue / 100);
                    }
                }

                using (StreamWriter file = new StreamWriter("tempImg.csv"))
                {
                    for (int x = 0; x < 160; x++)
                    {
                        for (int y = 0; y < 120; y++)
                        {
                            file.Write(tempImg[x, y].ToString("0.00"));
                            file.Write(",");
                        }
                        file.Write("\n");
                    }
                    file.Close();
                }
            }
           
            //Image <Gray, byte> image = new Image<Gray, byte>(160, 120);
            //image.Bytes = rawImg;
            //UMat gray = image.ToUMat();
            Mat gray = new Mat(120, 160, DepthType.Cv8U, 1);
            try
            {
                gray.SetTo<byte>(rawImg);
                Mat hotmap = new Mat();

                if (cbxHotmap.Checked)
                {
                    //LogUnit.Log.Info("ProcessCameraFrame(): ApplyColorMap() Start...");
                    CvInvoke.ApplyColorMap(gray, hotmap, ColorMapType.Jet);
                    ImageUnit.BindBitmapToPicture(picPreview, hotmap.ToBitmap());
                }
                else
                {
                    ImageUnit.BindBitmapToPicture(picPreview, gray.ToBitmap());
                }
            }
            catch (Exception ex)
            {
                int line = LogUnit.GetExceptionLineNumber(ex);
                LogUnit.Log.Error("ParseThermalData() Exception: line: " + line.ToString() + ", " + ex.Message);
            }
            finally
            {
                if (gray != null)
                    gray.Dispose();
            }

            return tempValue / 100.0f;
        }

        public double NextDouble(Random random, double minValue, double maxValue)
        {
            return random.NextDouble() * (maxValue - minValue) + minValue;
        }

        [HandleProcessCorruptedStateExceptions, SecurityCritical]
        private void ProcessCameraFrame(object sender, EventArgs e)
        {
            if (m_CaptureVideo == null)
                return;

            double framesNo = 0;
            double tempurature = NextDouble(m_Random, 36.0, 37.4);
            if (!m_GTCCamera)
                framesNo = m_CaptureVideo.GetCaptureProperty(CapProp.PosFrames);
            Mat frame = m_CaptureVideo.QueryFrame();
            if (frame != null)
            {
                Mat gray = new Mat();
                Mat hotmap = new Mat();
                try
                {
                    //LogUnit.Log.Info("ProcessCameraFrame(): CvtColor() Start...");
                    if (m_GTCCamera)
                    {
                        gray = frame.Clone();
                        if (cbxGTCFormat.Checked && cbxSaveImage.Checked)
                        {
                            //gray.Save("GTCImage.png");
                            using (FileStream fs = new FileStream("GTCImage.bin", FileMode.OpenOrCreate))
                            {
                                BinaryWriter bs = new BinaryWriter(fs);
                                bs.Write(gray.GetRawData());
                                LogUnit.Log.Info("Save GTCImage.bin.");
                            }
                        }
                    }
                    else
                        CvInvoke.CvtColor(frame, gray, ColorConversion.Bgr2Gray);

                    //LogUnit.Log.Info("ProcessCameraFrame(): BindBitmapToPicture() Start...");
                    if (!m_GTCCamera && cbxHotmap.Checked)
                    {
                        //LogUnit.Log.Info("ProcessCameraFrame(): ApplyColorMap() Start...");
                        CvInvoke.ApplyColorMap(gray, hotmap, ColorMapType.Jet);
                        ImageUnit.BindBitmapToPicture(picPreview, hotmap.ToBitmap());
                    }   
                    else
                    {
                        if (m_GTCCamera && cbxGTCFormat.Checked)
                        {
                            tempurature = ParseThermalData(gray.GetRawData(), false);
                        }
                        else
                        {
                            ImageUnit.BindBitmapToPicture(picPreview, frame.ToBitmap());
                        }
                    }

                    if (m_GTCCamera && cbxShowTemperature.Checked)
                    {
                        Graphics graphics = Graphics.FromImage(picPreview.Image);
                        DrawTemperature(graphics, tempurature.ToString("0.0") + "°C", 5, 5);
                    }
                    else if (!m_GTCCamera && cbxFaceDetect.Checked)
                    {
                        Graphics graphics = Graphics.FromImage(picPreview.Image);
                        Rectangle rectFace = new Rectangle(0, 0, 0, 0);
                        bool isFaceDetect = m_FaceUnit.DetectFaceAndEye(gray, graphics, cbxEyeDetect.Checked, ref rectFace);

                        if (cbxShowTemperature.Checked && isFaceDetect)
                        {
                            DrawTemperature(graphics, tempurature.ToString("0.0") + "°C", rectFace.X + 10, rectFace.Y + 10);

                            float lastSendTime = Properties.Settings.Default.LineDuration + 1;
                            if (m_LineStopwatch != null)
                                lastSendTime = (float)m_LineStopwatch.ElapsedMilliseconds / 1000;
                            else
                            {
                                m_LineStopwatch = new Stopwatch();
                                m_LineStopwatch.Start();
                            }

                            if (tempurature > 37.5 && lastSendTime > Properties.Settings.Default.LineDuration)
                            {
                                LogUnit.Log.Info("ProcessCameraFrame(): alarm over temperature...");
                                m_LineStopwatch.Restart();
                                if (cbxLineNotify.Checked)
                                {
                                    Image alarmImage = picPreview.Image;
                                    LineUnit.PushLineNotify(alarmImage);
                                }

                                if (cbxTTS.Checked)
                                {
                                    m_TTSUnit.SpeechAsync();
                                }
                            }
                        }

                    }

                    if (m_IsVideo && !m_GTCCamera)
                    {
                        //LogUnit.Log.Info("ProcessCameraFrame(): FPS compute Start...");
                        double fps = framesNo / ((float)m_Stopwatch.ElapsedMilliseconds / 1000);
                        lblFPS.Text = fps.ToString("0.0") + " fps";
                        lblFrame.Text = "Frame: " + framesNo.ToString();
                        double time_index = m_CaptureVideo.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosMsec);
                        lblTime.Text = "Time: " + TimeSpan.FromMilliseconds(time_index).ToString().Substring(0, 8);
                        Video_seek.Value = (int)(framesNo + 1);
                    }
                }
                catch (Exception ex)
                {
                    int line = LogUnit.GetExceptionLineNumber(ex);
                    LogUnit.Log.Error("ProcessCameraFrame() Exception: line: " + line.ToString() + ", " + ex.Message);
                }
                finally
                {
                    frame.Dispose();
                    gray.Dispose();
                    hotmap.Dispose();
                }
            }

            if (m_IsVideo && m_TotalFrames - 1 == framesNo)
            {
                if (cbxRepeat.Checked)
                {
                    btnReload_Click(sender, e);
                }
                else
                {
                    m_IsPlay = false;
                    btnPlay.Image = Properties.Resources.play;
#if USB_TIME
                    timerProcessFrame.Stop();
#else
                    Application.Idle -= ProcessCameraFrame;
#endif
                    Video_seek.Value = 0;
                    Video_seek_Scroll(sender, e);
                }
            }
        }

        private VideoCapture.API GetCameraDriver()
        {
            VideoCapture.API driver = 0;

            switch (cbxCameraMode.SelectedIndex)
            {
                case 0:
                    driver = VideoCapture.API.Msmf;
                    break;
                case 1:
                    driver = VideoCapture.API.DShow;
                    break;
                case 2:
                    driver = VideoCapture.API.Ffmpeg;
                    break;
                case 3:
                    driver = VideoCapture.API.V4L2;
                    break;
                default:
                    driver = VideoCapture.API.DShow;
                    break;
            }

            return driver;
        }

        private void DrawTemperature(Graphics graphics, string temperature, float x, float y)
        {
            if (cbxShowTemperature.Checked)
            {
                string probText = "Temp: " + temperature;
                var size = graphics.MeasureString(probText, m_FontTemperature);
                if (m_GTCCamera)
                    size = graphics.MeasureString(probText, m_FontGTCTemperature);
                var rectText = new RectangleF(x, y, size.Width, size.Height);
                graphics.FillRectangle(Brushes.White, rectText);
                if (m_GTCCamera)
                    graphics.DrawString(probText, m_FontGTCTemperature, m_BrushTemperature, x, y);
                else
                    graphics.DrawString(probText, m_FontTemperature, m_BrushTemperature, x, y);
            }
        }

        private void cbxCameraList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxCameraList.SelectedIndex != -1 && m_LstCamera[cbxCameraList.SelectedIndex].Caption.Contains("GTC"))
            {
                cbxCameraMode.SelectedIndex = 0;
                cbxFaceDetect.Checked = false;
                cbxEyeDetect.Checked = false;
                cbxGTCFormat.Checked = true;
            }
        }
    }
}
