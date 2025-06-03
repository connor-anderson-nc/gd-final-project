using Godot;
using System;
using System.Collections.Generic;
using System.Reflection;

public class Computer {
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

public class FileSys {
    int used_space;
    Pointer active_dir;

    private class node {
        string name;
        dir parent;
    }

    private class dir : node {
        Dictionary<String, node> children;
    }

    private class file : node {
        public enum format { BASIC, TXT, PRGM, CONF }
        int size;
        object data;
    }
}