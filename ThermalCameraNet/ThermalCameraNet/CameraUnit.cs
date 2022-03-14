using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.MediaFoundation;
using System.Management;

namespace ThermalCameraNet
{
    public class CameraDevice
    {
        public string Caption;
        public string DeviceID;
        public string PNPDeviceID;
        public string Description;
    }

    public class CameraUnit
    {

        public static List<CameraDevice> GetAllConnectedCameras()
        {
            var cameraNames = new List<CameraDevice>();
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE (PNPClass = 'Image' OR PNPClass = 'Camera')"))
            {
                foreach (var device in searcher.Get())
                {
                    CameraDevice cameraDevice = new CameraDevice();
                    cameraDevice.Caption = device["Caption"].ToString();
                    cameraDevice.DeviceID = device.GetPropertyValue("DeviceID").ToString();
                    cameraDevice.PNPDeviceID = device.GetPropertyValue("PNPDeviceID").ToString();
                    cameraDevice.Description = device.GetPropertyValue("Description").ToString();
                    LogUnit.Log.Info("GetAllConnectedCameras(): Status     : " + device["Status"].ToString());
                    LogUnit.Log.Info("GetAllConnectedCameras(): Caption    : " + cameraDevice.Caption);
                    LogUnit.Log.Info("GetAllConnectedCameras(): DeviceID   : " + cameraDevice.DeviceID);
                    LogUnit.Log.Info("GetAllConnectedCameras(): PNPDeviceID: " + cameraDevice.PNPDeviceID);
                    LogUnit.Log.Info("GetAllConnectedCameras(): Description: " + cameraDevice.Description);

                    if (device["Status"].ToString().Contains("OK"))
                        cameraNames.Add(cameraDevice);
                }
            }

            return cameraNames;
        }

        public static int GetCameraIndexForPartName(string partName)
        {
            var cameras = ListOfAttachedCameras();
            for (var i = 0; i < cameras.Count(); i++)
            {
                if (cameras[i].ToLower().Contains(partName.ToLower()))
                {
                    return i;
                }
            }
            return -1;
        }

        public static string[] ListOfAttachedCameras()
        {
            var cameras = new List<string>();
            var attributes = new MediaAttributes(1);
            attributes.Set(CaptureDeviceAttributeKeys.SourceType.Guid, CaptureDeviceAttributeKeys.SourceTypeVideoCapture.Guid);
            var devices = MediaFactory.EnumDeviceSources(attributes);
            for (var i = 0; i < devices.Count(); i++)
            {
                var friendlyName = devices[i].Get(CaptureDeviceAttributeKeys.FriendlyName);
                cameras.Add(friendlyName);
            }
            return cameras.ToArray();
        }
    }
}
