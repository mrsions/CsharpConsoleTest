using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.Diagnostics;

namespace RwSlam
{
#if !UNITY_2019_OR_NEWER
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector3
    {
        public float x, y, z;
    }
#endif

    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        public Vector3 a;
        public Vector3 w;
        public double t;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Pose
    {
        public Vector3 position;
        public Vector3 rotation;
    }

    public enum MatType
    {
        CV_8UC1 = 1,
        CV_8UC2,
        CV_8UC3,
        CV_8UC4,
    }

    public enum SensorType
    {
        MONOCULAR = 0,
        STEREO = 1,
        RGBD = 2,
        IMU_MONOCULAR = 3,
        IMU_STEREO = 4
    }

    public enum FileType
    {
        TEXT_FILE = 0,
        BINARY_FILE = 1,
    }

    public class RwSlamTest
    {
        public RwSlamTest()
        {
            //string settingsPath = @"D:\Work\RW.Slam\Project\ORB_SLAM3\Examples\Monocular-Inertial\Nreal.yaml";
            //string dataPath = @"D:\Work\RW.Slam\Project\Libraries\V1_01_Nreal";
            string settingsPath = @"D:\Work\RW.Slam\Project\ORB_SLAM3\Examples\Monocular-Inertial\EuRoC.yaml";
            string dataPath = @"D:\Work\RW.Slam\Project\Libraries\V1_01_easy";

            var imageFiles = Directory.GetFiles(dataPath + @"\mav0\cam0\data", "*");
            var timestamps = (from imgPath in imageFiles where double.TryParse(Path.GetFileNameWithoutExtension(imgPath), out _) select double.Parse(Path.GetFileNameWithoutExtension(imgPath)) / 1e9).ToArray();
            var imuMeas = (from line in File.ReadAllLines(dataPath + @"\mav0\imu0\data.csv")
                           where !string.IsNullOrWhiteSpace(line) && line[0] != '#'
                           select
                           ((Func<string, Point>)((string arg) =>
                           {
                               var data = arg.Split(',').Select(v => double.Parse(v)).ToArray();
                               return new Point
                               {
                                   t = data[0] / 1e9,
                                   w = new Vector3 { x = (float)data[1], y = (float)data[2], z = (float)data[3] },
                                   a = new Vector3 { x = (float)data[4], y = (float)data[5], z = (float)data[6] }
                               };
                           }))(line)).ToArray();

            Slam.SetLogging(false);
            var slam = new Slam("", settingsPath, SensorType.IMU_MONOCULAR);
            //Native.Rwslam_SetGravity(0);
            var mat = new Mat();

            int imuFirst = 0;
            Point[] imu = new Point[imuMeas.Length];
            for (int i = 0; i < imageFiles.Length; i++)
            {
                mat = OpenCV.imread(imageFiles[i], OpenCV.ImreadModes.IMREAD_UNCHANGED);
                //if (mat.IsEmpty())
                //{
                //    Console.WriteLine("Failed to load image " + imageFiles[i]);
                //    Environment.Exit(1);
                //    return;
                //}

                double timestamp = timestamps[i];

                // IMU 읽기
                int imuLength = 0;
                if (i > 0)
                {
                    while (imuMeas[imuFirst].t <= timestamp)
                    {
                        imu[imuLength] = imuMeas[imuFirst];
                        imuFirst++;
                        imuLength++;
                    }
                    Console.WriteLine("Counting " + imuLength);
                }

                // 트래킹
                Stopwatch st = Stopwatch.StartNew();
                Pose pose = slam.TrackMonocular(mat, timestamp, imu, imuLength);
                st.Stop();

                // 대기
                int waitMs = (int)(((i < timestamps.Length ? timestamps[i] : timestamps.Last()) - timestamp) * 1e6);
                waitMs -= (int)st.ElapsedMilliseconds;
                if (waitMs > 0)
                {
                    Slam.usleep(waitMs);
                }
            }
        }

        // WEBCAM
        public RwSlamTest(int a)
        {
            string vocabPath = @"D:\Work\RW.Slam\Project\ORB_SLAM3\Vocabulary\ORBvoc.txt";
            string settingsPath = @"D:\Work\RW.Slam\Project\ORB_SLAM3\Examples\Monocular\webcam.yaml";

            Slam.SetLogging(false);

            bool isLocalization = File.Exists("test.osa");
            using (var slam = new Slam("", settingsPath, (int)SensorType.MONOCULAR, loadingFilePath: isLocalization ? "test.osa" : ""))
            using (var cap = new VideoCap(0))
            using (var mat = new Mat())
            {
                //if (isLocalization) slam.ActivateLocalizationMode();

                while (true)
                {
                    if (cap.Read(mat))
                    {
                        var pose = slam.TrackMonocular(mat, GetTimestamp());

                        if (!isLocalization && slam.GetTrackingState() == Slam.TrackingState.RECENTLY_LOST)
                        {
                            break;
                        }
                    }

                    OpenCV.imshow("Image", mat);
                    if (OpenCV.waitKey(1) >= 0)
                    {
                        break;
                    }
                }

                if (!isLocalization) slam.Save("test");
                slam.Shutdown();
            }
        }

