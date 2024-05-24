using UnityEngine;

class DeviceIdModel
{
    DeviceType deviceType = SystemInfo.deviceType;
    string deviceID = SystemInfo.deviceUniqueIdentifier;
    string deviceName = SystemInfo.deviceName;
    string operatingSystem = SystemInfo.operatingSystem;
}