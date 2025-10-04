using FluxFramework.Core;

public class WeaponEquippedEvent : FluxEventBase
{
    public WeaponData Weapon { get; }
    public int SlotIndex { get; }
    public WeaponEquippedEvent(WeaponData weapon, int slotIndex) { Weapon = weapon; SlotIndex = slotIndex; }
}