using Godot;

namespace HolyBread.scripts.utiles;

public static class ButtonExtension
{
    public static void ClearPressed(this BaseButton button)
    {
        foreach (var connection in button.GetSignalConnectionList(BaseButton.SignalName.Pressed))
            button.Disconnect(BaseButton.SignalName.Pressed, connection["callable"].AsCallable());
    }
}