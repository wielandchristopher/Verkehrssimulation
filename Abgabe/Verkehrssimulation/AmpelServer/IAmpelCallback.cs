using System.ServiceModel;

namespace Ampelsteuerung
{
    public interface IAmpelCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnNewMessage(string msg);
    }
}
