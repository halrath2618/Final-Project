using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class DialogueLine
    {
        public DL_Speaker_Data speakerData;
        public DL_Dialogue_Data dialogueData;
        public DL_Command_Data commandData;

        public bool hasDialogue => dialogueData != null;
        public bool hasCommands => commandData != null;
        public bool hasSpeaker => speakerData != null;//speaker != string.Empty;

        public DialogueLine (string speaker, string dialogue, string commands)
        {
            this.speakerData = (string.IsNullOrWhiteSpace(speaker) ? null : new DL_Speaker_Data(speaker));
            this.dialogueData = (string.IsNullOrWhiteSpace(dialogue) ? null : new DL_Dialogue_Data(dialogue));
            this.commandData = (string.IsNullOrWhiteSpace(commands) ? null : new DL_Command_Data(commands));
        }
    }
}
