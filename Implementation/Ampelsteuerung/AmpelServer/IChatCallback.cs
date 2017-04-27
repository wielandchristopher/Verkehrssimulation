using System.ServiceModel;

namespace Ampelsteuerung
{
    public interface IChatCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnNewMessage(string msg);
    }
}
