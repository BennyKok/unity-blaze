using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blaze.Dialogue
{
    public class BlazeDialogueManager
    {
        public int selectedLang = 0;

        private BlazeDialogueSettings settings;

        private static BlazeDialogueManager _Instance;

        public static BlazeDialogueManager Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new BlazeDialogueManager();

                return _Instance;
            }
        }

        internal BlazeDialogueManager()
        {
            settings = Resources.Load<BlazeDialogueSettings>(BlazeDialogueSettings.settingsAssetName);
            // Debug.Log(settings.languageDefinition);
        }

    }
}