using UniFramework.Event;

public class SceneEventDefine
{
    public class ChangeToLobbyScene : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new ChangeToLobbyScene();
            UniEvent.SendMessage(msg);
        }
    }
 
}