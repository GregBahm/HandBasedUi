using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrototypeSwitcher : MonoBehaviour
{
    public PrototypeStyle Style;
    public PrototypeBinding[] Prototypes;

    public GameObject LayoutButtons;

    public enum PrototypeStyle
    {
        StyleA,
        StyleB,
        StyleB2,
        StyleC,
        StyleD,
        StyleE,
        StyleF,
        StyleG,
    }

    private void Start()
    {
        foreach (PrototypeBinding binding in Prototypes)
        {
            binding.SelectorButton.Released += binding.OnRelease;
            binding.Selected += Binding_Selected;
        }
    }

    private void Binding_Selected(object sender, EventArgs e)
    {
        PrototypeBinding binding = (PrototypeBinding)sender;
        Style = binding.Style;
    }

    private void Update()
    {
        HandPrototype prototype = Prototypes.First(item => item.Style == Style).Prototype;
        LayoutButtons.SetActive(!prototype.IsSummoned);

        foreach (PrototypeBinding binding in Prototypes)
        {
            bool isSelected = binding.Style == Style;
            binding.Prototype.gameObject.SetActive(isSelected);
            binding.SelectorButton.Toggled = isSelected;
        }
    }
}

[Serializable]
public class PrototypeBinding
{
    public PrototypeSwitcher.PrototypeStyle Style;
    public MenuItemButton SelectorButton;
    public HandPrototype Prototype;

    public event EventHandler Selected;

    public void OnRelease(object sender, EventArgs e)
    {
        Selected?.Invoke(this, EventArgs.Empty);
    }
}
