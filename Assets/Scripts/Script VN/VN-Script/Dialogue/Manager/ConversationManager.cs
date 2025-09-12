using CHARACTERS;
using COMMANDS;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace DIALOGUE
{
    public class ConversationManager
    {
        private DialogueSystem dialogueSystem => DialogueSystem.instance;
        private Coroutine process = null;
        public bool isRunning => process != null;

        public TextArchitect architect = null;
        private bool userPrompt = false;

        public ConversationManager(TextArchitect architect)
        {
            this.architect = architect;
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }

        private void OnUserPrompt_Next()
        {
            userPrompt = true;
        }

        public Coroutine StartConversation(List<string> conversation)
        {
            StopConversation();
            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));

            return process;
        }

        public void StopConversation()
        {
            if (!isRunning)
            {
                return;
            }

            dialogueSystem.StopCoroutine(process);
            process = null;
        }

        IEnumerator RunningConversation(List<string> conversation)
        {
            for (int i = 0; i < conversation.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(conversation[i]))
                    continue;
                DialogueLine line = DialogueParser.Parse(conversation[i]);

                if (line.hasDialogue)
                {
                    yield return Line_RunDialogue(line);
                }
                if (line.hasCommands)
                {
                    yield return Line_RunCommands(line);
                }
                if (line.hasDialogue)
                    //wait for user input
                    yield return WaitForUserInput();
            }
        }
        IEnumerator Line_RunDialogue(DialogueLine line)
        {
            if (line.hasSpeaker)
            {
                HandleSpeakerLogic(line.speakerData);
            }

            //build
            yield return BuildLineSegments(line.dialogueData);


        }

        private void HandleSpeakerLogic(DL_Speaker_Data speakerData)
        {
            bool characterMustBeCreated = (speakerData.makeCharacterEnter || speakerData.isCastingPosition || speakerData.isCastingExpressions);

            Character character = CharacterManager.instance.GetCharacter(speakerData.name, characterMustBeCreated);

            if (speakerData.makeCharacterEnter && (!character.isVisible && !character.isRevealing))
            {

                character.Show();

            }

            dialogueSystem.ShowSpeakerName(speakerData.displayName);

            DialogueSystem.instance.ApplySpeakerDataToDialogueContainer(speakerData.name);

            if (speakerData.isCastingPosition)
            {
                character.MoveToPosition(speakerData.castPosition);
            }

            if (speakerData.isCastingExpressions)
            {
                foreach (var ce in speakerData.CastExpressions)
                {
                    character.OnReceiveCastingExpression(ce.layer, ce.expression);
                }
            }

        }

        IEnumerator Line_RunCommands(DialogueLine line)
        {
            List<DL_Command_Data.Command> commands = line.commandData.commands;
            foreach (DL_Command_Data.Command command in commands)
            {
                if (command.waitForCompletion || command.name == "wait")
                    yield return CommandManager.instance.Execute(command.name, command.arguments);
                else
                    CommandManager.instance.Execute(command.name, command.arguments);
            }
            yield return null;
        }

        IEnumerator BuildLineSegments(DL_Dialogue_Data line)
        {
            for (int i = 0; i < line.segments.Count; i++)
            {
                DL_Dialogue_Data.DialogueSegment segment = line.segments[i];

                yield return WaitForDialogueSegmentSignalToBeTriggered(segment);
                yield return BuildDialogue(segment.dialogue, segment.appendText);
            }
        }

        IEnumerator WaitForDialogueSegmentSignalToBeTriggered(DL_Dialogue_Data.DialogueSegment segment)
        {
            switch (segment.startSignal)
            {
                case DL_Dialogue_Data.DialogueSegment.StartSignal.C:
                case DL_Dialogue_Data.DialogueSegment.StartSignal.A:
                    yield return WaitForUserInput();
                    break;
                case DL_Dialogue_Data.DialogueSegment.StartSignal.WC:
                case DL_Dialogue_Data.DialogueSegment.StartSignal.WA:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;
                default:
                    break;
            }
        }

        IEnumerator BuildDialogue(string dialogue, bool append = false)
        {
            if (!append)
                architect.Build(dialogue);
            else
                architect.Append(dialogue);

            while (architect.isBuilding)
            {

                if (userPrompt)
                {
                    if (!architect.hurryUp)
                        architect.hurryUp = true;
                    else
                        architect.ForceComplete();

                    userPrompt = false;
                }
                yield return null;
            }
        }

        IEnumerator WaitForUserInput()
        {
            dialogueSystem.prompt.Show();


            while (!userPrompt)
                yield return null;

            dialogueSystem.prompt.Hide();

            userPrompt = false;
        }
    }
}
