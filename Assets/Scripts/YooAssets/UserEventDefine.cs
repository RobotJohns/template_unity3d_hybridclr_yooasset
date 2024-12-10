using UniFramework.Event;

public class UserEventDefine
{
    /// <summary>
    /// �û������ٴγ�ʼ����Դ��
    /// </summary>
    public class UserTryInitialize : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new UserTryInitialize();
            UniEvent.SendMessage(msg);
        }
    }

    /// <summary>
    /// �û���ʼ���������ļ�
    /// </summary>
    public class UserBeginDownloadWebFiles : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new UserBeginDownloadWebFiles();
            UniEvent.SendMessage(msg);
        }
    }

    /// <summary>
    /// �û������ٴθ��¾�̬�汾
    /// </summary>
    public class UserTryUpdatePackageVersion : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new UserTryUpdatePackageVersion();
            UniEvent.SendMessage(msg);
        }
    }

    /// <summary>
    /// �û������ٴθ��²����嵥
    /// </summary>
    public class UserTryUpdatePatchManifest : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new UserTryUpdatePatchManifest();
            UniEvent.SendMessage(msg);
        }
    }

    /// <summary>
    /// �û������ٴ����������ļ�
    /// </summary>
    public class UserTryDownloadWebFiles : IEventMessage
    {
        public static void SendEventMessage()
        {
            var msg = new UserTryDownloadWebFiles();
            UniEvent.SendMessage(msg);
        }
    }
}