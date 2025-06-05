using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ComputerTerminal : TooltipObject
{
    [Header("UI References")]
    [SerializeField] private GameObject terminalPanelPrefab;
    [SerializeField] private GameObject commandLineContainer;
    [SerializeField] private GameObject responsePrefab;
    [SerializeField] private GameObject directoryPrefab;

    [Header("Terminal Settings")]
    [SerializeField] private string diskName = "DISK";
    [SerializeField] private int maxLineCount = 8;
    [SerializeField] private int terminalKey = 1234;
    [SerializeField] private EncryptedDoor encryptedDoor;


    private GameObject terminalInstance;


    private TerminalFileSystemNode root;
    private TerminalFileSystemNode current;
    private readonly List<GameObject> spawnedLines = new();
    private PlayerController playerController;

    private void Start()
    {

    }

    private void SetupFileSystem()
    {
        root = new TerminalFileSystemNode("root");
        current = root;

        var dir1 = new TerminalFileSystemNode("Directory_1", root);
        var dir2 = new TerminalFileSystemNode("Directory_2", root);

        root.children.Add(dir1.name, dir1);
        root.children.Add(dir2.name, dir2);

        var subDir = new TerminalFileSystemNode("SubDirectory", dir1);
        dir1.children.Add(subDir.name, subDir);
    }

    private void SpawnNewInputLine()
    {
        GameObject userLine = Instantiate(responsePrefab, commandLineContainer.transform);
        spawnedLines.Add(userLine);

        TMP_InputField inputField = userLine.GetComponentInChildren<TMP_InputField>();
        TextMeshProUGUI promptText = userLine.GetComponentInChildren<TextMeshProUGUI>();

        if (inputField == null || promptText == null)
        {
            Debug.LogError("responsePrefab must contain both TMP_InputField and TMP_Text!");
            return;
        }

        promptText.text = $"{diskName}:{GetCurrentPath()}$ ";

        // Handle when user presses enter
        inputField.onSubmit.AddListener((value) =>
        {
            OnPlayerEnteredCommand(value, userLine, promptText, inputField);
        });

        // Automatically focus new line
        EventSystem.current.SetSelectedGameObject(inputField.gameObject);
        inputField.ActivateInputField();
    }

    private void OnPlayerEnteredCommand(string input, GameObject userLine, TextMeshProUGUI promptText, TMP_InputField inputField)
    {
        input = input.Trim();
        if (string.IsNullOrEmpty(input)) return;

        inputField.interactable = false;
        inputField.textComponent.text = input;

        string result = ExecuteCommand(input);

        GameObject outputLine = Instantiate(directoryPrefab, commandLineContainer.transform);
        TextMeshProUGUI outputInput = outputLine.GetComponentInChildren<TextMeshProUGUI>();
        if (outputInput != null)
            outputInput.text = result;
        else
            Debug.LogWarning("Directory prefab is missing TMP_InputField!");
        spawnedLines.Add(outputLine);

        TrimCommandLines();

        if (input != "clear") // `clear` spawns no new input
            SpawnNewInputLine();
    }

    private void TrimCommandLines()
    {
        while (spawnedLines.Count > maxLineCount)
        {
            Destroy(spawnedLines[0]);
            spawnedLines.RemoveAt(0);
        }
    }

    public string ExecuteCommand(string input)
    {
        string[] parts = input.Trim().Split(' ');
        string command = string.Join(" ", parts[..Math.Min(2, parts.Length)]);
        string[] args = parts.Length > 2 ? parts[2..] : Array.Empty<string>();

        switch (command)
        {
            case "ls":
                return string.Join(" ", current.children.Keys);
            case "cd":
                return ChangeDirectory(args);
            case "pwd":
                return GetCurrentPath();
            case "clear":
                ClearTerminal();
                return "";
            case "open door":
                return ExecuteOpenDoor(args);
            case "close":
                CloseTerminal();
                return "";
            default:
                return $"Command not found: {command}";
        }
    }

    private string ExecuteOpenDoor(string[] args)
    {
        if (args.Length == 0)
            return "Usage: Open Door <code>";

        if (!int.TryParse(args[0], out int code))
            return "Invalid code format. Use a number.";

        if (code != terminalKey)
            return "Access denied: You cannot use another terminal's key.";

        if (encryptedDoor == null)
            return "No door connected.";

        return encryptedDoor.UnlockDoor(code);
    }

    private string ChangeDirectory(string[] args)
    {
        if (args.Length == 0)
            return "cd: missing argument";

        string target = args[0];

        if (target == "..")
        {
            if (current.parent != null)
            {
                current = current.parent;
                return "";
            }
        }

        if (current.children.TryGetValue(target, out TerminalFileSystemNode newDir))
        {
            current = newDir;
            return "";
        }

        return $"cd: no such directory: {target}";
    }

    private string GetCurrentPath()
    {
        List<string> parts = new();
        TerminalFileSystemNode node = current;

        while (node != null)
        {
            parts.Insert(0, node.name);
            node = node.parent;
        }

        return "/" + string.Join("/", parts);
    }

    private void ClearTerminal()
    {
        foreach (GameObject line in spawnedLines)
        {
            Destroy(line);
        }
        spawnedLines.Clear();

        // After clear, spawn fresh input
        SpawnNewInputLine();
    }

    public override void Interact(PlayerController player)
    {
        if (playerController == null)
        {
            player.CanMove = false;
            playerController = player;
            terminalPanelPrefab.SetActive(true);
            terminalInstance = Instantiate(terminalPanelPrefab, player.CanvasHandler.transform);
            commandLineContainer = terminalInstance.transform.Find("CommandLineContainer").gameObject;
            SetupFileSystem();
            SpawnNewInputLine();
        }
    }

    public void CloseTerminal()
    {
        playerController.CanMove = true;
        terminalPanelPrefab.SetActive(false);
        commandLineContainer = null;
        playerController = null;
        Destroy(terminalInstance);
    }
}
