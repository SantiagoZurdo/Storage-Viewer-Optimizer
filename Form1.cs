using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace Storage_Viewer___Optimizer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeListView();
            ShowDisks();
            AddApplicationToStartup();
            this.ShowInTaskbar = false;
        }

        /// <summary>
        /// Configura el ListView para mostrar información de los discos.
        /// </summary>
        private void InitializeListView()
        {
            DiskList.View = View.Details;
            DiskList.Columns.Add("Disk", 50);
            DiskList.Columns.Add("Free Space (GB)", 100);
            DiskList.Columns.Add("Total Space (GB)", 100);
            DiskList.OwnerDraw = true;
            DiskList.DrawColumnHeader += (s, e) => e.DrawDefault = true;
            DiskList.DrawItem += (s, e) => e.DrawDefault = true;
            DiskList.DrawSubItem += DiskList_DrawSubItem;
        }

        /// <summary>
        /// Muestra la información de los discos en el ListView.
        /// </summary>
        private void ShowDisks()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                if (!drive.IsReady) continue;

                var item = new ListViewItem(drive.Name);
                item.SubItems.Add($"{drive.AvailableFreeSpace / (1024 * 1024 * 1024)} GB"); // Espacio libre
                item.SubItems.Add($"{drive.TotalSize / (1024 * 1024 * 1024)} GB"); // Espacio total
                DiskList.Items.Add(item);
            }
        }

        /// <summary>
        /// Dibuja las subceldas del ListView, aplicando un fondo celeste a la columna de espacio libre.
        /// </summary>
        private void DiskList_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
        {
            if (e.ColumnIndex == 1) // Columna de espacio libre
            {
                e.Graphics.FillRectangle(Brushes.LightBlue, e.Bounds);
                e.Graphics.DrawString(e.SubItem.Text, DiskList.Font, Brushes.Black, e.Bounds);
            }
            else
            {
                e.DrawDefault = true;
            }
        }

        private void OptimizeSystem()
        {
            string script = @"
            # Eliminar archivos temporales
            Remove-Item -Path $env:TEMP\* -Recurse -Force -ErrorAction SilentlyContinue
            Remove-Item -Path C:\Windows\Temp\* -Recurse -Force -ErrorAction SilentlyContinue

            # Vaciar la papelera de reciclaje
            (New-Object -ComObject Shell.Application).NameSpace(0xA).Items() | ForEach-Object { $_.InvokeVerb('delete') }

            # Borrar la carpeta prefetch
            Remove-Item -Path C:\Windows\Prefetch\* -Recurse -Force -ErrorAction SilentlyContinue

            # Liberar memoria RAM
            [System.GC]::Collect()
            [System.GC]::WaitForPendingFinalizers()
            [System.GC]::Collect()

            'Optimization Complete'";

            ExecutePowerShellScript(script);

            // Borrar contenido de la carpeta Descargas
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            if (Directory.Exists(downloadsPath))
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(downloadsPath);
                    foreach (FileInfo file in di.GetFiles())
                    {
                        file.Delete();
                    }
                    foreach (DirectoryInfo dir in di.GetDirectories())
                    {
                        dir.Delete(true);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error clearing Downloads folder: {ex.Message}", "Optimization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            // Mostrar cuadro de diálogo al finalizar
            MessageBox.Show("Optimization complete.", "Optimization", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


        private void ExecutePowerShellScript(string script)
        {
            using (System.Diagnostics.Process process = new System.Diagnostics.Process())
            {
                process.StartInfo.FileName = "powershell.exe";
                process.StartInfo.Arguments = $"-NoProfile -NoLogo -NonInteractive -Command \"{script}\"";
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;

                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (!string.IsNullOrEmpty(error))
                {
                    MessageBox.Show($"Error: {error}", "Optimization Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnOpti_Click(object sender, EventArgs e)
        {
            OptimizeSystem();
        }

        private void AddApplicationToStartup()
        {
            string runKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
            string applicationName = "StorageViewerOptimizer";
            string applicationPath = Application.ExecutablePath;

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(runKey, true))
            {
                key.SetValue(applicationName, applicationPath);
            }
        }

        private bool isMouseDown = false;
        private Point lastLocation;

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            isMouseDown = true;
            lastLocation = e.Location;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown)
            {
                this.Location = new Point(
                    (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

                this.Update();
            }
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetParent(this.Handle, GetDesktopWindow());
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
    }
}
