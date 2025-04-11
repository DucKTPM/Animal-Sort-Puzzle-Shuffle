using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step
{
    public List<Animal> Animals { get; private set; }
    public Tree TreeStart { get; private set; }
    public Tree TreeEnd { get; private set; }

    public Step(List<Animal> animals, Tree treeStart, Tree treeEnd)
    {
        this.Animals = new List<Animal>(animals);
        this.TreeStart = treeStart;
        this.TreeEnd = treeEnd;
    }
}
