using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace VengeModPatcher
{
    public partial class ModPatcher : Form
    {
        private readonly string AppFolder;
        private readonly string ModsFolder;

        private string ClientPath;

        private const string VengeClientFolderName = "Venge Client";

        public ModPatcher()
        {
            InitializeComponent();

            AppFolder = Application.StartupPath;
            ModsFolder = Path.Combine(AppFolder, "Mods");

            EnsureFolderExists(ModsFolder);

            PatchType.Items.AddRange(new object[] { "Unmodded", "Modded" });
            PatchType.SelectedIndex = 0;
            PatchType.SelectedIndexChanged += PatchType_SelectedIndexChanged;

            BrowseButton.Click += BrowseButton_Click;
            PatchButton.Click += PatchButton_Click;
            ModsButton.Click += (s, e) => OpenModsFolder();

            editModPathMenu.Click += EditModPathMenu_Click;
            refreshModsMenu.Click += RefreshMenu_Click;
            helpMenu.Click += HelpMenu_Click;
            aboutMenu.Click += AboutMenu_Click;

            SetClientPathAutomatically();
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
            return Directory.Exists(path) && Directory.EnumerateDirectories(path).Any(d => Path.GetFileName(d).Equals(VengeClientFolderName, StringComparison.OrdinalIgnoreCase));
        }

        private static void ClearDirectory(string path)
        {
            var di = new DirectoryInfo(path);
            foreach (var file in di.GetFiles())
            {
                if (file.IsReadOnly) file.IsReadOnly = false;
                file.Delete();
            }

            foreach (var dir in di.GetDirectories())
            {
                RemoveReadOnlyRecursively(dir);
                dir.Delete(true);
            }
        }

        private static void CopyDirectory(string sourceDir, string destDir)
        {
            EnsureFolderExists(destDir);
            foreach (var file in Directory.EnumerateFiles(sourceDir))
            {
                var destFile = Path.Combine(destDir, Path.GetFileName(file));
                if (File.Exists(destFile))
                {
                    var existing = new FileInfo(destFile);
                    if (existing.IsReadOnly) existing.IsReadOnly = false;
                }
                File.Copy(file, destFile, true);
            }

            foreach (var dir in Directory.EnumerateDirectories(sourceDir))
                CopyDirectory(dir, Path.Combine(destDir, Path.GetFileName(dir)));
        }

        private static void RemoveReadOnlyRecursively(DirectoryInfo dir)
        {
            foreach (var subDir in dir.GetDirectories())
                RemoveReadOnlyRecursively(subDir);

            foreach (var file in dir.GetFiles())
                if (file.IsReadOnly) file.IsReadOnly = false;

            dir.Attributes &= ~FileAttributes.ReadOnly;
        }

        private void SetClientPathAutomatically()
        {
            string documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string vengePath = Path.Combine(documents, VengeClientFolderName);

            if (Directory.Exists(vengePath))
            {
                ClientPath = vengePath;
                FolderPath.Text = ClientPath;
            }
            else
            {
                ClientPath = "";
                FolderPath.Text = "";
                MessageBox.Show($"Could not find '{VengeClientFolderName}' in Documents.\nYou may need to browse manually.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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

            ClientPath = fbd.SelectedPath;
            FolderPath.Text = ClientPath;
            Activate();
        }

        private void PatchButton_Click(object sender, EventArgs e)
        {
            SetUiEnabled(false);
            try
            {
                var target = ClientPath;

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
                if (type == "Unmodded")
                {
                    try
                    {
                        ClearDirectory(target);
                        MessageBox.Show("Folder patched.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Folder is read-only or requires elevated permissions.\nPlease run the patcher as administrator.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        RemoveReadOnlyRecursively(new DirectoryInfo(target));
                        ClearDirectory(target);
                        MessageBox.Show("Folder patched after removing read-only.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Failed patching folder: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    try
                    {
                        ClearDirectory(target);

                        var nestedVenge = Directory.EnumerateDirectories(modPath)
                            .FirstOrDefault(d => Path.GetFileName(d).Equals(VengeClientFolderName, StringComparison.OrdinalIgnoreCase));

                        CopyDirectory(nestedVenge ?? modPath, target);

                        MessageBox.Show("Patched successfully.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        MessageBox.Show("Folder is read-only or requires elevated permissions.\nPlease run the patcher as administrator.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        RemoveReadOnlyRecursively(new DirectoryInfo(target));
                        ClearDirectory(target);
                        CopyDirectory(modPath, target);
                        MessageBox.Show("Patched successfully after removing read-only.", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Patch failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            finally
            {
                SetUiEnabled(true);
            }
        }

        private void SetUiEnabled(bool enabled)
        {
            PatchButton.Enabled = enabled;
            BrowseButton.Enabled = enabled;
            ModsList.Enabled = enabled && PatchType.SelectedItem?.ToString() == "Modded";
            PatchType.Enabled = enabled;
            ModsButton.Enabled = enabled;
        }

        private void RefreshMenu_Click(object sender, EventArgs e)
        {
            LoadAvailableMods();
            if (HasNestedVengeClient(ClientPath))
            {
                MessageBox.Show(
                    "Folder contains a nested Venge Client folder.\n" +
                    "Move its contents up to patch correctly.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HelpMenu_Click(object sender, EventArgs e) => ShowHelpForm();

        private void AboutMenu_Click(object sender, EventArgs e) => ShowAboutForm();

        private void EditModPathMenu_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox(
                "Enter the path to the Venge Client folder:",
                "Edit Client Path",
                ClientPath);

            if (!string.IsNullOrWhiteSpace(input))
            {
                ClientPath = input;
                FolderPath.Text = ClientPath;
            }
        }

        private void OpenModsFolder()
        {
            EnsureFolderExists(ModsFolder);
            System.Diagnostics.Process.Start("explorer.exe", ModsFolder);
        }

        private void ShowHelpForm()
        {
            var helpForm = new Form
            {
                Text = "Help",
                Size = new System.Drawing.Size(350, 215),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var label = new Label
            {
                Text = "1. Select the Venge Client folder in your Documents folder\n" +
                       "2. Choose if you want to factory reset or choose a mod\n" +
                       "3. If you want to reset, select Unmodded and press PATCH\n" +
                       "4. If you want to add a mod, choose Modded\n" +
                       "5. Open the Patcher mod folder with the Folder button\n" +
                       "6. Put all your mods folders inside\n" +
                       "7. Press PATCH and open your game\n\n" +
                       "NOTE: If you want to swap from one mod to another, you can directly swap without resetting.",
                Dock = DockStyle.Fill,
                AutoSize = false,
                TextAlign = System.Drawing.ContentAlignment.TopLeft,
                Padding = new Padding(10)
            };

            helpForm.Controls.Add(label);
            helpForm.ShowDialog(this);
        }

        private void ShowAboutForm()
        {
            var aboutForm = new Form
            {
                Text = "About",
                Size = new System.Drawing.Size(100, 100),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                MaximizeBox = false,
                MinimizeBox = false
            };

            var label = new Label
            {
                Text = "RVNG Mod Patcher\nVersion 1.2\n© 2025 aftrheavn",
                Dock = DockStyle.Fill,
                AutoSize = false,
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };

            aboutForm.Controls.Add(label);
            aboutForm.ShowDialog(this);
        }
    }
}