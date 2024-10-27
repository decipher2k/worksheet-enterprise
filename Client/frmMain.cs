using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static ManagedNativeWifi.NativeWifi;
using System.IO;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Diagnostics;

namespace WorksheetLog
{
    public partial class frmMain : Form
    {
        //source : http://csharptest.net/1043/how-to-prevent-users-from-killing-your-service-process/index.html
        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool GetKernelObjectSecurity(IntPtr Handle, int securityInformation, [Out] byte[] pSecurityDescriptor,
      uint nLength, out uint lpnLengthNeeded);

        public static RawSecurityDescriptor GetProcessSecurityDescriptor(IntPtr processHandle)
        {
            const int DACL_SECURITY_INFORMATION = 0x00000004;
            byte[] psd = new byte[0];
            uint bufSizeNeeded;
            // Call with 0 size to obtain the actual size needed in bufSizeNeeded
            GetKernelObjectSecurity(processHandle, DACL_SECURITY_INFORMATION, psd, 0, out bufSizeNeeded);
            if (bufSizeNeeded < 0 || bufSizeNeeded > short.MaxValue)
                throw new Win32Exception();
            // Allocate the required bytes and obtain the DACL
            if (!GetKernelObjectSecurity(processHandle, DACL_SECURITY_INFORMATION,
            psd = new byte[bufSizeNeeded], bufSizeNeeded, out bufSizeNeeded))
                throw new Win32Exception();
            // Use the RawSecurityDescriptor class from System.Security.AccessControl to parse the bytes:
            return new RawSecurityDescriptor(psd, 0);
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern bool SetKernelObjectSecurity(IntPtr Handle, int securityInformation, [In] byte[] pSecurityDescriptor);

        public static void SetProcessSecurityDescriptor(IntPtr processHandle, RawSecurityDescriptor dacl)
        {
            const int DACL_SECURITY_INFORMATION = 0x00000004;
            byte[] rawsd = new byte[dacl.BinaryLength];
            dacl.GetBinaryForm(rawsd, 0);
            if (!SetKernelObjectSecurity(processHandle, DACL_SECURITY_INFORMATION, rawsd))
                throw new Win32Exception();
        }

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentProcess();

        [Flags]
        public enum ProcessAccessRights
        {
            PROCESS_CREATE_PROCESS = 0x0080, //  Required to create a process.
            PROCESS_CREATE_THREAD = 0x0002, //  Required to create a thread.
            PROCESS_DUP_HANDLE = 0x0040, // Required to duplicate a handle using DuplicateHandle.
            PROCESS_QUERY_INFORMATION = 0x0400, //  Required to retrieve certain information about a process, such as its token, exit code, and priority class (see OpenProcessToken, GetExitCodeProcess, GetPriorityClass, and IsProcessInJob).
            PROCESS_QUERY_LIMITED_INFORMATION = 0x1000, //  Required to retrieve certain information about a process (see QueryFullProcessImageName). A handle that has the PROCESS_QUERY_INFORMATION access right is automatically granted PROCESS_QUERY_LIMITED_INFORMATION. Windows Server 2003 and Windows XP/2000:  This access right is not supported.
            PROCESS_SET_INFORMATION = 0x0200, //    Required to set certain information about a process, such as its priority class (see SetPriorityClass).
            PROCESS_SET_QUOTA = 0x0100, //  Required to set memory limits using SetProcessWorkingSetSize.
            PROCESS_SUSPEND_RESUME = 0x0800, // Required to suspend or resume a process.
            PROCESS_TERMINATE = 0x0001, //  Required to terminate a process using TerminateProcess.
            PROCESS_VM_OPERATION = 0x0008, //   Required to perform an operation on the address space of a process (see VirtualProtectEx and WriteProcessMemory).
            PROCESS_VM_READ = 0x0010, //    Required to read memory in a process using ReadProcessMemory.
            PROCESS_VM_WRITE = 0x0020, //   Required to write to memory in a process using WriteProcessMemory.
            DELETE = 0x00010000, // Required to delete the object.
            READ_CONTROL = 0x00020000, //   Required to read information in the security descriptor for the object, not including the information in the SACL. To read or write the SACL, you must request the ACCESS_SYSTEM_SECURITY access right. For more information, see SACL Access Right.
            SYNCHRONIZE = 0x00100000, //    The right to use the object for synchronization. This enables a thread to wait until the object is in the signaled state.
            WRITE_DAC = 0x00040000, //  Required to modify the DACL in the security descriptor for the object.
            WRITE_OWNER = 0x00080000, //    Required to change the owner in the security descriptor for the object.
            STANDARD_RIGHTS_REQUIRED = 0x000f0000,
            PROCESS_ALL_ACCESS = (STANDARD_RIGHTS_REQUIRED | SYNCHRONIZE | 0xFFF),//    All possible access rights for a process object.
        }



        public frmMain()
        {

            InitializeComponent();
            // Get the current process handle
            IntPtr hProcess = GetCurrentProcess();
            // Read the DACL
            var dacl = GetProcessSecurityDescriptor(hProcess);
            // Insert the new ACE
            for(int i=0;i< dacl.DiscretionaryAcl.Count; i++)
            {
                dacl.DiscretionaryAcl.RemoveAce(i);
            }
            dacl.DiscretionaryAcl.InsertAce(
            0,
            new CommonAce(
            AceFlags.None,
            AceQualifier.AccessDenied,
            (int)ProcessAccessRights.PROCESS_TERMINATE,
            new SecurityIdentifier(WellKnownSidType.WorldSid, null),
            false,
            null)
            );

            dacl.DiscretionaryAcl.InsertAce(
            0,
            new CommonAce(
            AceFlags.None,
            AceQualifier.AccessDenied,
            (int)ProcessAccessRights.PROCESS_TERMINATE,
            new SecurityIdentifier(WellKnownSidType.NTAuthoritySid, null),
            false,
            null)
            );


            dacl.DiscretionaryAcl.InsertAce(
            0,
            new CommonAce(
            AceFlags.None,
            AceQualifier.AccessDenied,
            (int)ProcessAccessRights.PROCESS_TERMINATE,
            new SecurityIdentifier(WellKnownSidType.LocalSystemSid, null),
            false,
            null)
            );
            // Save the DACL
            SetProcessSecurityDescriptor(hProcess, dacl);
        }


        public delegate void ControlStringConsumer(Control control, string text, bool ConnectOnWIFI);  // defines a delegate type

        public void SetText(Control control, string text, bool ConnectOnWIFI)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(new ControlStringConsumer(SetText), new object[] { control, text, ConnectOnWIFI });  // invoking itself
            }
            else
            {
                tbWiFi.Text = text;
                rbProgramm.Checked = !ConnectOnWIFI;
                rbWiFi.Checked = ConnectOnWIFI;
                //control.Text = text;      // the "functional part", executing only on the main thread
            }
        }

