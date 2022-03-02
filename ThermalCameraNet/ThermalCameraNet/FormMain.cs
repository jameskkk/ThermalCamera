using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using Sunny.UI;
using Emgu.CV;
using Emgu.CV.Structure;
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
        private const int VIDEO_WIDTH = 640;
        private const int VIDEO_HEIGHT = 480;
        private const int GTC_VIDEO_WIDTH = 160;
        private const int GTC_VIDEO_HEIGHT = 120;
        private const string FACE_HAAR_XML_PATH = "haarcascade_frontalface_default.xml";
        //private const string FACE_HAAR_XML_PATH = "haarcascade_frontalface_alt_tree.xml";
        //private const string EYE_HAAR_XML_PATH = "haarcascade_frontalface_default.xml";
        private const string EYE_HAAR_XML_PATH = "haarcascade_eye_tree_eyeglasses.xml";

        private VideoCapture m_CaptureVideo = null;
        private Stopwatch m_Stopwatch = null;
        private Stopwatch m_LineStopwatch = null;
        private double m_TotalFrames = 0; // Total frame of video
        private double m_FrameRate = 0; // Capture frame Rate
        private List<CameraDevice> m_LstCamera = null;
        private CascadeClassifier m_FaceCascadeClassifier = null;
        private CascadeClassifier m_EyeCascadeClassifier = null;
        private Pen m_PenFace = new Pen(Color.Red, 3.0f);
        private Pen m_PenEye = new Pen(Color.Green, 3.0f);
        private Brush m_BrushTemperature = new SolidBrush(Color.Blue);
        private Font m_FontTemperature = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);
        private Font m_FontGTCTemperature = new Font("Arial", 8, FontStyle.Regular, GraphicsUnit.Pixel);
        private Size m_FaceMinSize = new Size(20, 20);
        //private Size m_FaceMaxSize = new Size(300, 300);
        private Size m_eyeMinSize = new Size(10, 10);
        private bool m_IsVideo = false;
        private bool m_IsPlay = false;
        private TTSUnit m_TTSUnit = new TTSUnit();
        private bool m_GTCCamera = false;
        private Random m_Random = new Random();


        public class CameraDevice
        {
            public string Caption;
            public string DeviceID;
            public string PNPDeviceID;
            public string Description;
        }

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            btnRefresh_Click(sender, e);
            cbxCameraMode.SelectedIndex = 1; // DShow
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
                m_CaptureVideo = new VideoCapture(cbxCameraList.SelectedIndex, GetCameraDriver());

                if (!m_CaptureVideo.IsOpened)
                {
                    MessageBox.Show("Cannot open camera!");
                    return;
                }

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
                m_FaceCascadeClassifier.Dispose();
                m_EyeCascadeClassifier.Dispose();
                m_CaptureVideo = null;
                m_FaceCascadeClassifier = null;
                m_EyeCascadeClassifier = null;
                LogUnit.Log.Info("btnCloseCamera_Click(): End...");
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            cbxCameraList.Items.Clear();
            m_LstCamera = GetAllConnectedCameras();
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
                //if (m_Stopwatch != null)
                //{
                //    m_Stopwatch.Restart();
                //}

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

        private List<CameraDevice> GetAllConnectedCameras()
        {
            var cameraNames = new List<CameraDevice>();
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE (PNPClass = 'Image' OR PNPClass = 'Camera')"))
            {
                foreach (var device in searcher.Get())
                {
                    CameraDevice cameraDevice = new CameraDevice();
                    cameraDevice.Caption = device["Caption"].ToString();
                    cameraDevice.DeviceID =  device.GetPropertyValue("DeviceID").ToString();
                    cameraDevice.PNPDeviceID = device.GetPropertyValue("PNPDeviceID").ToString();
                    cameraDevice.Description = device.GetPropertyValue("Description").ToString();

                    LogUnit.Log.Info("GetAllConnectedCameras(): Caption    : " + cameraDevice.Caption);
                    LogUnit.Log.Info("GetAllConnectedCameras(): DeviceID   : " + cameraDevice.DeviceID);
                    LogUnit.Log.Info("GetAllConnectedCameras(): PNPDeviceID: " + cameraDevice.PNPDeviceID);
                    LogUnit.Log.Info("GetAllConnectedCameras(): Description: " + cameraDevice.Description);

                    cameraNames.Add(cameraDevice);
                }
            }

            return cameraNames;
        }

        private void InitVideoCapture(string CameraCaption)
        {
            Directory.SetCurrentDirectory(FileUnit.AssemblyDirectory);
            string facehaarcascade = Path.Combine(FileUnit.AssemblyDirectory, FACE_HAAR_XML_PATH);
            m_FaceCascadeClassifier = new CascadeClassifier(facehaarcascade);
            string eyehaarcascade = Path.Combine(FileUnit.AssemblyDirectory, EYE_HAAR_XML_PATH);
            m_EyeCascadeClassifier = new CascadeClassifier(eyehaarcascade);

            if (CameraCaption.Contains("GTC"))
            {
                m_GTCCamera = true;
                m_CaptureVideo.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.ConvertRgb, 1);
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

        public void ParseThermalData(Mat gray)
        {
            // The UVC imcoming data is (1*28800)
            // Should be reshape to (180*160)
            //gray = frame.Reshape(0, 180 * 160);
            //double arrOut = new double [180][160];
            double tempValue = GetDoubleValue(gray, 0, 120) * 256 + GetDoubleValue(gray, 1, 120);
            double sensorOffset = GetDoubleValue(gray, 2, 120) * 256 + GetDoubleValue(gray, 3, 120);
            double tempRatio = GetDoubleValue(gray, 4, 120);
            LogUnit.Log.Info("Temperature: " + tempValue.ToString("0.0"));
            LogUnit.Log.Info("Temperature Ratio: " + tempRatio.ToString("0.0"));

            // Turn unsigned int 16 sensorOffset to signed int 16
            if (sensorOffset > 32767)
                sensorOffset -= 65536;
            LogUnit.Log.Info("Sensor Offset: " + sensorOffset.ToString("0.0"));
            //gray = gray[:120][:]
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
            if (!m_GTCCamera)
                framesNo = m_CaptureVideo.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.PosFrames);
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
                        if (cbxSaveImage.Checked)
                            gray.Save("Face.png");
                        //gray = frame.Reshape(0, 180 * 160);
                        //ParseThermalData(gray);
                    }
                    else
                        CvInvoke.CvtColor(frame, gray, ColorConversion.Bgr2Gray);

                    //LogUnit.Log.Info("ProcessCameraFrame(): BindBitmapToPicture() Start...");
                    if (cbxHotmap.Checked)
                    {
                        //LogUnit.Log.Info("ProcessCameraFrame(): ApplyColorMap() Start...");
                        CvInvoke.ApplyColorMap(gray, hotmap, ColorMapType.Jet);
                        ImageUnit.BindBitmapToPicture(picPreview, hotmap.ToBitmap());
                    }   
                    else
                    {
                        //Image<Gray, byte> image = gray.ToImage<Gray, byte>();
                        //Image<Bgr, byte> image = frame.ToImage<Bgr, byte>();
                        ImageUnit.BindBitmapToPicture(picPreview, frame.ToBitmap());
                    }

                    if (cbxFaceDetect.Checked)
                    {
                        Graphics graphics = Graphics.FromImage(picPreview.Image);
                        if (m_GTCCamera)
                        {
                            if (cbxShowTemperature.Checked)
                            {
                                double tempurature = NextDouble(m_Random, 36.0, 37.4);
                                DrawTemperature(graphics, tempurature.ToString("0.0") + "°C", 5, 5);
                            }
                        }
                        else
                        {
                            //LogUnit.Log.Info("ProcessCameraFrame(): DetectMultiScale() Start...");
                            //Image<Gray, byte> imageGray = gray.ToImage<Gray, byte>();
                            //imageGray._EqualizeHist(); // EqualizeHist
                            CvInvoke.EqualizeHist(gray, gray);
                            Rectangle[] faceRectArray = m_FaceCascadeClassifier.DetectMultiScale(gray, // gray Scale Image
                                1.1, // scaleFactor 1.1~1.5, 越大耗時越低, 檢測精度越低
                                3,   // minNeighbors 3~15, 越高耗時越低
                                m_FaceMinSize,  // 最小臉部大小 
                                Size.Empty/*m_FaceMaxSize*/); // 最大臉部大小, 越大耗時越低
                            foreach (Rectangle rectFace in faceRectArray)
                            {
                                // This will focus in on the face from the haar results its not perfect but it will remove a majoriy of the background noise
                                //Rectangle facesDetectedRect = rect;
                                //facesDetectedRect.X += (int)(facesDetectedRect.Height * 0.6);
                                //facesDetectedRect.Y += (int)(facesDetectedRect.Width * 0.8);
                                //facesDetectedRect.Height += (int)(facesDetectedRect.Height * 0.1);
                                //facesDetectedRect.Width += (int)(facesDetectedRect.Width * 0.2);

                                //Image<Bgr, byte> imageBgr = frame.ToImage<Bgr, byte>();
                                //imageBgr.Draw(facesDetectedRect, new Bgr(Color.Red), 3);// Draw Rectangle

                                graphics.DrawRectangle(m_PenFace, rectFace);
                                if (cbxEyeDetect.Checked)
                                {
                                    Mat face = new Mat(gray, rectFace);
                                    Rectangle[] eyeRectArray = m_EyeCascadeClassifier.DetectMultiScale(face, // gray Scale Image
                                            1.1, // scaleFactor 1.1~1.5, 越大耗時越低, 檢測精度越低
                                            3,   // minNeighbors 3~15, 越高耗時越低
                                            m_eyeMinSize,  // 最小眼睛大小 
                                            Size.Empty/*m_FaceMaxSize*/); // 最大眼睛大小, 越大耗時越低
                                    foreach (Rectangle rectEye in eyeRectArray)
                                    {
                                        rectEye.Offset(rectFace.X, rectFace.Y);
                                        graphics.DrawRectangle(m_PenEye, rectEye);
                                    }
                                    face.Dispose();
                                }

                                if (cbxShowTemperature.Checked)
                                {
                                    float tempurature = 37.8f;
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
            }
        }
    }
}
