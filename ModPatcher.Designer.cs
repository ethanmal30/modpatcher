using System;
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
        private Button PatchButton;
        private Button ModsButton;
        private MenuStrip Menu;
        private ToolStripMenuItem editMenu;
        private ToolStripMenuItem editModPathMenu;
        private ToolStripMenuItem refreshModsMenu;
        private ToolStripMenuItem helpMenu;
        private ToolStripMenuItem aboutMenu;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ModPatcher));
            ButtonCard = new Panel();
            ModsButton = new Button();
            PatchBox = new TextBox();
            DirBox = new TextBox();
            FolderPath = new TextBox();
            BrowseButton = new Button();
            PatchType = new ComboBox();
            ModsList = new ComboBox();
            PatchButton = new Button();
            Menu = new MenuStrip();
            editMenu = new ToolStripMenuItem();
            editModPathMenu = new ToolStripMenuItem();
            refreshModsMenu = new ToolStripMenuItem();
            helpMenu = new ToolStripMenuItem();
            aboutMenu = new ToolStripMenuItem();
            ButtonCard.SuspendLayout();
            Menu.SuspendLayout();
            SuspendLayout();
            // 
            // ButtonCard
            // 
            ButtonCard.BackColor = Color.FromArgb(45, 50, 65);
            ButtonCard.Controls.Add(ModsButton);
            ButtonCard.Controls.Add(PatchBox);
            ButtonCard.Controls.Add(DirBox);
            ButtonCard.Controls.Add(FolderPath);
            ButtonCard.Controls.Add(BrowseButton);
            ButtonCard.Controls.Add(PatchType);
            ButtonCard.Controls.Add(ModsList);
            ButtonCard.Controls.Add(PatchButton);
            ButtonCard.Location = new Point(6, 30);
            ButtonCard.Name = "ButtonCard";
            ButtonCard.Size = new Size(267, 184);
            ButtonCard.TabIndex = 0;
            // 
            // ModsButton
            // 
            ModsButton.BackColor = Color.SteelBlue;
            ModsButton.FlatAppearance.BorderSize = 0;
            ModsButton.FlatStyle = FlatStyle.Flat;
            ModsButton.ForeColor = Color.White;
            ModsButton.Location = new Point(200, 85);
            ModsButton.Name = "ModsButton";
            ModsButton.Size = new Size(58, 23);
            ModsButton.TabIndex = 0;
            ModsButton.Text = "Mods";
            ModsButton.UseVisualStyleBackColor = false;
            // 
            // PatchBox
            // 
            PatchBox.BackColor = Color.FromArgb(45, 50, 65);
            PatchBox.BorderStyle = BorderStyle.None;
            PatchBox.ForeColor = Color.White;
            PatchBox.Location = new Point(8, 64);
            PatchBox.Name = "PatchBox";
            PatchBox.ReadOnly = true;
            PatchBox.Size = new Size(120, 16);
            PatchBox.TabIndex = 1;
            PatchBox.TabStop = false;
            PatchBox.Text = "Unmodded/Modded?";
            // 
            // DirBox
            // 
            DirBox.BackColor = Color.FromArgb(45, 50, 65);
            DirBox.BorderStyle = BorderStyle.None;
            DirBox.ForeColor = Color.White;
            DirBox.Location = new Point(8, 9);
            DirBox.Name = "DirBox";
            DirBox.ReadOnly = true;
            DirBox.Size = new Size(120, 16);
            DirBox.TabIndex = 2;
            DirBox.TabStop = false;
            DirBox.Text = "Select folder directory:";
            // 
            // FolderPath
            // 
            FolderPath.BackColor = Color.FromArgb(225, 225, 225);
            FolderPath.BorderStyle = BorderStyle.FixedSingle;
            FolderPath.ForeColor = Color.Black;
            FolderPath.Location = new Point(8, 31);
            FolderPath.Name = "FolderPath";
            FolderPath.ReadOnly = true;
            FolderPath.Size = new Size(186, 23);
            FolderPath.TabIndex = 3;
            // 
            // BrowseButton
            // 
            BrowseButton.BackColor = Color.FromArgb(70, 130, 180);
            BrowseButton.FlatAppearance.BorderSize = 0;
            BrowseButton.FlatStyle = FlatStyle.Flat;
            BrowseButton.ForeColor = Color.White;
            BrowseButton.Location = new Point(200, 31);
            BrowseButton.Name = "BrowseButton";
            BrowseButton.Size = new Size(58, 23);
            BrowseButton.TabIndex = 4;
            BrowseButton.Text = "Browse";
            BrowseButton.UseVisualStyleBackColor = false;
            // 
            // PatchType
            // 
            PatchType.BackColor = Color.White;
            PatchType.DropDownStyle = ComboBoxStyle.DropDownList;
            PatchType.ForeColor = Color.Black;
            PatchType.Location = new Point(8, 85);
            PatchType.Name = "PatchType";
            PatchType.Size = new Size(84, 23);
            PatchType.TabIndex = 5;
            // 
            // ModsList
            // 
            ModsList.BackColor = Color.White;
            ModsList.DropDownStyle = ComboBoxStyle.DropDownList;
            ModsList.ForeColor = Color.Black;
            ModsList.Location = new Point(98, 85);
            ModsList.Name = "ModsList";
            ModsList.Size = new Size(96, 23);
            ModsList.TabIndex = 6;
            // 
            // PatchButton
            // 
            PatchButton.BackColor = Color.FromArgb(110, 160, 90);
            PatchButton.FlatAppearance.BorderSize = 0;
            PatchButton.FlatStyle = FlatStyle.Flat;
            PatchButton.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            PatchButton.ForeColor = Color.White;
            PatchButton.Location = new Point(8, 127);
            PatchButton.Name = "PatchButton";
            PatchButton.Size = new Size(250, 47);
            PatchButton.TabIndex = 7;
            PatchButton.Text = "PATCH FOLDER";
            PatchButton.UseVisualStyleBackColor = false;
            // 
            // Menu
            // 
            Menu.Items.AddRange(new ToolStripItem[] { refreshModsMenu, editMenu, helpMenu, aboutMenu });
            Menu.Location = new Point(0, 0);
            Menu.Name = "Menu";
            Menu.Size = new Size(279, 24);
            Menu.TabIndex = 1;
            // 
            // editMenu
            // 
            editMenu.DropDownItems.AddRange(new ToolStripItem[] { editModPathMenu });
            editMenu.Name = "editMenu";
            editMenu.Size = new Size(39, 20);
            editMenu.Text = "Edit";
            // 
            // editModPathMenu
            // 
            editModPathMenu.Name = "editModPathMenu";
            editModPathMenu.Size = new Size(180, 22);
            editModPathMenu.Text = "Edit mod path";
            // 
            // refreshModsMenu
            // 
            refreshModsMenu.Name = "refreshModsMenu";
            refreshModsMenu.Size = new Size(91, 20);
            refreshModsMenu.Text = "Refresh Mods";
            // 
            // helpMenu
            // 
            helpMenu.Name = "helpMenu";
            helpMenu.Size = new Size(44, 20);
            helpMenu.Text = "Help";
            // 
            // aboutMenu
            // 
            aboutMenu.Name = "aboutMenu";
            aboutMenu.Size = new Size(52, 20);
            aboutMenu.Text = "About";
            // 
            // ModPatcher
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(28, 32, 40);
            ClientSize = new Size(279, 220);
            Controls.Add(ButtonCard);
            Controls.Add(Menu);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = Menu;
            MaximizeBox = false;
            Name = "ModPatcher";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "RVNG Mod Patcher";
            ButtonCard.ResumeLayout(false);
            ButtonCard.PerformLayout();
            Menu.ResumeLayout(false);
            Menu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}