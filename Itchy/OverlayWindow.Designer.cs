namespace Itchy
{
    partial class OverlayWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.itchyLabel = new System.Windows.Forms.Label();
            this.optionsTranslucentPanel = new ItchyControls.TranslucentPanel();
            this.settingsTabControl = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.hideDyingCheckBox = new System.Windows.Forms.CheckBox();
            this.hideItemCheckBox = new System.Windows.Forms.CheckBox();
            this.hideCorpsesCheckBox = new System.Windows.Forms.CheckBox();
            this.infravisionHackCheckBox = new System.Windows.Forms.CheckBox();
            this.closeButton = new System.Windows.Forms.Button();
            this.applyButton = new System.Windows.Forms.Button();
            this.refreshButton = new System.Windows.Forms.Button();
            this.chickenManaPctLabel = new System.Windows.Forms.Label();
            this.chickenLifePctLabel = new System.Windows.Forms.Label();
            this.chickenManaPctTextBox = new System.Windows.Forms.TextBox();
            this.chickenLifePctTextBox = new System.Windows.Forms.TextBox();
            this.chickenOnHostileTextBox = new System.Windows.Forms.CheckBox();
            this.chickenToTownCheckBox = new System.Windows.Forms.CheckBox();
            this.enableChickenCheckBox = new System.Windows.Forms.CheckBox();
            this.goToTownCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.fastExitLabel = new System.Windows.Forms.Label();
            this.openCubeLabel = new System.Windows.Forms.Label();
            this.townPortalKeybindButton = new ItchyControls.KeybindButton();
            this.fastExitKeybindButton = new ItchyControls.KeybindButton();
            this.openCubeKeybindButton = new ItchyControls.KeybindButton();
            this.openStashLabel = new System.Windows.Forms.Label();
            this.openStashKeybindButton = new ItchyControls.KeybindButton();
            this.revealActLabel = new System.Windows.Forms.Label();
            this.revealActKeybindButton = new ItchyControls.KeybindButton();
            this.viewInventoryKeyLabel = new System.Windows.Forms.Label();
            this.viewInventoryKeybindButton = new ItchyControls.KeybindButton();
            this.changeColorCheckBox = new System.Windows.Forms.CheckBox();
            this.showSocketsCheckBox = new System.Windows.Forms.CheckBox();
            this.showRuneNumberCheckBox = new System.Windows.Forms.CheckBox();
            this.showItemPriceCheckBox = new System.Windows.Forms.CheckBox();
            this.showItemLevelCheckBox = new System.Windows.Forms.CheckBox();
            this.showEthCheckBox = new System.Windows.Forms.CheckBox();
            this.fastPortalCheckBox = new System.Windows.Forms.CheckBox();
            this.fastTeleCheckBox = new System.Windows.Forms.CheckBox();
            this.blockFlashCheckBox = new System.Windows.Forms.CheckBox();
            this.viewInventoryHackCheckBox = new System.Windows.Forms.CheckBox();
            this.itemNameHackCheckBox = new System.Windows.Forms.CheckBox();
            this.packetReceiveHackCheckBox = new System.Windows.Forms.CheckBox();
            this.weatherHackCheckBox = new System.Windows.Forms.CheckBox();
            this.lightHackCheckBox = new System.Windows.Forms.CheckBox();
            this.propertiesExpandButton = new ItchyControls.ExpandButton();
            this.logExpandButton = new ItchyControls.ExpandButton();
            this.logTranslucentPanel = new ItchyControls.TranslucentPanel();
            this.logTextBox = new ItchyControls.TransparentRichTextBox();
            this.optionsTranslucentPanel.SuspendLayout();
            this.settingsTabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.logTranslucentPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // itchyLabel
            // 
            this.itchyLabel.AutoSize = true;
            this.itchyLabel.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.itchyLabel.ForeColor = System.Drawing.Color.Maroon;
            this.itchyLabel.Location = new System.Drawing.Point(286, 597);
            this.itchyLabel.Name = "itchyLabel";
            this.itchyLabel.Size = new System.Drawing.Size(37, 14);
            this.itchyLabel.TabIndex = 3;
            this.itchyLabel.Text = "ITCHY";
            // 
            // optionsTranslucentPanel
            // 
            this.optionsTranslucentPanel.BackColor = System.Drawing.Color.Transparent;
            this.optionsTranslucentPanel.Controls.Add(this.settingsTabControl);
            this.optionsTranslucentPanel.Location = new System.Drawing.Point(19, 25);
            this.optionsTranslucentPanel.Name = "optionsTranslucentPanel";
            this.optionsTranslucentPanel.Size = new System.Drawing.Size(570, 502);
            this.optionsTranslucentPanel.TabIndex = 6;
            // 
            // settingsTabControl
            // 
            this.settingsTabControl.Controls.Add(this.tabPage1);
            this.settingsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsTabControl.Location = new System.Drawing.Point(0, 0);
            this.settingsTabControl.Name = "settingsTabControl";
            this.settingsTabControl.SelectedIndex = 0;
            this.settingsTabControl.Size = new System.Drawing.Size(570, 502);
            this.settingsTabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.hideDyingCheckBox);
            this.tabPage1.Controls.Add(this.hideItemCheckBox);
            this.tabPage1.Controls.Add(this.hideCorpsesCheckBox);
            this.tabPage1.Controls.Add(this.infravisionHackCheckBox);
            this.tabPage1.Controls.Add(this.closeButton);
            this.tabPage1.Controls.Add(this.applyButton);
            this.tabPage1.Controls.Add(this.refreshButton);
            this.tabPage1.Controls.Add(this.chickenManaPctLabel);
            this.tabPage1.Controls.Add(this.chickenLifePctLabel);
            this.tabPage1.Controls.Add(this.chickenManaPctTextBox);
            this.tabPage1.Controls.Add(this.chickenLifePctTextBox);
            this.tabPage1.Controls.Add(this.chickenOnHostileTextBox);
            this.tabPage1.Controls.Add(this.chickenToTownCheckBox);
            this.tabPage1.Controls.Add(this.enableChickenCheckBox);
            this.tabPage1.Controls.Add(this.goToTownCheckBox);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.fastExitLabel);
            this.tabPage1.Controls.Add(this.openCubeLabel);
            this.tabPage1.Controls.Add(this.townPortalKeybindButton);
            this.tabPage1.Controls.Add(this.fastExitKeybindButton);
            this.tabPage1.Controls.Add(this.openCubeKeybindButton);
            this.tabPage1.Controls.Add(this.openStashLabel);
            this.tabPage1.Controls.Add(this.openStashKeybindButton);
            this.tabPage1.Controls.Add(this.revealActLabel);
            this.tabPage1.Controls.Add(this.revealActKeybindButton);
            this.tabPage1.Controls.Add(this.viewInventoryKeyLabel);
            this.tabPage1.Controls.Add(this.viewInventoryKeybindButton);
            this.tabPage1.Controls.Add(this.changeColorCheckBox);
            this.tabPage1.Controls.Add(this.showSocketsCheckBox);
            this.tabPage1.Controls.Add(this.showRuneNumberCheckBox);
            this.tabPage1.Controls.Add(this.showItemPriceCheckBox);
            this.tabPage1.Controls.Add(this.showItemLevelCheckBox);
            this.tabPage1.Controls.Add(this.showEthCheckBox);
            this.tabPage1.Controls.Add(this.fastPortalCheckBox);
            this.tabPage1.Controls.Add(this.fastTeleCheckBox);
            this.tabPage1.Controls.Add(this.blockFlashCheckBox);
            this.tabPage1.Controls.Add(this.viewInventoryHackCheckBox);
            this.tabPage1.Controls.Add(this.itemNameHackCheckBox);
            this.tabPage1.Controls.Add(this.packetReceiveHackCheckBox);
            this.tabPage1.Controls.Add(this.weatherHackCheckBox);
            this.tabPage1.Controls.Add(this.lightHackCheckBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(562, 476);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // hideDyingCheckBox
            // 
            this.hideDyingCheckBox.AutoSize = true;
            this.hideDyingCheckBox.Enabled = false;
            this.hideDyingCheckBox.Location = new System.Drawing.Point(32, 422);
            this.hideDyingCheckBox.Name = "hideDyingCheckBox";
            this.hideDyingCheckBox.Size = new System.Drawing.Size(107, 17);
            this.hideDyingCheckBox.TabIndex = 42;
            this.hideDyingCheckBox.Text = "Hide Dying Mobs";
            this.hideDyingCheckBox.UseVisualStyleBackColor = true;
            // 
            // hideItemCheckBox
            // 
            this.hideItemCheckBox.AutoSize = true;
            this.hideItemCheckBox.Enabled = false;
            this.hideItemCheckBox.Location = new System.Drawing.Point(32, 443);
            this.hideItemCheckBox.Name = "hideItemCheckBox";
            this.hideItemCheckBox.Size = new System.Drawing.Size(76, 17);
            this.hideItemCheckBox.TabIndex = 41;
            this.hideItemCheckBox.Text = "Hide Items";
            this.hideItemCheckBox.UseVisualStyleBackColor = true;
            // 
            // hideCorpsesCheckBox
            // 
            this.hideCorpsesCheckBox.AutoSize = true;
            this.hideCorpsesCheckBox.Enabled = false;
            this.hideCorpsesCheckBox.Location = new System.Drawing.Point(32, 401);
            this.hideCorpsesCheckBox.Name = "hideCorpsesCheckBox";
            this.hideCorpsesCheckBox.Size = new System.Drawing.Size(89, 17);
            this.hideCorpsesCheckBox.TabIndex = 40;
            this.hideCorpsesCheckBox.Text = "Hide Corpses";
            this.hideCorpsesCheckBox.UseVisualStyleBackColor = true;
            // 
            // infravisionHackCheckBox
            // 
            this.infravisionHackCheckBox.AutoSize = true;
            this.infravisionHackCheckBox.Location = new System.Drawing.Point(16, 377);
            this.infravisionHackCheckBox.Name = "infravisionHackCheckBox";
            this.infravisionHackCheckBox.Size = new System.Drawing.Size(115, 17);
            this.infravisionHackCheckBox.TabIndex = 39;
            this.infravisionHackCheckBox.Text = "Infravision Hack: 1";
            this.infravisionHackCheckBox.UseVisualStyleBackColor = true;
            this.infravisionHackCheckBox.CheckedChanged += new System.EventHandler(this.infravisionHackCheckBox_CheckedChanged);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(463, 441);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 38;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // applyButton
            // 
            this.applyButton.Location = new System.Drawing.Point(382, 441);
            this.applyButton.Name = "applyButton";
            this.applyButton.Size = new System.Drawing.Size(75, 23);
            this.applyButton.TabIndex = 37;
            this.applyButton.Text = "Apply";
            this.applyButton.UseVisualStyleBackColor = true;
            this.applyButton.Click += new System.EventHandler(this.applyButton_Click);
            // 
            // refreshButton
            // 
            this.refreshButton.Location = new System.Drawing.Point(233, 440);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(75, 23);
            this.refreshButton.TabIndex = 36;
            this.refreshButton.Text = "Refresh";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.refreshButton_Click);
            // 
            // chickenManaPctLabel
            // 
            this.chickenManaPctLabel.AutoSize = true;
            this.chickenManaPctLabel.Location = new System.Drawing.Point(434, 122);
            this.chickenManaPctLabel.Name = "chickenManaPctLabel";
            this.chickenManaPctLabel.Size = new System.Drawing.Size(116, 13);
            this.chickenManaPctLabel.TabIndex = 35;
            this.chickenManaPctLabel.Text = "Chicken Mana Percent";
            // 
            // chickenLifePctLabel
            // 
            this.chickenLifePctLabel.AutoSize = true;
            this.chickenLifePctLabel.Location = new System.Drawing.Point(434, 95);
            this.chickenLifePctLabel.Name = "chickenLifePctLabel";
            this.chickenLifePctLabel.Size = new System.Drawing.Size(106, 13);
            this.chickenLifePctLabel.TabIndex = 34;
            this.chickenLifePctLabel.Text = "Chicken Life Percent";
            // 
            // chickenManaPctTextBox
            // 
            this.chickenManaPctTextBox.Enabled = false;
            this.chickenManaPctTextBox.Location = new System.Drawing.Point(396, 115);
            this.chickenManaPctTextBox.Name = "chickenManaPctTextBox";
            this.chickenManaPctTextBox.Size = new System.Drawing.Size(31, 20);
            this.chickenManaPctTextBox.TabIndex = 33;
            // 
            // chickenLifePctTextBox
            // 
            this.chickenLifePctTextBox.Enabled = false;
            this.chickenLifePctTextBox.Location = new System.Drawing.Point(396, 89);
            this.chickenLifePctTextBox.Name = "chickenLifePctTextBox";
            this.chickenLifePctTextBox.Size = new System.Drawing.Size(31, 20);
            this.chickenLifePctTextBox.TabIndex = 32;
            // 
            // chickenOnHostileTextBox
            // 
            this.chickenOnHostileTextBox.AutoSize = true;
            this.chickenOnHostileTextBox.Enabled = false;
            this.chickenOnHostileTextBox.Location = new System.Drawing.Point(396, 65);
            this.chickenOnHostileTextBox.Name = "chickenOnHostileTextBox";
            this.chickenOnHostileTextBox.Size = new System.Drawing.Size(113, 17);
            this.chickenOnHostileTextBox.TabIndex = 31;
            this.chickenOnHostileTextBox.Text = "Chichen on hostile";
            this.chickenOnHostileTextBox.UseVisualStyleBackColor = true;
            // 
            // chickenToTownCheckBox
            // 
            this.chickenToTownCheckBox.AutoSize = true;
            this.chickenToTownCheckBox.Enabled = false;
            this.chickenToTownCheckBox.Location = new System.Drawing.Point(396, 42);
            this.chickenToTownCheckBox.Name = "chickenToTownCheckBox";
            this.chickenToTownCheckBox.Size = new System.Drawing.Size(103, 17);
            this.chickenToTownCheckBox.TabIndex = 30;
            this.chickenToTownCheckBox.Text = "Chichen to town";
            this.chickenToTownCheckBox.UseVisualStyleBackColor = true;
            // 
            // enableChickenCheckBox
            // 
            this.enableChickenCheckBox.AutoSize = true;
            this.enableChickenCheckBox.Enabled = false;
            this.enableChickenCheckBox.Location = new System.Drawing.Point(396, 18);
            this.enableChickenCheckBox.Name = "enableChickenCheckBox";
            this.enableChickenCheckBox.Size = new System.Drawing.Size(101, 17);
            this.enableChickenCheckBox.TabIndex = 29;
            this.enableChickenCheckBox.Text = "Enable Chicken";
            this.enableChickenCheckBox.UseVisualStyleBackColor = true;
            this.enableChickenCheckBox.CheckedChanged += new System.EventHandler(this.enableChickenCheckBox_CheckedChanged);
            // 
            // goToTownCheckBox
            // 
            this.goToTownCheckBox.AutoSize = true;
            this.goToTownCheckBox.Enabled = false;
            this.goToTownCheckBox.Location = new System.Drawing.Point(220, 148);
            this.goToTownCheckBox.Name = "goToTownCheckBox";
            this.goToTownCheckBox.Size = new System.Drawing.Size(106, 17);
            this.goToTownCheckBox.TabIndex = 27;
            this.goToTownCheckBox.Text = "Auto Take Portal";
            this.goToTownCheckBox.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(257, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Town Portal Key";
            // 
            // fastExitLabel
            // 
            this.fastExitLabel.AutoSize = true;
            this.fastExitLabel.Location = new System.Drawing.Point(257, 103);
            this.fastExitLabel.Name = "fastExitLabel";
            this.fastExitLabel.Size = new System.Drawing.Size(68, 13);
            this.fastExitLabel.TabIndex = 25;
            this.fastExitLabel.Text = "Fast Exit Key";
            // 
            // openCubeLabel
            // 
            this.openCubeLabel.AutoSize = true;
            this.openCubeLabel.Location = new System.Drawing.Point(257, 77);
            this.openCubeLabel.Name = "openCubeLabel";
            this.openCubeLabel.Size = new System.Drawing.Size(82, 13);
            this.openCubeLabel.TabIndex = 24;
            this.openCubeLabel.Text = "Open Cube Key";
            // 
            // townPortalKeybindButton
            // 
            this.townPortalKeybindButton.Key = System.Windows.Forms.Keys.None;
            this.townPortalKeybindButton.Location = new System.Drawing.Point(200, 122);
            this.townPortalKeybindButton.Name = "townPortalKeybindButton";
            this.townPortalKeybindButton.Size = new System.Drawing.Size(50, 20);
            this.townPortalKeybindButton.TabIndex = 23;
            this.townPortalKeybindButton.Text = "None";
            this.townPortalKeybindButton.UseVisualStyleBackColor = true;
            this.townPortalKeybindButton.WaitingKeyPress = false;
            // 
            // fastExitKeybindButton
            // 
            this.fastExitKeybindButton.Key = System.Windows.Forms.Keys.None;
            this.fastExitKeybindButton.Location = new System.Drawing.Point(200, 96);
            this.fastExitKeybindButton.Name = "fastExitKeybindButton";
            this.fastExitKeybindButton.Size = new System.Drawing.Size(50, 20);
            this.fastExitKeybindButton.TabIndex = 22;
            this.fastExitKeybindButton.Text = "None";
            this.fastExitKeybindButton.UseVisualStyleBackColor = true;
            this.fastExitKeybindButton.WaitingKeyPress = false;
            // 
            // openCubeKeybindButton
            // 
            this.openCubeKeybindButton.Key = System.Windows.Forms.Keys.None;
            this.openCubeKeybindButton.Location = new System.Drawing.Point(200, 70);
            this.openCubeKeybindButton.Name = "openCubeKeybindButton";
            this.openCubeKeybindButton.Size = new System.Drawing.Size(50, 20);
            this.openCubeKeybindButton.TabIndex = 21;
            this.openCubeKeybindButton.Text = "None";
            this.openCubeKeybindButton.UseVisualStyleBackColor = true;
            this.openCubeKeybindButton.WaitingKeyPress = false;
            // 
            // openStashLabel
            // 
            this.openStashLabel.AutoSize = true;
            this.openStashLabel.Location = new System.Drawing.Point(257, 51);
            this.openStashLabel.Name = "openStashLabel";
            this.openStashLabel.Size = new System.Drawing.Size(84, 13);
            this.openStashLabel.TabIndex = 20;
            this.openStashLabel.Text = "Open Stash Key";
            // 
            // openStashKeybindButton
            // 
            this.openStashKeybindButton.Key = System.Windows.Forms.Keys.None;
            this.openStashKeybindButton.Location = new System.Drawing.Point(200, 44);
            this.openStashKeybindButton.Name = "openStashKeybindButton";
            this.openStashKeybindButton.Size = new System.Drawing.Size(50, 20);
            this.openStashKeybindButton.TabIndex = 19;
            this.openStashKeybindButton.Text = "None";
            this.openStashKeybindButton.UseVisualStyleBackColor = true;
            this.openStashKeybindButton.WaitingKeyPress = false;
            // 
            // revealActLabel
            // 
            this.revealActLabel.AutoSize = true;
            this.revealActLabel.Location = new System.Drawing.Point(257, 24);
            this.revealActLabel.Name = "revealActLabel";
            this.revealActLabel.Size = new System.Drawing.Size(81, 13);
            this.revealActLabel.TabIndex = 18;
            this.revealActLabel.Text = "Reveal Act Key";
            // 
            // revealActKeybindButton
            // 
            this.revealActKeybindButton.Key = System.Windows.Forms.Keys.None;
            this.revealActKeybindButton.Location = new System.Drawing.Point(200, 18);
            this.revealActKeybindButton.Name = "revealActKeybindButton";
            this.revealActKeybindButton.Size = new System.Drawing.Size(50, 20);
            this.revealActKeybindButton.TabIndex = 17;
            this.revealActKeybindButton.Text = "None";
            this.revealActKeybindButton.UseVisualStyleBackColor = true;
            this.revealActKeybindButton.WaitingKeyPress = false;
            // 
            // viewInventoryKeyLabel
            // 
            this.viewInventoryKeyLabel.AutoSize = true;
            this.viewInventoryKeyLabel.Location = new System.Drawing.Point(89, 356);
            this.viewInventoryKeyLabel.Name = "viewInventoryKeyLabel";
            this.viewInventoryKeyLabel.Size = new System.Drawing.Size(98, 13);
            this.viewInventoryKeyLabel.TabIndex = 16;
            this.viewInventoryKeyLabel.Text = "View Inventory Key";
            // 
            // viewInventoryKeybindButton
            // 
            this.viewInventoryKeybindButton.Key = System.Windows.Forms.Keys.None;
            this.viewInventoryKeybindButton.Location = new System.Drawing.Point(32, 350);
            this.viewInventoryKeybindButton.Name = "viewInventoryKeybindButton";
            this.viewInventoryKeybindButton.Size = new System.Drawing.Size(50, 20);
            this.viewInventoryKeybindButton.TabIndex = 14;
            this.viewInventoryKeybindButton.Text = "None";
            this.viewInventoryKeybindButton.UseVisualStyleBackColor = true;
            this.viewInventoryKeybindButton.WaitingKeyPress = false;
            // 
            // changeColorCheckBox
            // 
            this.changeColorCheckBox.AutoSize = true;
            this.changeColorCheckBox.Enabled = false;
            this.changeColorCheckBox.Location = new System.Drawing.Point(32, 304);
            this.changeColorCheckBox.Name = "changeColorCheckBox";
            this.changeColorCheckBox.Size = new System.Drawing.Size(90, 17);
            this.changeColorCheckBox.TabIndex = 13;
            this.changeColorCheckBox.Text = "Change Color";
            this.changeColorCheckBox.UseVisualStyleBackColor = true;
            // 
            // showSocketsCheckBox
            // 
            this.showSocketsCheckBox.AutoSize = true;
            this.showSocketsCheckBox.Enabled = false;
            this.showSocketsCheckBox.Location = new System.Drawing.Point(32, 280);
            this.showSocketsCheckBox.Name = "showSocketsCheckBox";
            this.showSocketsCheckBox.Size = new System.Drawing.Size(95, 17);
            this.showSocketsCheckBox.TabIndex = 12;
            this.showSocketsCheckBox.Text = "Show Sockets";
            this.showSocketsCheckBox.UseVisualStyleBackColor = true;
            // 
            // showRuneNumberCheckBox
            // 
            this.showRuneNumberCheckBox.AutoSize = true;
            this.showRuneNumberCheckBox.Enabled = false;
            this.showRuneNumberCheckBox.Location = new System.Drawing.Point(32, 256);
            this.showRuneNumberCheckBox.Name = "showRuneNumberCheckBox";
            this.showRuneNumberCheckBox.Size = new System.Drawing.Size(122, 17);
            this.showRuneNumberCheckBox.TabIndex = 11;
            this.showRuneNumberCheckBox.Text = "Show Rune Number";
            this.showRuneNumberCheckBox.UseVisualStyleBackColor = true;
            // 
            // showItemPriceCheckBox
            // 
            this.showItemPriceCheckBox.AutoSize = true;
            this.showItemPriceCheckBox.Enabled = false;
            this.showItemPriceCheckBox.Location = new System.Drawing.Point(32, 232);
            this.showItemPriceCheckBox.Name = "showItemPriceCheckBox";
            this.showItemPriceCheckBox.Size = new System.Drawing.Size(103, 17);
            this.showItemPriceCheckBox.TabIndex = 10;
            this.showItemPriceCheckBox.Text = "Show Item Price";
            this.showItemPriceCheckBox.UseVisualStyleBackColor = true;
            // 
            // showItemLevelCheckBox
            // 
            this.showItemLevelCheckBox.AutoSize = true;
            this.showItemLevelCheckBox.Enabled = false;
            this.showItemLevelCheckBox.Location = new System.Drawing.Point(32, 208);
            this.showItemLevelCheckBox.Name = "showItemLevelCheckBox";
            this.showItemLevelCheckBox.Size = new System.Drawing.Size(105, 17);
            this.showItemLevelCheckBox.TabIndex = 9;
            this.showItemLevelCheckBox.Text = "Show Item Level";
            this.showItemLevelCheckBox.UseVisualStyleBackColor = true;
            // 
            // showEthCheckBox
            // 
            this.showEthCheckBox.AutoSize = true;
            this.showEthCheckBox.Enabled = false;
            this.showEthCheckBox.Location = new System.Drawing.Point(32, 184);
            this.showEthCheckBox.Name = "showEthCheckBox";
            this.showEthCheckBox.Size = new System.Drawing.Size(95, 17);
            this.showEthCheckBox.TabIndex = 8;
            this.showEthCheckBox.Text = "Show Ethereal";
            this.showEthCheckBox.UseVisualStyleBackColor = true;
            // 
            // fastPortalCheckBox
            // 
            this.fastPortalCheckBox.AutoSize = true;
            this.fastPortalCheckBox.Enabled = false;
            this.fastPortalCheckBox.Location = new System.Drawing.Point(32, 137);
            this.fastPortalCheckBox.Name = "fastPortalCheckBox";
            this.fastPortalCheckBox.Size = new System.Drawing.Size(126, 17);
            this.fastPortalCheckBox.TabIndex = 7;
            this.fastPortalCheckBox.Text = "No Town Portal Anim";
            this.fastPortalCheckBox.UseVisualStyleBackColor = true;
            // 
            // fastTeleCheckBox
            // 
            this.fastTeleCheckBox.AutoSize = true;
            this.fastTeleCheckBox.Enabled = false;
            this.fastTeleCheckBox.Location = new System.Drawing.Point(32, 113);
            this.fastTeleCheckBox.Name = "fastTeleCheckBox";
            this.fastTeleCheckBox.Size = new System.Drawing.Size(88, 17);
            this.fastTeleCheckBox.TabIndex = 6;
            this.fastTeleCheckBox.Text = "Fast Teleport";
            this.fastTeleCheckBox.UseVisualStyleBackColor = true;
            // 
            // blockFlashCheckBox
            // 
            this.blockFlashCheckBox.AutoSize = true;
            this.blockFlashCheckBox.Enabled = false;
            this.blockFlashCheckBox.Location = new System.Drawing.Point(32, 88);
            this.blockFlashCheckBox.Name = "blockFlashCheckBox";
            this.blockFlashCheckBox.Size = new System.Drawing.Size(81, 17);
            this.blockFlashCheckBox.TabIndex = 5;
            this.blockFlashCheckBox.Text = "Block Flash";
            this.blockFlashCheckBox.UseVisualStyleBackColor = true;
            // 
            // viewInventoryHackCheckBox
            // 
            this.viewInventoryHackCheckBox.AutoSize = true;
            this.viewInventoryHackCheckBox.Location = new System.Drawing.Point(16, 327);
            this.viewInventoryHackCheckBox.Name = "viewInventoryHackCheckBox";
            this.viewInventoryHackCheckBox.Size = new System.Drawing.Size(137, 17);
            this.viewInventoryHackCheckBox.TabIndex = 4;
            this.viewInventoryHackCheckBox.Text = "View Inventory Hack: 3";
            this.viewInventoryHackCheckBox.UseVisualStyleBackColor = true;
            this.viewInventoryHackCheckBox.CheckedChanged += new System.EventHandler(this.viewInventoryHackCheckBox_CheckedChanged);
            // 
            // itemNameHackCheckBox
            // 
            this.itemNameHackCheckBox.AutoSize = true;
            this.itemNameHackCheckBox.Location = new System.Drawing.Point(16, 160);
            this.itemNameHackCheckBox.Name = "itemNameHackCheckBox";
            this.itemNameHackCheckBox.Size = new System.Drawing.Size(118, 17);
            this.itemNameHackCheckBox.TabIndex = 3;
            this.itemNameHackCheckBox.Text = "Item Name Hack: 1";
            this.itemNameHackCheckBox.UseVisualStyleBackColor = true;
            this.itemNameHackCheckBox.CheckedChanged += new System.EventHandler(this.itemNameHackCheckBox_CheckedChanged);
            // 
            // packetReceiveHackCheckBox
            // 
            this.packetReceiveHackCheckBox.AutoSize = true;
            this.packetReceiveHackCheckBox.Location = new System.Drawing.Point(16, 65);
            this.packetReceiveHackCheckBox.Name = "packetReceiveHackCheckBox";
            this.packetReceiveHackCheckBox.Size = new System.Drawing.Size(144, 17);
            this.packetReceiveHackCheckBox.TabIndex = 2;
            this.packetReceiveHackCheckBox.Text = "Packet Receive Hack: 1";
            this.packetReceiveHackCheckBox.UseVisualStyleBackColor = true;
            this.packetReceiveHackCheckBox.CheckedChanged += new System.EventHandler(this.packetReceiveHackCheckBox_CheckedChanged);
            // 
            // weatherHackCheckBox
            // 
            this.weatherHackCheckBox.AutoSize = true;
            this.weatherHackCheckBox.Location = new System.Drawing.Point(16, 42);
            this.weatherHackCheckBox.Name = "weatherHackCheckBox";
            this.weatherHackCheckBox.Size = new System.Drawing.Size(108, 17);
            this.weatherHackCheckBox.TabIndex = 1;
            this.weatherHackCheckBox.Text = "Weather Hack: 1";
            this.weatherHackCheckBox.UseVisualStyleBackColor = true;
            // 
            // lightHackCheckBox
            // 
            this.lightHackCheckBox.AutoSize = true;
            this.lightHackCheckBox.Location = new System.Drawing.Point(16, 18);
            this.lightHackCheckBox.Name = "lightHackCheckBox";
            this.lightHackCheckBox.Size = new System.Drawing.Size(90, 17);
            this.lightHackCheckBox.TabIndex = 0;
            this.lightHackCheckBox.Text = "Light Hack: 1";
            this.lightHackCheckBox.UseVisualStyleBackColor = true;
            // 
            // propertiesExpandButton
            // 
            this.propertiesExpandButton.ChildPanel = this.optionsTranslucentPanel;
            this.propertiesExpandButton.Expanded = true;
            this.propertiesExpandButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.propertiesExpandButton.Location = new System.Drawing.Point(2, 49);
            this.propertiesExpandButton.Name = "propertiesExpandButton";
            this.propertiesExpandButton.Size = new System.Drawing.Size(18, 16);
            this.propertiesExpandButton.TabIndex = 5;
            this.propertiesExpandButton.Text = " \\/";
            this.propertiesExpandButton.UseVisualStyleBackColor = true;
            // 
            // logExpandButton
            // 
            this.logExpandButton.ChildPanel = this.logTranslucentPanel;
            this.logExpandButton.Expanded = true;
            this.logExpandButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.logExpandButton.Location = new System.Drawing.Point(114, 557);
            this.logExpandButton.Name = "logExpandButton";
            this.logExpandButton.Size = new System.Drawing.Size(18, 16);
            this.logExpandButton.TabIndex = 4;
            this.logExpandButton.Text = " \\/";
            this.logExpandButton.UseVisualStyleBackColor = true;
            // 
            // logTranslucentPanel
            // 
            this.logTranslucentPanel.AutoScroll = true;
            this.logTranslucentPanel.BackColor = System.Drawing.Color.Transparent;
            this.logTranslucentPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.logTranslucentPanel.Controls.Add(this.logTextBox);
            this.logTranslucentPanel.Location = new System.Drawing.Point(133, 533);
            this.logTranslucentPanel.Name = "logTranslucentPanel";
            this.logTranslucentPanel.Size = new System.Drawing.Size(556, 40);
            this.logTranslucentPanel.TabIndex = 2;
            // 
            // logTextBox
            // 
            this.logTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.logTextBox.DetectUrls = false;
            this.logTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.logTextBox.Enabled = false;
            this.logTextBox.Font = new System.Drawing.Font("Tahoma", 8.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.logTextBox.ForeColor = System.Drawing.Color.SandyBrown;
            this.logTextBox.Location = new System.Drawing.Point(0, 0);
            this.logTextBox.Name = "logTextBox";
            this.logTextBox.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.logTextBox.ShortcutsEnabled = false;
            this.logTextBox.Size = new System.Drawing.Size(554, 38);
            this.logTextBox.TabIndex = 1;
            this.logTextBox.Text = "";
            this.logTextBox.WordWrap = false;
            // 
            // OverlayWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(800, 647);
            this.Controls.Add(this.optionsTranslucentPanel);
            this.Controls.Add(this.propertiesExpandButton);
            this.Controls.Add(this.logExpandButton);
            this.Controls.Add(this.itchyLabel);
            this.Controls.Add(this.logTranslucentPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OverlayWindow";
            this.ShowInTaskbar = false;
            this.Text = "OverlayWindow";
            this.TransparencyKey = System.Drawing.Color.MidnightBlue;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OverlayWindow_FormClosing);
            this.Load += new System.EventHandler(this.OverlayWindow_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OverlayWindow_Paint);
            this.optionsTranslucentPanel.ResumeLayout(false);
            this.settingsTabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.logTranslucentPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public ItchyControls.TransparentRichTextBox logTextBox;
        private ItchyControls.TranslucentPanel logTranslucentPanel;
        private System.Windows.Forms.Label itchyLabel;
        private ItchyControls.ExpandButton logExpandButton;
        public ItchyControls.ExpandButton propertiesExpandButton;
        private ItchyControls.TranslucentPanel optionsTranslucentPanel;
        private System.Windows.Forms.TabControl settingsTabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox lightHackCheckBox;
        private System.Windows.Forms.CheckBox weatherHackCheckBox;
        private System.Windows.Forms.CheckBox packetReceiveHackCheckBox;
        private System.Windows.Forms.CheckBox itemNameHackCheckBox;
        private System.Windows.Forms.CheckBox viewInventoryHackCheckBox;
        private System.Windows.Forms.CheckBox showEthCheckBox;
        private System.Windows.Forms.CheckBox fastPortalCheckBox;
        private System.Windows.Forms.CheckBox fastTeleCheckBox;
        private System.Windows.Forms.CheckBox blockFlashCheckBox;
        private System.Windows.Forms.CheckBox changeColorCheckBox;
        private System.Windows.Forms.CheckBox showSocketsCheckBox;
        private System.Windows.Forms.CheckBox showRuneNumberCheckBox;
        private System.Windows.Forms.CheckBox showItemPriceCheckBox;
        private System.Windows.Forms.CheckBox showItemLevelCheckBox;
        public ItchyControls.KeybindButton viewInventoryKeybindButton;
        private System.Windows.Forms.Label viewInventoryKeyLabel;
        public ItchyControls.KeybindButton revealActKeybindButton;
        private System.Windows.Forms.Label revealActLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label fastExitLabel;
        private System.Windows.Forms.Label openCubeLabel;
        public ItchyControls.KeybindButton townPortalKeybindButton;
        public ItchyControls.KeybindButton fastExitKeybindButton;
        public ItchyControls.KeybindButton openCubeKeybindButton;
        private System.Windows.Forms.Label openStashLabel;
        public ItchyControls.KeybindButton openStashKeybindButton;
        private System.Windows.Forms.CheckBox goToTownCheckBox;
        private System.Windows.Forms.Label chickenManaPctLabel;
        private System.Windows.Forms.Label chickenLifePctLabel;
        private System.Windows.Forms.TextBox chickenManaPctTextBox;
        private System.Windows.Forms.TextBox chickenLifePctTextBox;
        private System.Windows.Forms.CheckBox chickenOnHostileTextBox;
        private System.Windows.Forms.CheckBox chickenToTownCheckBox;
        private System.Windows.Forms.CheckBox enableChickenCheckBox;
        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Button applyButton;
        private System.Windows.Forms.Button refreshButton;
        private System.Windows.Forms.CheckBox infravisionHackCheckBox;
        private System.Windows.Forms.CheckBox hideItemCheckBox;
        private System.Windows.Forms.CheckBox hideCorpsesCheckBox;
        private System.Windows.Forms.CheckBox hideDyingCheckBox;
    }
}