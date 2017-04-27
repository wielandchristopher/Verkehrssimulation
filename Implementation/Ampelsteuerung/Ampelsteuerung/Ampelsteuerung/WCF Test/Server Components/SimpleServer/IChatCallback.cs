using System.ServiceModel;

namespace SimpleServer
{
    public interface IChatCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnNewMessage(string msg);
    }
}