        public double GetTimestamp()
        {
            return (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }
    }

    internal class Native
    {
        public const string dllName = "slam";
        public const CallingConvention dllConvention = CallingConvention.Cdecl;

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern void Rwslam_Usleep(long usec);

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern void Rwslam_Log(bool enable);

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern void Rwslam_SetGravity(float v);

        #region System
        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern IntPtr Rwslam_System_Create(string strVocFile, string strSettingFile, int sensor, bool bUseViewer = true, int intFr = 0, string strSequence = "", string strLoadingFile = "");

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern void Rwslam_System_Shutdown(IntPtr handler);

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern void Rwslam_System_SaveAtlas(IntPtr handler, int type, string filename);

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern void Rwslam_System_Free(IntPtr handler);

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern void Rwslam_System_ActivateLocalizationMode(IntPtr handler);

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern void Rwslam_System_DeactivateLocalizationMode(IntPtr handler);

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern int Rwslam_System_GetState(IntPtr handler);

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern Pose Rwslam_System_TrackMonocular(IntPtr handler, IntPtr mat, double timestamp, Point[] imu, int imuCount, string fileName = "");

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern int Rwslam_System_Map_Count(IntPtr handler);

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern void Rwslam_System_Map_Change(IntPtr handler, int index);
        #endregion

        #region Opencv
        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern IntPtr Rwslam_Opencv_imread(string path, int flags);

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern void Rwslam_Opencv_imshow(string title, IntPtr mat);

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern int Rwslam_Opencv_waitKey(int delayMs);

        //--- Matrix
        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern IntPtr Rwslam_Opencv_Mat_Create();

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern IntPtr Rwslam_Opencv_Mat_Create2(int width, int height, int type);

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern bool Rwslam_Opencv_Mat_SetData(IntPtr mat, byte[] data, int length);

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern void Rwslam_Opencv_Mat_Free(IntPtr mat);

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern bool Rwslam_Opencv_Mat_Empty(IntPtr mat);

        //--- Video
        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern IntPtr Rwslam_Opencv_VideoCap_Create(int camera);

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern void Rwslam_Opencv_VideoCap_Free(IntPtr handler);

        [DllImport(dllName, CallingConvention = dllConvention)]
        public static extern bool Rwslam_Opencv_VideoCap_Read(IntPtr handler, IntPtr mat);
        #endregion
    }

    public abstract class PtrObject : IDisposable
    {
        internal IntPtr handler;

        public PtrObject(IntPtr handler)
        {
            this.handler = handler;
        }

        ~PtrObject()
        {
            if (!Valid) return;
            Dispose();
        }

        public virtual void Dispose()
        {
            if (!Valid) return;
            OnDispose(handler);
            handler = IntPtr.Zero;
        }

        protected abstract void OnDispose(IntPtr handler);

        protected bool Valid => handler != IntPtr.Zero;
    }

    public class Slam : PtrObject
    {
        public enum TrackingState
        {
            SYSTEM_NOT_READY = -1,
            NO_IMAGES_YET = 0,
            NOT_INITIALIZED = 1,
            OK = 2,
            RECENTLY_LOST = 3,
            LOST = 4,
            OK_KLT = 5
        };

        public static void SetLogging(bool enable) => Native.Rwslam_Log(enable);
        public static void usleep(int usec) => Native.Rwslam_Usleep(usec);

        public Slam(string vocFile, string settingFile, SensorType sensor, bool useViewer = true, int initFr = 0, string sequence = "", string loadingFilePath = "")
            : base(Native.Rwslam_System_Create(vocFile, settingFile, (int)sensor, useViewer, initFr, sequence, loadingFilePath))
        {
        }

        protected override void OnDispose(IntPtr handler)
        {
            Native.Rwslam_System_Free(handler);
        }

        public TrackingState GetTrackingState() => (TrackingState)Native.Rwslam_System_GetState(handler);

        public void ActivateLocalizationMode() => Native.Rwslam_System_ActivateLocalizationMode(handler);

