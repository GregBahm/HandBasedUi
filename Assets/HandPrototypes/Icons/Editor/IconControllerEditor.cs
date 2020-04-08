using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace RemoteAssist.UI
{
    [CustomEditor(typeof(IconController))]
    public class IconControllerEditor : Editor
    {
        private static string[] options;
        private static Dictionary<string, IconController.IconKey> mappingTable;
        private static Dictionary<IconController.IconKey, int> remappingTable;

        int selected;

        static IconControllerEditor()
        {
            mappingTable = IconController.IconsMap.ToDictionary(item => item.Key.ToString(), item => item.Key);
            List<string> optionsList = mappingTable.Keys.ToList();
            optionsList.Sort();
            options = optionsList.ToArray();
            remappingTable = CreateRemappingTable();
        }

        private static Dictionary<IconController.IconKey, int> CreateRemappingTable()
        {
            Dictionary<IconController.IconKey, int> ret = new Dictionary<IconController.IconKey, int>();
            for (int i = 0; i < options.Length; i++)
            {
                IconController.IconKey key = mappingTable[options[i]];
                ret.Add(key, i);
            }
            return ret;
        }

        private void OnEnable()
        {
            IconController controller = (IconController)target;
            selected = remappingTable[controller.Icon];
        }


        public override void OnInspectorGUI()
        {
            IconController controller = (IconController)target;
            selected = EditorGUILayout.Popup("Icon", selected, options);
            controller.Icon = mappingTable[options[selected]];
        }
    }
}
