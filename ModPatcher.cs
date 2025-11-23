using System.IO.Compression;

namespace VengeModPatcher
{
    public partial class ModPatcher : Form
    {
        private readonly string AppFolder;
        private readonly string ModsFolder;
        private readonly string BackupFolder;
        private readonly string FirstRunFile;
        private readonly string ClientPathFile;

        private const string VengeClientFolderName = "Venge Client";

        public ModPatcher()
        {
            InitializeComponent();

            AppFolder = Application.StartupPath;
            ModsFolder = Path.Combine(AppFolder, "Mods");
            BackupFolder = Path.Combine(AppFolder, "Backup");
            FirstRunFile = Path.Combine(AppFolder, "run.txt");
            ClientPathFile = Path.Combine(AppFolder, "client_path.txt");

            EnsureFolderExists(ModsFolder);
            EnsureFolderExists(BackupFolder);

            PatchType.Items.AddRange(["Default", "Modded"]);
            PatchType.SelectedIndex = 0;
            PatchType.SelectedIndexChanged += PatchType_SelectedIndexChanged;

            BrowseButton.Click += BrowseButton_Click;
            PatchButton.Click += PatchButton_Click;
            RefreshButton.Click += RefreshButton_Click;
            FolderButton.Click += FolderButton_Click;
            HelpButton.Click += HelpButton_Click;

            CheckFirstRun();
            LoadAvailableMods();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DirBox.ReadOnly = PatchBox.ReadOnly = true;
            DirBox.TabStop = PatchBox.TabStop = false;
        }


        private static void EnsureFolderExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private static bool IsValidVengeFolder(string path)
        {
            return !string.IsNullOrWhiteSpace(path) &&
                   new DirectoryInfo(path).Name.Equals(VengeClientFolderName, StringComparison.OrdinalIgnoreCase);
        }

        private static bool HasNestedVengeClient(string path)
        {
            return Directory.Exists(path) &&
                   Directory.EnumerateDirectories(path)
                            .Any(d => Path.GetFileName(d).Equals(VengeClientFolderName, StringComparison.OrdinalIgnoreCase));
        }

        private static void ClearDirectory(string path)
        {
            var di = new DirectoryInfo(path);
            foreach (var file in di.GetFiles()) file.Delete();
            foreach (var dir in di.GetDirectories()) dir.Delete(true);
        }

        private static void CopyDirectory(string sourceDir, string destDir)
        {
            EnsureFolderExists(destDir);
            foreach (var file in Directory.EnumerateFiles(sourceDir))
                File.Copy(file, Path.Combine(destDir, Path.GetFileName(file)), true);

            foreach (var dir in Directory.EnumerateDirectories(sourceDir))
                CopyDirectory(dir, Path.Combine(destDir, Path.GetFileName(dir)));
        }

        private static void CreateBackup(string sourcePath, string backupPath)
        {
            ZipFile.CreateFromDirectory(sourcePath, backupPath, CompressionLevel.Fastest, true);
        }

        private static void RestoreBackup(string backupZip, string targetPath)
        {
            ClearDirectory(targetPath);

            var tempExtract = Path.Combine(Path.GetTempPath(), "VengePatchTemp");
            if (Directory.Exists(tempExtract)) Directory.Delete(tempExtract, true);

            ZipFile.ExtractToDirectory(backupZip, tempExtract);

            var sourcePath = Path.Combine(tempExtract, VengeClientFolderName);
            if (!Directory.Exists(sourcePath)) sourcePath = tempExtract;

            CopyDirectory(sourcePath, targetPath);
            Directory.Delete(tempExtract, true);
        }