        cTimes times = new cTimes();
        bool isLocked = false;

       

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Microsoft.Win32.SystemEvents.SessionSwitch += new Microsoft.Win32.SessionSwitchEventHandler(SystemEvents_SessionSwitch);
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\data.dat"))
                    times = times.Load(AppDomain.CurrentDomain.BaseDirectory + "\\data.dat");
            }
            catch (Exception ex) { }
            SetText(tbWiFi, times.WLANID, times.connectOnWLAN);
            if (rbProgramm.Checked)
            {
                times.beginTimespan();
                while (true)
                {
                    
                    if (!isLocked || !cbLogBreaks.Checked)
                    {
                        times.addTimestamp();                        
                    }
                    try
                    {
                        times.Save(times, AppDomain.CurrentDomain.BaseDirectory + "\\data.dat");
                    }catch (Exception ex) { }
                    System.Threading.Thread.Sleep(60000);
                }
            }
            else
            {
                try
                {
                    ManagedNativeWifi.NativeWifi client = new ManagedNativeWifi.NativeWifi();
                    while (true)
                    {
                        foreach (ManagedNativeWifi.NetworkIdentifier wlanInterface in ManagedNativeWifi.NativeWifi.EnumerateAvailableNetworkSsids())
                        {
                            {
                                {
                                    String nam = wlanInterface.ToString();
                                    //if (nam == "wlan_lab_01")
                                    if (nam == times.WLANID)
                                    {

                                        if (!isLocked || !cbLogBreaks.Checked)
                                        {
                                            times.addTimestamp();
                                            times.Save(times, AppDomain.CurrentDomain.BaseDirectory + "\\data.dat");
                                        }
                                    }
                                }
                            }

                            System.Threading.Thread.Sleep(10000);
                        }
                    }
                }
                catch (Exception) { }
            }
        }

       

        void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                isLocked = true;
                //I left my desk
            }
            else if (e.Reason == SessionSwitchReason.SessionUnlock)
            {
                if(cbLogBreaks.Checked)
                    times.beginTimespan();
                isLocked = false;

                //I returned to my desk
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if(!License.Status.Licensed)
            {
                MessageBox.Show(this, "No License Found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
            backgroundWorker1.RunWorkerAsync();
            this.WindowState = FormWindowState.Minimized;

        }

        private void rbWiFi_CheckedChanged(object sender, EventArgs e)
        {
            times.connectOnWLAN = rbWiFi.Checked;
            if (rbWiFi.Checked)
                tbWiFi.Enabled = true;
            else
                tbWiFi.Enabled = false;
        }

        private void tbWiFi_TextChanged(object sender, EventArgs e)
        {
            times.WLANID = tbWiFi.Text;
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                //this.notifyIcon1.ContextMenu = contextMenu1;
                this.ShowInTaskbar = false;
                this.WindowState = FormWindowState.Minimized;
                this.notifyIcon1.Visible = true;
            }
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.ShowInTaskbar = true;
            this.notifyIcon1.Visible = false;
            this.WindowState = FormWindowState.Normal;
        }

        private void bnExport_Click(object sender, EventArgs e)
        {
            times.Export(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date);;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
