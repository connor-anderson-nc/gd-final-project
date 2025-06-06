using Godot;
using System;
using System.Collections.Generic;


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
    [Export]
    public string start_dir;
}

public class FileSys
{
    const string PathStart = "#";

    int max_storage;
    int used_space;

    public static node root = new node("root", null, node.Type.Dir);
    public node active_dir;

    public FileSys(string start_dir)
    {
        node start = ReadPath(start_dir);
        if (start.type == node.Type.Dir)
        {
            active_dir = start;
        }
        else
        {
            active_dir = root; 
        }   
    }

    /////////////////////
    /// utility funcs ///
    /////////////////////

    private node ReadPath(String _Path)
    {
        string[] PathArr = (PathStart + _Path).Split('/');
        node current;
        switch (PathArr[0])
        {
            case PathStart:
                current = root;
                break;
            case PathStart + ".":
                current = active_dir;
                break;
            case PathStart + "..":
                current = active_dir.parent;
                break;
            default:
                throw new CustomException("Invalid Path");
        }

        return current;
    }

    ////////////////////
    /// node class ///
    ////////////////////

    public class node
    {
        public enum Type { Dir, BASIC, TXT, PRGM, CONF, Null }

        public string _name;
        public node parent;
        public Type type;

        /// dir vars
        Dictionary<string, node> children;

        // file vars 
        public int size = 0;
        public object data = null;

        // funcs
        public node(string n, node p, Type t)
        {
            _name = n;
            parent = p;
            type = t;
            children = new Dictionary<string, node>();
        }
    }
}

public class CustomException : Exception
{
    public CustomException(string message) : base(message) { }
}