using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

[GlobalClass]
public partial class Computer : Node
{
    [ExportGroup("sys info")]
    [Export]
    string sys_name;
    [Export]
    string os_name;
    [Export]
    int storage_space;
    [Export]
    string ram_name;
    [Export]
    int ram_capacity;
    [Export]
    string processor_name;
    [Export]
    int processor_capacity;
}

public class FileSys
{
    const String PathStart = "#";

    int max_storage;
    int used_space;

    Pointer active_dir;

    dir root = new dir();

    public FileSys() { }

    /////////////////////
    /// utility funcs ///
    /////////////////////

    private Pointer ReadPath(String _Path)
    {
        String[] PathArr = (PathStart + _Path).Split('/');
        Pointer current;
        switch (PathArr[0])
        {
            case PathStart:
                current = &root
                break;
        }
       
    }

    ////////////////////
    /// node classes ///
    ////////////////////

    private class node
    {
        string _name;
        dir parent;
    }

    private class dir : node
    {
        Dictionary<String, node> children;
        public dir()
        {
            children = new Dictionary<String, node>();
        }

    }

    private class file : node
    {
        public enum format { BASIC, TXT, PRGM, CONF }
        int size;
        object data;
    }
}

public class CustomException : Exception
{
    public CustomException(string message) : base(message) { }
}