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
            if (IsDevicePaired(PositionType.Head))
            {
                BhapticsAndroidManager.Ping(PositionType.Head);
                return;
            }

            var deviceList = BhapticsAndroidManager.GetDevices();
            for (int i = 0; i < deviceList.Count; i++)
            {
                if (deviceList[i].Position == (PositionType.Head))
                {
                    BhapticsAndroidManager.Pair(deviceList[i].Address);
                    break;
                }
            }
        }
        public void TactalUnPairButton()
        {
            var pairedDevices = BhapticsAndroidManager.GetPairedDevices(PositionType.Head);
            foreach (var pairedDevice in pairedDevices)
            {
                BhapticsAndroidManager.Unpair(pairedDevice.Address);
            }
        }

        public void TactotPairButton()
        {
            if (IsDevicePaired(PositionType.Vest))
            {
                BhapticsAndroidManager.Ping(PositionType.Vest);
                return;
            }

            var deviceList = BhapticsAndroidManager.GetDevices();
            for (int i = 0; i < deviceList.Count; i++)
            {
                if (deviceList[i].Position == (PositionType.Vest)) {
                    BhapticsAndroidManager.Pair(deviceList[i].Address);
                    break;
                }
            }
        }

        public void TactotUnPairButton()
        {
            var pairedDevices = BhapticsAndroidManager.GetPairedDevices(PositionType.Vest);
            foreach (var pairedDevice in pairedDevices)
            {
                BhapticsAndroidManager.Unpair(pairedDevice.Address);
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
                if (!BhapticsAndroidManager.IsScanning())
                {
                    BhapticsAndroidManager.Scan();
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
                if (!IsDevicePaired(PositionType.Head))
                {
                    if (!CanPairedDevice(PositionType.Head))
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

                if (!IsDevicePaired(PositionType.Vest))
                {
                    if (!CanPairedDevice(PositionType.Vest))
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




        private bool CanPairedDevice(PositionType deviceType)
        {
            var deviceList = BhapticsAndroidManager.GetDevices();

            if (!IsDevicePaired(deviceType))
            {
                for (int i = 0; i < deviceList.Count; i++)
                {
                    if (!deviceList[i].IsPaired && AndroidUtils.ConvertConnectionStatus(deviceList[i].ConnectionStatus) == 2 &&
                        deviceList[i].Position == (deviceType))
                    {
                        if (deviceType == PositionType.Head)
                        {
                            tactalStateText.text = "Ready to be paired";
                        }

                        if (deviceType == PositionType.Vest)
                        {
                            tactotStateText.text = "Ready to be paired";
                        }
                        return true;
                    }
                }

                if (deviceType == PositionType.Head)
                {
                    tactalStateText.text = "Not Found";
                }

                if(deviceType == PositionType.Vest)
                {
                    tactotStateText.text = "Not Found";
                }
            }
            return false;
        }


        private bool IsDevicePaired(PositionType deviceType)
        {
            var deviceList = BhapticsAndroidManager.GetDevices();

            for(int i = 0; i < deviceList.Count; i++)
            {
                if(deviceList[i].IsPaired && deviceList[i].Position == (deviceType))
                {
                    if (deviceType == PositionType.Head)
                    {
                        if (AndroidUtils.ConvertConnectionStatus(deviceList[i].ConnectionStatus) == 0)
                        {
                            tactalStateText.text = "Connected";

                            tactalPairButton.interactable = true;
                            tactalPairButtonText.color = Color.white;
                            tactalPairButtonText.text = "Ping"; 
                        }
                        else if(AndroidUtils.ConvertConnectionStatus(deviceList[i].ConnectionStatus) == 2)
                        {
                            tactalStateText.text = "DisConnected";

                            tactalPairButton.interactable = false;
                            tactalPairButtonText.color = new Color(0.78f, 0.78f, 0.78f, 0.5f);
                            tactalPairButtonText.text = "Ping";
                        }
                    }

                    if (deviceType == PositionType.Vest)
                    {
                        if (AndroidUtils.ConvertConnectionStatus(deviceList[i].ConnectionStatus) == 0)
                        {
                            tactotStateText.text = "Connected";

                            tactotPairButton.interactable = true;
                            tactotPairButtonText.color = Color.white;
                            tactotPairButtonText.text = "Ping";
                        }
                        else if (AndroidUtils.ConvertConnectionStatus(deviceList[i].ConnectionStatus) == 0)
                        {
                            tactotStateText.text = "DisConnected";

                            tactotPairButton.interactable = false;
                            tactotPairButtonText.color = new Color(0.78f, 0.78f, 0.78f, 0.5f);
                            tactotPairButtonText.text = "Ping";
                        }
                    }
                    return true;
                }
            }
            return false;
        }




        private IEnumerator SuccessPairCheck(PositionType deviceType)
        {
            yield return null;
        }



    }
}
