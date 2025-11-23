using System.Drawing;
using System.Windows.Forms;

namespace VengeModPatcher
{
    partial class ModPatcher
    {
        private System.ComponentModel.IContainer components = null;

        private Panel ButtonCard;
        private TextBox DirBox;
        private TextBox PatchBox;
        private TextBox FolderPath;
        private Button BrowseButton;
        private ComboBox PatchType;
        private ComboBox ModsList;
        private Button RefreshButton;
        private Button PatchButton;
        private new Button HelpButton;
        private Button FolderButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModPatcher));

            ButtonCard = new Panel
            {
                BackColor = Color.FromArgb(45, 50, 65),
                Location = new Point(6, 6),
                Name = "ButtonCard",
                Size = new Size(392, 184)
            };

            DirBox = new TextBox
            {
                BackColor = Color.FromArgb(45, 50, 65),
                BorderStyle = BorderStyle.None,
                ForeColor = Color.White,
                Location = new Point(8, 9),
                Name = "DirBox",
                ReadOnly = true,
                Size = new Size(120, 16),
                TabStop = false,
                Text = "Select folder directory:"
            };

            FolderPath = new TextBox
            {
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = Color.Black,
                Location = new Point(8, 31),
                Name = "FolderPath",
                ReadOnly = true,
                Size = new Size(250, 23)
            };

            PatchBox = new TextBox
            {
                BackColor = Color.FromArgb(45, 50, 65),
                BorderStyle = BorderStyle.None,
                ForeColor = Color.White,
                Location = new Point(8, 64),
                Name = "PatchBox",
                ReadOnly = true,
                Size = new Size(126, 16),
                TabStop = false,
                Text = "Default/Modded patch?"
            };

            PatchType = new ComboBox
            {
                BackColor = Color.White,
                DropDownStyle = ComboBoxStyle.DropDownList,
                ForeColor = Color.Black,
                Location = new Point(8, 86),
                Name = "PatchType",
                Size = new Size(121, 23)
            };

            ModsList = new ComboBox
            {
                BackColor = Color.White,
                DropDownStyle = ComboBoxStyle.DropDownList,
                ForeColor = Color.Black,
                Location = new Point(137, 86),
                Name = "ModsList",
                Size = new Size(121, 23)
            };

            BrowseButton = new Button
            {
                BackColor = Color.FromArgb(70, 130, 180),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Location = new Point(264, 31),
                Name = "BrowseButton",
                Size = new Size(120, 23),
                Text = "Browse",
                UseVisualStyleBackColor = false
            };

            BrowseButton.FlatAppearance.BorderSize = 0;

            RefreshButton = new Button
            {
                BackColor = Color.SteelBlue,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Location = new Point(264, 86),
                Name = "RefreshButton",
                Size = new Size(58, 23),
                Text = "Refresh",
                UseVisualStyleBackColor = false
            };

            RefreshButton.FlatAppearance.BorderSize = 0;

            FolderButton = new Button
            {
                BackColor = Color.SteelBlue,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Location = new Point(326, 86),
                Name = "FolderButton",
                Size = new Size(58, 23),
                Text = "Folder",
                UseVisualStyleBackColor = false
            };

            FolderButton.FlatAppearance.BorderSize = 0;

            PatchButton = new Button
            {
                BackColor = Color.FromArgb(110, 160, 90),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Location = new Point(8, 128),
                Name = "PatchButton",
                Size = new Size(250, 47),
                Text = "PATCH FOLDER",
                UseVisualStyleBackColor = false
            };

            PatchButton.FlatAppearance.BorderSize = 0;

            HelpButton = new Button
            {
                BackColor = Color.FromArgb(90, 110, 160),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Location = new Point(264, 128),
                Name = "HelpButton",
                Size = new Size(120, 47),
                Text = "HELP",
                UseVisualStyleBackColor = false
            };

            HelpButton.FlatAppearance.BorderSize = 0;

            ButtonCard.Controls.AddRange(new Control[]
            {
                FolderButton, PatchBox, HelpButton, DirBox, FolderPath, BrowseButton,
                PatchType, ModsList, RefreshButton, PatchButton
            });

            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(28, 32, 40);
            ClientSize = new Size(404, 195);
            Controls.Add(ButtonCard);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "ModPatcher";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RVNG Mod Patcher (v1.0) - by aftrheavn";
        }
    }
}