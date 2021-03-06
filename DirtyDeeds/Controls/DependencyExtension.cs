﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DD
{
    public enum DependencyType
    {
        OnEnable = 0,
        Complex = 1,
    }

    class ChildInfo
    {
        public Control Child { get; set; }
        public DependencyType DependencyType { get; set; }
    }

    public static class DependencyExtension
    {
        private static Dictionary<Control, List<ChildInfo>> childsForControls = new Dictionary<Control, List<ChildInfo>>();

        public static void SetDependency(this Control control, Control parent, DependencyType type = DependencyType.Complex)
        {
            if (!childsForControls.ContainsKey(parent))
            {
                childsForControls[parent] = new List<ChildInfo>();

                parent.EnabledChanged += DependencyAdded;
                if (parent is CheckBox)
                    (parent as CheckBox).CheckedChanged += DependencyAdded;
            }

            childsForControls[parent].Add(new ChildInfo { Child = control, DependencyType = type });
        }

        private static void DependencyAdded(object sender, EventArgs e)
        {
            var parent = sender as Control;
            if (!childsForControls.ContainsKey(parent))
                return;

            foreach (var ctrl in childsForControls[parent])
            {
                if (ctrl.DependencyType == DependencyType.OnEnable)
                    ctrl.Child.Enabled = parent.Enabled;
                else if (ctrl.DependencyType == DependencyType.Complex)
                    ctrl.Child.Enabled = parent.Enabled &&
                        (parent is CheckBox ? (parent as CheckBox).Checked : true);
            }
        }

        public static bool Active(this CheckBox control)
        {
            return control.Enabled && control.Checked;
        }
    }
}
