﻿using ColossalFramework.UI;
using CustomizeItExtended.GUI;
using CustomizeItExtended.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CustomizeItExtended.Extensions
{
    public static class BuildingInfoExtensions
    {
        public static Properties GetOriginalProperties(this BuildingInfo info)
        {
            return CustomizeItExtendedTool.instance.OriginalData.TryGetValue(info.name, out Properties props) ? props : null;
        }

        public static void LoadProperties(this BuildingInfo info, Properties props)
        {
            var customFields = props.GetType().GetFields();

            var originalFields = info.m_buildingAI.GetType().GetFields();

            var namedFields = customFields.ToDictionary(customField => customField.Name);

            foreach (var originalField in originalFields)
            {
                try
                {
                    if (namedFields.TryGetValue(originalField.Name, out FieldInfo fieldInfo))
                    {
                        originalField.SetValue(info.m_buildingAI, fieldInfo.GetValue(props));
                    }
                }
                catch (Exception e)
                {
                    DebugOutputPanel.AddMessage(ColossalFramework.Plugins.PluginManager.MessageType.Error, $"[Customize It! Extended] Failed to Load Properties. {e.Message} - {e.StackTrace}");
                }
            }
        }

        public static Properties GetProperties(this BuildingInfo info)
        {
            return new Properties(info);
        }

        public static UiPanelWrapper GenerateCustomizeItExtendedPanel(this BuildingInfo info)
        {
            CustomizeItExtendedTool.instance.CurrentSelectedBuilding = info;
            UiUtils.DeepDestroy(UIView.Find("CustomizeItExtendedPanelWrapper"));

            return UIView.GetAView().AddUIComponent(typeof(UiPanelWrapper)) as UiPanelWrapper;
        }
    }
}