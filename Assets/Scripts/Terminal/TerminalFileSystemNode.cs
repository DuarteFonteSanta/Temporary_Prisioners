using System.Collections.Generic;
using UnityEngine;

public class TerminalFileSystemNode
{
    public string name;
    public TerminalFileSystemNode parent;
    public Dictionary<string, TerminalFileSystemNode> children = new();
    public bool IsDirectory => children.Count > 0;

    public TerminalFileSystemNode(string name, TerminalFileSystemNode parent = null)
    {
        this.name = name;
        this.parent = parent;
    }
}
