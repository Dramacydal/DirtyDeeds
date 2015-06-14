using DD.Game.Enums;
using DD.Game.Settings;
using DD.Tools;
using DirtyDeedsControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DD
{
    public partial class SettingsForm : Form
    {
        protected DirtyDeeds DD = null;

        public SettingsForm(DirtyDeeds dd) : base()
        {
            InitializeComponent();
            InitializeDependencies();

            this.DD = dd;
            SetupFromGameSettings(DD.Settings);
        }

        private void InitializeDependencies()
        {
            blockFlashCheckBox.SetDependency(packetReceiveHackCheckBox);
            fastTeleportCheckBox.SetDependency(packetReceiveHackCheckBox);
            noTownPortalAnimCheckBox.SetDependency(packetReceiveHackCheckBox);
            itemTrackerCheckBox.SetDependency(packetReceiveHackCheckBox);

            showEtherealCheckBox.SetDependency(itemNameHackCheckBox);
            showItemLevelCheckBox.SetDependency(itemNameHackCheckBox);
            showItemPriceCheckBox.SetDependency(itemNameHackCheckBox);
            showRuneNumberCheckBox.SetDependency(itemNameHackCheckBox);
            showSocketsCheckBox.SetDependency(itemNameHackCheckBox);
            showItemCodeCheckBox.SetDependency(itemNameHackCheckBox);
            changeColorCheckBox.SetDependency(itemNameHackCheckBox);

            viewInventoryKeybindButton.SetDependency(viewInventoryHackCheckBox);

            hideCorpsesCheckBox.SetDependency(infravisionHackCheckBox);
            hideDyingCheckBox.SetDependency(infravisionHackCheckBox);
            hideItemsCheckBox.SetDependency(infravisionHackCheckBox);

            autoTakePortalCheckBox.SetDependency(packetReceiveHackCheckBox);

            enableChickenCheckBox.SetDependency(packetReceiveHackCheckBox);
            chickenToTownCheckBox.SetDependency(enableChickenCheckBox);
            chickenOnHostilityCheckBox.SetDependency(enableChickenCheckBox);
            chickenLifePctTextBox.SetDependency(enableChickenCheckBox);
            chickenManaPctTextBox.SetDependency(enableChickenCheckBox);

            enablePickitCheckBox.SetDependency(itemTrackerCheckBox);
            useTelekinesisCheckBox.SetDependency(enablePickitCheckBox);
            enableTelepickCheckBox.SetDependency(enablePickitCheckBox);
            teleBackCheckBox.SetDependency(enableTelepickCheckBox);
            pickInTownCheckBox.SetDependency(enablePickitCheckBox);
            reactivatePickitKeyBindButton.SetDependency(enablePickitCheckBox);

            logRunesCheckBox.SetDependency(itemTrackerCheckBox);
            logSetsCheckBox.SetDependency(itemTrackerCheckBox);
            logUniquesCheckBox.SetDependency(itemTrackerCheckBox);
            logItemsCheckBox.SetDependency(itemTrackerCheckBox);
        }

        private void SetupFromGameSettings(GameSettings settings)
        {
            lightHackCheckBox.Checked = settings.LightHack.Enabled;
            weatherHackCheckBox.Checked = settings.WeatherHack.Enabled;

            packetReceiveHackCheckBox.Checked = settings.ReceivePacketHack.Enabled;
            blockFlashCheckBox.Checked = settings.ReceivePacketHack.BlockFlash.Enabled;
            fastTeleportCheckBox.Checked = settings.ReceivePacketHack.FastTele.Enabled;
            noTownPortalAnimCheckBox.Checked = settings.ReceivePacketHack.NoTownPortalAnim.Enabled;
            itemTrackerCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.Enabled;
            enablePickitCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.EnablePickit.Enabled;
            useTelekinesisCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.UseTelekinesis.Enabled;
            enableTelepickCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.EnableTelepick.Enabled;
            teleBackCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.TeleBack.Enabled;
            pickInTownCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.TownPick.Enabled;
            logRunesCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.LogRunes.Enabled;
            logSetsCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.LogSets.Enabled;
            logUniquesCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.LogUniques.Enabled;
            logItemsCheckBox.Checked = settings.ReceivePacketHack.ItemTracker.LogItems.Enabled;
            reactivatePickitKeyBindButton.Key = settings.ReceivePacketHack.ItemTracker.ReactivatePickit.Key;

            itemNameHackCheckBox.Checked = settings.ItemNameHack.Enabled;
            showEtherealCheckBox.Checked = settings.ItemNameHack.ShowEth.Enabled;
            showItemLevelCheckBox.Checked = settings.ItemNameHack.ShowItemLevel.Enabled;
            showItemPriceCheckBox.Checked = settings.ItemNameHack.ShowItemPrice.Enabled;
            showRuneNumberCheckBox.Checked = settings.ItemNameHack.ShowRuneNumber.Enabled;
            showSocketsCheckBox.Checked = settings.ItemNameHack.ShowSockets.Enabled;
            showItemCodeCheckBox.Checked = settings.ItemNameHack.ShowItemCode.Enabled;
            changeColorCheckBox.Checked = settings.ItemNameHack.ChangeItemColor.Enabled;

            viewInventoryHackCheckBox.Checked = settings.ViewInventory.Enabled;
            viewInventoryKeybindButton.Key = settings.ViewInventory.ViewInventoryKey;

            infravisionHackCheckBox.Checked = settings.Infravision.Enabled;
            hideCorpsesCheckBox.Checked = settings.Infravision.HideCorpses.Enabled;
            hideDyingCheckBox.Checked = settings.Infravision.HideDying.Enabled;
            hideItemsCheckBox.Checked = settings.Infravision.HideItems.Enabled;

            revealActKeybindButton.Key = settings.RevealAct.Key;
            openStashKeybindButton.Key = settings.OpenStash.Key;
            openCubeKeybindButton.Key = settings.OpenCube.Key;
            fastExitKeybindButton.Key = settings.FastExit.Key;
            townPortalKeybindButton.Key = settings.FastPortal.Key;
            autoTakePortalCheckBox.Checked = settings.GoToTownAfterPortal.Enabled;

            nextLevelKeybindButton.Key = settings.AutoteleNext.Key;
            miscPointKeybindButton.Key = settings.AutoteleMisc.Key;
            wpKeybindButton.Key = settings.AutoteleWP.Key;
            prevLevelKeybindButton.Key = settings.AutotelePrev.Key;

            enableChickenCheckBox.Checked = settings.Chicken.Enabled;
            chickenToTownCheckBox.Checked = settings.Chicken.ChickenToTown.Enabled;
            chickenOnHostilityCheckBox.Checked = settings.Chicken.ChickenOnHostility.Enabled;
            chickenLifePctTextBox.Text = settings.Chicken.ChickenLifePercent.ToString();
            chickenManaPctTextBox.Text = settings.Chicken.ChickenManaPercent.ToString();
        }

        public GameSettings ProduceSettings()
        {
            var settings = new GameSettings();

            settings.LightHack.Enabled = lightHackCheckBox.Checked;
            settings.WeatherHack.Enabled = weatherHackCheckBox.Checked;

            settings.ReceivePacketHack.Enabled = packetReceiveHackCheckBox.Checked;
            settings.ReceivePacketHack.BlockFlash.Enabled = blockFlashCheckBox.Checked;
            settings.ReceivePacketHack.FastTele.Enabled = fastTeleportCheckBox.Checked;
            settings.ReceivePacketHack.NoTownPortalAnim.Enabled = noTownPortalAnimCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.Enabled = itemTrackerCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.EnablePickit.Enabled = enablePickitCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.UseTelekinesis.Enabled = useTelekinesisCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.EnableTelepick.Enabled = enableTelepickCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.TownPick.Enabled = pickInTownCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.TeleBack.Enabled = teleBackCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.LogRunes.Enabled = logRunesCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.LogSets.Enabled = logSetsCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.LogUniques.Enabled = logUniquesCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.LogItems.Enabled = logItemsCheckBox.Checked;
            settings.ReceivePacketHack.ItemTracker.ReactivatePickit.Key = reactivatePickitKeyBindButton.Key;

            settings.ItemNameHack.Enabled = itemNameHackCheckBox.Checked;
            settings.ItemNameHack.ShowEth.Enabled = showEtherealCheckBox.Checked;
            settings.ItemNameHack.ShowItemLevel.Enabled = showItemLevelCheckBox.Checked;
            settings.ItemNameHack.ShowItemPrice.Enabled = showItemPriceCheckBox.Checked;
            settings.ItemNameHack.ShowRuneNumber.Enabled = showRuneNumberCheckBox.Checked;
            settings.ItemNameHack.ShowSockets.Enabled = showSocketsCheckBox.Checked;
            settings.ItemNameHack.ShowItemCode.Enabled = showItemCodeCheckBox.Checked;
            settings.ItemNameHack.ChangeItemColor.Enabled = changeColorCheckBox.Checked;

            settings.ViewInventory.Enabled = viewInventoryHackCheckBox.Checked;
            settings.ViewInventory.ViewInventoryKey = viewInventoryKeybindButton.Key;

            settings.Infravision.Enabled = infravisionHackCheckBox.Checked;
            settings.Infravision.HideCorpses.Enabled = hideCorpsesCheckBox.Checked;
            settings.Infravision.HideDying.Enabled = hideDyingCheckBox.Checked;
            settings.Infravision.HideItems.Enabled = hideItemsCheckBox.Checked;

            settings.RevealAct.Key = revealActKeybindButton.Key;
            settings.OpenStash.Key = openStashKeybindButton.Key;
            settings.OpenCube.Key = openCubeKeybindButton.Key;
            settings.FastExit.Key = fastExitKeybindButton.Key;
            settings.FastPortal.Key = townPortalKeybindButton.Key;
            settings.GoToTownAfterPortal.Enabled = autoTakePortalCheckBox.Checked;

            settings.AutoteleNext.Key = nextLevelKeybindButton.Key;
            settings.AutoteleMisc.Key = miscPointKeybindButton.Key;
            settings.AutoteleWP.Key = wpKeybindButton.Key;
            settings.AutotelePrev.Key = prevLevelKeybindButton.Key;

            settings.Chicken.Enabled = enableChickenCheckBox.Checked;
            settings.Chicken.ChickenToTown.Enabled = chickenToTownCheckBox.Checked;
            settings.Chicken.ChickenOnHostility.Enabled = chickenOnHostilityCheckBox.Checked;

            try
            {
                var val = Convert.ToDouble(chickenLifePctTextBox.Text);
                settings.Chicken.ChickenLifePercent = val;
            }
            catch { }

            try
            {
                var val = Convert.ToDouble(chickenManaPctTextBox.Text);
                settings.Chicken.ChickenManaPercent = val;
            }
            catch { }

            return settings;
        }

        private int GetHackCost()
        {
            int cost = 0;
            if (lightHackCheckBox.Checked)
                cost += HackSettings.Cost;
            if (weatherHackCheckBox.Checked)
                cost += HackSettings.Cost;
            if (packetReceiveHackCheckBox.Checked)
                cost += PacketReceivedHackSettings.Cost;
            if (itemNameHackCheckBox.Checked)
                cost += ItemNameHackSettings.Cost;
            if (viewInventoryHackCheckBox.Checked)
                cost += ViewInventoryHackSettings.Cost;
            if (infravisionHackCheckBox.Checked)
                cost += InfravisionHackSettings.Cost;

            return cost;
        }

        private bool ValidateSettings()
        {
            return GetHackCost() <= 4;
        }


        static List<Control> GetAllControls(Control start)
        {
            var ret = new List<Control>();

            if (start.Controls != null)
            {
                foreach (Control control in start.Controls)
                    ret.AddRange(GetAllControls(control));

                ret.AddRange(start.Controls.Cast<Control>());
            }

            return ret;
        }


        public bool HandleMessage(Keys key, MessageEvent mEvent)
        {
            var t = this.GetType();

            var changed = false;
            var kbs = GetAllControls(this).Where(it => it is KeybindButton);
            foreach (KeybindButton f in kbs)
            {
                if (f.WaitingKeyPress)
                {
                    changed = true;
                    if (key == Keys.Escape)
                        f.Reset();
                    else
                        f.Key = key;

                    foreach (KeybindButton f2 in kbs)
                    {
                        if (f2.Key == key && f2 != f)
                            f2.Key = Keys.None;
                    }
                }
            }

            return !changed;
        }

        private void reloadItemsIniButton_Click(object sender, EventArgs e)
        {
            try
            {
                DD.ItemProcessingSettings.Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (ValidateSettings())
                this.DialogResult = DialogResult.OK;
            else
                MessageBox.Show("Too many hacks selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
             if (ValidateSettings())
             {
                 DD.Settings = ProduceSettings();
                 DD.ApplySettings();
             }
             else
                 MessageBox.Show("Too many hacks selected", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
