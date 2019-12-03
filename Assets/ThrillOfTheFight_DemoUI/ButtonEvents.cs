using Bhaptics.Tact.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Bhaptics.Tact.Unity
{
    public class ButtonEvents : MonoBehaviour {

        [SerializeField] private Button permissionButton;
        [SerializeField] private GameObject deviceUIContainer;

        [SerializeField] private Button tactalPairButton;
        [SerializeField] private Button tactalUnPairButton;
        [SerializeField] private Button tactotPairButton;
        [SerializeField] private Button tactotUnPairButton;

        [SerializeField] private Text tactalStateText;
        [SerializeField] private Text tactotStateText;


        private Text tactalPairButtonText;
        private Text tactalUnPairButtonText;
        private Text tactotPairButtonText;
        private Text tactotUnPairButtonText;

        private string headDeviceAddress;
        private string vestDeviceAddress;


        private void Start()
        {
            tactalPairButtonText = tactalPairButton.transform.GetChild(0).GetComponent<Text>();
            tactalUnPairButtonText = tactalUnPairButton.transform.GetChild(0).GetComponent<Text>();
            tactotPairButtonText = tactotPairButton.transform.GetChild(0).GetComponent<Text>();
            tactotUnPairButtonText = tactotUnPairButton.transform.GetChild(0).GetComponent<Text>();
        }
        private void OnEnable()
        {
            StartCoroutine(AlwaysScan());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        public void GetPermission()
        {
            if (!AndroidPermissionsManager.CheckBluetoothPermissions())
            {
                AndroidPermissionsManager.RequestPermission();
            }
        }

        public void TactalPairButton()
        {
            if (IsDevicePaired(TactDeviceType.Tactal))
            {
                AndroidWidget_DeviceManager.Instance.Ping(headDeviceAddress);
                return;
            }

            var deviceList = AndroidWidget_DeviceManager.Instance.GetDeviceList();
            for (int i = 0; i < deviceList.Count; i++)
            {
                if (deviceList[i].Position.StartsWith(AndroidWidget_CompareDeviceString.GetPositionString(TactDeviceType.Tactal)))
                {
                    AndroidWidget_DeviceManager.Instance.Pair(deviceList[i].Address);
                    break;
                }
            }
        }
        public void TactalUnPairButton()
        {
            if (IsDevicePaired(TactDeviceType.Tactal))
            {
                AndroidWidget_DeviceManager.Instance.Unpair(headDeviceAddress);
                headDeviceAddress = null;
                return;
            }
        }

        public void TactotPairButton()
        {
            if (IsDevicePaired(TactDeviceType.Tactot))
            {
                AndroidWidget_DeviceManager.Instance.Ping(vestDeviceAddress);
                return;
            }

            var deviceList = AndroidWidget_DeviceManager.Instance.GetDeviceList();
            for (int i = 0; i < deviceList.Count; i++)
            {
                if (deviceList[i].Position.StartsWith(AndroidWidget_CompareDeviceString.GetPositionString(TactDeviceType.Tactot))){
                    AndroidWidget_DeviceManager.Instance.Pair(deviceList[i].Address);
                    break;
                }
            }
        }

        public void TactotUnPairButton()
        {
            if (IsDevicePaired(TactDeviceType.Tactot))
            {
                AndroidWidget_DeviceManager.Instance.Unpair(vestDeviceAddress);
                vestDeviceAddress = null;
                return;
            }
        }


        private void Update()
        {
            CheckPermissionUI();
            CheckDeviceStateUI();
        }

        private IEnumerator AlwaysScan()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.2f);
                if (!AndroidWidget_DeviceManager.Instance.IsScanning)
                {
                    AndroidWidget_DeviceManager.Instance.Scan();
                }
            }
        }


        private void CheckPermissionUI()
        {
            if (AndroidPermissionsManager.CheckBluetoothPermissions())
            {
                permissionButton.gameObject.SetActive(false);
                deviceUIContainer.SetActive(true);
            }
            else
            {
                permissionButton.gameObject.SetActive(true);
                deviceUIContainer.SetActive(false);
            }
        }


        private void CheckDeviceStateUI()
        {
            if (AndroidPermissionsManager.CheckBluetoothPermissions())
            {
                if (!IsDevicePaired(TactDeviceType.Tactal))
                {
                    if (!CanPairedDevice(TactDeviceType.Tactal))
                    {
                        tactalPairButton.interactable = false;
                        tactalPairButtonText.color = new Color(0.78f, 0.78f, 0.78f, 0.5f);
                        tactalPairButtonText.text = "Pair";
                        tactalUnPairButton.interactable = false;
                        tactalUnPairButtonText.color = new Color(0.78f, 0.78f, 0.78f, 0.5f);
                    }
                    else
                    {
                        tactalPairButton.interactable = true;
                        tactalPairButtonText.color = Color.white;
                        tactalPairButtonText.text = "Pair";
                        tactalUnPairButton.interactable = false;
                        tactalUnPairButtonText.color = new Color(0.78f, 0.78f, 0.78f, 0.5f);                        
                    }
                }
                else
                {
                    tactalUnPairButton.interactable = true;
                    tactalUnPairButtonText.color = Color.white;
                }

                if (!IsDevicePaired(TactDeviceType.Tactot))
                {
                    if (!CanPairedDevice(TactDeviceType.Tactot))
                    {
                        tactotPairButton.interactable = false;
                        tactotPairButtonText.color = new Color(0.78f, 0.78f, 0.78f, 0.5f);
                        tactotPairButtonText.text = "Pair";
                        tactotUnPairButton.interactable = false;
                        tactotUnPairButtonText.color = new Color(0.78f, 0.78f, 0.78f, 0.5f);
                    }
                    else
                    {
                        tactotPairButton.interactable = true;
                        tactotPairButtonText.color = Color.white;
                        tactotPairButtonText.text = "Pair";
                        tactotUnPairButton.interactable = false;
                        tactotUnPairButtonText.color = new Color(0.78f, 0.78f, 0.78f, 0.5f);
                    }
                }
                else
                {
                    tactotUnPairButton.interactable = true;
                    tactotUnPairButtonText.color = Color.white;
                }
            }
        }




        private bool CanPairedDevice(TactDeviceType deviceType)
        {
            var deviceList = AndroidWidget_DeviceManager.Instance.GetDeviceList();

            if (!IsDevicePaired(deviceType))
            {
                for (int i = 0; i < deviceList.Count; i++)
                {
                    if (!deviceList[i].IsPaired && AndroidWidget_CompareDeviceString.convertConnectionStatus(deviceList[i].ConnectionStatus) == 2 &&
                        deviceList[i].Position.StartsWith(AndroidWidget_CompareDeviceString.GetPositionString(deviceType)))
                    {
                        if(deviceType == TactDeviceType.Tactal)
                        {
                            tactalStateText.text = "Ready to be paired";
                        }

                        if (deviceType == TactDeviceType.Tactot)
                        {
                            tactotStateText.text = "Ready to be paired";
                        }
                        return true;
                    }
                }

                if (deviceType == TactDeviceType.Tactal)
                {
                    tactalStateText.text = "Not Found";
                }

                if (deviceType == TactDeviceType.Tactot)
                {
                    tactotStateText.text = "Not Found";
                }
            }
            return false;
        }


        private bool IsDevicePaired(TactDeviceType deviceType)
        {
            var deviceList = AndroidWidget_DeviceManager.Instance.GetDeviceList();

            for(int i = 0; i < deviceList.Count; i++)
            {
                if(deviceList[i].IsPaired && deviceList[i].Position.StartsWith(AndroidWidget_CompareDeviceString.GetPositionString(deviceType)))
                //CompareDeviceString.convertConnectionStatus(deviceList[i].ConnectionStatus) == 0 &&
                {
                    if (deviceType == TactDeviceType.Tactal)
                    {
                        if (AndroidWidget_CompareDeviceString.convertConnectionStatus(deviceList[i].ConnectionStatus) == 0)
                        {
                            tactalStateText.text = "Connected";

                            tactalPairButton.interactable = true;
                            tactalPairButtonText.color = Color.white;
                            tactalPairButtonText.text = "Ping"; 

                            headDeviceAddress = deviceList[i].Address;
                        }
                        else if(AndroidWidget_CompareDeviceString.convertConnectionStatus(deviceList[i].ConnectionStatus) == 2)
                        {
                            tactalStateText.text = "DisConnected";

                            tactalPairButton.interactable = false;
                            tactalPairButtonText.color = new Color(0.78f, 0.78f, 0.78f, 0.5f);
                            tactalPairButtonText.text = "Ping";


                            headDeviceAddress = deviceList[i].Address;
                        }
                    }

                    if (deviceType == TactDeviceType.Tactot)
                    {
                        if (AndroidWidget_CompareDeviceString.convertConnectionStatus(deviceList[i].ConnectionStatus) == 0)
                        {
                            tactotStateText.text = "Connected";

                            tactotPairButton.interactable = true;
                            tactotPairButtonText.color = Color.white;
                            tactotPairButtonText.text = "Ping";

                            vestDeviceAddress = deviceList[i].Address;
                        }
                        else
                        if (AndroidWidget_CompareDeviceString.convertConnectionStatus(deviceList[i].ConnectionStatus) == 0)
                        {
                            tactotStateText.text = "DisConnected";

                            tactotPairButton.interactable = false;
                            tactotPairButtonText.color = new Color(0.78f, 0.78f, 0.78f, 0.5f);
                            tactotPairButtonText.text = "Ping";

                            vestDeviceAddress = deviceList[i].Address;
                        }
                    }
                    return true;
                }
            }

            headDeviceAddress = null;
            vestDeviceAddress = null;
            return false;
        }




        private IEnumerator SuccessPairCheck(TactDeviceType deviceType)
        {
            yield return null;
        }



    }
}
