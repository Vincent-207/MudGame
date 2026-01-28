using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Ingredient
{
    public ItemSO item;
    public int amount;
}
[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/ New Recipe")]
public class Recipe : ScriptableObject
{
    public List<Ingredient> ingredients;
    public ItemSO result;
    public int resultAmount = 1;
}
