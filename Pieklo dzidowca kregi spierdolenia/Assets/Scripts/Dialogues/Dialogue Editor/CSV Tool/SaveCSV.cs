using System;
using System.IO;
using UnityEngine;
using jbzdy.DialogueSystem.SO;
using System.Collections.Generic;
using jbzdy.DialogueSystem.Enums;

namespace jbzdy.DialogueSystem.CSV
{
    public class SaveCSV
    {
        private readonly string csvDirectoryName = "Resources/Dialogue/CSV File";
        private readonly string csvFileName = "DialogueCSV_Save.csv";
        private readonly string csvSeparator = ",";
        private readonly string idName = "Guid ID";
        private List<string> csvHeader;

        public void Save()
        {
            List<DialogueContainerSO> dialogueContainers = Helper.FindAllObjectFromResources<DialogueContainerSO>();

            CreateFile();

            foreach (DialogueContainerSO dialogueContainer in dialogueContainers)
            {
                foreach (DialogueNodeData nodeData in dialogueContainer.DialogueNodeDatas)
                {
                    List<string> texts = new List<string>();

                    texts.Add(nodeData.NodeGuid);

                    foreach (LanguageType languageType in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
                    {
                        string tmp = nodeData.TextLanguages.Find(language => language.LanguageType == languageType).LanguageGenericType.Replace("\"", "\"\"");
                        texts.Add($"\"{tmp}\"");
                    }

                    AppendToFile(texts);

                    foreach (DialogueNodePort nodePorts in nodeData.DialogueNodePorts)
                    {
                        texts = new List<string>();

                        texts.Add(nodePorts.PortGuid);

                        foreach (LanguageType languageType in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
                        {
                            string tmp = nodePorts.TextLanguages.Find(language => language.LanguageType == languageType).LanguageGenericType.Replace("\"", "\"\"");
                            texts.Add($"\"{tmp}\"");
                        }

                        AppendToFile(texts);
                    }
                }
            }
        }

        private void AppendToFile(List<string> strings)
        {
            using (StreamWriter sw = File.AppendText(GetFilePath()))
            {
                string finalString = "";
                foreach (string text in strings)
                {
                    if (finalString != "")
                    {
                        finalString += csvSeparator;
                    }
                    finalString += text;
                }

                sw.WriteLine(finalString);
            }
        }

        private void CreateFile()
        {
            VerifDirectory();
            MakeHeader();
            using (StreamWriter sw = File.CreateText(GetFilePath()))
            {
                string finalString = "";
                foreach (string header in csvHeader)
                {
                    if (finalString != "")
                    {
                        finalString += csvSeparator;
                    }
                    finalString += header;
                }

                sw.WriteLine(finalString);
            }
        }

        private void MakeHeader()
        {
            List<string> headerText = new List<string>();
            headerText.Add(idName);

            foreach (LanguageType language in (LanguageType[])Enum.GetValues(typeof(LanguageType)))
            {
                headerText.Add(language.ToString());
            }

            csvHeader = headerText;
        }

        private void VerifDirectory()
        {
            string directory = GetDirectoryPath();

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private string GetDirectoryPath()
        {
            return $"{Application.dataPath}/{csvDirectoryName}";
        }

        private string GetFilePath()
        {
            return $"{GetDirectoryPath()}/{csvFileName}";
        }
    }
}
