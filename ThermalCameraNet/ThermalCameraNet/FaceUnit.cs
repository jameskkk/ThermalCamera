using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThermalCameraNet
{
    public class FaceUnit
    {
        private const string FACE_HAAR_XML_PATH = "haarcascade_frontalface_default.xml";
        //private const string FACE_HAAR_XML_PATH = "haarcascade_frontalface_alt_tree.xml";
        //private const string EYE_HAAR_XML_PATH = "haarcascade_frontalface_default.xml";
        private const string EYE_HAAR_XML_PATH = "haarcascade_eye_tree_eyeglasses.xml";

        private CascadeClassifier m_FaceCascadeClassifier = null;
        private CascadeClassifier m_EyeCascadeClassifier = null;
        private Pen m_PenFace = new Pen(Color.Red, 3.0f);
        private Pen m_PenEye = new Pen(Color.Green, 3.0f);
        private Size m_FaceMinSize = new Size(20, 20);
        //private Size m_FaceMaxSize = new Size(300, 300);
        private Size m_eyeMinSize = new Size(10, 10);

        public void InitFaceDetect()
        {
            if (m_FaceCascadeClassifier != null)
            {
                m_FaceCascadeClassifier.Dispose();
                m_FaceCascadeClassifier = null;
            }

            if (m_EyeCascadeClassifier != null)
            {
                m_EyeCascadeClassifier.Dispose();
                m_EyeCascadeClassifier = null;
            }

            string facehaarcascade = Path.Combine(FileUnit.AssemblyDirectory, FACE_HAAR_XML_PATH);
            m_FaceCascadeClassifier = new CascadeClassifier(facehaarcascade);
            string eyehaarcascade = Path.Combine(FileUnit.AssemblyDirectory, EYE_HAAR_XML_PATH);
            m_EyeCascadeClassifier = new CascadeClassifier(eyehaarcascade);
        }

        public bool DetectFaceAndEye(Mat gray, Graphics graphics, bool isEyeDetect, ref Rectangle rectFaceFirst)
        {
            bool isFaceDetect = false;
            //LogUnit.Log.Info("ProcessCameraFrame(): DetectMultiScale() Start...");
            //Image<Gray, byte> imageGray = gray.ToImage<Gray, byte>();
            //imageGray._EqualizeHist(); // EqualizeHist
            CvInvoke.EqualizeHist(gray, gray);
            Rectangle[] faceRectArray = m_FaceCascadeClassifier.DetectMultiScale(gray, // gray Scale Image
                1.1, // scaleFactor 1.1~1.5, 越大耗時越低, 檢測精度越低
                3,   // minNeighbors 3~15, 越高耗時越低
                m_FaceMinSize,  // 最小臉部大小 
                Size.Empty/*m_FaceMaxSize*/); // 最大臉部大小, 越大耗時越低
            if (faceRectArray.Length > 0)
            {
                isFaceDetect = true;
                rectFaceFirst = faceRectArray[0];
            }
                
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
                if (isEyeDetect)
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
            }

            return isFaceDetect;
        }
 
    }
}
