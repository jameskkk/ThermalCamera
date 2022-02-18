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

namespace ThermalCameraNet
{
    public partial class FormMain : UIForm
    {
        private const int VIDEO_WIDTH = 640;
        private const int VIDEO_HEIGHT = 480;

        private VideoCapture m_CaptureVideo = null;
        private double m_FrameRate = 0; // Capture frame Rate
        private List<CameraDevice> m_LstCamera = null;

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
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            btnCloseCamera_Click(sender, e);
        }

        private void btnOpenCamera_Click(object sender, EventArgs e)
        {
            if (cbxCameraList.SelectedIndex != -1)
            {
                m_CaptureVideo = new VideoCapture(cbxCameraList.SelectedIndex, GetCameraDriver());

                if (!m_CaptureVideo.IsOpened)
                {
                    MessageBox.Show("Cannot open camera!");
                    return;
                }
                InitVideoCapture();
            }
        }

        private void btnCloseCamera_Click(object sender, EventArgs e)
        {
            if (m_CaptureVideo != null && m_CaptureVideo.IsOpened)
            {
                timerProcessFrame.Stop();
                m_CaptureVideo.Stop();
                m_CaptureVideo.Dispose();
                m_CaptureVideo = null;
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

        private void timerProcessFrame_Tick(object sender, EventArgs e)
        {
            ProcessCameraFrame(sender, e);
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

        private void InitVideoCapture()
        {
            m_CaptureVideo.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameHeight, VIDEO_HEIGHT);
            m_CaptureVideo.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.FrameWidth, VIDEO_WIDTH);
            m_CaptureVideo.SetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps, 30);
            m_FrameRate = m_CaptureVideo.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
            var codec_double = m_CaptureVideo.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.FourCC);
            string codec = new string(Encoding.UTF8.GetString(BitConverter.GetBytes(Convert.ToUInt32(codec_double))).ToCharArray());
            LogUnit.Log.Info("InitVideoCapture() Codec: " + codec + ", FrameRate: " + m_FrameRate.ToString());

            lblFPS.Text = m_FrameRate.ToString("0.0") + " fps";

            timerProcessFrame.Start();
            //Application.Idle += ProcessCameraFrame;
        }

        private void ProcessCameraFrame(object sender, EventArgs e)
        {
            Mat frame = m_CaptureVideo.QueryFrame();
            if (frame != null)
            {
                Mat gray = new Mat();
                Mat hotmap = new Mat();
                CvInvoke.CvtColor(frame, gray, ColorConversion.Bgr2Gray);
                CvInvoke.ApplyColorMap(gray, hotmap, ColorMapType.Jet);

                //Image<Bgr, byte> image = frame.ToImage<Bgr, byte>();
                //Image<Gray, byte> image = gray.ToImage<Gray, byte>();
                ImageUnit.BindBitmapToPicture(picPreview, hotmap.ToBitmap());

                m_FrameRate = m_CaptureVideo.GetCaptureProperty(Emgu.CV.CvEnum.CapProp.Fps);
                lblFPS.Text = m_FrameRate.ToString("0.0") + " fps";
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
    }
}