        public void DeactivateLocalizationMode() => Native.Rwslam_System_DeactivateLocalizationMode(handler);

        public void Shutdown()
        {
            if (!Valid) return;
            Native.Rwslam_System_Shutdown(handler);
        }

        public Pose TrackMonocular(Mat mat, double timestamp, Point[] imu = null, string fileName = "")
        {
            if (!Valid) return default;
            return Native.Rwslam_System_TrackMonocular(handler, mat.handler, timestamp, imu, imu != null ? imu.Length : 0, fileName);
        }

        public Pose TrackMonocular(Mat mat, double timestamp, Point[] imu, int imuLength, string fileName = "")
        {
            if (!Valid) return default;
            return Native.Rwslam_System_TrackMonocular(handler, mat.handler, timestamp, imu, imuLength, fileName);
        }

        public void Save(string fileName)
        {
            if (!Valid) return;
            Native.Rwslam_System_SaveAtlas(handler, (int)FileType.BINARY_FILE, fileName);
        }

        public int GetMapCount() => Native.Rwslam_System_Map_Count(handler);

        public void ChangeMap(int index) => Native.Rwslam_System_Map_Change(handler, index);
    }

    public class OpenCV
    {
        public enum ImreadModes
        {
            IMREAD_UNCHANGED = -1, //!< If set, return the loaded image as is (with alpha channel, otherwise it gets cropped).
            IMREAD_GRAYSCALE = 0,  //!< If set, always convert image to the single channel grayscale image.
            IMREAD_COLOR = 1,  //!< If set, always convert image to the 3 channel BGR color image.
            IMREAD_ANYDEPTH = 2,  //!< If set, return 16-bit/32-bit image when the input has the corresponding depth, otherwise convert it to 8-bit.
            IMREAD_ANYCOLOR = 4,  //!< If set, the image is read in any possible color format.
            IMREAD_LOAD_GDAL = 8,  //!< If set, use the gdal driver for loading the image.
            IMREAD_REDUCED_GRAYSCALE_2 = 16, //!< If set, always convert image to the single channel grayscale image and the image size reduced 1/2.
            IMREAD_REDUCED_COLOR_2 = 17, //!< If set, always convert image to the 3 channel BGR color image and the image size reduced 1/2.
            IMREAD_REDUCED_GRAYSCALE_4 = 32, //!< If set, always convert image to the single channel grayscale image and the image size reduced 1/4.
            IMREAD_REDUCED_COLOR_4 = 33, //!< If set, always convert image to the 3 channel BGR color image and the image size reduced 1/4.
            IMREAD_REDUCED_GRAYSCALE_8 = 64, //!< If set, always convert image to the single channel grayscale image and the image size reduced 1/8.
            IMREAD_REDUCED_COLOR_8 = 65, //!< If set, always convert image to the 3 channel BGR color image and the image size reduced 1/8.
            IMREAD_IGNORE_ORIENTATION = 128 //!< If set, do not rotate the image according to EXIF's orientation flag.
        };

        public static Mat imread(string path, ImreadModes flags = 0)
        {
            return new Mat(Native.Rwslam_Opencv_imread(path, (int)flags));
        }

        public static void imshow(string title, Mat mat)
        {
            Native.Rwslam_Opencv_imshow(title, mat.handler);
        }

        public static int waitKey(int delayMs = 1)
        {
            return (sbyte)Native.Rwslam_Opencv_waitKey(delayMs);
        }
    }

    public class Mat : PtrObject
    {
        public Mat() : base(Native.Rwslam_Opencv_Mat_Create())
        {
        }

        internal Mat(IntPtr handler) : base(handler)
        {
        }

        public Mat(int width, int height, MatType type) : base(Native.Rwslam_Opencv_Mat_Create2(width, height, (int)type))
        {
        }

        public bool SetData(byte[] data)
        {
            return Native.Rwslam_Opencv_Mat_SetData(handler, data, data.Length);
        }

        public bool IsEmpty()
        {
            return Native.Rwslam_Opencv_Mat_Empty(handler);
        }

        protected override void OnDispose(IntPtr handler)
        {
            Native.Rwslam_Opencv_Mat_Free(handler);
        }
    }

    public class VideoCap : PtrObject
    {
        public VideoCap(int camera) : base(Native.Rwslam_Opencv_VideoCap_Create(camera))
        {
        }

        protected override void OnDispose(IntPtr handler)
        {
            Native.Rwslam_Opencv_VideoCap_Free(handler);
        }

        public bool Read(Mat mat)
        {
            return Native.Rwslam_Opencv_VideoCap_Read(handler, mat.handler);
        }
    }

}
