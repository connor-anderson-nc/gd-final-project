using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

public static ref struct pointers
{
     public ref FileSys.node active_dir = ref FileSys.root;
}


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

    public node root = new node();

    ref struct pointers
    {
        public ref node active_dir = ref root;
    }

    public FileSys() { }

    /////////////////////
    /// utility funcs ///
    /////////////////////

    private ref node ReadPath(String _Path)
    {
        String[] PathArr = (PathStart + _Path).Split('/');
        ref node current = ref root;
        switch (PathArr[0])
        {
            case PathStart:
                current = ref root;
                break;
            case PathStart + ".":
                current = Pointers.active_dir;
                break;
            case PathStart + "..":
                current = active_dir.parent;
                break;
            default:
                throw new CustomException("Invalid Path");
        }
       
    }

    ////////////////////
    /// node class ///
    ////////////////////

    public class node
    {
        public enum type {Dir, File}

        public string _name;
        public node parent;

        /// dir vars
        Dictionary<String, node> children;

        // file vars 
        public enum format { BASIC, TXT, PRGM, CONF, Null }
        public int size = 0;
        public object data = null;
        format form = format.Null;
    }
}

public class CustomException : Exception
{
    public CustomException(string message) : base(message) { }
}