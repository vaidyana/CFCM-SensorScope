using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Sensor_Scope
{
    static class cEncapsulateCp210x
    {
        private const string CP210xManu = "CP210xManufacturing.dll";

        [DllImport(CP210xManu, EntryPoint = "CP210x_GetNumDevices")]
        public static extern int getNumDevices(
        [In, Out] ref UInt32 deviceNumb);

        [DllImport(CP210xManu, EntryPoint = "CP210x_Open")]
        public static extern int open(
        [In] UInt32 deviceNumb,
        [In, Out] ref IntPtr deviceHandle);

        [DllImport(CP210xManu, EntryPoint = "CP210x_Close")]
        public static extern int close(
        [In, Out] IntPtr deviceHandle);

        [DllImport(CP210xManu, EntryPoint = "CP210x_GetDeviceProductString", CharSet = CharSet.Ansi)]
        public static extern int getDeviceProductString(
        [In, Out] IntPtr deviceHandle,
        [In, Out] IntPtr Product,
        [In, Out] ref byte Length,
        [In] bool ConvertToASCII);

        [DllImport(CP210xManu, EntryPoint = "CP210x_GetDeviceInterfaceString", CharSet = CharSet.Ansi)]
        public static extern int getDeviceInterfaceString(
        [In, Out] IntPtr deviceHandle,
        [In]    Byte InterfaceNum,
        [In, Out] IntPtr Interface,
        [In, Out]  ref byte Length,
        [In] bool ConvertToASCII);


        [DllImport(CP210xManu, EntryPoint = "CP210x_GetDeviceSerialNumber", CharSet = CharSet.Ansi)]
        public static extern int getDeviceSerialNumber(
        [In,Out]IntPtr deviceHandle,
        [In, Out] IntPtr Product,
        [In, Out]ref byte Length,
        [In, Out] bool ConvertToASCII);

        [DllImport(CP210xManu, EntryPoint = "CP210x_GetDeviceVid")]
        public static extern int getDeviceVid(
        [In, Out] IntPtr deviceHandle,
        [In, Out] ref ushort Vid);

        [DllImport(CP210xManu, EntryPoint = "CP210x_GetDevicePid")]
        public static extern int getDevicePid(
        [In, Out] IntPtr deviceHandle,
        [In, Out] ref ushort Pid);

    }
}
 