        private void CheckFirstRun()
        {
            if (File.Exists(FirstRunFile))
            {
                if (File.Exists(ClientPathFile))
                    FolderPath.Text = File.ReadAllText(ClientPathFile);
                return;
            }

            MessageBox.Show(
                "Please select the Venge Client folder in the Documents folder.\nA full backup will be created automatically.",
                "Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);

            using var fbd = new FolderBrowserDialog();
            while (true)
            {
                if (fbd.ShowDialog() != DialogResult.OK)
                {
                    Application.Exit();
                    return;
                }

                if (!IsValidVengeFolder(fbd.SelectedPath))
                {
                    MessageBox.Show("Invalid folder! You must select the \"Venge Client\" folder!",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue;
                }

                FolderPath.Text = fbd.SelectedPath;
                File.WriteAllText(ClientPathFile, fbd.SelectedPath);

                var backupZip = Path.Combine(BackupFolder, "InitialBackup.zip");
                CreateBackup(fbd.SelectedPath, backupZip);

                break;
            }

            File.WriteAllText(FirstRunFile, "1");
        }

        private void LoadAvailableMods()
        {
            ModsList.Items.Clear();
            if (!Directory.Exists(ModsFolder)) return;

            foreach (var mod in Directory.EnumerateDirectories(ModsFolder).Select(Path.GetFileName))
                ModsList.Items.Add(mod);

            if (ModsList.Items.Count > 0) ModsList.SelectedIndex = 0;
            ModsList.Enabled = PatchType.SelectedItem?.ToString() == "Modded";
        }

        private void PatchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModsList.Enabled = PatchType.SelectedItem?.ToString() == "Modded";
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog { Description = "Select Venge Client folder" };
            if (fbd.ShowDialog() != DialogResult.OK) return;

            FolderPath.Text = fbd.SelectedPath;
            File.WriteAllText(ClientPathFile, fbd.SelectedPath);
            Activate();
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadAvailableMods();

            if (HasNestedVengeClient(FolderPath.Text))
            {
                MessageBox.Show("Folder contains a nested \"Venge Client\" folder.\nMove its contents up to patch correctly.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FolderButton_Click(object sender, EventArgs e)
        {
            EnsureFolderExists(ModsFolder);
            System.Diagnostics.Process.Start("explorer.exe", ModsFolder);
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this,
                "1. Select the Venge Client folder in your Documents folder\n" +
                "2. Choose if you want to factory reset or choose a mod\n" +
                "3. If you want to reset, select Default and press PATCH\n" +
                "4. If you want to add a mod, choose Modded\n" +
                "5. Open the Patcher mod folder with the Folder button\n" +
                "6. Put all your mods folders inside\n" +
                "7. Press PATCH and open your game\n\n" +
                "NOTE: If you want to swap from one mod to another, you can directly swap without resetting.",
                "Venge Mod Patcher Help", MessageBoxButtons.OK, MessageBoxIcon.Information
            );
        }

        private void SetUiEnabled(bool enabled)
        {
            PatchButton.Enabled = enabled;
            RefreshButton.Enabled = enabled;
            BrowseButton.Enabled = enabled;
            HelpButton.Enabled = enabled;
            ModsList.Enabled = enabled && PatchType.SelectedItem?.ToString() == "Modded";
            PatchType.Enabled = enabled;
            FolderButton.Enabled = enabled;
        }

        private void PatchButton_Click(object sender, EventArgs e)
        {
            SetUiEnabled(false);
            try
            {
                var target = FolderPath.Text;

                if (!IsValidVengeFolder(target) || !Directory.Exists(target))
                {
                    MessageBox.Show("Invalid folder! Select a valid \"Venge Client\" folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (HasNestedVengeClient(target))
                {
                    MessageBox.Show("Folder contains a nested \"Venge Client\" folder.\nMove its contents up to patch correctly.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var type = PatchType.SelectedItem.ToString();
                if (type == "Default")
                {
                    var latestZip = Directory.EnumerateFiles(BackupFolder, "*.zip")
                        .OrderByDescending(f => f).FirstOrDefault();

                    if (latestZip == null)
                    {
                        MessageBox.Show("No backups found.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    try
                    {
                        RestoreBackup(latestZip, target);
                        MessageBox.Show("Restored latest backup.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        MessageBox.Show("Restore failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    var mod = ModsList.SelectedItem?.ToString();
                    if (mod == null)
                    {
                        MessageBox.Show("Select a mod first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var modPath = Path.Combine(ModsFolder, mod);
                    if (!Directory.Exists(modPath))
                    {
                        MessageBox.Show("Mod folder missing.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var backupZip = Path.Combine(BackupFolder, $"Backup_{DateTime.Now:yyyyMMdd_HHmmss}.zip");
                    CreateBackup(target, backupZip);

                    try
                    {
                        ClearDirectory(target);

                        var nestedVenge = Directory.EnumerateDirectories(modPath)
                            .FirstOrDefault(d => Path.GetFileName(d).Equals(VengeClientFolderName, StringComparison.OrdinalIgnoreCase));

                        CopyDirectory(nestedVenge ?? modPath, target);

                        MessageBox.Show("Patched successfully.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch
                    {
                        RestoreBackup(backupZip, target);
                        MessageBox.Show("Patch failed and backup restored.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                SetUiEnabled(true);
            }
        }
    }
}